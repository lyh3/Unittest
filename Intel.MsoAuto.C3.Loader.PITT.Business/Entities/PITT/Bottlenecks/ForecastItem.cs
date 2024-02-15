using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks
{
    [BsonIgnoreExtraElements]
    public class ForecastItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public KeyValuePairs? projects { get; set; }
        public string siteName { get; set; } = null!;
        public int week { get; set; }
        public int year { get; set; }
        public int numOfProjects { get; set; }
        public string workWeek { get; set; } = null!;
        public string createdBy { get; set; } = "system"; //idsid, system
        public DateTime? createdOn { get; set; } = DateTime.UtcNow;
        public string updatedBy { get; set; } = "system"; //idsid, system
        public DateTime? updatedOn { get; set; } = DateTime.UtcNow;
    }
}
