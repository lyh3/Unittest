using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        public string? wwid { get; set; }

        public string? idsid { get; set; }

        public string? name { get; set; }

        public string? firstName { get; set; }

        public string? middleName { get; set; }

        public string? lastName { get; set; }

        public string? email { get; set; }

        public string? location { get; set; }

        public string? badgeType { get; set; }

        public User? manager { get; set; }

        public List<string>? roles { get; set; }

        public UserProcessRoles? processRoles { get; set; }

        public bool? isUser { get; set; }

        public bool? isSuperUser { get; set; }

        public bool? isOwner { get; set; }

        public bool? isSystemAdmin { get; set; }

        public bool? isBusinessAdmin { get; set; }
    }
}

