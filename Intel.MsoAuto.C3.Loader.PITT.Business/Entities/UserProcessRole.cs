using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    [BsonIgnoreExtraElements]
    public class UserProcessRole
    {
        public string process { get; set; } = null!;

        public string role { get; set; } = null!;
    }

    public class UserProcessRoles : List<UserProcessRole>
    {
    }
}
