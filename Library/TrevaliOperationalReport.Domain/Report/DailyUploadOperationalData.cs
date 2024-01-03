using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Domain.Report
{
    [Table("RPT_DailyUploadOperationalData", Schema = "dbo")]
    public class DailyUploadOperationalData : BaseEntity
    {

        [Key]
        public long DailyOperationalDataId { get; set; }

        public int SiteMetricId { get; set; }
        public Nullable<DateTime> DailyUploadDate { get; set; }


        public virtual General.SiteMetrics SiteMetrics { get; set; }

        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        [Required(ErrorMessage = "*")]
        //[RegularExpression(@"^[0-9]\d{0,9}(\.\d{1,3})?%?$", ErrorMessage = "Must be a positive number.")]
        public decimal? ActualValue { get; set; }

        [StringLength(1000, ErrorMessage = "Comment must be within 500 characters.")]
        public string Comment { get; set; }

        [NotMapped]
        public Site Site { get; set; }

        [NotMapped]
        public TrevaliOperationalReport.Domain.General.Reports Report { get; set; }

        [NotMapped]
        public List<DailyUploadMetricsData> DailyUploadMetricsData { get; set; }
        [NotMapped]
        public string SiteName { get; set; }
        [NotMapped]
        public string MetricName { get; set; }
        [NotMapped]
        public string ReportName { get; set; }
        [NotMapped]
        public string SectionName { get; set; }

        [NotMapped]
        public int SiteId { get; set; }
    }
    public class DailyUploadMetricsData
    {

        public long DailyOperationalDataId { get; set; }
        //public int MetricId { get; set; }
        public int SiteId { get; set; }
        //public int ReportId { get; set; }

        public int SiteMetricId { get; set; }

        public int SectionId { get; set; }
        public string SectionName { get; set; }

        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        //[RegularExpression(@"^[0-9]\d{0,9}(\.\d{1,3})?%?$", ErrorMessage = "Must be a positive number.")]
        public decimal? ActualValue { get; set; }


        public string Comment { get; set; }
        [ForeignKey("MetricId")]
        public Metrics Metric { get; set; }

        public bool IsDefault { get; set; }
        public DateTime DailyUploadDate { get; set; }

    }
    public class InvalidDailyData
    {
        /// <summary>
        /// Get or set SiteId
        /// </summary>
        public int SiteId { get; set; }

        public string SiteName { get; set; }
        public string SectionName { get; set; }

        /// <summary>
        /// Get or set ReportId
        /// </summary>
        public int ReportId { get; set; }


        public string ReportName { get; set; }

        /// <summary>
        /// Get or set MetricId
        /// </summary>
        public int MetricId { get; set; }

        public string MetricName { get; set; }

        /// <summary>
        /// Get or set Budget
        /// </summary>
        public decimal? ActualValue { get; set; }
        public string CreatedDate { get; set; }


        public string Remarks { get; set; }
    }
    public partial class RPT_DailyData_Result
    {
        public long DailyOperationalDataId { get; set; }
        public int SiteMetricId { get; set; }
        public int SectionId { get; set; }
        public int MetricId { get; set; }
        public string SectionName { get; set; }
        public string MetricsName { get; set; }
        public string Units { get; set; }
        public Nullable<decimal> ActualValue { get; set; }
        public string Comment { get; set; }
    }
    [Table("RPT_UploadSheetData", Schema = "dbo")]
    public class UploadSheetData : BaseEntity
    {

        [Key]
        public long UploadSheetId { get; set; }
        public int SiteId { get; set; }
        public DateTime UploadSheetDate { get; set; }
        public string Comment { get; set; }
    }
}
