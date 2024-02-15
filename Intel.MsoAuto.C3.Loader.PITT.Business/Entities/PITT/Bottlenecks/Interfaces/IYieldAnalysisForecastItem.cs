using Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT;


namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.Interfaces
{
    public interface IYieldAnalysisForecastItem
    {
        string? id { get; set; }
        KeyValuePairs? projects { get; set; }
        string siteName { get; set; }
        int week { get; set; }
        int year { get; set; }
        int numOfProjects { get; set; }
        string workWeek { get; set; }
        string createdBy { get; set; } 
        DateTime? createdOn { get; set; }
        string updatedBy { get; set; } 
        DateTime? updatedOn { get; set; }
    }
}
