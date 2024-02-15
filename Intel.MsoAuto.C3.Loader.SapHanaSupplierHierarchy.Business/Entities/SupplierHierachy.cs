using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.SapHanaSuplHier.Business.Entity {
    internal class SupplierHierachy {
        public SupplierHierachy() { }
        public string SupplierHierachyTypeNm { get; set; } = string.Empty;
        public string GolbalUltimateSupplierId { get; set; } = string.Empty;
        public string ParentSupplierId { get; set; } = string.Empty;
        public string ChildSupplierId { get; set; } = string.Empty;
        public string EffetiveEndDtm { get; set;} = string.Empty;
        public string SupplierHierachyRelationshipClassificationNm { get;set; } = string.Empty;
        public string CreateAgentId { get; set; } = string.Empty;
        public string CreateDtm { get; set; } = string.Empty;
        public string ChangeAgentId { get; set; } = string.Empty;   
        public string ChangeDtm { get; set; } =  string.Empty;
    }
}
