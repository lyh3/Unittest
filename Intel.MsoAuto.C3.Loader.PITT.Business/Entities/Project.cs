using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Intel.MsoAuto.C3.PITT.Business.Models.Workflows;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    public enum LifeCycleStatus
    {
        DRAFT,
        ACTIVE,
        DROPPED,
        COMPLETED,
        INACTIVE,
        HOLD
    }

    [BsonIgnoreExtraElements]
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string? projectCriticality { get; set; }
        public UcmPitt? ucm { get; set; }
        public string? currentSourcingSupplier { get; set; }
        public List<string>? ipnSpnCurrent { get; set; }
        public List<string>? reasonForChange { get; set; }
        public List<string>? sitesAffected { get; set; }
        public string? projectProcessId { get; set; }
        public DateTime? updatedProjectECD { get; set; }
        public bool isActive { get; set; } = true;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LifeCycleStatus lifeCycleStatus { get; set; } = LifeCycleStatus.DRAFT;
        public bool isDraft { get; set; } = true;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ExternalStatus externalStatus { get; set; } = ExternalStatus.ON_TRACK; // We assume the project is on track until it is calculated otherwise
        public string createdBy { get; set; } = null!; //idsid, system
        public DateTime? createdOn { get; set; } = DateTime.UtcNow;
        public string updatedBy { get; set; } = null!; //idsid, system
        public DateTime? updatedOn { get; set; } = DateTime.UtcNow;
    }
}
