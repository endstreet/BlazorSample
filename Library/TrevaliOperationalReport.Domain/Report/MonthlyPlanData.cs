using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.Report
{
    [Table("RPT_MonthlyPlanData", Schema = "dbo")]
    public class MonthlyPlanData : BaseEntity
    {
        /// <summary>
        /// Get or set MonthlyPlanDataId
        /// </summary>
        [Key]
        public long MonthlyPlanDataId { get; set; }

        /// <summary>
        /// Get or set SiteMetricId
        /// </summary>
        public int SiteMetricId { get; set; }

        /// <summary>
        /// Get or Set Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Get or Set Year
        /// </summary>
        public int Year { get; set; }

        [ForeignKey("SiteMetricId")]
        public virtual General.SiteMetrics SiteMetrics { get; set; }

        /// <summary>
        /// Get or Set Current day forecast value
        /// </summary>
        public decimal? ForecastValue { get; set; }

        /// <summary>
        /// Get or set D1
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D1 { get; set; }

        /// <summary>
        /// Get or set D2
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D2 { get; set; }

        /// <summary>
        /// Get or set D3
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D3 { get; set; }

        /// <summary>
        /// Get or set D4
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D4 { get; set; }

        /// <summary>
        /// Get or set D5
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D5 { get; set; }

        /// <summary>
        /// Get or set D6
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D6 { get; set; }

        /// <summary>
        /// Get or set D7
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D7 { get; set; }

        /// <summary>
        /// Get or set D8
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D8 { get; set; }

        /// <summary>
        /// Get or set D9
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D9 { get; set; }

        /// <summary>
        /// Get or set D10
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D10 { get; set; }

        /// <summary>
        /// Get or set D11
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D11 { get; set; }

        /// <summary>
        /// Get or set D12
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D12 { get; set; }

        /// <summary>
        /// Get or set D13
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D13 { get; set; }

        /// <summary>
        /// Get or set D14
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D14 { get; set; }

        /// <summary>
        /// Get or set D15
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D15 { get; set; }

        /// <summary>
        /// Get or set D16
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D16 { get; set; }

        /// <summary>
        /// Get or set D17
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D17 { get; set; }

        /// <summary>
        /// Get or set D18
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D18 { get; set; }

        /// <summary>
        /// Get or set D19
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D19 { get; set; }

        /// <summary>
        /// Get or set D20
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D20 { get; set; }

        /// <summary>
        /// Get or set D21
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D21 { get; set; }

        /// <summary>
        /// Get or set D22
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D22 { get; set; }

        /// <summary>
        /// Get or set D23
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D23 { get; set; }

        /// <summary>
        /// Get or set D24
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D24 { get; set; }

        /// <summary>
        /// Get or set D25
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D25 { get; set; }

        /// <summary>
        /// Get or set D26
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D26 { get; set; }

        /// <summary>
        /// Get or set D27
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D27 { get; set; }

        /// <summary>
        /// Get or set D28
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D28 { get; set; }

        /// <summary>
        /// Get or set D29
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D29 { get; set; }

        /// <summary>
        /// Get or set D30
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D30 { get; set; }

        /// <summary>
        /// Get or set D31
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? D31 { get; set; }

        /// <summary>
        /// Get or Set Total
        /// </summary>
        [NotMapped]
        public decimal Total { get; set; }
    }

    public class MonthlyPlanUploadModel
    {
        public string PlanDataId { get; set; }

        public string SiteName { get; set; }

        public int SiteMetricId { get; set; }

        public string MetricName { get; set; }

        public string SectionName { get; set; }

        public string Year { get; set; }

        public string Month { get; set; }

        public decimal? D1 { get; set; }
        public decimal? D2 { get; set; }
        public decimal? D3 { get; set; }
        public decimal? D4 { get; set; }
        public decimal? D5 { get; set; }
        public decimal? D6 { get; set; }
        public decimal? D7 { get; set; }
        public decimal? D8 { get; set; }
        public decimal? D9 { get; set; }
        public decimal? D10 { get; set; }
        public decimal? D11 { get; set; }
        public decimal? D12 { get; set; }
        public decimal? D13 { get; set; }
        public decimal? D14 { get; set; }
        public decimal? D15 { get; set; }
        public decimal? D16 { get; set; }
        public decimal? D17 { get; set; }
        public decimal? D18 { get; set; }
        public decimal? D19 { get; set; }
        public decimal? D20 { get; set; }
        public decimal? D21 { get; set; }
        public decimal? D22 { get; set; }
        public decimal? D23 { get; set; }
        public decimal? D24 { get; set; }
        public decimal? D25 { get; set; }
        public decimal? D26 { get; set; }
        public decimal? D27 { get; set; }
        public decimal? D28 { get; set; }
        public decimal? D29 { get; set; }
        public decimal? D30 { get; set; }
        public decimal? D31 { get; set; }
    }

    public class MonthlyPlanDataInvalid
    {
        public string month;

        public string PlanDataId { get; set; }

        public string SiteName { get; set; }

        public int SiteId { get; set; }

        public int MetricId { get; set; }

        public string MetricName { get; set; }

        public string SectionName { get; set; }

        public string Year { get; set; }

        public string Month { get; set; }

        public decimal? D1 { get; set; }
        public decimal? D2 { get; set; }
        public decimal? D3 { get; set; }
        public decimal? D4 { get; set; }
        public decimal? D5 { get; set; }
        public decimal? D6 { get; set; }
        public decimal? D7 { get; set; }
        public decimal? D8 { get; set; }
        public decimal? D9 { get; set; }
        public decimal? D10 { get; set; }
        public decimal? D11 { get; set; }
        public decimal? D12 { get; set; }
        public decimal? D13 { get; set; }
        public decimal? D14 { get; set; }
        public decimal? D15 { get; set; }
        public decimal? D16 { get; set; }
        public decimal? D17 { get; set; }
        public decimal? D18 { get; set; }
        public decimal? D19 { get; set; }
        public decimal? D20 { get; set; }
        public decimal? D21 { get; set; }
        public decimal? D22 { get; set; }
        public decimal? D23 { get; set; }
        public decimal? D24 { get; set; }
        public decimal? D25 { get; set; }
        public decimal? D26 { get; set; }
        public decimal? D27 { get; set; }
        public decimal? D28 { get; set; }
        public decimal? D29 { get; set; }
        public decimal? D30 { get; set; }
        public decimal? D31 { get; set; }
        public string Comment { get; set; }
    }
}
