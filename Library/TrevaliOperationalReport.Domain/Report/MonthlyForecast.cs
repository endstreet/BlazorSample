using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.Report
{
    [Table("RPT_MonthlyForecast", Schema = "dbo")]
    public class MonthlyForecast : BaseEntity
    {
        /// <summary>
        /// Get or set MonthlyForecastId
        /// </summary>
        [Key]
        public int MonthlyForecastId { get; set; }

        /// <summary>
        /// Get or set UniqueId
        /// </summary>
        public string UniqueId { get; set; }

        /// <summary>
        /// Get or set SiteId
        /// </summary>
        public int SiteId { get; set; }

        [ForeignKey("SiteId")]
        public virtual General.Site Site { get; set; }

        /// <summary>
        /// Get or set Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Get or set Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Get or set IsApprove
        /// </summary>
        public bool IsApprove { get; set; }

        /// <summary>
        /// Get or set IsAnnualApprove
        /// </summary>
        public bool IsAnnualApprove { get; set; }
    }

}
