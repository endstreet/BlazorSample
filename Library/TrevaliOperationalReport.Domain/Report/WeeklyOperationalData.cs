using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Domain.Report
{
    [Table("RPT_WeeklyOperationalData", Schema = "dbo")]
    public class WeeklyOperationalData : BaseEntity
    {
        /// <summary>
        /// Get or set WeeklyOperationalDataId
        /// </summary>
        public long WeeklyOperationalDataId { get; set; }

        /// <summary>
        /// get or set SiteMetricId
        /// </summary>
        public int SiteMetricId { get; set; }

        [ForeignKey("SiteMetricId")]
        public virtual General.SiteMetrics SiteMetrics { get; set; }

        /// <summary>
        /// Get or set Year
        /// </summary>
        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        /// <summary>
        /// Get or set Week
        /// </summary>
        public int Week
        {
            get { return _week; }
            set { _week = value; }
        }

        /// <summary>
        /// Get or set ActualValue
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        [Required(ErrorMessage = "*")]
        //[RegularExpression(@"^[0-9]\d{0,9}(\.\d{1,3})?%?$", ErrorMessage = "Must be a positive number.")]
        public decimal? ActualValue { get; set; }

        /// <summary>
        /// Get or set MTDValue
        /// </summary>
        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        //[Required(ErrorMessage = "*")]
        //[RegularExpression(@"^[0-9]\d{0,9}(\.\d{1,3})?%?$", ErrorMessage = "Must be a positive number.")]
        public decimal? MTDValue { get; set; }


        /// <summary>
        /// Get or set Comment
        /// </summary>
        [StringLength(1000, ErrorMessage = "Comment must be within 500 characters.")]
        public string Comment { get; set; }

        [NotMapped]
        public Site Site { get; set; }

        [NotMapped]
        public TrevaliOperationalReport.Domain.General.Reports Report { get; set; }

        [NotMapped]
        public List<WeeklyMetricsData> WeeklyMetricsData { get; set; }
        [NotMapped]
        public string SiteName { get; set; }
        [NotMapped]
        public string MetricName { get; set; }
        [NotMapped]
        public string ReportName { get; set; }
        [NotMapped]
        public string SectionName { get; set; }
        //[NotMapped]
        //public string WeekString { get; set; }
        [NotMapped]
        public string YearString
        {
            get { return _year.ToString(); }
            set { _year = int.Parse(value); }
        }

        [NotMapped]
        public int SiteId { get; set; }
        [NotMapped]
        public List<SelectListItem> SearchWeeks { get; set; }

        private int _year;
        //private int _month;
        //private int _day;
        private int _week;
        private string _weekSelect;
        private DateTime _dateSelect;

        [NotMapped]
        //    public string WeekString
        //    {
        //        set
        //        {
        //            if (!string.IsNullOrEmpty(value))
        //            {
        //                _year = int.Parse(value.Substring(0, 4));
        //                _week = int.Parse(value.Substring(6, 2));
        //            }
        //            _weekSelect = value;
        //        }
        //        get
        //        {
        //            return _weekSelect;
        //        }
        //    }

        public string WeekString
        {
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length >= 8)
                {
                    if (int.TryParse(value.Substring(0, 4), out int year))
                    {
                        _year = year;
                    }
                    else
                    {
                        // Handle parsing error for year, e.g., set to a default value or log the issue.
                        _year = 0;
                    }

                    if (int.TryParse(value.Substring(6, 2), out int week))
                    {
                        _week = week;
                    }
                    else
                    {
                        // Handle parsing error for week, e.g., set to a default value or log the issue.
                        _week = 0;
                    }
                }
                _weekSelect = value;
            }
            get
            {
                return _weekSelect;
            }
        }

    }
        public class InvalidWeeklyData
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
        /// Get or set Year
        /// </summary>
        public String Year { get; set; }

        /// <summary>
        /// Get or set Week
        /// </summary>
        public String Week { get; set; }

        /// <summary>
        /// Get or set Budget
        /// </summary>
        public decimal? ActualValue { get; set; }

        /// <summary>
        /// Get or set MTD Value
        /// </summary>
        public decimal? MTDValue { get; set; }


        /// <summary>
        /// Get or set YTD Value
        /// </summary>
        public decimal? YTDValue { get; set; }

        public string Remarks { get; set; }
    }

    //public class WeeklyDataModel
    //{
    //    public int SiteId { get; set; }
    //    public int Week { get; set; }
    //    public int Year { get; set; }

    //    public List<WeeklySections> Sections { get; set; }

    //}

    //public class WeeklySections {

    //    public string SectionName {get;set;}
    //    public List<WeeklyMetrics> Metrics { get; set; }
    //}
    //public class WeeklyMetrics
    //{
    //    public string Name { get; set; }
    //    public int WeeklyOperationalDataId { get; set; }
    //    public int UnitId { get; set; }
    //    public string UnitName { get; set; }
    //    public decimal ActualValue { get; set; }
    //    public decimal BudgetValue { get; set; }
    //    public decimal PlanValue { get; set; }
    //    public string Comment { get; set; }



    //}

    public class WeeklyMetricsData
    {

        public long WeeklyOperationalDataId { get; set; }
        //public int MetricId { get; set; }
        public int SiteId { get; set; }
        //public int ReportId { get; set; }

        public int SiteMetricId { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }

        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        //[RegularExpression(@"^[0-9]\d{0,9}(\.\d{1,3})?%?$", ErrorMessage = "Must be a positive number.")]
        public decimal? ActualValue { get; set; }

        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        //[RegularExpression(@"^[0-9]\d{0,9}(\.\d{1,3})?%?$", ErrorMessage = "Must be a positive number.")]
        public decimal? MTDValue { get; set; }

        [Range(0, 99999999, ErrorMessage = "Must be between 0-99999999")]
        //[RegularExpression(@"^[0-9]\d{0,9}(\.\d{1,3})?%?$", ErrorMessage = "Must be a positive number.")]
        public decimal? YTDValue { get; set; }

        public decimal WeeklyBudget { get; set; }
        public decimal WeeklyForecast { get; set; }

        public decimal MTD { get; set; }

        [StringLength(1000, ErrorMessage = "Comment must be within 500 characters.")]
        public string Comment { get; set; }
        [ForeignKey("MetricId")]
        public Metrics Metric { get; set; }

        public bool IsDefault { get; set; }

    }


    public partial class RPT_WeeklyData_Result
    {
        public long WeeklyOperationalDataId { get; set; }
        public int SiteMetricId { get; set; }
        public int SectionId { get; set; }
        public int MetricId { get; set; }
        public string SectionName { get; set; }
        public string MetricsName { get; set; }
        public string Units { get; set; }
        public int Week { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public Nullable<Decimal> ActualValue { get; set; }
        public Nullable<Decimal> WeeklyBudget { get; set; }
        public Nullable<Decimal> WeeklyForecast { get; set; }
        public Nullable<Decimal> MTDValue { get; set; }
        public Nullable<Decimal> YTDValue { get; set; }
        public string Comment { get; set; }
    }


}
