using Intel.MsoAuto.C3.Loader.XCCB.Business.Entities;
using Intel.MsoAuto.C3.Loader.XCCB.Business.Core;
using log4net;
using MongoDB.Driver;
using System.Reflection;
using Intel.MsoAuto.Shared.Extensions;
using System.Text.Json;

namespace Intel.MsoAuto.C3.Loader.XCCB.Business.DataContext
{
    public class XccbDataContext : IXccbDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string? _apiUrl;
        private readonly IMongoCollection<XccbDocument> _xccbCollection;

        public XccbDataContext()
        {
            this._apiUrl = Settings.XccbServiceApiUrl;
            _xccbCollection = new MongoDataAccess().GetMongoCollection<XccbDocument>(Settings.XccbCollectionName);
        }

        public void SyncXccbDocumentsToMongo()
        {
            log.Info("--> SyncXccbDocumentsToMongo");
            string docIDs = getXccbDocumentIDs();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string[] docs = docIDs.Split(',');

                    foreach (string docID in docs)
                    {
                        HttpResponseMessage response = client.GetAsync(_apiUrl + "/api/Xccb/GetDocument/" + docID).Result;
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        log.Info(responseBody);
                        XccbDocument xccbDocument = null;
                        if (response.IsSuccessStatusCode && response != null && responseBody != "")
                        {
                            xccbDocument = JsonSerializer.Deserialize<XccbDocument>(responseBody);
                            XccbForum f = null;
                            if (xccbDocument.forumInfo.IsNeitherNullNorEmpty())
                            {
                                xccbDocument.forumInfos = new XccbForums();
                                string[] forums = xccbDocument.forumInfo.Split("),");
                                foreach (string forum in forums)
                                {
                                    /*ForumInfo is complex string:
                                     * ex: P1270 FECM- OFFLINE ONLY, see Andrew Yee if needed (null | null | Pending | Pending), P1270 PCCB (null | null | Pending | Pending)
                                     * Each Forum is comma separated with more data inside the parentheses 
                                     * The first field is the date initiated or null if it is not yet been assigned to review the forum. 
                                     * The second field is the date it is on a meeting (if it is assigned to a meeting) - not every review will have this data.
                                     * The third and fourth are nearly the same this is the state of the workflow activity and workflow activity forum
                                     */
                                    f = new XccbForum();
                                    string[] info = forum.Split("(");
                                    f.name = info[0].Trim();
                                    f.initiatedDate = info[1].Split('|')[0].Trim();
                                    f.meetingDate = info[1].Split('|')[1].Trim();
                                    f.workflowActivityState = info[1].Split('|')[2].Trim();
                                    f.workflowActivityForumState = info[1].Split('|')[3].Trim().Replace(")", "");
                                    xccbDocument.forumInfos.Add(f);
                                }
                            }

                            //Update the mongoDb collection
                            UpsertXccbDocument(xccbDocument);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            log.Info("<-- SyncXccbDocumentsToMongo");
        }

        public string getXccbDocumentIDs()
        {
            log.Info("--> getXccbDocumentIDs");
            string docIDs = "";
            _xccbCollection.Find(_ => true).ToList().ForEach(xccbDoc =>
            {
                docIDs = docIDs + xccbDoc.documentID + ",";
            });
            docIDs = docIDs.Substring(0, docIDs.Length - 1);
            Console.WriteLine("Document IDs in MongoDB: " + docIDs);
            log.Info("<-- getXccbDocumentIDs");
            return docIDs;
        }

        public void UpsertXccbDocument(XccbDocument xccbDocument)
        {
            log.Info("--> UpsertXccbDocument ID: " + xccbDocument.documentID);

            try
            {
                IMongoCollection<XccbDocument> xccbCollection = new MongoDataAccess().GetMongoCollection<XccbDocument>(Settings.XccbCollectionName);

                // Query existing document from the collection
                XccbDocument existingDocument = xccbCollection.Find(Builders<XccbDocument>.Filter.Eq("documentID", xccbDocument.documentID)).FirstOrDefault();

                // Check if the existing document has the same values
                if (existingDocument != null && DocumentValuesAreEqual(existingDocument, xccbDocument))
                {
                    log.Info("Skipping upsert as document values are the same.");
                    return;
                }

                // Update or insert the document
                FilterDefinition<XccbDocument> filter = Builders<XccbDocument>.Filter.Eq("documentID", xccbDocument.documentID);
                UpdateDefinition<XccbDocument> update = Builders<XccbDocument>.Update.Set(x => x.phase, xccbDocument.phase)
                    .Set(x => x.status, xccbDocument.status)
                    .Set(x => x.forumName, xccbDocument.forumName)
                    .Set(x => x.approvalNumber, xccbDocument.approvalNumber)
                    .Set(x => x.hasOpenARs, xccbDocument.hasOpenARs)
                    .Set(x => x.hasOpenEditOrGattingARs, xccbDocument.hasOpenEditOrGattingARs)
                    .Set(x => x.forumInfo, xccbDocument.forumInfo)
                    .Set(x => x.forumInfos, xccbDocument.forumInfos)
                    .Set(x => x.ceid, xccbDocument.ceid)
                    .Set(x => x.updatedOn, DateTime.UtcNow)
                    .Set(x => x.updatedBy, Environment.UserName);

                UpdateOptions options = new UpdateOptions { IsUpsert = true };
                UpdateResult result = xccbCollection.UpdateOne(filter, update, options);
            }
            catch (Exception ex)
            {
                log.Error("Failed to update xccbDocument with id: " + xccbDocument.documentID, ex);
                throw;
            }

            log.Info("<-- UpsertXccbDocument");
        }

        // Helper method to check if document values are equal
        private bool DocumentValuesAreEqual(XccbDocument existingDocument, XccbDocument newDocument)
        {
            // Compare values of all fields and return true if they are equal
            return existingDocument.phase == newDocument.phase
                && existingDocument.status == newDocument.status
                && existingDocument.forumName == newDocument.forumName
                && existingDocument.approvalNumber == newDocument.approvalNumber
                && existingDocument.hasOpenARs == newDocument.hasOpenARs
                && existingDocument.hasOpenEditOrGattingARs == newDocument.hasOpenEditOrGattingARs
                && existingDocument.forumInfo == newDocument.forumInfo
                && existingDocument.forumInfos == newDocument.forumInfos
                && existingDocument.ceid == newDocument.ceid;
        }

    }
}
