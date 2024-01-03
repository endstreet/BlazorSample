using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Domain.Report
{
    [Table("RPT_DailyOperationalData", Schema = "dbo")]
    public class DailyOperationalData : BaseEntity
    {
        /// <summary>
        /// Get or set DailyOperationalDataId
        /// </summary>
        [Key]
        public long DailyOperationalDataId { get; set; }

        /// <summary>
        /// Get or set DailyOperationalDataId
        /// </summary>
        public int SiteMetricId { get; set; }

        /// <summary>
        /// Get or set Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Get or set ActualValue
        /// </summary>
        public decimal? ActualValue { get; set; }

        [ForeignKey("SiteMetricId")]
        public virtual General.SiteMetrics SiteMetrics { get; set; }
    }




    public class DailyMetricsData
    {

        public long DailyOperationalDataId { get; set; }
        public int SiteMetricId { get; set; }
        public int ShiftId { get; set; }
        public int EquipmentId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yyyy}")]
        public DateTime Date { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }

        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        //[RegularExpression(@"^[0-9]\d{0,9}(\.\d{1,3})?%?$", ErrorMessage = "Must be a positive number.")]
        public decimal? ActualValueDaily { get; set; }
        public string CommentDaily { get; set; }
        public Metrics Metric { get; set; }

        public string MetricName { get; set; }
        public bool IsDailyDataAvailable { get; set; }

    }

    public class DailyShiftMetricsData
    {
        public long DailyShiftOperationalDataId { get; set; }
        public long DailyOperationalDataId { get; set; }
        public int SiteMetricId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int? ShiftId { get; set; }
        public int? EquipmentId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yyyy}")]
        public DateTime Date { get; set; }
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        public decimal? ActualValueShift { get; set; }
        public Metrics Metric { get; set; }
        public string UOM { get; set; }
        public Shift Shift { get; set; }
        public string ShiftName { get; set; }
        public Equipment Equipment { get; set; }
        public string EquipmentName { get; set; }

    }


}
