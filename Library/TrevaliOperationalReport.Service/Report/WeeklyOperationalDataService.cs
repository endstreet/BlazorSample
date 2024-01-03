using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;
using TrevaliOperationalReport.Domain.Report;
using TrevaliOperationalReport.Service.General;

namespace TrevaliOperationalReport.Service.Report
{
    public class WeeklyOperationalDataService : IWeeklyOperationalDataService
    {
        #region Fields

        private readonly IRepository<WeeklyOperationalData> _weeklyOperationalDataRepository;
        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<Metrics> _metricsRepository;
        private readonly IRepository<Reports> _reportRepository;
        private readonly IRepository<DailyOperationalData> _dailyOperationalDataRepository;
        private readonly IRepository<WeeklyOperationalDataApprove> _weeklyDataApproveRepository;
        public static TrevaliOperationalReportObjectContext _dbContext;
        //private readonly IRepository<Users> _userRepository;

        #endregion

        #region Ctor

        public WeeklyOperationalDataService(IRepository<WeeklyOperationalData> weeklyOperationalDataRepository, IRepository<Site> siteRepository,
            IRepository<Metrics> metricsRepository, IRepository<Reports> reportRepository, IRepository<DailyOperationalData> dailyOperationalDataRepository,
            IRepository<WeeklyOperationalDataApprove> weeklyDataApproveRepository)
        {
            _weeklyOperationalDataRepository = weeklyOperationalDataRepository;
            _siteRepository = siteRepository;
            _metricsRepository = metricsRepository;
            _reportRepository = reportRepository;
            _dailyOperationalDataRepository = dailyOperationalDataRepository;
            _weeklyDataApproveRepository = weeklyDataApproveRepository;
            _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
        }

        #endregion

        #region Methods


        #region Weekly Operational Data


        /// <summary>
        /// Get Weekly data metrics 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="ReportId"></param>
        /// <param name="Week"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        public IList<WeeklyMetricsData> GetWeeklyOperationalDataMetrics(int SiteId, int ReportId, int Week, int Year)
        {
            //var weeklydata = SearchWeeklyOperationalData(SiteId, ReportId, 0, Week, Year).FirstOrDefault();


            var WeeklyMetricsData = new List<WeeklyMetricsData>();

            object[] xparams = {
                            new SqlParameter("SiteId", SiteId),
                            new SqlParameter("ReportId", ReportId),
                              new SqlParameter("Week", Week),
                                new SqlParameter("Year", Year),
                         };

            var WeeklyGridData = _dbContext.ExecuteStoredProcedureList<RPT_WeeklyData_Result>("RPT_WeeklyData", xparams);
            WeeklyGridData = WeeklyGridData.ToList().OrderBy(z => z.SectionId).ToList();
            if (WeeklyGridData != null && WeeklyGridData.Count > 0)
            {

                foreach (RPT_WeeklyData_Result result in WeeklyGridData)
                {
                    if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == result.SectionId))
                    {


                        var objmetric = (from p in _dbContext.SiteMetrics
                                         where p.SiteMetricId == result.SiteMetricId
                                         select
                                             p.Metric
                                      ).FirstOrDefault();
                        if (result.WeeklyOperationalDataId != 0)
                        {
                            WeeklyMetricsData.Add(new WeeklyMetricsData
                            {
                                WeeklyOperationalDataId = result.WeeklyOperationalDataId,
                                SiteMetricId = result.SiteMetricId,
                                Week = Week,
                                Year = Year,
                                SiteId = SiteId,
                                SectionId = result.SectionId,
                                SectionName = result.SectionName,
                                Metric = objmetric,
                                Comment = result.Comment ?? "",
                                ActualValue = CommonHelper.SetRounding(result.ActualValue ?? 0, objmetric.Unit.UOM.Trim()),
                                MTDValue = CommonHelper.SetRounding(result.MTDValue ?? 0, objmetric.Unit.UOM.Trim()),
                                YTDValue = CommonHelper.SetRounding(result.YTDValue ?? 0, objmetric.Unit.UOM.Trim()),
                                WeeklyBudget = decimal.Round(result.WeeklyBudget ?? 0, 0, MidpointRounding.AwayFromZero),
                                WeeklyForecast = decimal.Round(result.WeeklyForecast ?? 0, 0, MidpointRounding.AwayFromZero),
                                IsDefault = false

                            });
                        }
                        else
                        {
                            long r = 0;
                            do
                            {
                                Random generator = new Random();
                                r = generator.Next(0, 999999);
                            }
                            while (WeeklyMetricsData.Any(x => x.WeeklyOperationalDataId == r) != false);

                            WeeklyMetricsData.Add(new WeeklyMetricsData
                            {

                                WeeklyOperationalDataId = r,
                                SiteMetricId = result.SiteMetricId,
                                SiteId = SiteId,
                                Week = Week,
                                Year = Year,
                                SectionId = result.SectionId,
                                SectionName = result.SectionName,
                                Metric = objmetric,
                                Comment = result.Comment ?? "",
                                ActualValue = CommonHelper.SetRounding(result.ActualValue ?? 0, objmetric.Unit.UOM.Trim()),
                                MTDValue = CommonHelper.SetRounding(result.MTDValue ?? 0, objmetric.Unit.UOM.Trim()),
                                YTDValue = CommonHelper.SetRounding(result.YTDValue ?? 0, objmetric.Unit.UOM.Trim()),
                                WeeklyBudget = decimal.Round(result.WeeklyBudget ?? 0, 0, MidpointRounding.AwayFromZero),
                                WeeklyForecast = decimal.Round(result.WeeklyForecast ?? 0, 0, MidpointRounding.AwayFromZero),
                                IsDefault = true
                                // MTD = decimal.Round(MTDVal, 4, MidpointRounding.AwayFromZero),

                            });
                        }

                    }

                }
            }

            return WeeklyMetricsData;
        }




        /// <summary>
        /// search the Weekly Operational Data .
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>WeeklyOperationalData.</returns>
        public IList<WeeklyOperationalData> SearchWeeklyOperationalData(int siteId, int reportId, int metricId, int week, int year)
        {
            var query = (from w in _dbContext.WeeklyOperationalData
                         join SM in _dbContext.SiteMetrics on
                         w.SiteMetricId equals SM.SiteMetricId
                         where (SM.SiteId == siteId || siteId == 0) &&
                         (SM.MetricId == metricId || metricId == 0) &&
                           (SM.ReportId == reportId || reportId == 0) &&
                           (w.Week == week || week == 0) &&
                         (w.Year == year || year == 0)
                         select new
                         {
                             //SiteMetricId = SM.SiteMetricId,
                             SiteId = SM.SiteId,
                             Site = SM.Site,
                             ReportId = SM.ReportId,
                             Report = SM.Report,
                             Week = w.Week,
                             Year = w.Year
                         }).Distinct().ToList().Select(s => new WeeklyOperationalData
                         {
                             //SiteMetricId = s.SiteMetricId,
                             Site = s.Site,
                             Report = s.Report,
                             Week = s.Week,
                             Year = s.Year,
                         });
            return query.ToList();
        }



        /// <summary>
        /// Uploads weekly data
        /// </summary>
        /// <param name="lstMontlhyPlanData"></param>
        /// <returns></returns>
        public List<InvalidWeeklyData> UploadWeeklyOperationalData(List<WeeklyOperationalData> lstWeeklyData, int reportId)
        {
            var InvalidCount = 0;
            List<InvalidWeeklyData> lstInvalid = new List<InvalidWeeklyData>();
            var sitename = lstWeeklyData[0].SiteName;
            var site = from p in _dbContext.Site where p.SiteName == sitename select p;
            int siteId = site.FirstOrDefault().SiteId;


            var sites = GetSitesSelectList(0, -1, true);
            var metrics = GetMetricsSelectList(siteId, reportId);
            var reports = GetReportsSelectList();
            //var comments = GetCommentsSelectList();
            if (lstWeeklyData != null && lstWeeklyData.Count > 0)
            {
                foreach (WeeklyOperationalData weeklydata in lstWeeklyData)
                {
                    if (!sites.Any(x => x.Text == weeklydata.SiteName))
                    {
                        lstInvalid.Add(new InvalidWeeklyData
                        {
                            SiteName = weeklydata.SiteName,
                            SectionName = weeklydata.SectionName,
                            ReportName = weeklydata.ReportName,
                            MetricName = weeklydata.MetricName,
                            Year = weeklydata.YearString,
                            Week = weeklydata.WeekString,
                            ActualValue = weeklydata.ActualValue,
                            Remarks = "Invalid site OR permission denied for site"
                        });
                        InvalidCount++;
                        continue;

                    }
                    //else if (!reports.Any(x => x.Value == weeklydata.ReportName))
                    //{

                    //    lstInvalid.Add(new InvalidWeeklyData
                    //    {
                    //        SiteName = weeklydata.SiteName,
                    //        ReportName = weeklydata.ReportName,
                    //        MetricName = weeklydata.MetricName,
                    //        Year = weeklydata.Year,
                    //        Week = weeklydata.Week,
                    //        ActualValue = weeklydata.ActualValue,
                    //        Remarks = "Invalid Report"
                    //    });
                    //    InvalidCount++;
                    //    continue;
                    //}
                    else if (!metrics.Any(x => x.Text == weeklydata.SectionName + "^" + weeklydata.MetricName))
                    {

                        lstInvalid.Add(new InvalidWeeklyData
                        {
                            SiteName = weeklydata.SiteName,
                            SectionName = weeklydata.SectionName,
                            ReportName = weeklydata.ReportName,
                            MetricName = weeklydata.MetricName,
                            Year = weeklydata.YearString,
                            Week = weeklydata.WeekString,
                            ActualValue = weeklydata.ActualValue,
                            Remarks = "Invalid Metric/Section name OR Permission denied for section"
                        });
                        InvalidCount++;
                        continue;
                    }
                    //else if (!string.IsNullOrEmpty(weeklydata.CommentString) && !comments.Any(x => x.Text == weeklydata.CommentString))
                    //{

                    //    lstInvalid.Add(new InvalidWeeklyData
                    //    {
                    //        SiteName = weeklydata.SiteName,
                    //        ReportName = weeklydata.ReportName,
                    //        MetricName = weeklydata.MetricName,
                    //        Year = weeklydata.Year,
                    //        Week = weeklydata.Week,
                    //        ActualValue = weeklydata.ActualValue,
                    //        Remarks = "Invalid Comment"
                    //    });
                    //    InvalidCount++;
                    //    continue;
                    //}
                    else if (weeklydata.ActualValue < 0)
                    {
                        lstInvalid.Add(new InvalidWeeklyData
                        {
                            SiteName = weeklydata.SiteName,
                            SectionName = weeklydata.SectionName,
                            ReportName = weeklydata.ReportName,
                            MetricName = weeklydata.MetricName,
                            Year = weeklydata.YearString,
                            Week = weeklydata.WeekString,
                            ActualValue = weeklydata.ActualValue,
                            Remarks = "Actual value must be positive number"
                        });
                        InvalidCount++;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(weeklydata.YearString))
                    //else if ((weeklydata.Year < DateTime.Now.Year - 10) || (weeklydata.Year > DateTime.Now.Year + 10))
                    {
                        lstInvalid.Add(new InvalidWeeklyData
                        {
                            SiteName = weeklydata.SiteName,
                            SectionName = weeklydata.SectionName,
                            ReportName = weeklydata.ReportName,
                            MetricName = weeklydata.MetricName,
                            Year = weeklydata.YearString,
                            Week = weeklydata.WeekString,
                            ActualValue = weeklydata.ActualValue,
                            Remarks = "Year value is missing."
                            //Remarks = "Year must be between " + (DateTime.Now.Year - 10).ToString() + " and " + (DateTime.Now.Year + 10).ToString()
                        });
                        InvalidCount++;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(weeklydata.WeekString))
                    //else if (weeklydata.Week < 1 || weeklydata.Week >= 52)
                    {
                        lstInvalid.Add(new InvalidWeeklyData
                        {
                            SiteName = weeklydata.SiteName,
                            SectionName = weeklydata.SectionName,
                            ReportName = weeklydata.ReportName,
                            MetricName = weeklydata.MetricName,
                            Year = weeklydata.YearString,
                            Week = weeklydata.WeekString,
                            ActualValue = weeklydata.ActualValue,
                            Remarks = "Week value is missing."
                        });
                        InvalidCount++;
                        continue;
                    }



                    if (!string.IsNullOrEmpty(weeklydata.YearString))
                    {
                        try
                        {

                            weeklydata.Year = Convert.ToInt32(weeklydata.YearString);
                            if (((weeklydata.Year < DateTime.Now.Year - 10) || (weeklydata.Year > DateTime.Now.Year + 21)))
                            {
                                lstInvalid.Add(new InvalidWeeklyData
                                {
                                    SiteName = weeklydata.SiteName,
                                    SectionName = weeklydata.SectionName,
                                    ReportName = weeklydata.ReportName,
                                    MetricName = weeklydata.MetricName,
                                    Year = weeklydata.YearString,
                                    Week = weeklydata.WeekString,
                                    ActualValue = weeklydata.ActualValue,
                                    Remarks = "Year must be between " + (DateTime.Now.Year - 10).ToString() + " and " + (DateTime.Now.Year + 21).ToString()
                                });
                                InvalidCount++;
                                continue;
                            }
                        }
                        catch
                        {
                            lstInvalid.Add(new InvalidWeeklyData
                            {
                                SiteName = weeklydata.SiteName,
                                SectionName = weeklydata.SectionName,
                                ReportName = weeklydata.ReportName,
                                MetricName = weeklydata.MetricName,
                                Year = weeklydata.YearString,
                                Week = weeklydata.WeekString,
                                ActualValue = weeklydata.ActualValue,
                                Remarks = "Invalid Year value."
                            });
                            InvalidCount++;
                            continue;
                        }
                    }



                    if (!string.IsNullOrEmpty(weeklydata.WeekString))
                    {
                        try
                        {


                            weeklydata.Week = Convert.ToInt32(weeklydata.WeekString);
                            if (weeklydata.Week < 0 || weeklydata.Week > 52)
                            {
                                lstInvalid.Add(new InvalidWeeklyData
                                {
                                    SiteName = weeklydata.SiteName,
                                    SectionName = weeklydata.SectionName,
                                    ReportName = weeklydata.ReportName,
                                    MetricName = weeklydata.MetricName,
                                    Year = weeklydata.YearString,
                                    Week = weeklydata.WeekString,
                                    ActualValue = weeklydata.ActualValue,
                                    Remarks = "Week must be between 1-52."
                                });
                                InvalidCount++;
                                continue;
                            }
                        }
                        catch
                        {
                            lstInvalid.Add(new InvalidWeeklyData
                            {
                                SiteName = weeklydata.SiteName,
                                SectionName = weeklydata.SectionName,
                                ReportName = weeklydata.ReportName,
                                MetricName = weeklydata.MetricName,
                                Year = weeklydata.YearString,
                                Week = weeklydata.WeekString,
                                ActualValue = weeklydata.ActualValue,
                                Remarks = "Invalid Week value."
                            });
                            InvalidCount++;
                            continue;
                        }
                    }


                    if (metrics.Any(x => x.Text == weeklydata.SectionName + "^" + weeklydata.MetricName))
                    {
                        weeklydata.SiteMetricId = Convert.ToInt32(metrics.Where(x => x.Text == weeklydata.SectionName + "^" + weeklydata.MetricName).FirstOrDefault().Value);
                    }
                    else
                    {
                        lstInvalid.Add(new InvalidWeeklyData
                        {
                            SiteName = weeklydata.SiteName,
                            SectionName = weeklydata.SectionName,
                            ReportName = weeklydata.ReportName,
                            MetricName = weeklydata.MetricName,
                            Year = weeklydata.YearString,
                            Week = weeklydata.WeekString,
                            ActualValue = weeklydata.ActualValue,
                            Remarks = "Metric is not assigned to any section."
                        });
                        InvalidCount++;
                        continue;
                    }
                    //weeklydata.MetricId = Convert.ToInt32(metrics.Where(x => x.Text == weeklydata.MetricName).FirstOrDefault().Value);
                    //weeklydata.SiteId = Convert.ToInt32(sites.Where(x => x.Text == weeklydata.SiteName).FirstOrDefault().Value);
                    //weeklydata.ReportId = Convert.ToInt32(weeklydata.ReportName);// Convert.ToInt32(reports.Where(x => x.Value == weeklydata.ReportName).FirstOrDefault().Value);


                    if (!string.IsNullOrEmpty(weeklydata.Comment))
                    {
                        weeklydata.Comment = weeklydata.Comment;
                    }
                    weeklydata.CreatedBy = ProjectSession.UserID;
                    weeklydata.CreatedDate = DateTime.Now;

                    var model = (from x in _weeklyOperationalDataRepository.Table
                                 where x.SiteMetricId == weeklydata.SiteMetricId &&
                                 x.Week == weeklydata.Week &&
                                 x.Year == weeklydata.Year
                                 select x).FirstOrDefault();
                    if (model != null)
                    {

                        if (!string.IsNullOrEmpty(weeklydata.Comment))
                            model.Comment = weeklydata.Comment;

                        model.ActualValue = weeklydata.ActualValue ?? 0;
                        model.MTDValue = weeklydata.MTDValue ?? 0;

                        model.ModifiedBy = ProjectSession.UserID;
                        model.ModifiedDate = DateTime.Now;
                        _weeklyOperationalDataRepository.Update(model);
                    }
                    else
                    {
                        _weeklyOperationalDataRepository.Insert(weeklydata);
                    }
                }
            }
            return lstInvalid;

        }


        public decimal GetYTDFiguresCalculatedForWeek(int week, int year, int sitemetricId, ref bool IsPercentage)
        {
            try
            {

                var LastDayOfWeek = CommonHelper.FirstDateOfWeek(year, week).AddDays(7);

                var UOM = (from SM in _dbContext.SiteMetrics
                           join M in _dbContext.Metrics on SM.MetricId equals M.MetricId
                           join U in _dbContext.Unit on M.UnitId equals U.UnitId
                           where SM.SiteMetricId == sitemetricId
                           select U.UOM).FirstOrDefault();
                if (UOM.Trim().ToUpper() == "%" || UOM.Trim().ToUpper() == "TPH" || UOM.Trim() == "troy oz/t")
                {
                    IsPercentage = true;
                    var returnVal = (from x in _dbContext.MonthlyBudgetPlanData
                                     where x.SiteMetricId == sitemetricId &&
                                     x.Month < LastDayOfWeek.Month &&
                                     x.Year == year
                                     select x.Actual.Value).ToList();
                    if (returnVal != null && returnVal.Count > 0)
                        return returnVal.Average();
                    else
                        return 0;


                }
                else
                {
                    return (from x in _dbContext.MonthlyBudgetPlanData
                            where x.SiteMetricId == sitemetricId &&
                            x.Month < LastDayOfWeek.Month &&
                            x.Year == year
                            select x.Actual.Value).ToList().Sum();
                }
            }
            catch (Exception e)
            {
                return 0;
            }



        }


        /// <summary>
        /// Gets sites  selectlist.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetSitesSelectList(int siteId = 0, int IsSync = -1, bool IsFullRightsRequired = false)
        {
            bool BoolIsSync = false;
            if (IsSync != -1)
                BoolIsSync = Convert.ToBoolean(IsSync);

            List<int> SiteIds = new List<int>();
            if (IsFullRightsRequired)
                SiteIds = ProjectSession.UserSiteRightsDynamic.Where(x => x.IsFullRights == true).Select(x => x.SiteId).ToList();
            else
                SiteIds = ProjectSession.UserSiteRightsDynamic.Where(x => x.IsFullRights == true || x.IsView == true).Select(x => x.SiteId).ToList();



            if (siteId == 0)
            {
                var query = from p in _siteRepository.Table
                            where ((p.IsSync == BoolIsSync || (p.IsSync == null && !BoolIsSync)) || IsSync == -1)
                            && (SiteIds.Contains(p.SiteId) || ProjectSession.IsAdmin == true)
                            orderby p.SiteName ascending
                            select new SelectListItem
                            {
                                Text = p.SiteName,
                                Value = p.SiteId.ToString()
                            };
                return query.ToList();
            }
            else
            {
                var query = from p in _siteRepository.Table
                            where p.SiteId == siteId &&
                            ((p.IsSync == BoolIsSync || (p.IsSync == null && !BoolIsSync)) || IsSync == -1)
                             && (SiteIds.Contains(p.SiteId) || ProjectSession.IsAdmin == true)
                            orderby p.SiteName ascending
                            select new SelectListItem
                            {
                                Text = p.SiteName,
                                Value = p.SiteId.ToString()
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// Gets metrics  selectlist.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetMetricsSelectList(int siteId = 0, int ReportId = 0)
        {
            if (siteId > 0)
            {

                var list = new List<SelectListItem>();
                var metrics = (from p in _dbContext.SiteMetrics
                               where p.SiteId == siteId
                               && (p.ReportId == ReportId || ReportId == 0)
                               && p.IsWeekly == true
                               select p).OrderBy(a => a.SectionId).ToList();

                if (metrics != null && metrics.Count() > 0)
                {
                    metrics = metrics.AsEnumerable().OrderBy(x => x.DisplayOrder).OrderBy(a => a.SectionId).ToList();
                    List<string> weeklyMetricList = null;
                    if (siteId == 2)
                    {
                        weeklyMetricList = new List<string>(new string[]
                        {
                            "Zn Conc Stocks on Site", "Pb Conc Stocks on Site",
                            "Fe grade", "Cu grade", "Mg grade", "As Grade", "Sn grade",
                            "Sb grade", "Hg grade", "Cd grade", "Ag fines", "SiO2", "Ca", "S",
                            "Mn grade", "Zn Concentrate at Port", "Pb Concentrate at Port",
                            "Zn Concentrate Port", "Pb Concentrate Port","Mill Throughput", "Zn Concentrate at Port",
                            "Zn Concentrate Port", "Zn Concentrate In-transit"
                        });
                    }
                    else
                    {
                        weeklyMetricList = new List<string>(new string[]
                        {
                            "Zn Conc Stocks on Site", "Pb Conc Stocks on Site",
                            "Dev Ore", "Fe grade", "Cu grade", "Mg grade", "As Grade", "Sn grade",
                            "Sb grade", "Hg grade", "Cd grade", "Ag fines", "SiO2", "Ca", "S",
                            "Mn grade", "Zn Concentrate at Port", "Pb Concentrate at Port",
                            "Zn Concentrate Port", "Pb Concentrate Port", "Mill Throughput"
                        });
                    }

                    foreach (var itm in metrics)
                    {
                        if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == itm.SectionId))
                        {
                            var listItem = new SelectListItem { Text = itm.Section.SectionName + "^" + itm.Metric.MetricsName + " (" + itm.Metric.Unit.UOM + ")", Value = itm.SiteMetricId.ToString() };

                            // List of Weekly Metrics list.
                            if (weeklyMetricList.Contains(itm.MetricsName))
                            {
                                list.Add(listItem);
                            }
                        }
                    }


                    //var query = from p in _metricsRepository.Table
                    //            where metrics.Contains(p.MetricId)
                    //            orderby p.MetricsName ascending
                    //            select new SelectListItem
                    //            {
                    //                Text = p.MetricsName,
                    //                Value = p.MetricId.ToString()
                    //            };
                    //return query.ToList();
                    return list;
                }
                else
                {
                    return new List<SelectListItem>();
                }

            }
            else
            {
                var query = from p in _metricsRepository.Table
                            orderby p.MetricsName ascending
                            select new SelectListItem
                            {
                                Text = p.MetricsName,
                                Value = p.MetricId.ToString()
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// Gets reports  selectlist.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetReportsSelectList()
        {
            var query = from p in _reportRepository.Table
                        orderby p.Name ascending
                        select new SelectListItem
                        {
                            Text = p.Name,
                            Value = p.ReportId.ToString()
                        };
            return query.ToList();
        }



        /// <summary>
        /// Updates WeeklyData
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="ReportId"></param>
        /// <returns></returns>
        public void UpdateWeeklyData(WeeklyMetricsData WeeklyData)
        {
            var model = (from x in _weeklyOperationalDataRepository.Table
                         where x.SiteMetricId == WeeklyData.SiteMetricId &&
                         x.Week == WeeklyData.Week &&
                         x.Year == WeeklyData.Year
                         select x).FirstOrDefault();

            if (model != null)
            {
                if (!string.IsNullOrEmpty(WeeklyData.Comment))
                    model.Comment = WeeklyData.Comment;
                model.ActualValue = WeeklyData.ActualValue;
                model.MTDValue = WeeklyData.MTDValue;

                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
                _weeklyOperationalDataRepository.Update(model);

            }
        }
        /// <summary>
        /// Get ActualValue MtdValue Comments to map download
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="ReportId"></param>
        /// <returns></returns>
        public IList<WeeklyMetricsData> GetCommentsDownloadSample(WeeklyMetricsData weeklyMetricsData)
             
        {
            List<WeeklyMetricsData> weeklyDataMatrix = new List<WeeklyMetricsData>();

            var model = (from x in _weeklyOperationalDataRepository.Table.ToList()
                         where x.SiteId == weeklyMetricsData.SiteId &&
                         x.Week == weeklyMetricsData.Week &&
                         x.Year == weeklyMetricsData.Year
                         select x).FirstOrDefault();

            if (model != null)
            {

                weeklyDataMatrix.Add(new WeeklyMetricsData
                {

                    ActualValue = model.ActualValue,
                    MTDValue = model.MTDValue,
                    Comment = model.Comment

                });

            }

            return weeklyDataMatrix;

        }

        /// <summary>
        /// Get daily operational data list
        /// </summary>
        /// <param name="WeeklyOperationalDataId"></param>
        /// <returns></returns>
        public IList<DailyOperationalData> GetDailyOperationalDataList(int WeeklyOperationalDataId)
        {
            return (from p in _dailyOperationalDataRepository.Table
                        //where
                        //    p.WeeklyOperationalDataId == WeeklyOperationalDataId
                    select p).ToList();

        }

        /// <summary>
        /// get metrics list for dropdown list
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetMeticsList(int siteId, int reportId)
        {
            if (siteId > 0 && reportId > 0)
            {

                var list = new List<SelectListItem>();
                var metrics = (from p in _dbContext.SiteMetrics
                               where p.SiteId == siteId
                               && (p.ReportId == reportId || reportId == 0)
                               && p.IsWeekly == true
                               select p).OrderBy(a => a.SectionId).ToList();



                if (metrics != null && metrics.Count() > 0)
                {
                    List<string> weeklyMetricList = new List<string>(new string[]
                            {
                                "Zn Conc Stocks on Site", "Pb Conc Stocks on Site",
                                "Dev Ore", "Fe grade", "Cu grade","Mg grade", "As Grade", "Sn grade",
                                "Sb grade", "Hg grade", "Cd grade", "Ag fines", "SiO2", "Ca", "S",
                                "Mn grade", "Zn Concentrate at Port", "Pb Concentrate at Port",
                                "Zn Concentrate Port", "Pb Concentrate Port"
                            });

                    foreach (var itm in metrics)
                    {
                        if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == itm.SectionId))
                        {
                            var listitem = (from p in _metricsRepository.Table
                                            where p.MetricId == itm.MetricId
                                            select new SelectListItem
                                            {
                                                Text = p.MetricsName + "(" + p.Unit.UOM + ")" + " (" + itm.Section.SectionName + ")",
                                                Value = itm.SiteMetricId.ToString()
                                            }).ToList();

                            // Weekly metrics list.
                            if (weeklyMetricList.Contains(itm.MetricsName))
                            {
                                list.AddRange(listitem);
                            }
                        }
                    }

                    return list;
                }
                else
                {
                    return new List<SelectListItem>();
                }

            }
            else
            {
                var query = from p in _metricsRepository.Table
                            orderby p.MetricsName ascending
                            select new SelectListItem
                            {
                                Text = p.MetricsName,
                                Value = p.MetricId.ToString()
                            };
                return query.ToList();
            }

        }

        /// <summary>
        /// Insert weekly operational data
        /// </summary>
        /// <param name="modelWeekly"></param>
        /// <returns></returns>
        public long InsertWeeklyData(WeeklyOperationalData modelWeekly)
        {
            if (modelWeekly == null)
                throw new ArgumentNullException("WeeklyOperationalData");


            var model = (from x in _weeklyOperationalDataRepository.Table
                         where x.SiteMetricId == modelWeekly.SiteMetricId &&
                         x.Week == modelWeekly.Week &&
                         x.Year == modelWeekly.Year
                         select x).FirstOrDefault();


            if (model != null)
            {
                if (!string.IsNullOrEmpty(modelWeekly.Comment))
                    model.Comment = modelWeekly.Comment;


                model.ActualValue = modelWeekly.ActualValue ?? 0;
                model.MTDValue = modelWeekly.MTDValue ?? 0;

                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
                _weeklyOperationalDataRepository.Update(model);
                return model.WeeklyOperationalDataId;
            }
            else
            {
                modelWeekly.CreatedBy = ProjectSession.UserID;
                modelWeekly.CreatedDate = DateTime.Now;

                _weeklyOperationalDataRepository.Insert(modelWeekly);
                return modelWeekly.WeeklyOperationalDataId;
            }
        }


        public bool CheckExistingSectionsData(WeeklyOperationalData model)
        {
            int week = Convert.ToInt32(model.WeekString ?? "0");
            int year = Convert.ToInt32(model.YearString ?? "0");
            var data = (from p in _weeklyOperationalDataRepository.Table
                        where
                            p.Week == week && p.Year == year
                        select p).ToList();

            if (data != null && data.Count > 0)
            {
                if (data.Any(z => z.SiteMetrics.Section.SectionName == model.SectionName))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }


        public bool ApproveWeeklyData(int SiteId, int Year, int Week)
        {
            var obj = (from w in _weeklyDataApproveRepository.Table
                       where w.SiteId == SiteId &&
                             w.Week == Week &&
                             w.Year == Year
                       select w).FirstOrDefault();
            if (obj == null)
            {
                _weeklyDataApproveRepository.Insert(new WeeklyOperationalDataApprove
                {
                    SiteId = SiteId,
                    Week = Week,
                    Year = Year,
                    IsApprove = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = ProjectSession.UserID
                });

            }
            else if (obj != null && obj.IsApprove != true)
            {
                obj.IsApprove = true;

                _weeklyDataApproveRepository.Update(obj);
            }
            return true;
        }

        public bool SendWeeklyOperationalUploadData(int SiteId, int Year, int Week)
        {
            //Create Weekly Report PDF
            byte[] WeeklyReportPDF = CreateWeeklyReportPDF(SiteId, Year, Week);

            List<byte[]> lstAttachments = new List<byte[]>() { WeeklyReportPDF };
            string WeeklySummaryReportName = "WeeklySummaryReport_Week" + Week + "_Year" + Year + ".pdf";

            List<string> attachmentName = new List<string>() { WeeklySummaryReportName };
            var settings = SettingService.SelectSettingList();
            string Body = "Please Find attachment of sites weekly operational reports.";
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string Name = ProjectSession.UserName;
            //string MailCC = "";
            string Subject = "Sites Weekly Operational Reports: " + "Week-" + Week + " Year-" + Year;
            string emailTemplatePath = System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplates/DistributionOperationalReports.html");
            //string emailTemplatePath = Path.Combine(Directory.GetParent(Directory.GetParent(path).ToString()).ToString(), "EmailTempletes\\PMNotification.html"); ;
            using (StreamReader sReader = new StreamReader(emailTemplatePath))
            {
                string siteName = settings.Where(x => x.SettingKey == "SiteNameKey").Select(x => x.SettingValue).FirstOrDefault();
                string imglogo = settings.Where(x => x.SettingKey == "HostURLKey").Select(x => x.SettingValue).FirstOrDefault() + "/content/images/trevali.png";

                string htmlTemplate = sReader.ReadToEnd();
                htmlTemplate = htmlTemplate.Replace("$TYPE$", Subject);
                htmlTemplate = htmlTemplate.Replace("$SITEURL$", settings.Where(x => x.SettingKey == "HostURLKey").Select(x => x.SettingValue).FirstOrDefault());
                htmlTemplate = htmlTemplate.Replace("$LOGOURL$", imglogo);
                htmlTemplate = htmlTemplate.Replace("$SITENAME$", siteName);
                htmlTemplate = htmlTemplate.Replace("$NAME$", Name);
                htmlTemplate = htmlTemplate.Replace("$HEADERTEXT$", Body);
                bool success = Send(ProjectSession.Email, null, null, Subject, htmlTemplate, lstAttachments, attachmentName);
                return success;
            }
        }

        public bool Send(string mailTo, string mailCC, string mailBCC, string subject, string body, List<byte[]> attachmentFile = null, List<string> attachmentName = null)
        {
            Boolean issent = true;
            string mailFrom;
            var settings = SettingService.SelectSettingList();
            mailFrom = settings.Where(x => x.SettingKey == "FromEmailAddress").Select(x => x.SettingValue).FirstOrDefault();

            try
            {
                //if (ValidateEmail(mailFrom, mailTo) && (string.IsNullOrEmpty(mailCC) || IsEmail(mailCC)) && (string.IsNullOrEmpty(mailBCC) || IsEmail(mailBCC)))
                //{
                MailMessage mailMesg = new MailMessage();
                //SmtpClient objSMTP = new SmtpClient();
                SmtpClient objSMTP = new SmtpClient
                {
                    Host = settings.Where(x => x.SettingKey == "SMTP").Select(x => x.SettingValue).FirstOrDefault(),
                    Credentials = new NetworkCredential(settings.Where(x => x.SettingKey == "MailUserName").Select(x => x.SettingValue).FirstOrDefault(), settings.Where(x => x.SettingKey == "MailPassword").Select(x => x.SettingValue).FirstOrDefault()),
                    Port = ConvertTo.Integer(settings.Where(x => x.SettingKey == "SMTPPort").Select(x => x.SettingValue).FirstOrDefault()),
                    EnableSsl = ConvertTo.Boolean(settings.Where(x => x.SettingKey == "EnableSSL").Select(x => x.SettingValue).FirstOrDefault())
                };

                if (ConfigItems.TestMode)
                {
                    mailFrom = ConfigItems.TestEmailAddress;
                    mailTo = ConfigItems.TestEmailAddress;
                    mailCC = string.Empty;
                    mailBCC = string.Empty;

                    //objSMTP.UseDefaultCredentials = true;
                }

                mailMesg.From = new System.Net.Mail.MailAddress(mailFrom);
                mailMesg.To.Add(mailTo);

                //Include Default Addresses.
                if (!string.IsNullOrWhiteSpace(ConfigItems.DefaultEmailAddress))
                {
                    if (ConfigItems.DefaultEmailAddress.Contains(';'))
                    {
                        foreach (var address in ConfigItems.DefaultEmailAddress.Split(';'))
                        {
                            if (string.IsNullOrWhiteSpace(address))
                            {
                                continue;
                            }
                            mailMesg.To.Add(address);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(mailCC))
                {
                    string[] mailCCArray = mailCC.Split(';');
                    foreach (string email in mailCCArray)
                    {
                        mailMesg.CC.Add(email);
                    }
                }

                if (!string.IsNullOrEmpty(mailBCC))
                {
                    mailBCC = mailBCC.Replace(";", ",");
                    mailMesg.Bcc.Add(mailBCC);
                }

                if (attachmentFile != null && attachmentName != null)
                {
                    byte[][] Files = attachmentFile.ToArray();
                    string[] Names = attachmentName.ToArray();
                    if (Files != null)
                    {
                        for (int i = 0; i < Files.Length; i++)
                        {
                            mailMesg.Attachments.Add(new Attachment(new MemoryStream(Files[i]), Names[i]));
                        }
                    }
                }

                mailMesg.Subject = subject;
                mailMesg.Body = body;
                mailMesg.IsBodyHtml = true;

                try
                {
                    objSMTP.Send(mailMesg);
                    issent = true;
                    return issent;
                }
                catch (Exception e)
                {
                    mailMesg.Dispose();
                    mailMesg = null;
                    issent = false;
                    return issent;
                }
                finally
                {
                    mailMesg.Dispose();
                }
                //}
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public byte[] CreateWeeklyReportPDF(int SiteId, int Year, int Week)
        {
            //Render Report
            TrevaliOperationalReportObjectContext _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
            int reportId = 2;
            //bool IsShowReport = false;

            var localReport = new LocalReport
            {
                ReportPath = HttpContext.Current.Server.MapPath("~/Reports/rptWeekly.rdlc")
            };
            object[] xparams = {
                            new SqlParameter("SiteId", SiteId),
                            new SqlParameter("ReportId", reportId),
                              new SqlParameter("Week", Week),
                                new SqlParameter("Year", Year),
                         };
            object[] xparams1 = {
                            new SqlParameter("SiteId", SiteId),
                            new SqlParameter("ReportId", reportId),
                              new SqlParameter("Week", Week),
                                new SqlParameter("Year", Year),
                         };

            var reportdata = _dbContext.ExecuteStoredProcedureList<RPT_WeeklyOperationalReport_Result>("RPT_WeeklyOperationalReport", xparams);

            var Safetydata = _dbContext.ExecuteStoredProcedureList<RPT_GetSafety_Result>("RPT_GetSafety", xparams1);

            localReport.DataSources.Add(new ReportDataSource("DataSet2", ConvertListToDataTableforWeeklyReport(reportdata)));

            localReport.DataSources.Add(new ReportDataSource("dsSafety", ConvertListToDataTableforSafety(Safetydata)));

            localReport.DisplayName = "Weekly Operational Report";

            ReportParameter[] Rparams = new ReportParameter[2];
            Rparams[0] = new ReportParameter("paramSection", "", false);
            Rparams[1] = new ReportParameter("paramApproved", "true", false);
            localReport.SetParameters(Rparams);

            return localReport.Render("pdf");
        }

        static DataTable ConvertListToDataTableforWeeklyReport(IList<RPT_WeeklyOperationalReport_Result> listWeekly)
        {
            //new Table

            DataTable table = new DataTable();

            table.Columns.Add("MetricId");
            table.Columns.Add("SectionName");
            table.Columns.Add("MetricsName");
            table.Columns.Add("Units");
            table.Columns.Add("Week");
            table.Columns.Add("Month");
            table.Columns.Add("Year");
            table.Columns.Add("Budget");
            table.Columns.Add("Forecast");
            table.Columns.Add("ActualValue");
            table.Columns.Add("WeeklyBudget");
            table.Columns.Add("WeeklyForecast");
            table.Columns.Add("MTDValue");
            table.Columns.Add("MTDBudget");
            table.Columns.Add("MTDForecast");
            table.Columns.Add("YTDValue");
            table.Columns.Add("YTDBudget");
            table.Columns.Add("YTDForecast");
            table.Columns.Add("Comment");
            table.Columns.Add("CalculationType");

            for (int i = 0; i < listWeekly.Count; i++)
                table.Rows.Add(listWeekly[i].MetricId, listWeekly[i].SectionName, listWeekly[i].MetricsName, listWeekly[i].Units, listWeekly[i].Week,
                    listWeekly[i].Month, listWeekly[i].Year, listWeekly[i].Budget ?? Decimal.Zero, listWeekly[i].Forecast ?? Decimal.Zero, listWeekly[i].ActualValue ?? Decimal.Zero, listWeekly[i].WeeklyBudget ?? Decimal.Zero, listWeekly[i].WeeklyForecast ?? Decimal.Zero, listWeekly[i].MTDValue ?? Decimal.Zero,
                    listWeekly[i].MTDBudget ?? Decimal.Zero, listWeekly[i].MTDForecast ?? Decimal.Zero, listWeekly[i].YTDValue ?? Decimal.Zero, listWeekly[i].YTDBudget ?? Decimal.Zero, listWeekly[i].YTDForecast ?? Decimal.Zero, listWeekly[i].Comment, listWeekly[i].CalculationType);
            return table;
        }

        static DataTable ConvertListToDataTableforSafety(IList<RPT_GetSafety_Result> listSafety)
        {
            //new Table

            System.Data.DataTable table = new DataTable();

            table.Columns.Add("StartDay");
            table.Columns.Add("EndDay");
            table.Columns.Add("Logo");
            table.Columns.Add("Actual");
            table.Columns.Add("Target");
            table.Columns.Add("Rating");
            table.Columns.Add("FirstAidInjury");
            table.Columns.Add("RestrictedWorkInjury");
            table.Columns.Add("LostTimeInjury");
            table.Columns.Add("NearHit");
            table.Columns.Add("EquipmentDamage");
            table.Columns.Add("BusinessImpact");
            table.Columns.Add("EnviroIncident");
            table.Columns.Add("MTD_FirstAidInjury");
            table.Columns.Add("MTD_RestrictedWokInjury");
            table.Columns.Add("MTD_LostTimeInjury");
            table.Columns.Add("MTD_NearHit");
            table.Columns.Add("MTD_EquipmentDamage");
            table.Columns.Add("MTD_BusinessImpact");
            table.Columns.Add("MTD_EnviroIncident");
            table.Columns.Add("MTD_MedicalCases");
            table.Columns.Add("YTD_FirstAidInjury");
            table.Columns.Add("YTD_RestrictedWokInjury");
            table.Columns.Add("YTD_LostTimeInjury");
            table.Columns.Add("YTD_NearHit");
            table.Columns.Add("YTD_EquipmentDamage");
            table.Columns.Add("YTD_BusinessImpact");
            table.Columns.Add("YTD_EnviroIncident");
            table.Columns.Add("YTD_MedicalCases");
            table.Columns.Add("SummaryOfInitiatives");
            table.Columns.Add("SummaryOfIncidents");

            for (int i = 0; i < listSafety.Count; i++)
                table.Rows.Add(
                   listSafety[i].StartDay,
                   listSafety[i].EndDay,
                   Convert.ToBase64String(listSafety[i].Logo ?? new byte[1]),
                   listSafety[i].Actual,
                   listSafety[i].Target,
                   listSafety[i].Rating,
                   listSafety[i].FirstAidInjury,
                   listSafety[i].RestrictedWorkInjury,
                   listSafety[i].LostTimeInjury,
                   listSafety[i].NearHit,
                   listSafety[i].EquipmentDamage,
                   listSafety[i].BusinessImpact,
                   listSafety[i].EnviroIncident,
                   listSafety[i].MTD_FirstAidInjury,
                   listSafety[i].MTD_RestrictedWorkInjury,
                   listSafety[i].MTD_LostTimeInjury,
                   listSafety[i].MTD_NearHit,
                   listSafety[i].MTD_EquipmentDamage,
                   listSafety[i].MTD_BusinessImpact,
                   listSafety[i].MTD_EnviroIncident,
                   listSafety[i].MTD_MedicalCases,
                   listSafety[i].YTD_FirstAidInjury,
                   listSafety[i].YTD_RestrictedWorkInjury,
                   listSafety[i].YTD_LostTimeInjury,
                   listSafety[i].YTD_NearHit,
                   listSafety[i].YTD_EquipmentDamage,
                   listSafety[i].YTD_BusinessImpact,
                   listSafety[i].YTD_EnviroIncident,
                   listSafety[i].YTD_MedicalCases,
                   listSafety[i].SummaryOfInitiatives,
                   listSafety[i].SummaryOfIncidents
                   );
            return table;
        }

        public bool CheckApproveStatus(int SiteId, int Year, int Week)
        {
            var obj = (from w in _weeklyDataApproveRepository.Table
                       where w.SiteId == SiteId &&
                             w.Week == Week &&
                             w.Year == Year
                       select w).FirstOrDefault();
            if (obj == null)
            {
                return false;
            }
            else
            {
                return obj.IsApprove;
            }
        }

        public int GetSiteIdFromSiteName(string sitename)
        {
            int siteid = 0;
            if (!String.IsNullOrEmpty(sitename))
            {
                var site = from p in _dbContext.Site where p.SiteName == sitename select p;
                siteid = site.FirstOrDefault().SiteId;

            }
            return siteid;
        }

        #endregion

        #region Weekly Analysis Data



        #endregion

        #endregion
    }
}
