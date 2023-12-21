using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    [BsonIgnoreExtraElements]
    public class Process 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string id { get; set; } = null!;

        [Required]
        public string name { get; set; } = null!;

        [Required]
        public bool isMorWManaged { get; set; } = false;

        [Required]
        public string manufacturingMode { get; set; } = null!;

        public List<string>? pittEntitlements { get; set; }
        
        public List<string>? xccbEntitlements { get; set; }

        public List<string>? xeusEntitlements { get; set; }

        public List<ProcessMapping>? processMappings { get; set; }

        [Required]
        public bool isActive { get; set; } = true;
    }
    public class Processes : List<Process>
    {
    }
}
