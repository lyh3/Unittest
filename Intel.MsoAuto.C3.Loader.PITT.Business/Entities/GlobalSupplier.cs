using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace Intel.MsoAuto.C3.PITT.Business.Models
{
    public class GlobalSupplier
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        [Required]
        public string supplierName { get; set; } = null!;
        [Required]
        public string supplierId { get; set; } = null!;
        public bool? isActive { get; set; }
        public DateTime createdOn { get; set; } = DateTime.UtcNow;
        public DateTime updatedOn { get; set; } = DateTime.UtcNow;
    }
}
