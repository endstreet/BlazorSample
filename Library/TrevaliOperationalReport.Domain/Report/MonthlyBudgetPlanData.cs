using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.Report
{
    [Table("RPT_MonthlyBudgetPlanData", Schema = "dbo")]
    public class MonthlyBudgetPlanData : BaseEntity
    {

        /// <summary>
        /// Get or set MonthlyPlanBudgetDataId
        /// </summary>
        [Key]
        public long MonthlyPlanBudgetDataId { get; set; }

        /// <summary>
        /// Get or set SiteMetricId
        /// </summary>
        public int SiteMetricId { get; set; }


        /// <summary>
        /// Get or set Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Get or set Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Get or set Budget
        /// </summary>
        //[Required]
        public decimal? Budget { get; set; }

        /// <summary>
        /// Get or set Forecast
        /// </summary>
       // [Required]
        public decimal? Forecast { get; set; }

        /// <summary>
        /// Get or set Recon
        /// </summary>
        //[Required]
        public decimal? Actual { get; set; }

        /// <summary>
        /// Get or sets the Metrics 
        /// </summary>
        [ForeignKey("SiteMetricId")]
        public virtual General.SiteMetrics SiteMetrics { get; set; }


        [NotMapped]
        public int CurrentMonth { get; set; }
        [NotMapped]
        public int CurrentYear { get; set; }

        [NotMapped]
        public string SiteName { get; set; }
        [NotMapped]
        public string MetricName { get; set; }
        [NotMapped]
        public string ReportName { get; set; }
        [NotMapped]
        public string SectionName { get; set; }
        [NotMapped]
        public string MonthString { get; set; }
        [NotMapped]
        public string YearString { get; set; }


        [NotMapped]
        public int? SiteId { get; set; }
        [NotMapped]
        public int? ReportId { get; set; }
    }

    public class InvalidMonthlyPlanData
    {
        public int SiteId { get; set; }

        public string SiteName { get; set; }

        public int MetricId { get; set; }

        public string MetricName { get; set; }

        public string SectionName { get; set; }

        public string Year { get; set; }

        public string Type { get; set; }

        public decimal? Jan { get; set; }
        public decimal? Feb { get; set; }
        public decimal? Mar { get; set; }
        public decimal? Apr { get; set; }
        public decimal? May { get; set; }

        public decimal? Jun { get; set; }
        public decimal? Jul { get; set; }
        public decimal? Aug { get; set; }
        public decimal? Sep { get; set; }
        public decimal? Oct { get; set; }
        public decimal? Nov { get; set; }
        public decimal? Dec { get; set; }

        public string Comment { get; set; }
    }

    public class MonthlyUploadModel
    {

        public int Year { get; set; }

        public string Type { get; set; }

        public string SiteName { get; set; }
        public int SiteMetricId { get; set; }

        public string MetricName { get; set; }

        public string ReportName { get; set; }

        public string SectionName { get; set; }

        public string YearString { get; set; }

        public decimal? Jan { get; set; }
        public decimal? Feb { get; set; }
        public decimal? Mar { get; set; }
        public decimal? Apr { get; set; }
        public decimal? May { get; set; }

        public decimal? Jun { get; set; }
        public decimal? Jul { get; set; }
        public decimal? Aug { get; set; }
        public decimal? Sep { get; set; }
        public decimal? Oct { get; set; }
        public decimal? Nov { get; set; }
        public decimal? Dec { get; set; }
    }

    public class BudgetYearView
    {
        public int SiteMetricId { get; set; }
        public string SiteName { get; set; }

        public int SiteId { get; set; }
        public string SectionName { get; set; }
        public string SectionMappingName { get; set; }
        public int SectionId { get; set; }
        public string MetricsName { get; set; }
        public string MetricsMappingName { get; set; }
        public int? DisplayOrder { get; set; }
        public int Year { get; set; }


        public decimal? Jan_Budget { get; set; }
        public decimal Jan_Forecast { get; set; }
        public decimal Jan_Actual { get; set; }

        public decimal? Feb_Budget { get; set; }
        public decimal Feb_Forecast { get; set; }
        public decimal Feb_Actual { get; set; }

        public decimal? Mar_Budget { get; set; }
        public decimal Mar_Forecast { get; set; }
        public decimal Mar_Actual { get; set; }

        public decimal? Apr_Budget { get; set; }
        public decimal Apr_Forecast { get; set; }
        public decimal Apr_Actual { get; set; }

        public decimal? May_Budget { get; set; }
        public decimal May_Forecast { get; set; }
        public decimal May_Actual { get; set; }

        public decimal? Jun_Budget { get; set; }
        public decimal Jun_Forecast { get; set; }
        public decimal Jun_Actual { get; set; }

        public decimal? Jul_Budget { get; set; }
        public decimal Jul_Forecast { get; set; }
        public decimal Jul_Actual { get; set; }

        public decimal? Aug_Budget { get; set; }
        public decimal Aug_Forecast { get; set; }
        public decimal Aug_Actual { get; set; }

        public decimal? Sep_Budget { get; set; }
        public decimal Sep_Forecast { get; set; }
        public decimal Sep_Actual { get; set; }


        public decimal? Oct_Budget { get; set; }
        public decimal Oct_Forecast { get; set; }
        public decimal Oct_Actual { get; set; }

        public decimal? Nov_Budget { get; set; }
        public decimal Nov_Forecast { get; set; }
        public decimal Nov_Actual { get; set; }

        public decimal? Dec_Budget { get; set; }
        public decimal Dec_Forecast { get; set; }
        public decimal Dec_Actual { get; set; }
    }

    public class RPT_CalculateFinanceMetrics_Result
    {
        public int Result { get; set; } //(int, not null)
    }

    public class ApprovalDetails
    {
        public int id { get; set; }
        public string Contract { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public bool IsApproved { get; set; }

    }


}
