using Intel.MsoAuto.C3.Loader.XCCB.Business.Entities;
using MongoDB.Driver;
using System.Reflection;
using log4net;
using Intel.MsoAuto.C3.Loader.PITT.Business.Core;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.DataContext
{

    public class XCCBDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMongoCollection<XccbDocument> _xccbCollection;

        public XCCBDataContext()
        {
            _xccbCollection = new MongoDataAccess().GetMongoCollection<XccbDocument>(Constants.PITT_XCCB_DOCS);
        }

        public List<XccbDocument> GetAllxCCBDocumentsUpdatedSince(DateTime since)
        {
            log.Info("--> GetAllxCCBDocumentsUpdatedSince: " + since.ToString());
            IQueryable<XccbDocument> docs = _xccbCollection.AsQueryable()
                .Where(x => x.updatedOn != null)
                .Where(x => x.updatedOn >= since);

            List<XccbDocument> xccbDocuments = new List<XccbDocument>();
            xccbDocuments.AddRange(docs.ToList());

            log.Info("<-- GetAllxCCBDocumentsUpdatedSince");
            return xccbDocuments;
        }
    }
}
