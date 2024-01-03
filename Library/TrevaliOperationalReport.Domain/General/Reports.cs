using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Web;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_Report", Schema = "dbo")]
    public class Reports : BaseEntity
    {
        /// <summary>
        /// Get or set ReportId
        /// </summary>
        [Key]
        public int ReportId { get; set; }

        /// <summary>
        /// Get or set Name
        /// </summary>
        [Required]
        public string Name { get; set; }

    }


    public partial class EnumHelp
    {
        public enum Reports
        {
            OperationalReport = 2,
            AnalysisReport = 3

        }
    }


    public partial class RPT_WeeklyOperationalReport_Result
    {
        public int MetricId { get; set; }
        public string SectionName { get; set; }
        public string MetricsName { get; set; }
        public string Units { get; set; }
        public int Week { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public Nullable<Decimal> Budget { get; set; }
        public Nullable<Decimal> Forecast { get; set; }
        public Nullable<Decimal> ActualValue { get; set; }
        public Nullable<Decimal> WeeklyBudget { get; set; }
        public Nullable<Decimal> WeeklyForecast { get; set; }
        public Nullable<Decimal> MTDValue { get; set; }
        public Nullable<Decimal> MTDBudget { get; set; }
        public Nullable<Decimal> MTDForecast { get; set; }
        public Nullable<Decimal> YTDValue { get; set; }
        public Nullable<Decimal> YTDBudget { get; set; }
        public Nullable<Decimal> YTDForecast { get; set; }
        public string Comment { get; set; }

        public string CalculationType { get; set; }
    }

    public partial class RPT_GetSafety_Result
    {
        public string StartDay { get; set; }
        public string EndDay { get; set; }
        public byte[] Logo { get; set; }
        public int Actual { get; set; }
        public int Target { get; set; }
        public int Rating { get; set; }
        public int FirstAidInjury { get; set; }
        public int RestrictedWorkInjury { get; set; }
        public int LostTimeInjury { get; set; }
        public int NearHit { get; set; }
        public int EquipmentDamage { get; set; }
        public int BusinessImpact { get; set; }
        public int EnviroIncident { get; set; }
        public int MedicalCases { get; set; }
        public int MTD_FirstAidInjury { get; set; }
        public int MTD_RestrictedWorkInjury { get; set; }
        public int MTD_LostTimeInjury { get; set; }
        public int MTD_NearHit { get; set; }
        public int MTD_EquipmentDamage { get; set; }
        public int MTD_BusinessImpact { get; set; }
        public int MTD_EnviroIncident { get; set; }
        public int MTD_MedicalCases { get; set; }
        public int YTD_FirstAidInjury { get; set; }
        public int YTD_RestrictedWorkInjury { get; set; }
        public int YTD_LostTimeInjury { get; set; }
        public int YTD_NearHit { get; set; }
        public int YTD_EquipmentDamage { get; set; }
        public int YTD_BusinessImpact { get; set; }
        public int YTD_EnviroIncident { get; set; }
        public int YTD_MedicalCases { get; set; }
        public string SummaryOfInitiatives { get; set; }
        public string SummaryOfIncidents { get; set; }

    }
    public partial class RPT_WeeklyAnalysisReport_Result
    {
        public int MetricId { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public string MetricsName { get; set; }
        public int Week { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string YearYY { get; set; }
        public Nullable<Decimal> ActualValue { get; set; }
        public Nullable<Decimal> MTDValue { get; set; }
        public Nullable<Decimal> JanuaryForecast { get; set; }
        public Nullable<Decimal> FebruaryForecast { get; set; }
        public Nullable<Decimal> MarchForecast { get; set; }
        public Nullable<Decimal> AprilForecast { get; set; }
        public Nullable<Decimal> MayForecast { get; set; }
        public Nullable<Decimal> JuneForecast { get; set; }
        public Nullable<Decimal> JulyForecast { get; set; }
        public Nullable<Decimal> AugustForecast { get; set; }
        public Nullable<Decimal> SeptemberForecast { get; set; }
        public Nullable<Decimal> OctoberForecast { get; set; }
        public Nullable<Decimal> NovemberForecast { get; set; }
        public Nullable<Decimal> DecemberForecast { get; set; }


        public Nullable<Decimal> JanuaryActual { get; set; }
        public Nullable<Decimal> FebruaryActual { get; set; }
        public Nullable<Decimal> MarchActual { get; set; }
        public Nullable<Decimal> AprilActual { get; set; }
        public Nullable<Decimal> MayActual { get; set; }
        public Nullable<Decimal> JuneActual { get; set; }
        public Nullable<Decimal> JulyActual { get; set; }
        public Nullable<Decimal> AugustActual { get; set; }
        public Nullable<Decimal> SeptemberActual { get; set; }
        public Nullable<Decimal> OctoberActual { get; set; }
        public Nullable<Decimal> NovemberActual { get; set; }
        public Nullable<Decimal> DecemberActual { get; set; }
        public Nullable<Decimal> YTD { get; set; }
        public Nullable<Decimal> FullYearForecast { get; set; }
        public string Comment { get; set; }
    }
    public partial class RPT_GetSiteLogo_Result
    {
        public string StartDay { get; set; }
        public string EndDay { get; set; }
        public string SiteName { get; set; }
        public byte[] Logo { get; set; }

    }

    public partial class RPT_DailyAnalysisReport_Result
    {
        public long? DailyShiftOperationalDataId { get; set; }
        public string SectionName { get; set; }
        public DateTime Date { get; set; }
        public string MetricsName { get; set; }
        public int MetricId { get; set; }
        public string EquipmentName { get; set; }
        public int? EquipmentId { get; set; }
        public string ShiftName { get; set; }
        public int? ShiftId { get; set; }
        public Nullable<Decimal> ActualValue { get; set; }

        public Nullable<Decimal> ActualValueDaily { get; set; }
        public Nullable<Decimal> ActualValueMonthly { get; set; }
        public Nullable<Decimal> Budget { get; set; }
        public Nullable<Decimal> Forecast { get; set; }
        public Nullable<Decimal> Actual { get; set; }
        public Nullable<Decimal> DailyBudget { get; set; }
        public Nullable<Decimal> DailyForecast { get; set; }
        public Nullable<Decimal> DailyActual { get; set; }

        public Nullable<int> DisplayOrder { get; set; }
    }

    public class ReportParams
    {
        private int _year;
        private int _month;
        private int _day;
        private int _week;
        private string _weekSelect;
        private DateTime _dateSelect;

        public ReportParams()
        {
            if (_dateSelect == DateTime.MinValue)
            {
                _dateSelect = DateTime.Now;
                _year = _dateSelect.Year;
                _month = _dateSelect.Month;
                _day = _dateSelect.Day;
            }
        }
        public int SiteId { get; set; }
        public int Week {
            get
            {
                return _week;
            }
            set
            {
                _week = value;
            }
        }
        public int Month
        {
            get
            {
                return _month;
            }
            set
            {
                _month = value;
            }
        }
        public int Year {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
            }
        }
        public int Day { get; set; }
        public DateTime Date
        {
            get
            {
                return _dateSelect;
            }
            set
            {
                if ((DateTime)value != DateTime.MinValue)
                {
                    _year = value.Year;
                    _month = value.Month;
                    _day = value.Day;
                }
                _dateSelect = value;
            }
        }
        public int ReportType { get; set; }
        public string ReportTypeName { get; set; }
        public List<string> Sections { get; set; }
        public bool IsConsolidated { get; set; }
        public bool IsExecutiveSummary { get; set; }
        public bool IsMetalProduction { get; set; }
        public string SectionString
        {
            get
            {
                return Sections == null || Sections.Count == 0? "": string.Join(",", Sections);
            }
        }
        public string WeekSelect
        { set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _year = int.Parse(value.Substring(0, 4));
                    _week = int.Parse(value.Substring(6, 2));
                }
                _weekSelect = value;
            }
            get
            {
                return _weekSelect;
            }
        }
        public string MonthSelect
        {
            get
            {
                return _year.ToString() + "-" + _month.ToString();
            }
            set
            {
                //2022-02
                _month = int.Parse(value.ToString().Split('-')[1]);
                _year = int.Parse(value.ToString().Split('-')[0]);
                _week = CalculateWeek();
            }
        }

        private int CalculateWeek()
        {
            DateTime dt = new DateTime(_year, _month, 1);
            Calendar cal = new CultureInfo("en-US").Calendar;
            return cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

    }

    public partial class RPT_MonthlyMetalProdTracking_Result
    {
        public string MetricsName { get; set; }
        public Nullable<Decimal> Jan { get; set; }
        public Nullable<Decimal> Feb { get; set; }
        public Nullable<Decimal> Mar { get; set; }
        public Nullable<Decimal> Apr { get; set; }
        public Nullable<Decimal> May { get; set; }
        public Nullable<Decimal> Jun { get; set; }
        public Nullable<Decimal> Jul { get; set; }
        public Nullable<Decimal> Aug { get; set; }
        public Nullable<Decimal> Sep { get; set; }
        public Nullable<Decimal> Oct { get; set; }
        public Nullable<Decimal> Nov { get; set; }
        public Nullable<Decimal> Dec { get; set; }

    }


    public class RPT_MonthlyActualReport_Result
    {
        public int SiteMetricId { get; set; } //(int, not null)
        public int SectionId { get; set; } //(int, not null)
        public string SectionName { get; set; } //(varchar(100), not null)
        public string SectionMappingName { get; set; } //(varchar(100), not null)
        public string MetricsName { get; set; } //(varchar(100), not null)
        public string UOM { get; set; } //(varchar(10), not null)
        public int Year { get; set; } //(int, null)
        public Nullable<Decimal> January { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> February { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> March { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> April { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> May { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> June { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> July { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> August { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> September { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> October { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> November { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> December { get; set; } //(decimal(12,4), null)
        public Nullable<Decimal> MTD { get; set; } //(decimal(12,4), not null)
        public Nullable<Decimal> YTD { get; set; } //(decimal(16,4), not null)
    }

    public class RPT_MonthlyDetailReport_Result
    {
        public string Site { get; set; } //(varchar(200), null)
        public int SiteMetricId { get; set; } //(int, not null)
        public int SectionId { get; set; } //(int, not null)
        public int Month { get; set; } //(int, null)
        public string MonthName { get; set; } //(int, null)
        public int Year { get; set; } //(int, null)
        public string SectionName { get; set; } //(varchar(100), not null)
        public string SectionMappingName { get; set; } //(varchar(100), not null)
        public string MetricsName { get; set; } //(varchar(100), not null)
        public string UOM { get; set; } //(varchar(10), not null)
        public Nullable<Decimal> Actual { get; set; } //(decimal(12,4), not null)
        public Nullable<Decimal> Budget { get; set; } //(decimal(12,4), not null)
        public Nullable<Decimal> Forecast { get; set; } //(decimal(12,4), not null)
    }

    public class RPT_DailyOperationalReport_Result
    {
        public string Site { get; set; }
        public int MetricId { get; set; }
        public string SectionName { get; set; }
        public string MetricsName { get; set; }
        public string Units { get; set; }
        public DateTime Date { get; set; }
        public Nullable<Decimal> Actual { get; set; }
        public Nullable<Decimal> Forecast { get; set; }
        public Nullable<Decimal> MTDActual { get; set; }
        public Nullable<Decimal> MTDBudget { get; set; }
        public Nullable<Decimal> MTDForcast { get; set; }
        public Nullable<Decimal> YTDActual { get; set; }
        public Nullable<Decimal> YTDForecast { get; set; }
        public string Comment { get; set; }
    }
}

