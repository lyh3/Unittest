using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{  /*
    * This the "raw" version of UCM that is from c3 commons
    **/
    [BsonIgnoreExtraElements]
    public class Ucm
    {


        public Ucm() { }
      
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string changeId { get; set; } = null!;
        public string? changeCriticality { get; set; } //project criticality
        public string? reasonForChange { get; set; } 
        public string? detailedReasonForChange { get; set; } 
        public DateTime? materialsAvailableFromSupplierDate { get; set; }
        public DateTime? supplierConversionDate { get; set; }
        public string? mainSupplierContact { get; set; }  //will need to insert within Contacts
        public string? intelResponseName { get; set; }
        public string? supplierId { get; set; }
        public string? supplierName { get; set; }  // Contact Current Souring Supplier
        public string? ipnImpacted { get; set; } // Current IPNs/SPNs
        public string? processId { get; set; }
        public string? processName { get; set; } // will need to be converted to Process
        public string? xccbBinderNumber { get; set; }
        public string? qualLocation { get; set; } 
        public string? changeDescription { get; set; } // Project Description
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
