using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;
using TrevaliOperationalReport.Domain.Report;

namespace TrevaliOperationalReport.Service.Report
{
    public class MonthlyForecastDataService : IMonthlyForecastDataService
    {
        #region Fields

        private readonly IRepository<MonthlyForecastData> _montlhyForecastDataRepository;
        private readonly IRepository<MonthlyForecast> _montlhyForecastRepository;
        private readonly IRepository<Metrics> _metricsRepository;
        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<SiteMetrics> _siteMetricsRepository;
        public static TrevaliOperationalReportObjectContext _dbContext;

        #endregion

        #region Ctor

        public MonthlyForecastDataService(IRepository<MonthlyForecastData> montlhyForecastDataRepository, IRepository<MonthlyForecast> montlhyForecastRepository, IRepository<Metrics> metricsRepository,
            IRepository<Site> siteRepository, IRepository<SiteMetrics> siteMetricsRepository)
        {
            _montlhyForecastDataRepository = montlhyForecastDataRepository;
            _montlhyForecastRepository = montlhyForecastRepository;
            _siteMetricsRepository = siteMetricsRepository;
            _metricsRepository = metricsRepository;
            _siteRepository = siteRepository;
            _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
        }

        #endregion


        #region Methods


        /// <summary>
        /// Gets sites  selectlist.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetSitesSelectList(int siteId = 0, bool IsFullRightsRequired = false)
        {
            List<int> SiteIds = new List<int>();
            if (IsFullRightsRequired)
                SiteIds = ProjectSession.UserSiteRightsDynamic.Where(x => x.IsFullRights == true).Select(x => x.SiteId).ToList();
            else
                SiteIds = ProjectSession.UserSiteRightsDynamic.Where(x => x.IsFullRights == true || x.IsView == true).Select(x => x.SiteId).ToList();


            if (siteId == 0)
            {
                var query = from p in _siteRepository.Table
                            where (SiteIds.Contains(p.SiteId) || ProjectSession.IsAdmin == true)
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
                            (SiteIds.Contains(p.SiteId) || ProjectSession.IsAdmin == true)
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
        /// Gets current and next 5 months selectlist.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetMonthsSelectList()
        {
            List<SelectListItem> monthslist = new List<SelectListItem>();

            var month = DateTime.Now.ToString("yyyy");
            SelectListItem item = new SelectListItem()
            {
                Text = month,
                Value = month
            };
            monthslist.Add(item);

            for (int i = 1; i <= 5; i++)
            {
                month = (Convert.ToInt32(month) + 1).ToString();
                item = new SelectListItem()
                {
                    Text = month,
                    Value = month
                };
                monthslist.Add(item);
            }
            return monthslist;
        }


        /// <summary>
        /// Gets metrics  selectlist.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetMetricsSelectList(int siteId = 0, int ReportId = 0, bool IsDataDownload = false)
        {
            string[] CalculatedMetrics = {
                                            "Zn Payable Production (Mt)",
                                            "Pb Payable Production (Mt)",
                                            "Ag Payable Production",
                                            "Zn Payable Production (lbs)" ,
                                            "Pb Payable Production (lbs)",
                                            "Payable Zn sold (Mt)",
                                            "Payable Pb sold (Mt)",
                                            "Payable Ag sold",
                                            "Payable Zn sold (lbs)",
                                            "Payable Pb sold (lbs)",
                                         };
            if (siteId > 0)
            {

                var list = new List<SelectListItem>();
                var metrics = (from p in _dbContext.SiteMetrics
                               where p.SiteId == siteId
                               && (p.ReportId == ReportId || ReportId == 0)
                               && p.IsYearly == true
                               select p).OrderBy(x => x.DisplayOrder).OrderBy(a => a.SectionId).ToList();

                if (metrics != null && metrics.Count() > 0)
                {

                    metrics = metrics.AsEnumerable().OrderBy(x => x.DisplayOrder).OrderBy(a => a.SectionId).ToList();

                    foreach (var itm in metrics)
                    {
                        if (IsDataDownload == false && itm.Section.SectionMappingName == "Finance" && CalculatedMetrics.Contains(itm.Metric.MetricsMappingName))
                            continue;
                        if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == itm.SectionId))
                        {
                            var listItem = new SelectListItem { Text = itm.Section.SectionName + "^" + itm.Metric.MetricsName + " (" + itm.Metric.Unit.UOM + ")", Value = itm.SiteMetricId.ToString() };

                            list.Add(listItem);
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
        /// Gets the monthly forecast data.
        /// </summary>
        /// <param name="SiteId">The site identifier.</param>
        /// <param name="forecast">The forecast.</param>
        /// <returns>IList&lt;MonthlyForecastData&gt;.</returns>
        public IList<MonthlyForecastData> GetMonthlyForecastData(int SiteId, string forecast)
        {
            var data = new List<MonthlyForecastData>();
            var forecastdata = (from p in _montlhyForecastDataRepository.Table
                                where p.MonthlyForecast.SiteId == SiteId &&
                                p.MonthlyForecast.UniqueId == forecast
                                select p).ToList().Where(x => x.SiteMetrics.HideInReports == 0).ToList();

            if (forecastdata != null && forecastdata.Count > 0)
            {
                foreach (var item in forecastdata)
                {
                    if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == item.SiteMetrics.SectionId))
                    {
                        data.Add(item);
                    }
                }

            }

            return data;
        }

        public IList<MonthlyForecastData> GetMonthlyForecastData(int SiteId, string year, string forecastCadence)
        {
            List<MonthlyForecastData> data = new List<MonthlyForecastData>();

            object[] xparams = {
                            new SqlParameter("SiteId", SiteId),
                                new SqlParameter("Year", year),
                                new SqlParameter("ForecastCandence", forecastCadence),
                                new SqlParameter("UserId", ProjectSession.UserID)
                         };

            var forecastdata = _dbContext.ExecuteStoredProcedureList<MonthlyForecastData>("RPT_GetMonthlyAnnualForecastData", xparams);

            if (forecastdata != null && forecastdata.Count > 0)
            {
                var uniqueId = year + "-1";
                foreach (var item in forecastdata)
                {
                    var sitemetric = (from s in _siteMetricsRepository.Table
                                      where s.SiteMetricId == item.SiteMetricId
                                      select s).FirstOrDefault();
                    item.SiteMetrics = sitemetric;

                    var monthlyForecast = (from mf in _montlhyForecastRepository.Table
                                           where mf.UniqueId == uniqueId && mf.SiteId == SiteId
                                           select mf).FirstOrDefault();
                    item.MonthlyForecast = monthlyForecast;

                    if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == item.SiteMetrics.SectionId))
                    {
                        data.Add(item);
                    }
                }

            }

            return data;
        }

        public IList<SelectListItem> GetForecastListFormatted(int siteId = 0, bool IsDownload = false)
        {
            var forecast = (from p in _montlhyForecastRepository.Table
                            where (p.SiteId == siteId || siteId == 0)
                            select p).ToList();


            var query = (from p in forecast
                         select new
                         {
                             month = p.Month,
                             year = p.Year,
                             Value = p.MonthlyForecastId.ToString(),
                             IsApprove = p.IsApprove
                         }).OrderByDescending(p => p.year).ThenByDescending(p => p.month).Take(15).ToList();

            var list = new List<SelectListItem>();

            if (query != null && query.Count > 0)
            {
                if (query[0].IsApprove == true && IsDownload)
                {
                    int m = query[0].month + 1;
                    int y = query[0].year;
                    if (m == 13)
                    {
                        m = 1;
                        y = y + 1;
                    }
                    list.Add(new SelectListItem
                    {
                        Text = y.ToString() + "-" + m.ToString(),
                        Value = y.ToString() + "-" + m.ToString() + "_Black",
                        Selected = true
                    });
                }

                for (int i = 0; i < query.Count(); i++)
                {
                    if (i == 0)
                    {
                        list.Add(new SelectListItem
                        {
                            Text = query[i].year.ToString() + "-" + query[i].month.ToString(),
                            Value = query[i].Value + (query[i].IsApprove == true ? "_Green" : "_Black")
                        });
                    }
                    else if (i == 1)
                    {
                        list.Add(new SelectListItem
                        {
                            Text = query[i].year.ToString() + "-" + query[i].month.ToString(),
                            Value = query[i].Value + (query[0].IsApprove == false ? "_Green" : "_Red")

                        });
                    }
                    else
                        list.Add(new SelectListItem
                        {
                            Text = query[i].year.ToString() + "-" + query[i].month.ToString(),
                            Value = query[i].Value + "_Red"
                        });
                }
            }
            else if (IsDownload == true)
            {
                list.Add(new SelectListItem
                {
                    Text = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString(),
                    Value = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "_Black"
                });

            }

            return list;

        }


        /// <summary>
        /// Gets metrics  selectlist.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetMetricsSelectList(int siteId = 0)
        {
            string[] CalculatedMetrics = {
                                            "Zn Payable Production (Mt)",
                                            "Pb Payable Production (Mt)",
                                            "Ag Payable Production",
                                            "Zn Payable Production (lbs)" ,
                                            "Pb Payable Production (lbs)",
                                            "Payable Zn sold (Mt)",
                                            "Payable Pb sold (Mt)",
                                            "Payable Ag sold",
                                            "Payable Zn sold (lbs)",
                                            "Payable Pb sold (lbs)",
                                         };
            if (siteId > 0)
            {

                var list = new List<SelectListItem>();
                var metrics = (from p in _dbContext.SiteMetrics
                               where p.SiteId == siteId
                               && p.IsYearly == true
                               select p).OrderBy(x => x.DisplayOrder).OrderBy(a => a.SectionId).ToList();

                if (metrics != null && metrics.Count() > 0)
                {

                    metrics = metrics.AsEnumerable().OrderBy(x => x.DisplayOrder).OrderBy(a => a.SectionId).ToList();

                    foreach (var itm in metrics)
                    {
                        if (itm.Section.SectionMappingName == "Finance" && CalculatedMetrics.Contains(itm.Metric.MetricsMappingName))
                            continue;
                        if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == itm.SectionId))
                        {
                            var listItem = new SelectListItem { Text = itm.Section.SectionName + "^" + itm.Metric.MetricsName + " (" + itm.Metric.Unit.UOM + ")", Value = itm.SiteMetricId.ToString() };

                            list.Add(listItem);
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
        /// Uploads monthly Forecast data
        /// </summary>
        /// <param name="lstMontlhyForecastData"></param>
        /// <returns></returns>
        public List<InvalidMonthlyForecastData> UploadMonthlyForecastData(List<MonthlyForecastUploadModel> lstMonthlyForecastData)
        {
            var InvalidCount = 0;
            List<InvalidMonthlyForecastData> lstInvalid = new List<InvalidMonthlyForecastData>();
            var sitename = lstMonthlyForecastData[0].SiteName;
            var site = from p in _dbContext.Site where p.SiteName == sitename select p;
            int siteId = site.FirstOrDefault().SiteId;

            string forecastUniqueId = lstMonthlyForecastData[0].ForecastId;
            int forecastMonth = Convert.ToInt32(forecastUniqueId.Split('-')[1]);
            int forecastYear = Convert.ToInt32(forecastUniqueId.Split('-')[0]);

            var sites = GetSitesSelectList(siteId, true);

            var metrics = GetMetricsSelectList(siteId, 0, false);
            if (lstMonthlyForecastData != null && lstMonthlyForecastData.Count > 0)
            {
                int MonthlyForecastId = 0;

                var id = lstMonthlyForecastData[0].ForecastId;
                var MonthlyForecast = (from x in _dbContext.MonthlyForecast
                                       where x.UniqueId == id &&
                                             x.Year == forecastYear &&
                                             x.Month == forecastMonth &&
                                             x.SiteId == siteId
                                       select x).FirstOrDefault();
                if (MonthlyForecast != null)
                {
                    MonthlyForecastId = MonthlyForecast.MonthlyForecastId;
                }

                int tempMonth = Convert.ToInt32(lstMonthlyForecastData[0].ForecastId.Split('-')[1]);
                int tempYear = Convert.ToInt32(lstMonthlyForecastData[0].ForecastId.Split('-')[0]);

                int M1Days = (tempMonth + 1) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 1) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 1) - 12);
                int M2Days = (tempMonth + 2) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 2) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 2) - 12);
                int M3Days = (tempMonth + 3) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 3) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 3) - 12);
                int M4Days = (tempMonth + 4) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 4) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 4) - 12);
                int M5Days = (tempMonth + 5) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 5) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 5) - 12);
                int M6Days = (tempMonth + 6) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 6) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 6) - 12);
                int M7Days = (tempMonth + 7) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 7) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 7) - 12);
                int M8Days = (tempMonth + 8) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 8) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 8) - 12);
                int M9Days = (tempMonth + 9) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 9) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 9) - 12);
                int M10Days = (tempMonth + 10) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 10) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 10) - 12);
                int M11Days = (tempMonth + 11) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 11) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 11) - 12);
                int M12Days = (tempMonth + 12) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 12) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 12) - 12);

                int M13Days = (tempMonth + 13) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 1) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 1) - 12);
                int M14Days = (tempMonth + 14) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 2) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 2) - 12);
                int M15Days = (tempMonth + 15) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 3) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 3) - 12);
                int M16Days = (tempMonth + 16) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 4) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 4) - 12);
                int M17Days = (tempMonth + 17) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 5) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 5) - 12);
                int M18Days = (tempMonth + 18) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 6) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 6) - 12);
                int M19Days = (tempMonth + 19) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 7) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 7) - 12);
                int M20Days = (tempMonth + 20) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 8) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 8) - 12);
                int M21Days = (tempMonth + 21) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 9) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 9) - 12);
                int M22Days = (tempMonth + 22) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 10) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 10) - 12);
                int M23Days = (tempMonth + 23) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 11) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 11) - 12);
                int M24Days = (tempMonth + 24) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 12) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 12) - 12);


                var millThroughtputM1 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M1).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M1).FirstOrDefault()) / (M1Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M1).FirstOrDefault()) / 100)));
                var millThroughtputM2 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M2).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M2).FirstOrDefault()) / (M2Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M2).FirstOrDefault()) / 100)));
                var millThroughtputM3 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M3).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M3).FirstOrDefault()) / (M3Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M3).FirstOrDefault()) / 100)));
                var millThroughtputM4 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M4).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M4).FirstOrDefault()) / (M4Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M4).FirstOrDefault()) / 100)));
                var millThroughtputM5 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M5).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M5).FirstOrDefault()) / (M5Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M5).FirstOrDefault()) / 100)));
                var millThroughtputM6 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M6).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M6).FirstOrDefault()) / (M6Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M6).FirstOrDefault()) / 100)));
                var millThroughtputM7 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M7).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M7).FirstOrDefault()) / (M7Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M7).FirstOrDefault()) / 100)));
                var millThroughtputM8 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M8).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M8).FirstOrDefault()) / (M8Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M8).FirstOrDefault()) / 100)));
                var millThroughtputM9 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M9).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M9).FirstOrDefault()) / (M9Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M9).FirstOrDefault()) / 100)));
                var millThroughtputM10 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M10).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M10).FirstOrDefault()) / (M10Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M10).FirstOrDefault()) / 100)));
                var millThroughtputM11 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M11).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M11).FirstOrDefault()) / (M11Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M11).FirstOrDefault()) / 100)));
                var millThroughtputM12 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M12).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M12).FirstOrDefault()) / (M12Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M12).FirstOrDefault()) / 100)));

                var millThroughtputM13 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M13).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M13).FirstOrDefault()) / (M13Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M13).FirstOrDefault()) / 100)));
                var millThroughtputM14 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M14).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M14).FirstOrDefault()) / (M14Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M14).FirstOrDefault()) / 100)));
                var millThroughtputM15 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M15).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M15).FirstOrDefault()) / (M15Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M15).FirstOrDefault()) / 100)));
                var millThroughtputM16 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M16).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M16).FirstOrDefault()) / (M16Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M16).FirstOrDefault()) / 100)));
                var millThroughtputM17 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M17).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M17).FirstOrDefault()) / (M17Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M17).FirstOrDefault()) / 100)));
                var millThroughtputM18 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M18).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M18).FirstOrDefault()) / (M18Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M18).FirstOrDefault()) / 100)));
                var millThroughtputM19 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M19).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M19).FirstOrDefault()) / (M19Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M19).FirstOrDefault()) / 100)));
                var millThroughtputM20 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M20).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M20).FirstOrDefault()) / (M20Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M20).FirstOrDefault()) / 100)));
                var millThroughtputM21 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M21).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M21).FirstOrDefault()) / (M21Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M21).FirstOrDefault()) / 100)));
                var millThroughtputM22 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M22).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M22).FirstOrDefault()) / (M22Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M22).FirstOrDefault()) / 100)));
                var millThroughtputM23 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M23).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M23).FirstOrDefault()) / (M23Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M23).FirstOrDefault()) / 100)));
                var millThroughtputM24 = (lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M24).FirstOrDefault()) == 0 ? 0 : ((lstMonthlyForecastData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.M24).FirstOrDefault()) / (M24Days * 24 * ((lstMonthlyForecastData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.M24).FirstOrDefault()) / 100)));

                foreach (MonthlyForecastUploadModel monthlydata in lstMonthlyForecastData)
                {
                    if (tempMonth < 1 || tempMonth > 12)
                    {
                        lstInvalid.Add(new InvalidMonthlyForecastData
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            ForecastId = monthlydata.ForecastId,

                            M1 = monthlydata.M1,
                            M2 = monthlydata.M2,
                            M3 = monthlydata.M3,
                            M4 = monthlydata.M4,
                            M5 = monthlydata.M5,
                            M6 = monthlydata.M6,
                            M7 = monthlydata.M7,
                            M8 = monthlydata.M8,
                            M9 = monthlydata.M9,
                            M10 = monthlydata.M10,
                            M11 = monthlydata.M11,
                            M12 = monthlydata.M12,
                            M13 = monthlydata.M13,
                            M14 = monthlydata.M14,
                            M15 = monthlydata.M15,
                            M16 = monthlydata.M16,
                            M17 = monthlydata.M17,
                            M18 = monthlydata.M18,
                            M19 = monthlydata.M19,
                            M20 = monthlydata.M20,
                            M21 = monthlydata.M21,
                            M22 = monthlydata.M22,
                            M23 = monthlydata.M23,
                            M24 = monthlydata.M24,

                            Y1 = monthlydata.Y1,
                            Y2 = monthlydata.Y2,
                            Y3 = monthlydata.Y3,
                            Y4 = monthlydata.Y4,
                            Y5 = monthlydata.Y5,
                            Y6 = monthlydata.Y6,
                            Y7 = monthlydata.Y7,
                            Y8 = monthlydata.Y8,
                            Y9 = monthlydata.Y9,
                            Y10 = monthlydata.Y10,
                            Y11 = monthlydata.Y11,
                            Y12 = monthlydata.Y12,
                            Y13 = monthlydata.Y13,
                            Y14 = monthlydata.Y14,
                            Y15 = monthlydata.Y15,
                            Y16 = monthlydata.Y16,
                            Y17 = monthlydata.Y17,
                            Y18 = monthlydata.Y18,
                            Y19 = monthlydata.Y19,
                            Y20 = monthlydata.Y20,
                            Y21 = monthlydata.Y21,
                            Y22 = monthlydata.Y22,
                            Y23 = monthlydata.Y23,
                            Y24 = monthlydata.Y24,
                            Y25 = monthlydata.Y25,

                            Comment = "invalid forecastId"
                        });
                        InvalidCount++;
                        continue;
                    }
                    else if (!sites.Any(x => x.Text == monthlydata.SiteName))
                    {
                        lstInvalid.Add(new InvalidMonthlyForecastData
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            ForecastId = monthlydata.ForecastId,

                            M1 = monthlydata.M1,
                            M2 = monthlydata.M2,
                            M3 = monthlydata.M3,
                            M4 = monthlydata.M4,
                            M5 = monthlydata.M5,
                            M6 = monthlydata.M6,
                            M7 = monthlydata.M7,
                            M8 = monthlydata.M8,
                            M9 = monthlydata.M9,
                            M10 = monthlydata.M10,
                            M11 = monthlydata.M11,
                            M12 = monthlydata.M12,
                            M13 = monthlydata.M13,
                            M14 = monthlydata.M14,
                            M15 = monthlydata.M15,
                            M16 = monthlydata.M16,
                            M17 = monthlydata.M17,
                            M18 = monthlydata.M18,
                            M19 = monthlydata.M19,
                            M20 = monthlydata.M20,
                            M21 = monthlydata.M21,
                            M22 = monthlydata.M22,
                            M23 = monthlydata.M23,
                            M24 = monthlydata.M24,

                            Y1 = monthlydata.Y1,
                            Y2 = monthlydata.Y2,
                            Y3 = monthlydata.Y3,
                            Y4 = monthlydata.Y4,
                            Y5 = monthlydata.Y5,
                            Y6 = monthlydata.Y6,
                            Y7 = monthlydata.Y7,
                            Y8 = monthlydata.Y8,
                            Y9 = monthlydata.Y9,
                            Y10 = monthlydata.Y10,
                            Y11 = monthlydata.Y11,
                            Y12 = monthlydata.Y12,
                            Y13 = monthlydata.Y13,
                            Y14 = monthlydata.Y14,
                            Y15 = monthlydata.Y15,
                            Y16 = monthlydata.Y16,
                            Y17 = monthlydata.Y17,
                            Y18 = monthlydata.Y18,
                            Y19 = monthlydata.Y19,
                            Y20 = monthlydata.Y20,
                            Y21 = monthlydata.Y21,
                            Y22 = monthlydata.Y22,
                            Y23 = monthlydata.Y23,
                            Y24 = monthlydata.Y24,
                            Y25 = monthlydata.Y25,

                            Comment = "Invalid site OR permission denied for site"
                        });
                        InvalidCount++;
                        continue;

                    }
                    else if (!metrics.Any(x => x.Text == monthlydata.SectionName + "^" + monthlydata.MetricName))
                    {

                        lstInvalid.Add(new InvalidMonthlyForecastData
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            ForecastId = monthlydata.ForecastId,

                            M1 = monthlydata.M1,
                            M2 = monthlydata.M2,
                            M3 = monthlydata.M3,
                            M4 = monthlydata.M4,
                            M5 = monthlydata.M5,
                            M6 = monthlydata.M6,
                            M7 = monthlydata.M7,
                            M8 = monthlydata.M8,
                            M9 = monthlydata.M9,
                            M10 = monthlydata.M10,
                            M11 = monthlydata.M11,
                            M12 = monthlydata.M12,
                            M13 = monthlydata.M13,
                            M14 = monthlydata.M14,
                            M15 = monthlydata.M15,
                            M16 = monthlydata.M16,
                            M17 = monthlydata.M17,
                            M18 = monthlydata.M18,
                            M19 = monthlydata.M19,
                            M20 = monthlydata.M20,
                            M21 = monthlydata.M21,
                            M22 = monthlydata.M22,
                            M23 = monthlydata.M23,
                            M24 = monthlydata.M24,

                            Y1 = monthlydata.Y1,
                            Y2 = monthlydata.Y2,
                            Y3 = monthlydata.Y3,
                            Y4 = monthlydata.Y4,
                            Y5 = monthlydata.Y5,
                            Y6 = monthlydata.Y6,
                            Y7 = monthlydata.Y7,
                            Y8 = monthlydata.Y8,
                            Y9 = monthlydata.Y9,
                            Y10 = monthlydata.Y10,
                            Y11 = monthlydata.Y11,
                            Y12 = monthlydata.Y12,
                            Y13 = monthlydata.Y13,
                            Y14 = monthlydata.Y14,
                            Y15 = monthlydata.Y15,
                            Y16 = monthlydata.Y16,
                            Y17 = monthlydata.Y17,
                            Y18 = monthlydata.Y18,
                            Y19 = monthlydata.Y19,
                            Y20 = monthlydata.Y20,
                            Y21 = monthlydata.Y21,
                            Y22 = monthlydata.Y22,
                            Y23 = monthlydata.Y23,
                            Y24 = monthlydata.Y24,
                            Y25 = monthlydata.Y25,

                            Comment = "Invalid Metric/Section name OR Permission denied for section"
                        });
                        InvalidCount++;
                        continue;
                    }

                    InvalidMonthlyForecastData invalid = new InvalidMonthlyForecastData();
                    bool invalidfound = false;
                    int m;
                    for (m = 1; m <= 24; m++)
                    {
                        invalidfound = false;
                        switch (m)
                        {
                            case 1:
                                if (monthlydata.M1 < 0 || monthlydata.M1 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 2:
                                if (monthlydata.M2 < 0 || monthlydata.M2 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 3:
                                if (monthlydata.M3 < 0 || monthlydata.M3 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 4:
                                if (monthlydata.M4 < 0 || monthlydata.M4 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 5:
                                if (monthlydata.M5 < 0 || monthlydata.M5 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 6:
                                if (monthlydata.M6 < 0 || monthlydata.M6 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 7:
                                if (monthlydata.M7 < 0 || monthlydata.M7 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 8:
                                if (monthlydata.M8 < 0 || monthlydata.M8 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 9:
                                if (monthlydata.M9 < 0 || monthlydata.M9 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 10:
                                if (monthlydata.M10 < 0 || monthlydata.M10 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 11:
                                if (monthlydata.M11 < 0 || monthlydata.M11 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 12:
                                if (monthlydata.M12 < 0 || monthlydata.M12 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 13:
                                if (monthlydata.M13 < 0 || monthlydata.M13 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 14:
                                if (monthlydata.M14 < 0 || monthlydata.M14 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 15:
                                if (monthlydata.M15 < 0 || monthlydata.M15 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 16:
                                if (monthlydata.M16 < 0 || monthlydata.M16 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 17:
                                if (monthlydata.M17 < 0 || monthlydata.M17 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 18:
                                if (monthlydata.M18 < 0 || monthlydata.M18 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 19:
                                if (monthlydata.M19 < 0 || monthlydata.M19 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 20:
                                if (monthlydata.M20 < 0 || monthlydata.M20 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 21:
                                if (monthlydata.M21 < 0 || monthlydata.M21 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 22:
                                if (monthlydata.M22 < 0 || monthlydata.M22 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 23:
                                if (monthlydata.M23 < 0 || monthlydata.M23 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 24:
                                if (monthlydata.M24 < 0 || monthlydata.M24 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetForecastMonthSequence(forecastMonth + m - 1, forecastYear) + ".";
                                    invalidfound = true;
                                }
                                break;
                        }
                        if (invalidfound)
                            break;
                    }
                    if (monthlydata.Y1 < 0 || monthlydata.Y1 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 2) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y2 < 0 || monthlydata.Y2 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 3) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y3 < 0 || monthlydata.Y3 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 4) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y4 < 0 || monthlydata.Y4 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 5) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y5 < 0 || monthlydata.Y5 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 6) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y6 < 0 || monthlydata.Y6 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 7) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y7 < 0 || monthlydata.Y7 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 8) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y8 < 0 || monthlydata.Y8 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 9) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y9 < 0 || monthlydata.Y9 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 10) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y10 < 0 || monthlydata.Y10 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 11) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y11 < 0 || monthlydata.Y11 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 12) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y12 < 0 || monthlydata.Y12 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 13) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y13 < 0 || monthlydata.Y13 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 14) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y14 < 0 || monthlydata.Y14 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 15) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y15 < 0 || monthlydata.Y15 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 16) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y16 < 0 || monthlydata.Y16 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 17) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y17 < 0 || monthlydata.Y17 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 18) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y18 < 0 || monthlydata.Y18 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 19) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y19 < 0 || monthlydata.Y19 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 20) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y20 < 0 || monthlydata.Y20 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 21) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y21 < 0 || monthlydata.Y21 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 22) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y22 < 0 || monthlydata.Y22 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 23) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y23 < 0 || monthlydata.Y23 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 24) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y24 < 0 || monthlydata.Y24 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 24) + ".";
                        invalidfound = true;
                    }
                    if (monthlydata.Y25 < 0 || monthlydata.Y25 > 99999999)
                    {
                        invalid.Comment = "Invalid value for " + CommonHelper.GetNextForecastYear(forecastYear + 25) + ".";
                        invalidfound = true;
                    }
                    if (invalidfound)
                    {
                        invalid.SiteName = monthlydata.SiteName;
                        invalid.SectionName = monthlydata.SectionName;
                        invalid.MetricName = monthlydata.MetricName;

                        invalid.M1 = monthlydata.M1;
                        invalid.M2 = monthlydata.M2;
                        invalid.M3 = monthlydata.M3;
                        invalid.M4 = monthlydata.M4;
                        invalid.M5 = monthlydata.M5;
                        invalid.M6 = monthlydata.M6;
                        invalid.M7 = monthlydata.M7;
                        invalid.M8 = monthlydata.M8;
                        invalid.M9 = monthlydata.M9;
                        invalid.M10 = monthlydata.M10;
                        invalid.M11 = monthlydata.M11;
                        invalid.M12 = monthlydata.M12;

                        invalid.M13 = monthlydata.M13;
                        invalid.M14 = monthlydata.M14;
                        invalid.M15 = monthlydata.M15;
                        invalid.M16 = monthlydata.M16;
                        invalid.M17 = monthlydata.M17;
                        invalid.M18 = monthlydata.M18;
                        invalid.M19 = monthlydata.M19;
                        invalid.M20 = monthlydata.M20;
                        invalid.M21 = monthlydata.M21;
                        invalid.M22 = monthlydata.M22;
                        invalid.M23 = monthlydata.M23;
                        invalid.M24 = monthlydata.M24;

                        invalid.Y1 = monthlydata.Y1;
                        invalid.Y2 = monthlydata.Y2;
                        invalid.Y3 = monthlydata.Y3;
                        invalid.Y4 = monthlydata.Y4;
                        invalid.Y5 = monthlydata.Y5;
                        invalid.Y6 = monthlydata.Y6;
                        invalid.Y7 = monthlydata.Y7;
                        invalid.Y8 = monthlydata.Y8;
                        invalid.Y9 = monthlydata.Y9;
                        invalid.Y10 = monthlydata.Y10;
                        invalid.Y11 = monthlydata.Y11;
                        invalid.Y12 = monthlydata.Y12;
                        invalid.Y13 = monthlydata.Y13;
                        invalid.Y14 = monthlydata.Y14;
                        invalid.Y15 = monthlydata.Y15;
                        invalid.Y16 = monthlydata.Y16;
                        invalid.Y17 = monthlydata.Y17;
                        invalid.Y18 = monthlydata.Y18;
                        invalid.Y19 = monthlydata.Y19;
                        invalid.Y20 = monthlydata.Y20;
                        invalid.Y21 = monthlydata.Y21;
                        invalid.Y22 = monthlydata.Y22;
                        invalid.Y23 = monthlydata.Y23;
                        invalid.Y24 = monthlydata.Y24;
                        invalid.Y25 = monthlydata.Y25;

                        continue;
                    }

                    if (metrics.Any(x => x.Text == monthlydata.SectionName + "^" + monthlydata.MetricName))
                    {
                        monthlydata.SiteMetricId = Convert.ToInt32(metrics.Where(x => x.Text == monthlydata.SectionName + "^" + monthlydata.MetricName).FirstOrDefault().Value);
                    }
                    else
                    {
                        lstInvalid.Add(new InvalidMonthlyForecastData
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,

                            M1 = monthlydata.M1,
                            M2 = monthlydata.M2,
                            M3 = monthlydata.M3,
                            M4 = monthlydata.M4,
                            M5 = monthlydata.M5,
                            M6 = monthlydata.M6,
                            M7 = monthlydata.M7,
                            M8 = monthlydata.M8,
                            M9 = monthlydata.M9,
                            M10 = monthlydata.M10,
                            M11 = monthlydata.M11,
                            M12 = monthlydata.M12,

                            M13 = monthlydata.M13,
                            M14 = monthlydata.M14,
                            M15 = monthlydata.M15,
                            M16 = monthlydata.M16,
                            M17 = monthlydata.M17,
                            M18 = monthlydata.M18,
                            M19 = monthlydata.M19,
                            M20 = monthlydata.M20,
                            M21 = monthlydata.M21,
                            M22 = monthlydata.M22,
                            M23 = monthlydata.M23,
                            M24 = monthlydata.M24,

                            Y1 = monthlydata.Y1,
                            Y2 = monthlydata.Y2,
                            Y3 = monthlydata.Y3,
                            Y4 = monthlydata.Y4,
                            Y5 = monthlydata.Y5,
                            Y6 = monthlydata.Y6,
                            Y7 = monthlydata.Y7,
                            Y8 = monthlydata.Y8,
                            Y9 = monthlydata.Y9,
                            Y10 = monthlydata.Y10,
                            Y11 = monthlydata.Y11,
                            Y12 = monthlydata.Y12,
                            Y13 = monthlydata.Y13,
                            Y14 = monthlydata.Y14,
                            Y15 = monthlydata.Y15,
                            Y16 = monthlydata.Y16,
                            Y17 = monthlydata.Y17,
                            Y18 = monthlydata.Y18,
                            Y19 = monthlydata.Y19,
                            Y20 = monthlydata.Y20,
                            Y21 = monthlydata.Y21,
                            Y22 = monthlydata.Y22,
                            Y23 = monthlydata.Y23,
                            Y24 = monthlydata.Y24,
                            Y25 = monthlydata.Y25,

                            Comment = "Invalid Metric/Section name OR Permission denied for section"
                        });
                        InvalidCount++;
                        continue;
                    }

                    if (MonthlyForecastId == 0)
                    {
                        var objmonthlyforecast = new MonthlyForecast
                        {
                            Month = forecastMonth,
                            Year = forecastYear,
                            SiteId = siteId,
                            UniqueId = monthlydata.ForecastId,
                            IsApprove = false,
                            CreatedBy = ProjectSession.UserID,
                            CreatedDate = DateTime.Now
                        };
                        _montlhyForecastRepository.Insert(objmonthlyforecast);
                        MonthlyForecastId = objmonthlyforecast.MonthlyForecastId;


                    }

                    var modellist = (from x in _montlhyForecastDataRepository.Table
                                     where x.SiteMetricId == monthlydata.SiteMetricId &&
                                   x.MonthlyForecast.UniqueId == monthlydata.ForecastId &&
                                   x.MonthlyForecast.Year == forecastYear &&
                                   x.MonthlyForecast.Month == forecastMonth &&
                                   x.MonthlyForecastId == MonthlyForecastId
                                     select x).FirstOrDefault();

                    if (modellist != null)
                    {
                        modellist.M1 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM1 : monthlydata.M1 ?? modellist.M1;
                        modellist.M2 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM2 : monthlydata.M2 ?? modellist.M2;
                        modellist.M3 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM3 : monthlydata.M3 ?? modellist.M3;
                        modellist.M4 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM4 : monthlydata.M4 ?? modellist.M4;
                        modellist.M5 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM5 : monthlydata.M5 ?? modellist.M5;
                        modellist.M6 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM6 : monthlydata.M6 ?? modellist.M6;
                        modellist.M7 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM7 : monthlydata.M7 ?? modellist.M7;
                        modellist.M8 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM8 : monthlydata.M8 ?? modellist.M8;
                        modellist.M9 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM9 : monthlydata.M9 ?? modellist.M9;
                        modellist.M10 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM10 : monthlydata.M10 ?? modellist.M10;
                        modellist.M11 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM11 : monthlydata.M11 ?? modellist.M11;
                        modellist.M12 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M12 ?? modellist.M12;

                        modellist.M13 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM13 : monthlydata.M13 ?? modellist.M13;
                        modellist.M14 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM14 : monthlydata.M14 ?? modellist.M14;
                        modellist.M15 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM15 : monthlydata.M15 ?? modellist.M15;
                        modellist.M16 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM16 : monthlydata.M16 ?? modellist.M16;
                        modellist.M17 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM17 : monthlydata.M17 ?? modellist.M17;
                        modellist.M18 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM18 : monthlydata.M18 ?? modellist.M18;
                        modellist.M19 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM19 : monthlydata.M19 ?? modellist.M19;
                        modellist.M20 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM20 : monthlydata.M20 ?? modellist.M20;
                        modellist.M21 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM21 : monthlydata.M21 ?? modellist.M21;
                        modellist.M22 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM22 : monthlydata.M22 ?? modellist.M22;
                        modellist.M23 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM23 : monthlydata.M23 ?? modellist.M23;
                        modellist.M24 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM24 : monthlydata.M24 ?? modellist.M24;

                        modellist.Y1 = monthlydata.Y1 ?? modellist.Y1;
                        modellist.Y2 = monthlydata.Y2 ?? modellist.Y2;
                        modellist.Y3 = monthlydata.Y3 ?? modellist.Y3;
                        modellist.Y4 = monthlydata.Y4 ?? modellist.Y4;
                        modellist.Y5 = monthlydata.Y5 ?? modellist.Y5;
                        modellist.Y6 = monthlydata.Y6 ?? modellist.Y6;
                        modellist.Y7 = monthlydata.Y7 ?? modellist.Y7;
                        modellist.Y8 = monthlydata.Y8 ?? modellist.Y8;
                        modellist.Y9 = monthlydata.Y9 ?? modellist.Y9;
                        modellist.Y10 = monthlydata.Y10 ?? modellist.Y10;
                        modellist.Y11 = monthlydata.Y11 ?? modellist.Y11;
                        modellist.Y12 = monthlydata.Y12 ?? modellist.Y12;
                        modellist.Y13 = monthlydata.Y13 ?? modellist.Y13;
                        modellist.Y14 = monthlydata.Y14 ?? modellist.Y14;
                        modellist.Y15 = monthlydata.Y15 ?? modellist.Y15;
                        modellist.Y16 = monthlydata.Y16 ?? modellist.Y16;
                        modellist.Y17 = monthlydata.Y17 ?? modellist.Y17;
                        modellist.Y18 = monthlydata.Y18 ?? modellist.Y18;
                        modellist.Y19 = monthlydata.Y19 ?? modellist.Y19;
                        modellist.Y20 = monthlydata.Y20 ?? modellist.Y20;
                        modellist.Y21 = monthlydata.Y21 ?? modellist.Y21;
                        modellist.Y22 = monthlydata.Y22 ?? modellist.Y22;
                        modellist.Y23 = monthlydata.Y23 ?? modellist.Y23;
                        modellist.Y24 = monthlydata.Y24 ?? modellist.Y24;
                        modellist.Y25 = monthlydata.Y25 ?? modellist.Y25;

                        modellist.ModifiedBy = ProjectSession.UserID;
                        modellist.ModifiedDate = DateTime.Now;

                        _montlhyForecastDataRepository.Update(modellist);

                        var forecastDate = new DateTime(
                            modellist.MonthlyForecast.Year, modellist.MonthlyForecast.Month, 1);
                        UpdateOrInsertToInsightsTable(modellist.SiteMetrics.SiteMetricId, forecastDate);
                    }
                    else
                    {
                        var objmonthlyforecastdata = new MonthlyForecastData();

                        objmonthlyforecastdata.M1 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM1 : monthlydata.M1;
                        objmonthlyforecastdata.M2 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM2 : monthlydata.M2;
                        objmonthlyforecastdata.M3 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM3 : monthlydata.M3;
                        objmonthlyforecastdata.M4 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM4 : monthlydata.M4;
                        objmonthlyforecastdata.M5 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM5 : monthlydata.M5;
                        objmonthlyforecastdata.M6 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM6 : monthlydata.M6;
                        objmonthlyforecastdata.M7 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM7 : monthlydata.M7;
                        objmonthlyforecastdata.M8 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM8 : monthlydata.M8;
                        objmonthlyforecastdata.M9 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM9 : monthlydata.M9;
                        objmonthlyforecastdata.M10 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM10 : monthlydata.M10;
                        objmonthlyforecastdata.M11 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM11 : monthlydata.M11;
                        objmonthlyforecastdata.M12 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M12;

                        objmonthlyforecastdata.M13 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M13;
                        objmonthlyforecastdata.M14 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M14;
                        objmonthlyforecastdata.M15 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M15;
                        objmonthlyforecastdata.M16 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M16;
                        objmonthlyforecastdata.M17 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M17;
                        objmonthlyforecastdata.M18 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M18;
                        objmonthlyforecastdata.M19 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M19;
                        objmonthlyforecastdata.M20 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M20;
                        objmonthlyforecastdata.M21 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M21;
                        objmonthlyforecastdata.M22 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M22;
                        objmonthlyforecastdata.M23 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M23;
                        objmonthlyforecastdata.M24 = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughtputM12 : monthlydata.M24;

                        objmonthlyforecastdata.Y1 = monthlydata.Y1;
                        objmonthlyforecastdata.Y2 = monthlydata.Y2;
                        objmonthlyforecastdata.Y3 = monthlydata.Y3;
                        objmonthlyforecastdata.Y4 = monthlydata.Y4;
                        objmonthlyforecastdata.Y5 = monthlydata.Y5;
                        objmonthlyforecastdata.Y6 = monthlydata.Y6;
                        objmonthlyforecastdata.Y7 = monthlydata.Y7;
                        objmonthlyforecastdata.Y8 = monthlydata.Y8;
                        objmonthlyforecastdata.Y9 = monthlydata.Y9;
                        objmonthlyforecastdata.Y10 = monthlydata.Y10;
                        objmonthlyforecastdata.Y11 = monthlydata.Y11;
                        objmonthlyforecastdata.Y12 = monthlydata.Y12;
                        objmonthlyforecastdata.Y13 = monthlydata.Y13;
                        objmonthlyforecastdata.Y14 = monthlydata.Y14;
                        objmonthlyforecastdata.Y15 = monthlydata.Y15;
                        objmonthlyforecastdata.Y16 = monthlydata.Y16;
                        objmonthlyforecastdata.Y17 = monthlydata.Y17;
                        objmonthlyforecastdata.Y18 = monthlydata.Y18;
                        objmonthlyforecastdata.Y19 = monthlydata.Y19;
                        objmonthlyforecastdata.Y20 = monthlydata.Y20;
                        objmonthlyforecastdata.Y21 = monthlydata.Y21;
                        objmonthlyforecastdata.Y22 = monthlydata.Y22;
                        objmonthlyforecastdata.Y23 = monthlydata.Y23;
                        objmonthlyforecastdata.Y24 = monthlydata.Y24;
                        objmonthlyforecastdata.Y25 = monthlydata.Y25;

                        objmonthlyforecastdata.MonthlyForecastId = MonthlyForecastId;
                        objmonthlyforecastdata.SiteMetricId = monthlydata.SiteMetricId;
                        objmonthlyforecastdata.CreatedBy = ProjectSession.UserID;
                        objmonthlyforecastdata.CreatedDate = DateTime.Now;

                        _montlhyForecastDataRepository.Insert(objmonthlyforecastdata);

                        var forecastDate = new DateTime(
                            objmonthlyforecastdata.MonthlyForecast.Year, objmonthlyforecastdata.MonthlyForecast.Month, 1);
                        UpdateOrInsertToInsightsTable(modellist.SiteMetrics.SiteMetricId, forecastDate);

                    }

                }

                //Update Forecast approved to Monthly Budget table
                UpdateApprovedForecast(siteId, forecastUniqueId);
                UpdateRollingForecastFinanceMetrics(siteId, forecastMonth, forecastYear);

            }
            return lstInvalid;
        }

        private void UpdateApprovedForecast(int siteId, string ForecastUniqueId)
        {

            object[] xparams = {
                            new SqlParameter("SiteId", siteId),
                              new SqlParameter("ForecastUniqueId", ForecastUniqueId),
                         };

            var obj = _dbContext.ExecuteStoredProcedureList<RPT_UpdateForecastApproved_Result>("RPT_UpdateForecastApproved", xparams);
            try
            {
                int year = 0, month = 0;
                year = Convert.ToInt32(ForecastUniqueId.Trim().Split('-')[0]);
                month = Convert.ToInt32(ForecastUniqueId.Trim().Split('-')[1]);

                for (int i = 0; i < 12; i++)
                {
                    month = month + 1;
                    if (month > 12)
                    {
                        month = 1;
                        year = year + 1;
                    }
                    UpdateFinanceMetricsData_Forecast(siteId, month, year);
                }
            }
            catch (Exception)
            {

            }

        }

        private void UpdateRollingForecastFinanceMetrics(int siteId, int month, int year)
        {
            object[] xparams = {
                            new SqlParameter("SiteId", siteId),
                              new SqlParameter("Month", month),
                                new SqlParameter("Year", year),
                         };

            var Result = _dbContext.ExecuteStoredProcedureList<RPT_CalculateFinanceMetrics_Result>("RPT_CalculateFinanceMetrics_RollingForecast", xparams);
        }

        private void UpdateFinanceMetricsData_Forecast(int siteId, int month, int year)
        {

            object[] xparams = {
                            new SqlParameter("SiteId", siteId),
                              new SqlParameter("Month", month),
                                new SqlParameter("Year", year),
                         };

            var Result = _dbContext.ExecuteStoredProcedureList<RPT_CalculateFinanceMetrics_Result>("RPT_CalculateFinanceMetrics_Forecast", xparams);
        }

        public void UpdateMonthlyForecastData(MonthlyForecastData ForecastData)
        {
            var model = (from x in _montlhyForecastDataRepository.Table
                         where x.MonthlyForecastDataId == ForecastData.MonthlyForecastDataId
                         select x).FirstOrDefault();

            int tempMonth = Convert.ToInt32(model.MonthlyForecast.Month);
            int tempYear = Convert.ToInt32(model.MonthlyForecast.Year);

            if (model != null)
            {
                model.M1 = ForecastData.M1;
                model.M2 = ForecastData.M2;
                model.M3 = ForecastData.M3;
                model.M4 = ForecastData.M4;
                model.M5 = ForecastData.M5;
                model.M6 = ForecastData.M6;
                model.M7 = ForecastData.M7;
                model.M8 = ForecastData.M8;
                model.M9 = ForecastData.M9;
                model.M10 = ForecastData.M10;
                model.M11 = ForecastData.M11;
                model.M12 = ForecastData.M12;

                model.M13 = ForecastData.M13;
                model.M14 = ForecastData.M14;
                model.M15 = ForecastData.M15;
                model.M16 = ForecastData.M16;
                model.M17 = ForecastData.M17;
                model.M18 = ForecastData.M18;
                model.M19 = ForecastData.M19;
                model.M20 = ForecastData.M20;
                model.M21 = ForecastData.M21;
                model.M22 = ForecastData.M22;
                model.M23 = ForecastData.M23;
                model.M24 = ForecastData.M24;

                model.Y1 = ForecastData.Y1;
                model.Y2 = ForecastData.Y2;
                model.Y3 = ForecastData.Y3;
                model.Y4 = ForecastData.Y4;
                model.Y5 = ForecastData.Y5;
                model.Y6 = ForecastData.Y6;
                model.Y7 = ForecastData.Y7;
                model.Y8 = ForecastData.Y8;
                model.Y9 = ForecastData.Y9;
                model.Y10 = ForecastData.Y10;
                model.Y11 = ForecastData.Y11;
                model.Y12 = ForecastData.Y12;
                model.Y13 = ForecastData.Y13;
                model.Y14 = ForecastData.Y14;
                model.Y15 = ForecastData.Y15;
                model.Y16 = ForecastData.Y16;
                model.Y17 = ForecastData.Y17;
                model.Y18 = ForecastData.Y18;
                model.Y19 = ForecastData.Y19;
                model.Y20 = ForecastData.Y20;
                model.Y21 = ForecastData.Y21;
                model.Y22 = ForecastData.Y22;
                model.Y23 = ForecastData.Y23;
                model.Y24 = ForecastData.Y24;
                model.Y25 = ForecastData.Y25;

                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
                _montlhyForecastDataRepository.Update(model);

                var forecastDate = new DateTime(
                    model.MonthlyForecast.Year, model.MonthlyForecast.Month, 1);
                UpdateOrInsertToInsightsTable(model.SiteMetrics.SiteMetricId, forecastDate);

                var siteId = (from p in _dbContext.SiteMetrics
                              where p.SiteMetricId == model.SiteMetricId
                              select p.SiteId).SingleOrDefault();

                if (model.SiteMetrics.MetricsName == "Mill Availability" ||
                    model.SiteMetrics.MetricsName == "Ore Milled")
                {

                    var millOred = (from x in _montlhyForecastDataRepository.Table
                                    where x.MonthlyForecastId == model.MonthlyForecastId
                                          && x.SiteMetrics.MetricId == 5
                                    select x).FirstOrDefault();

                    var millAvailability = (from x in _montlhyForecastDataRepository.Table
                                            where x.MonthlyForecastId == model.MonthlyForecastId
                                                  && x.SiteMetrics.MetricId == 49
                                            select x).FirstOrDefault();

                    if (millOred != null && millAvailability != null)
                    {
                        // Set Mill Ored Forcast Data
                        var millOredM1 = millOred.M1 == null ? 0 : millOred.M1;
                        var millOredM2 = millOred.M2 == null ? 0 : millOred.M2;
                        var millOredM3 = millOred.M3 == null ? 0 : millOred.M3;
                        var millOredM4 = millOred.M4 == null ? 0 : millOred.M4;
                        var millOredM5 = millOred.M5 == null ? 0 : millOred.M5;
                        var millOredM6 = millOred.M6 == null ? 0 : millOred.M6;
                        var millOredM7 = millOred.M7 == null ? 0 : millOred.M7;
                        var millOredM8 = millOred.M8 == null ? 0 : millOred.M8;
                        var millOredM9 = millOred.M9 == null ? 0 : millOred.M9;
                        var millOredM10 = millOred.M10 == null ? 0 : millOred.M10;
                        var millOredM11 = millOred.M11 == null ? 0 : millOred.M11;
                        var millOredM12 = millOred.M12 == null ? 0 : millOred.M12;

                        var millOredM13 = millOred.M13 == null ? 0 : millOred.M13;
                        var millOredM14 = millOred.M14 == null ? 0 : millOred.M14;
                        var millOredM15 = millOred.M15 == null ? 0 : millOred.M15;
                        var millOredM16 = millOred.M16 == null ? 0 : millOred.M16;
                        var millOredM17 = millOred.M17 == null ? 0 : millOred.M17;
                        var millOredM18 = millOred.M18 == null ? 0 : millOred.M18;
                        var millOredM19 = millOred.M19 == null ? 0 : millOred.M19;
                        var millOredM20 = millOred.M20 == null ? 0 : millOred.M20;
                        var millOredM21 = millOred.M21 == null ? 0 : millOred.M21;
                        var millOredM22 = millOred.M22 == null ? 0 : millOred.M22;
                        var millOredM23 = millOred.M23 == null ? 0 : millOred.M23;
                        var millOredM24 = millOred.M24 == null ? 0 : millOred.M24;

                        // Set Mill Availability Forcast Data
                        var millAvailabilityM1 = millAvailability.M1 == null ? 0 : millAvailability.M1;
                        var millAvailabilityM2 = millAvailability.M2 == null ? 0 : millAvailability.M2;
                        var millAvailabilityM3 = millAvailability.M3 == null ? 0 : millAvailability.M3;
                        var millAvailabilityM4 = millAvailability.M4 == null ? 0 : millAvailability.M4;
                        var millAvailabilityM5 = millAvailability.M5 == null ? 0 : millAvailability.M5;
                        var millAvailabilityM6 = millAvailability.M6 == null ? 0 : millAvailability.M6;
                        var millAvailabilityM7 = millAvailability.M7 == null ? 0 : millAvailability.M7;
                        var millAvailabilityM8 = millAvailability.M8 == null ? 0 : millAvailability.M8;
                        var millAvailabilityM9 = millAvailability.M9 == null ? 0 : millAvailability.M9;
                        var millAvailabilityM10 = millAvailability.M10 == null ? 0 : millAvailability.M10;
                        var millAvailabilityM11 = millAvailability.M11 == null ? 0 : millAvailability.M11;
                        var millAvailabilityM12 = millAvailability.M12 == null ? 0 : millAvailability.M12;

                        var millAvailabilityM13 = millAvailability.M13 == null ? 0 : millAvailability.M13;
                        var millAvailabilityM14 = millAvailability.M14 == null ? 0 : millAvailability.M14;
                        var millAvailabilityM15 = millAvailability.M15 == null ? 0 : millAvailability.M15;
                        var millAvailabilityM16 = millAvailability.M16 == null ? 0 : millAvailability.M16;
                        var millAvailabilityM17 = millAvailability.M17 == null ? 0 : millAvailability.M17;
                        var millAvailabilityM18 = millAvailability.M18 == null ? 0 : millAvailability.M18;
                        var millAvailabilityM19 = millAvailability.M19 == null ? 0 : millAvailability.M19;
                        var millAvailabilityM20 = millAvailability.M20 == null ? 0 : millAvailability.M20;
                        var millAvailabilityM21 = millAvailability.M21 == null ? 0 : millAvailability.M21;
                        var millAvailabilityM22 = millAvailability.M22 == null ? 0 : millAvailability.M22;
                        var millAvailabilityM23 = millAvailability.M23 == null ? 0 : millAvailability.M23;
                        var millAvailabilityM24 = millAvailability.M24 == null ? 0 : millAvailability.M24;

                        int M1Days = (tempMonth + 1) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 1) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 1) - 12);
                        int M2Days = (tempMonth + 2) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 2) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 2) - 12);
                        int M3Days = (tempMonth + 3) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 3) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 3) - 12);
                        int M4Days = (tempMonth + 4) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 4) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 4) - 12);
                        int M5Days = (tempMonth + 5) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 5) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 5) - 12);
                        int M6Days = (tempMonth + 6) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 6) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 6) - 12);
                        int M7Days = (tempMonth + 7) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 7) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 7) - 12);
                        int M8Days = (tempMonth + 8) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 8) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 8) - 12);
                        int M9Days = (tempMonth + 9) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 9) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 9) - 12);
                        int M10Days = (tempMonth + 10) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 10) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 10) - 12);
                        int M11Days = (tempMonth + 11) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 11) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 11) - 12);
                        int M12Days = (tempMonth + 12) <= 12 ? DateTime.DaysInMonth(tempYear, tempMonth + 12) : DateTime.DaysInMonth(tempYear + 1, (tempMonth + 12) - 12);

                        int M13Days = (tempMonth + 13) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 1) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 1) - 12);
                        int M14Days = (tempMonth + 14) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 2) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 2) - 12);
                        int M15Days = (tempMonth + 15) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 3) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 3) - 12);
                        int M16Days = (tempMonth + 16) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 4) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 4) - 12);
                        int M17Days = (tempMonth + 17) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 5) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 5) - 12);
                        int M18Days = (tempMonth + 18) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 6) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 6) - 12);
                        int M19Days = (tempMonth + 19) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 7) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 7) - 12);
                        int M20Days = (tempMonth + 20) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 8) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 8) - 12);
                        int M21Days = (tempMonth + 21) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 9) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 9) - 12);
                        int M22Days = (tempMonth + 22) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 10) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 10) - 12);
                        int M23Days = (tempMonth + 23) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 11) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 11) - 12);
                        int M24Days = (tempMonth + 24) <= 24 ? DateTime.DaysInMonth(tempYear + 1, tempMonth + 12) : DateTime.DaysInMonth(tempYear + 2, (tempMonth + 12) - 12);

                        var millThroughtputM1 = millAvailabilityM1 == 0 ? 0 : millOredM1 / (M1Days * 24 * (millAvailabilityM1) / 100);
                        var millThroughtputM2 = millAvailabilityM2 == 0 ? 0 : millOredM2 / (M2Days * 24 * (millAvailabilityM2) / 100);
                        var millThroughtputM3 = millAvailabilityM3 == 0 ? 0 : millOredM3 / (M3Days * 24 * (millAvailabilityM3) / 100);
                        var millThroughtputM4 = millAvailabilityM4 == 0 ? 0 : millOredM4 / (M4Days * 24 * (millAvailabilityM4) / 100);
                        var millThroughtputM5 = millAvailabilityM5 == 0 ? 0 : millOredM5 / (M5Days * 24 * (millAvailabilityM5) / 100);
                        var millThroughtputM6 = millAvailabilityM6 == 0 ? 0 : millOredM6 / (M6Days * 24 * (millAvailabilityM6) / 100);
                        var millThroughtputM7 = millAvailabilityM7 == 0 ? 0 : millOredM7 / (M7Days * 24 * (millAvailabilityM7) / 100);
                        var millThroughtputM8 = millAvailabilityM8 == 0 ? 0 : millOredM8 / (M8Days * 24 * (millAvailabilityM8) / 100);
                        var millThroughtputM9 = millAvailabilityM9 == 0 ? 0 : millOredM9 / (M9Days * 24 * (millAvailabilityM9) / 100);
                        var millThroughtputM10 = millAvailabilityM10 == 0 ? 0 : millOredM10 / (M10Days * 24 * (millAvailabilityM10) / 100);
                        var millThroughtputM11 = millAvailabilityM11 == 0 ? 0 : millOredM11 / (M11Days * 24 * (millAvailabilityM11) / 100);
                        var millThroughtputM12 = millAvailabilityM12 == 0 ? 0 : millOredM12 / (M12Days * 24 * (millAvailabilityM12) / 100);

                        var millThroughtputM13 = millAvailabilityM13 == 0 ? 0 : millOredM13 / (M13Days * 24 * (millAvailabilityM13) / 100);
                        var millThroughtputM14 = millAvailabilityM14 == 0 ? 0 : millOredM14 / (M14Days * 24 * (millAvailabilityM14) / 100);
                        var millThroughtputM15 = millAvailabilityM15 == 0 ? 0 : millOredM15 / (M15Days * 24 * (millAvailabilityM15) / 100);
                        var millThroughtputM16 = millAvailabilityM16 == 0 ? 0 : millOredM16 / (M16Days * 24 * (millAvailabilityM16) / 100);
                        var millThroughtputM17 = millAvailabilityM17 == 0 ? 0 : millOredM17 / (M17Days * 24 * (millAvailabilityM17) / 100);
                        var millThroughtputM18 = millAvailabilityM18 == 0 ? 0 : millOredM18 / (M18Days * 24 * (millAvailabilityM18) / 100);
                        var millThroughtputM19 = millAvailabilityM19 == 0 ? 0 : millOredM19 / (M19Days * 24 * (millAvailabilityM19) / 100);
                        var millThroughtputM20 = millAvailabilityM20 == 0 ? 0 : millOredM20 / (M20Days * 24 * (millAvailabilityM20) / 100);
                        var millThroughtputM21 = millAvailabilityM21 == 0 ? 0 : millOredM21 / (M21Days * 24 * (millAvailabilityM21) / 100);
                        var millThroughtputM22 = millAvailabilityM22 == 0 ? 0 : millOredM22 / (M22Days * 24 * (millAvailabilityM22) / 100);
                        var millThroughtputM23 = millAvailabilityM23 == 0 ? 0 : millOredM23 / (M23Days * 24 * (millAvailabilityM23) / 100);
                        var millThroughtputM24 = millAvailabilityM24 == 0 ? 0 : millOredM24 / (M24Days * 24 * (millAvailabilityM24) / 100);

                        var millThroughputModel = (from x in _montlhyForecastDataRepository.Table
                                                   where x.MonthlyForecastId == model.MonthlyForecastId
                                                         && x.SiteMetrics.MetricId == 4
                                                   select x).FirstOrDefault();

                        if (millThroughputModel != null)
                        {
                            millThroughputModel.ModifiedBy = ProjectSession.UserID;
                            millThroughputModel.ModifiedDate = DateTime.Now;

                            millThroughputModel.M1 = millThroughtputM1 != 0 ? millThroughtputM1 : null;
                            millThroughputModel.M2 = millThroughtputM2 != 0 ? millThroughtputM2 : null;
                            millThroughputModel.M3 = millThroughtputM3 != 0 ? millThroughtputM3 : null;
                            millThroughputModel.M4 = millThroughtputM4 != 0 ? millThroughtputM4 : null;
                            millThroughputModel.M5 = millThroughtputM5 != 0 ? millThroughtputM5 : null;
                            millThroughputModel.M6 = millThroughtputM6 != 0 ? millThroughtputM6 : null;
                            millThroughputModel.M7 = millThroughtputM7 != 0 ? millThroughtputM7 : null;
                            millThroughputModel.M8 = millThroughtputM8 != 0 ? millThroughtputM8 : null;
                            millThroughputModel.M9 = millThroughtputM9 != 0 ? millThroughtputM9 : null;
                            millThroughputModel.M10 = millThroughtputM10 != 0 ? millThroughtputM10 : null;
                            millThroughputModel.M11 = millThroughtputM11 != 0 ? millThroughtputM11 : null;
                            millThroughputModel.M12 = millThroughtputM12 != 0 ? millThroughtputM12 : null;

                            millThroughputModel.M13 = millThroughtputM13 != 0 ? millThroughtputM13 : null;
                            millThroughputModel.M14 = millThroughtputM14 != 0 ? millThroughtputM14 : null;
                            millThroughputModel.M15 = millThroughtputM15 != 0 ? millThroughtputM15 : null;
                            millThroughputModel.M16 = millThroughtputM16 != 0 ? millThroughtputM16 : null;
                            millThroughputModel.M17 = millThroughtputM17 != 0 ? millThroughtputM17 : null;
                            millThroughputModel.M18 = millThroughtputM18 != 0 ? millThroughtputM18 : null;
                            millThroughputModel.M19 = millThroughtputM19 != 0 ? millThroughtputM19 : null;
                            millThroughputModel.M20 = millThroughtputM20 != 0 ? millThroughtputM20 : null;
                            millThroughputModel.M21 = millThroughtputM21 != 0 ? millThroughtputM21 : null;
                            millThroughputModel.M22 = millThroughtputM22 != 0 ? millThroughtputM22 : null;
                            millThroughputModel.M23 = millThroughtputM23 != 0 ? millThroughtputM23 : null;
                            millThroughputModel.M24 = millThroughtputM24 != 0 ? millThroughtputM24 : null;

                            //Update Mill Throught put data.
                            _montlhyForecastDataRepository.Update(millThroughputModel);
                            var mtpForecastDate = new DateTime(
                                millThroughputModel.MonthlyForecast.Year, millThroughputModel.MonthlyForecast.Month, 1);
                            UpdateOrInsertToInsightsTable(model.SiteMetrics.SiteMetricId, mtpForecastDate);
                        }
                    }

                }
                var query = (from x in _montlhyForecastRepository.Table
                             where x.MonthlyForecastId == model.MonthlyForecastId
                             select x).FirstOrDefault();
                if (query != null)
                {
                    //Update Forecast approved to Monthly Budget table
                    UpdateApprovedForecast(query.SiteId, query.UniqueId);
                    UpdateRollingForecastFinanceMetrics(query.SiteId, query.Month, query.Year);
                }

            }
        }

        public bool ApproveMonthlyForecastData(int SiteId, string forecast)
        {
            try
            {
                var obj = (from w in _montlhyForecastRepository.Table
                           where w.SiteId == SiteId &&
                                 w.UniqueId == forecast
                           select w).FirstOrDefault();

                if (obj != null)
                {
                    if (obj.IsApprove != true)
                    {
                        obj.IsApprove = true;
                        obj.ModifiedBy = ProjectSession.UserID;
                        obj.ModifiedDate = DateTime.Now;

                        _montlhyForecastRepository.Update(obj);
                    }

                    //Update Forecast approved to Monthly Budget table
                    UpdateApprovedForecast(SiteId, forecast);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool CheckApproveStatus(int SiteId, string forecast)
        {
            var obj = (from w in _montlhyForecastRepository.Table
                       where w.SiteId == SiteId &&
                             w.UniqueId == forecast
                       select w).FirstOrDefault();
            if (obj != null)
            {
                return obj.IsApprove;
            }
            else
            {
                return false;
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

        private void UpdateOrInsertToInsightsTable(int siteMetricId, DateTime forecastDate)
        {
            var m1Date = forecastDate.AddMonths(1); // Only do forecast updates for the current month
            var startOfMonth = new DateTime(m1Date.Year, m1Date.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            for (var date = startOfMonth.Date; date <= endOfMonth; date = date.AddDays(1))
            {
                object[] xparams = {
                    new SqlParameter("SiteMetricId", siteMetricId),
                    new SqlParameter("Date", date),
                };

                _dbContext.ExecuteStoredProcedureList<int>("GEN_UpdateInsightsTable", xparams);
            }
        }

        #endregion
    }
}
