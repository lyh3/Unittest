using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.Entity
{
    public class AllocatedGasChemTransaction
    {
        public string FacilityNm { get; set; }
        public string TechnologyNodeNm { get; set; }
        public string ProcessNm { get; set; }
        public string FunctionalAreaNm { get; set; }
        public string OrganizationAreaNm { get; set; }
        public string OrganizationUnitNm { get; set; }
        public string ParentCapitalEquipmentId { get; set; }
        public string CapitalEquipmentId { get; set; }
        public string GasChemGroupNm { get; set; }
        public string GasChemNm { get;set; }
        public string GasChemDsc { get; set; }
        public string SupplierNm { get; set; }
        public string WorkWeekNbr { get; set; }
        public string UniqueEquipmentId { get; set; }
        public string AllocatedGasChemQty { get; set; }
        public string SupplierId { get; set; }
        public string TransactionDtm { get; set; }
        public string LastUpdatedDtm { get; set; }
        public string AsOfSourceDtm { get; set; }
        public string AsOfTargetDtm { get; set; }
    }
}
