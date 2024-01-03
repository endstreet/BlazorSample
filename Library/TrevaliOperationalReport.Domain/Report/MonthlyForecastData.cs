using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.Report
{
    [Table("RPT_MonthlyForecastData", Schema = "dbo")]
    public class MonthlyForecastData : BaseEntity
    {
        /// <summary>
        /// Get or set MonthlyForecastDataId
        /// </summary>
        [Key]
        public long MonthlyForecastDataId { get; set; }

        /// <summary>
        /// Get or set MonthlyForecastId
        /// </summary>
        public int MonthlyForecastId { get; set; }

        [ForeignKey("MonthlyForecastId")]
        public virtual MonthlyForecast MonthlyForecast { get; set; }

        /// <summary>
        /// Get or set SiteMetricId
        /// </summary>
        public int? SiteMetricId { get; set; }

        [ForeignKey("SiteMetricId")]
        public virtual General.SiteMetrics SiteMetrics { get; set; }

        /// <summary>
        /// Get or set M1
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M1 { get; set; }

        /// <summary>
        /// Get or set M2
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M2 { get; set; }

        /// <summary>
        /// Get or set M3
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M3 { get; set; }

        /// <summary>
        /// Get or set M4
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M4 { get; set; }

        /// <summary>
        /// Get or set M5
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M5 { get; set; }

        /// <summary>
        /// Get or set M6
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M6 { get; set; }

        /// <summary>
        /// Get or set M7
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M7 { get; set; }

        /// <summary>
        /// Get or set M8
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M8 { get; set; }

        /// <summary>
        /// Get or set M9
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M9 { get; set; }

        /// <summary>
        /// Get or set M10
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M10 { get; set; }

        /// <summary>
        /// Get or set M11
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M11 { get; set; }

        /// <summary>
        /// Get or set M12
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M12 { get; set; }

        /// <summary>
        /// Get or set M13
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M13 { get; set; }

        /// <summary>
        /// Get or set M14
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M14 { get; set; }

        /// <summary>
        /// Get or set M15
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M15 { get; set; }

        /// <summary>
        /// Get or set M16
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M16 { get; set; }

        /// <summary>
        /// Get or set M17
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M17 { get; set; }

        /// <summary>
        /// Get or set M18
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M18 { get; set; }

        /// <summary>
        /// Get or set M19
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M19 { get; set; }

        /// <summary>
        /// Get or set M20
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M20 { get; set; }

        /// <summary>
        /// Get or set M21
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M21 { get; set; }

        /// <summary>
        /// Get or set M22
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M22 { get; set; }

        /// <summary>
        /// Get or set M23
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M23 { get; set; }

        /// <summary>
        /// Get or set M24
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? M24 { get; set; }

        /// <summary>
        /// Get or set Y1
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y1 { get; set; }

        /// <summary>
        /// Get or set Y2
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y2 { get; set; }

        /// <summary>
        /// Get or set Y3
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y3 { get; set; }

        /// <summary>
        /// Get or set Y4
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y4 { get; set; }

        /// <summary>
        /// Get or set Y5
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y5 { get; set; }

        /// <summary>
        /// Get or set Y6
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y6 { get; set; }

        /// <summary>
        /// Get or set Y7
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y7 { get; set; }

        /// <summary>
        /// Get or set Y8
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y8 { get; set; }

        /// <summary>
        /// Get or set Y9
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y9 { get; set; }

        /// <summary>
        /// Get or set Y10
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y10 { get; set; }

        /// <summary>
        /// Get or set Y11
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y11 { get; set; }

        /// <summary>
        /// Get or set Y12
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y12 { get; set; }

        /// <summary>
        /// Get or set Y13
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y13 { get; set; }

        /// <summary>
        /// Get or set Y14
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y14 { get; set; }

        /// <summary>
        /// Get or set Y15
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y15 { get; set; }

        /// <summary>
        /// Get or set Y16
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y16 { get; set; }

        /// <summary>
        /// Get or set Y17
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y17 { get; set; }

        /// <summary>
        /// Get or set Y18
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y18 { get; set; }

        /// <summary>
        /// Get or set Y19
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y19 { get; set; }

        /// <summary>
        /// Get or set Y20
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y20 { get; set; }

        /// <summary>
        /// Get or set Y21
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y21 { get; set; }

        /// <summary>
        /// Get or set Y22
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y22 { get; set; }

        /// <summary>
        /// Get or set Y23
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y23 { get; set; }

        /// <summary>
        /// Get or set Y24
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y24 { get; set; }

        /// <summary>
        /// Get or set Y25
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? Y25 { get; set; }
    }


    public class MonthlyForecastUploadModel
    {
        public string ForecastId { get; set; }

        public string SiteName { get; set; }
        public int SiteMetricId { get; set; }

        public string MetricName { get; set; }

        public string SectionName { get; set; }

        public string YearString { get; set; }

        public decimal? M1 { get; set; }
        public decimal? M2 { get; set; }
        public decimal? M3 { get; set; }
        public decimal? M4 { get; set; }
        public decimal? M5 { get; set; }
        public decimal? M6 { get; set; }
        public decimal? M7 { get; set; }
        public decimal? M8 { get; set; }
        public decimal? M9 { get; set; }
        public decimal? M10 { get; set; }
        public decimal? M11 { get; set; }
        public decimal? M12 { get; set; }
        public decimal? M13 { get; set; }
        public decimal? M14 { get; set; }
        public decimal? M15 { get; set; }
        public decimal? M16 { get; set; }
        public decimal? M17 { get; set; }
        public decimal? M18 { get; set; }
        public decimal? M19 { get; set; }
        public decimal? M20 { get; set; }
        public decimal? M21 { get; set; }
        public decimal? M22 { get; set; }
        public decimal? M23 { get; set; }
        public decimal? M24 { get; set; }

        public decimal? Y1 { get; set; }
        public decimal? Y2 { get; set; }
        public decimal? Y3 { get; set; }
        public decimal? Y4 { get; set; }
        public decimal? Y5 { get; set; }
        public decimal? Y6 { get; set; }
        public decimal? Y7 { get; set; }
        public decimal? Y8 { get; set; }
        public decimal? Y9 { get; set; }
        public decimal? Y10 { get; set; }
        public decimal? Y11 { get; set; }
        public decimal? Y12 { get; set; }
        public decimal? Y13 { get; set; }
        public decimal? Y14 { get; set; }
        public decimal? Y15 { get; set; }
        public decimal? Y16 { get; set; }
        public decimal? Y17 { get; set; }
        public decimal? Y18 { get; set; }
        public decimal? Y19 { get; set; }
        public decimal? Y20 { get; set; }
        public decimal? Y21 { get; set; }
        public decimal? Y22 { get; set; }
        public decimal? Y23 { get; set; }
        public decimal? Y24 { get; set; }
        public decimal? Y25 { get; set; }
    }

    public class InvalidMonthlyForecastData
    {
        public int SiteId { get; set; }

        public string SiteName { get; set; }

        public int MetricId { get; set; }

        public string MetricName { get; set; }

        public string SectionName { get; set; }

        public string Year { get; set; }

        public string ForecastId { get; set; }

        public decimal? M1 { get; set; }
        public decimal? M2 { get; set; }
        public decimal? M3 { get; set; }
        public decimal? M4 { get; set; }
        public decimal? M5 { get; set; }
        public decimal? M6 { get; set; }
        public decimal? M7 { get; set; }
        public decimal? M8 { get; set; }
        public decimal? M9 { get; set; }
        public decimal? M10 { get; set; }
        public decimal? M11 { get; set; }
        public decimal? M12 { get; set; }
        public decimal? M13 { get; set; }
        public decimal? M14 { get; set; }
        public decimal? M15 { get; set; }
        public decimal? M16 { get; set; }
        public decimal? M17 { get; set; }
        public decimal? M18 { get; set; }
        public decimal? M19 { get; set; }
        public decimal? M20 { get; set; }
        public decimal? M21 { get; set; }
        public decimal? M22 { get; set; }
        public decimal? M23 { get; set; }
        public decimal? M24 { get; set; }
        public string Comment { get; set; }

        public decimal? Y1 { get; set; }
        public decimal? Y2 { get; set; }
        public decimal? Y3 { get; set; }
        public decimal? Y4 { get; set; }
        public decimal? Y5 { get; set; }
        public decimal? Y6 { get; set; }
        public decimal? Y7 { get; set; }
        public decimal? Y8 { get; set; }
        public decimal? Y9 { get; set; }
        public decimal? Y10 { get; set; }
        public decimal? Y11 { get; set; }
        public decimal? Y12 { get; set; }
        public decimal? Y13 { get; set; }
        public decimal? Y14 { get; set; }
        public decimal? Y15 { get; set; }
        public decimal? Y16 { get; set; }
        public decimal? Y17 { get; set; }
        public decimal? Y18 { get; set; }
        public decimal? Y19 { get; set; }
        public decimal? Y20 { get; set; }
        public decimal? Y21 { get; set; }
        public decimal? Y22 { get; set; }
        public decimal? Y23 { get; set; }
        public decimal? Y24 { get; set; }
        public decimal? Y25 { get; set; }
    }

    public class RPT_UpdateForecastApproved_Result
    {
        public int Result { get; set; }
    }
}
