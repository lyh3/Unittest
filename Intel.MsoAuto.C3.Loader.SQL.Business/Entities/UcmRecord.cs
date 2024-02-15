namespace Intel.MsoAuto.C3.Loader.SQL.Business.Entities
{
    public class UcmRecord
    {
        public string ChangeID { get; set; }
        public string ChangeCriticality { get; set; }
        public string ReasonforChange { get; set; }
        public string DetailedReasonForChange { get; set; }
        public string MaterialsAvailableFromSupplierDate { get; set; }
        public string SupplierConversionDate { get; set; }
        public string MainSupplierContact { get; set; }
        public string IntelResponseName { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string LastUpdatedOn { get; set; }
        public string IpnImpacted { get; set; }
        public string ChangeDescription { get; set; }
        public string ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string XCCBBinderNumber { get; set; }
        public string QualLocation { get; set; }
    }
}
