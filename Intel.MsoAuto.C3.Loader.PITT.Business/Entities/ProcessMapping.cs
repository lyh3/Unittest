using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    [BsonIgnoreExtraElements]
    public class ProcessMapping
    {
        public string systemName { get; set; } = null!;

        public string processName { get; set; } = null!;
    }
}
