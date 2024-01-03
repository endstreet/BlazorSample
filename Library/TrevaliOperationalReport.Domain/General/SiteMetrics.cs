using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_SiteMetrics", Schema = "dbo")]
    public class SiteMetrics : BaseEntity
    {
        /// <summary>
        /// Get or set SiteMetricId
        /// </summary>
        [Key]
        public int SiteMetricId { get; set; }

        /// <summary>
        /// Get or set SiteId
        /// </summary>
        [Required]
        public int SiteId { get; set; }

        /// <summary>
        /// Get or sets the Site value
        /// </summary>
        /// <value>The Report.</value>
        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        /// <summary>
        /// Get or set ReportId
        /// </summary>
        [Required]
        public int ReportId { get; set; }

        /// <summary>
        /// Get or set MetricId
        /// </summary>
        [Required]
        public int MetricId { get; set; }

        /// <summary>
        /// Get or set SectionId
        /// </summary>
        [Required]
        public int SectionId { get; set; }

        /// <summary>
        /// Get or set DisplayOrder
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Get or set IsMonthly
        /// </summary>
        public bool IsYearly { get; set; }

        /// <summary>
        /// Get or set IsWeekly
        /// </summary>
        public bool IsWeekly { get; set; }

        /// <summary>
        /// Get or set IsWeekly
        /// </summary>
        public bool IsDaily { get; set; }

        /// <summary>
        /// Get or sets the Site Name
        /// </summary>
        /// <value>The name of the Site.</value>
        [NotMapped]
        public string SiteName { get; set; }

        /// <summary>
        /// Get or sets the Report value
        /// </summary>
        /// <value>The Report.</value>
        [ForeignKey("ReportId")]
        public virtual Reports Report { get; set; }

        /// <summary>
        /// Get or sets the Report Name
        /// </summary>
        /// <value>The name of the Report.</value>
        [NotMapped]
        public string ReportName => Report != null ? Report.Name : "";

        /// <summary>
        /// Get or sets the Section value
        /// </summary>
        /// <value>The Section.</value>
        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; }

        /// <summary>
        /// Get or sets the Section Name
        /// </summary>
        /// <value>The name of the Section.</value>
        [NotMapped]
        public string SectionName => Section != null ? Section.SectionName : "";


        /// <summary>
        /// Get or sets the Section HideInReports
        /// </summary>
        /// <value>The name of the Section.</value>
        public int HideInReports => Section != null ? Section.HideInReports : 0;

        /// <summary>
        /// Get or sets the Metric value
        /// </summary>
        /// <value>The Metric.</value>
        [ForeignKey("MetricId")]
        public virtual Metrics Metric { get; set; }

        /// <summary>
        /// Get or sets the Site Name
        /// </summary>
        /// <value>The name of the Site.</value>
        [NotMapped]
        public string MetricsName => Metric != null ? Metric.MetricsName : "";
    }
}
