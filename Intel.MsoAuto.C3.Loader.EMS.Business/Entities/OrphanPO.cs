using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.EMS.Business.Entities
{
    public partial class OrphanPO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("entityCode")]
        public string EntityCode { get; set; }

        [JsonProperty("budgetArea")]
        public string BudgetArea { get; set; }

        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("rtd")]
        public string Rtd { get; set; }

        [JsonProperty("std")]
        public string Std { get; set; }

        [JsonProperty("toolConfig")]
        public ToolConfig ToolConfig { get; set; }

        [JsonProperty("material")]
        public Material Material { get; set; }

        [JsonProperty("poLines")]
        public PoLine[] PoLines { get; set; }

        [JsonProperty("orphanInfo")]
        public OrphanInfo OrphanInfo { get; set; }
    }

    public partial class OrphanInfo
    {
        [JsonProperty("isOpen")]
        public string IsOpen { get; set; }

        [JsonProperty("disposition")]
        public string Disposition { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("updatedBy")]
        public string UpdatedBy { get; set; }

        [JsonProperty("proposedNeed")]
        public ProposedNeed ProposedNeed { get; set; }

        [JsonProperty("proposeCancelDecisionEstimates")]
        public CancellationFeeInfo ProposeCancelDecisionEstimates { get; set; }

        [JsonProperty("actualCancellationFeeInfo")]
        public CancellationFeeInfo ActualCancellationFeeInfo { get; set; }
    }

    public partial class ProposedNeed
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("entityCode")]
        public string EntityCode { get; set; }

        [JsonProperty("ceid")]
        public string Ceid { get; set; }

        [JsonProperty("process")]
        public string Process { get; set; }

        [JsonProperty("rtd")]
        public string Rtd { get; set; }

        [JsonProperty("prDue")]
        public string PrDue { get; set; }
    }

    public partial class CancellationFeeInfo
    {
        [JsonProperty("cost")]
        public string Cost { get; set; }

        [JsonProperty("nextCost")]
        public string NextCost { get; set; }

        [JsonProperty("nextDate")]
        public string NextDate { get; set; }
    }

    public partial class PoLine
    {
        [JsonProperty("po")]
        public string Po { get; set; }

        [JsonProperty("line")]
        public string Line { get; set; }

        [JsonProperty("supplierId")]
        public string SupplierId { get; set; }

        [JsonProperty("supplier")]
        public string Supplier { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("cost")]
        public string Cost { get; set; }

        [JsonProperty("std")]
        public string Std { get; set; }

        [JsonProperty("sdd")]
        public string Sdd { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("isGoodsReceiptExpected")]
        public string IsGoodsReceiptExpected { get; set; }

        [JsonProperty("isDeleted")]
        public string IsDeleted { get; set; }
    }

    public partial class ToolConfig
    {
        [JsonProperty("ceid")]
        public string Ceid { get; set; }

        [JsonProperty("draft")]
        public string Draft { get; set; }

        [JsonProperty("hand")]
        public string Hand { get; set; }

        [JsonProperty("process")]
        public Process Process { get; set; }
    }

    public partial class Process
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }
    }

    public partial class Material
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("mfg")]
        public string Mfg { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("prefacType")]
        public string PrefacType { get; set; }

        [JsonProperty("erps")]
        public string Erps { get; set; }

        [JsonProperty("crossPlantStatus")]
        public string CrossPlantStatus { get; set; }

        [JsonProperty("oa")]
        public object Oa { get; set; }
    }
}
