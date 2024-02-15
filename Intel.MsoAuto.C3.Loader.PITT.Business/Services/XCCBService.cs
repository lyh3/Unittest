using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using Intel.MsoAuto.C3.Loader.XCCB.Business.Entities;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class XCCBService
    {

        private XCCBDataContext _xCCBDataContext;

        public XCCBService()
        {
            _xCCBDataContext = new XCCBDataContext();
        }

        public List<XccbDocument> GetAllxCCBDocumentsUpdatedSince(DateTime since)
        {
            return _xCCBDataContext.GetAllxCCBDocumentsUpdatedSince(since);
        }

        public List<XccbDocument> GetUpdatedFWPApprovedxCCBDocs()
        {
            // Get the date for two days ago (in case something failed or xCCB hasnt finished for today)
            DateTime twoDaysAgo = DateTime.UtcNow;
            twoDaysAgo = twoDaysAgo.AddDays(-2);

            // Get docs
            List<XccbDocument> recentlyUpdatedDocs = GetAllxCCBDocumentsUpdatedSince(twoDaysAgo);

            // Filter down to all that are status == "Approved" and phase = "Pilot"
            IEnumerable<XccbDocument> filteredDocs = from doc in recentlyUpdatedDocs where 
                                                        (doc.phase == "Final" || doc.phase == "FWP") 
                                                        && doc.status == "Approved" 
                                                     select doc;
        
            List<XccbDocument> xccbDocuments = new List<XccbDocument>();
            xccbDocuments.AddRange(filteredDocs);

            return xccbDocuments;
        }

    }
}
