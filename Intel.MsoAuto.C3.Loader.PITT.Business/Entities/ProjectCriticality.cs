using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    public class ProjectCriticality
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string? name { get; set; }
        public string? shortName { get; set; }
        public bool? isActive { get; set; }
        public DateTime? updatedOn { get; set; }
        public DateTime? createdOn { get; set; }
    }
}
