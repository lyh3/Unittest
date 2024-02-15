using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Intel.MsoAuto.C3.Loader.XCCB.Business.Entities
{
    public class XccbDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public double documentID { get; set; }
        public string phase { get; set; }
        public string status { get; set; }
        public string forumName { get; set; }
        public string approvalNumber { get; set; }
        public int hasOpenARs { get; set; }
        public int hasOpenEditOrGattingARs { get; set; }
        public string forumInfo { get; set; }
        public XccbForums forumInfos { get; set; }
        public string ceid { get; set; }
        public DateTime? updatedOn { get; set; }
        public string updatedBy { get; set; }
    }
}
