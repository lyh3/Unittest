using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.XCCB.Business.Entities
{
    public class XccbForum
    {
        public string name { get; set; }
        public string initiatedDate { get; set; }
        public string meetingDate { get; set; }
        public string workflowActivityState { get; set; }
        public string workflowActivityForumState { get; set; }
    }
}
