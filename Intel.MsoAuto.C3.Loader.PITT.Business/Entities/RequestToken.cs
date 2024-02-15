using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities.Interfaces;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    public class RequestToken : IRequestToken
    {
        public string accessToken { get; set; } = string.Empty;
        public DateTimeOffset expiresOn { get; set; } = DateTimeOffset.MinValue;
        public string tokenType { get; set; } = string.Empty;
    }
}
