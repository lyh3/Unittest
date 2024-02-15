using System;
using System.Collections.Generic;
using System.Text;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.Entity
{
    public class AllocatedSpareTransaction
    {
        public string FacilityName { get; set; }
        public string TechnologyNodeName { get; set; }
        public string ProcessName { get; set; }
        public string FunctionalAreaName { get; set; }
        public string OrganizationAreaName { get; set; }
        public string OrganizationUnitName { get; set; }
        public string ParentCapitalEquipmentIdentifier { get; set; }
        public string CapitalEquipmentIdentifier { get; set; }
        public string SparePartNumber { get; set; }
        public string SparePartDescription { get; set; }
        public string SupplierIdentifier { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPartNumber { get; set; }
        public string WorkWeekNumber { get; set; }
        public string UniqueEquipmentIdentifier { get; set; }
        public string ConsumedQuantity { get; set; }
        public string ConsumptionAmount { get; set; }
        public string CostToIntelAmount { get; set; }
        public string TransactionDateTime { get; set; }
        public string LastUpdatedDateTime { get; set; }
        public string AsOfSourceDtm { get; set; }
        public string AsOfTargetDtm { get; set; }
    }
}
