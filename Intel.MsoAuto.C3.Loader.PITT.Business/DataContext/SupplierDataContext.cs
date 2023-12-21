using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.DataAccess;
using System.Reflection;
using log4net;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using MongoDB.Driver;
using Intel.MsoAuto.C3.PITT.Business.Models;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.DataContext
{
    public class SupplierDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public SupplierDataContext() { }

        public void SyncGlobalSupplierData()
        {
            log.Info("--> SyncGlobalSupplierData" + "(" + Settings.Env + ")");
            ISqlDataAccess dataAccess = null;
            try
            {
                log.Info("Connecting...");

                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.C3ConnectionString, Settings.GlobalSuppliersSp);
                dataAccess.ExecuteReader();
                log.Info("Connected...");
                List<GlobalSupplier> data = new List<GlobalSupplier>();
                GlobalSupplier d = null;
                while (dataAccess.DataReader.Read())
                {
                    d = new GlobalSupplier();
                    d.supplierId = dataAccess.DataReader["ParentBusinessPartyId"].ToStringSafely();
                    d.supplierName = dataAccess.DataReader["ParentBusinessOrgName"].ToStringSafely();
                    data.Add(d);
                }
                log.Info("Total number of records from GlobalSuppliers: " + data.Count);
                UpdateGlobalSuppliersToPitt(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }
            log.Info("<-- SyncGlobalSupplierData" + "(" + Settings.Env + ")");

        }

        private void UpdateGlobalSuppliersToPitt(List<GlobalSupplier> suppliers)
        {
            IMongoCollection<GlobalSupplier> globalSuppliersCollection = new MongoDataAccess().GetMongoCollection<GlobalSupplier>(Constants.PITT_GLOBAL_SUPPLIERS);
            IClientSessionHandle clientSession = new MongoDataAccess().GetClientSession();

            clientSession.StartTransaction();
            try
            {
                List<GlobalSupplier> globalSuppliersPitt = globalSuppliersCollection.Find(clientSession, _ => true).ToList();
                List<string> deleteIds = new List<string>();


                // Update new values into pitt
                foreach (GlobalSupplier supplier in suppliers)
                {
                    GlobalSupplier? foundSupplier = globalSuppliersPitt.Find(u => u.supplierName == supplier.supplierName && u.supplierId == supplier.supplierId);

                    //Update record
                    if (foundSupplier != null)
                    {
                        supplier.updatedOn = DateTime.UtcNow;
                        supplier.createdOn = foundSupplier.createdOn;
                        supplier.id = foundSupplier.id;
                        supplier.isActive = true;
                        deleteIds.Add(foundSupplier.id);
                    }
                    else  //Create record
                    {
                        supplier.updatedOn = DateTime.UtcNow;
                        supplier.createdOn = DateTime.UtcNow;
                        supplier.isActive = true;
                    }
                }

                if (suppliers.Count > 0)
                {
                    globalSuppliersCollection.DeleteMany(clientSession, x => deleteIds.Contains(x.id));
                    globalSuppliersCollection.InsertMany(clientSession, suppliers);
                }

                // If it doesn't appear in SQL table then soft delete
                //foreach (GlobalSupplier pittSupplier in globalSuppliersPitt)
                //{
                //    GlobalSupplier? foundSupplier = suppliers.Find(u => u.supplierName == pittSupplier.supplierName && u.supplierId == pittSupplier.supplierId);

                //    if (foundSupplier == null)
                //    {
                //        pittSupplier.updatedOn = DateTime.UtcNow;
                //        pittSupplier.isActive = false;
                //        globalSuppliersCollection.DeleteOne(u => u.id == pittSupplier.id);
                //        globalSuppliersCollection.InsertOne(pittSupplier);
                //    }
                //}

                clientSession.CommitTransaction();
            }
            catch
            {
                clientSession.AbortTransaction();
                // TODO: Log error.
                throw;
            }
        }

    }
}
