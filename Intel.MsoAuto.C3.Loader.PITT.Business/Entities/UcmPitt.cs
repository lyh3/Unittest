using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    /*
    * This the formated/parsed version of UCM that will compatiable w/ PITT
    **/

    [BsonIgnoreExtraElements]
    public class UcmPitt
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string changeId { get; set; } = null!;
        public string? changeCriticality { get; set; } 
        public List<string>? reasonForChange { get; set; }
        public List<string>? detailedReasonForChange { get; set; }
        public DateTime? materialsAvailableFromSupplierDate { get; set; }
        public DateTime? supplierConversionDate { get; set; }
        public string? mainSupplierContact { get; set; }  
        public string? intelResponseName { get; set; }
        public string? supplierId { get; set; }
        public string? supplierName { get; set; }  
        public List<string>? ipnImpacted { get; set; } 
        public Process? process { get; set; }
        public string? xccbBinderNumber { get; set; }
        public List<string>? qualLocation { get; set; }
        public string? changeDescription { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
