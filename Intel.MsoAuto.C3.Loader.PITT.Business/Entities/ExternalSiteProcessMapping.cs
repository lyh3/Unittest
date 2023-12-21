using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    public class ExternalSiteProcessMapping
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string? siteName { get; set; }
        public string? process { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
        public List<PittSiteMappingItem>? siteMappings { get; set; }
    }
}
