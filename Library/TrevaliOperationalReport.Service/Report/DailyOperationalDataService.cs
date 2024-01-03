using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;
using TrevaliOperationalReport.Domain.Report;

namespace TrevaliOperationalReport.Service.Report
{
    public class DailyOperationalDataService : IDailyOperationalDataService
    {
        #region Fields

        private readonly IRepository<DailyOperationalData> _dailyOperationalDataRepository;
        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<Metrics> _metricsRepository;
        private readonly IRepository<Reports> _reportRepository;
        private readonly IRepository<DailyShiftOperationalData> _dailyShiftOperationalDataRepository;
        public static TrevaliOperationalReportObjectContext _dbContext;

        #endregion

        #region Ctor

        public DailyOperationalDataService(IRepository<DailyOperationalData> dailyOperationalDataRepository, IRepository<Site> siteRepository,
            IRepository<Metrics> metricsRepository, IRepository<Reports> reportRepository, IRepository<DailyShiftOperationalData> dailyShiftOperationalDataRepository)
        {
            _dailyOperationalDataRepository = dailyOperationalDataRepository;
            _siteRepository = siteRepository;
            _metricsRepository = metricsRepository;
            _reportRepository = reportRepository;
            _dailyShiftOperationalDataRepository = dailyShiftOperationalDataRepository;
            _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets sites  selectlist.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetSitesSelectList(int siteId = 0, int IsSync = -1)
        {
            bool BoolIsSync = false;
            if (IsSync != -1)
                BoolIsSync = Convert.ToBoolean(IsSync);

            if (siteId == 0)
            {
                var query = from p in _siteRepository.Table
                            where ((p.IsSync == BoolIsSync || (p.IsSync == null && !BoolIsSync)) || IsSync == -1)
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
            if (siteId > 0 && ReportId > 0)
            {

                var list = new List<SelectListItem>();
                var metrics = (from p in _dbContext.SiteMetrics
                               where p.SiteId == siteId
                               && p.ReportId == ReportId
                               select p).OrderBy(a => a.SectionId).ToList();



                if (metrics != null && metrics.Count() > 0)
                {
                    foreach (var itm in metrics)
                    {

                        var listitem = (from p in _metricsRepository.Table
                                        where p.MetricId == itm.MetricId
                                        select new SelectListItem
                                        {
                                            Text = itm.Section.SectionName + "^" + p.MetricsName,
                                            Value = itm.SiteMetricId.ToString()
                                        }).ToList();
                        list.AddRange(listitem);
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
        /// Get Weekly data metrics 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="ReportId"></param>
        /// <param name="Week"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        public IList<DailyMetricsData> GetDailyOperationalDataMetrics(int SiteId, int ReportId, int Month, int Year, int Day)
        {
            var lstdailyMetricsData = new List<DailyMetricsData>();

            var querySections = (from p in _dbContext.SiteMetrics
                                 where p.SiteId == SiteId &&
                                 p.ReportId == ReportId
                                 select p.SectionId).Distinct().ToList();

            lstdailyMetricsData = new List<DailyMetricsData>();
            if (querySections != null && querySections.Count > 0)
            {

                foreach (int sectionid in querySections)
                {
                    if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == sectionid))
                    {
                        var sectionname = (from p in _dbContext.Section where p.SectionId == sectionid select p.SectionName).FirstOrDefault().ToString();
                        var metrics = (from p in _dbContext.SiteMetrics
                                       where p.SectionId == sectionid &&
                                       p.ReportId == ReportId &&
                                       p.SiteId == SiteId
                                       select
                                           p.SiteMetricId
                                       ).Distinct().ToList();


                        if (metrics != null && metrics.Count > 0)
                        {
                            var queryDailydata = (from p in _dailyOperationalDataRepository.Table
                                                  where
                                                  p.Date.Month == Month &&
                                                  p.Date.Year == Year &&
                                                   (p.Date.Day == Day || Day == 0) &&
                                                  metrics.Contains(p.SiteMetricId)
                                                  select p).ToList();

                            if (queryDailydata != null && queryDailydata.Count > 0)
                            {

                                foreach (DailyOperationalData d in queryDailydata)
                                {
                                    //decimal weeklyplan = 0;
                                    //decimal weeklybudget = 0;

                                    bool IsDaily = _dailyShiftOperationalDataRepository.Table.Any(z => z.DailyOperationalDataId == d.DailyOperationalDataId);

                                    lstdailyMetricsData.Add(new DailyMetricsData
                                    {
                                        DailyOperationalDataId = d.DailyOperationalDataId,
                                        SiteMetricId = d.SiteMetricId,
                                        Month = d.Date.Month,
                                        Year = d.Date.Year,
                                        SectionId = sectionid,
                                        SectionName = sectionname,
                                        MetricName = d.SiteMetrics.Metric.MetricsName + " (" + d.SiteMetrics.Metric.Unit.UOM.Trim() + ")",
                                        ActualValueDaily = d.ActualValue,
                                        Date = d.Date,
                                        Metric = d.SiteMetrics.Metric,
                                        IsDailyDataAvailable = IsDaily
                                    });


                                    //decimal MTDVal = 0;

                                    //weekly budget and plan calculation from monthly data.
                                    //var FirstDay = CommonHelper.FirstDateOfWeek(Year, Week);
                                    //var LastDay = FirstDay.AddDays(6);

                                    //var monthlydata1 = (from p in _dbContext.MonthlyBudgetPlanData
                                    //                    where p.SiteMetrics.MetricId == w.SiteMetrics.MetricId && p.SiteMetrics.SiteId == w.SiteMetrics.SiteId && p.Month == FirstDay.Month && p.Year == weeklydata.Year
                                    //                    select p).FirstOrDefault();
                                    //decimal monthlyBudget1 = (decimal)(monthlydata1 == null ? 0 : (monthlydata1.Budget == null ? 0 : monthlydata1.Budget));
                                    //decimal monthlyPlan1 = (decimal)(monthlydata1 == null ? 0 : (monthlydata1.Plan == null ? 0 : monthlydata1.Plan));
                                    //decimal monthlyBudget2 = 0;
                                    //decimal monthlyPlan2 = 0;
                                    //if (FirstDay.Month != LastDay.Month)
                                    //{
                                    //    var monthlydata2 = (from p in _dbContext.MonthlyBudgetPlanData
                                    //                        where p.SiteMetrics.MetricId == w.SiteMetrics.MetricId && p.SiteMetrics.SiteId == w.SiteMetrics.SiteId && p.Month == LastDay.Month && p.Year == weeklydata.Year
                                    //                        select p).FirstOrDefault();
                                    //    monthlyBudget2 = (decimal)(monthlydata2 == null ? 0 : (monthlydata2.Budget == null ? 0 : monthlydata2.Budget));
                                    //    monthlyPlan2 = (decimal)(monthlydata2 == null ? 0 : (monthlydata2.Plan == null ? 0 : monthlydata2.Plan));
                                    //}

                                    //weeklyplan = CommonHelper.CalculateBudgetWeeklyFromMonthly(FirstDay, LastDay, monthlyPlan1, monthlyPlan2);
                                    //weeklybudget = CommonHelper.CalculateBudgetWeeklyFromMonthly(FirstDay, LastDay, monthlyBudget1, monthlyBudget2);

                                    //MTD Values calculation
                                    //var weeklydataForMTD = (from p in _dbContext.WeeklyOperationalData
                                    //                        where p.SiteMetricId == w.SiteMetricId && p.Year == weeklydata.Year && p.Week < weeklydata.Week
                                    //                        select p).ToList();

                                    //MTDVal = CommonHelper.CalculateMTDVal(weeklydataForMTD, weeklydata);


                                }
                            }
                        }
                    }

                }

            }
            lstdailyMetricsData = lstdailyMetricsData.OrderBy(x => x.Date).ToList();
            return lstdailyMetricsData;
        }


        /// <summary>
        /// Updates DailyData
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="ReportId"></param>
        /// <returns></returns>
        public void UpdateDailyData(DailyMetricsData DailyData)
        {
            var model = (from x in _dailyOperationalDataRepository.Table
                         where x.DailyOperationalDataId == DailyData.DailyOperationalDataId
                         select x).FirstOrDefault();

            if (model != null)
            {
                model.ActualValue = DailyData.ActualValueDaily;

                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
                _dailyOperationalDataRepository.Update(model);

            }
        }

        /// <summary>
        /// Get daily operational data list
        /// </summary>
        /// <param name="WeeklyOperationalDataId"></param>
        /// <returns></returns>
        public IList<DailyShiftMetricsData> GetDailyShiftOperationalDataList(int DailyOperationalDataId)
        {
            return (from p in _dailyShiftOperationalDataRepository.Table
                    where
                        p.DailyOperationalDataId == DailyOperationalDataId
                    select new DailyShiftMetricsData
                    {
                        DailyOperationalDataId = p.DailyOperationalDataId,
                        DailyShiftOperationalDataId = p.DailyShiftOperationalDataId,
                        Equipment = p.Equipment == null ? null : p.Equipment,
                        EquipmentName = p.Equipment == null ? "" : p.Equipment.EquipmentName,
                        EquipmentId = p.Equipment == null ? 0 : p.Equipment.EquipmentId,
                        ActualValueShift = p.ActualValue,
                        Shift = p.Shift == null ? null : p.Shift,
                        ShiftId = p.ShiftId,
                        ShiftName = p.Shift == null ? "" : p.Shift.ShiftName
                    }).OrderBy(x => x.ShiftName).ToList();

        }


        /// <summary>
        /// updates daily shift data
        /// </summary>
        /// <param name="DailyShiftData"></param>
        public void UpdateDailyShiftData(DailyShiftMetricsData DailyShiftData)
        {
            var model = (from x in _dailyShiftOperationalDataRepository.Table
                         where x.DailyOperationalDataId == DailyShiftData.DailyOperationalDataId
                         && x.DailyShiftOperationalDataId == DailyShiftData.DailyShiftOperationalDataId
                         select x).FirstOrDefault();


            if (model != null)
            {

                model.ActualValue = DailyShiftData.ActualValueShift;

                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
                _dailyShiftOperationalDataRepository.Update(model);

                UpdateActualValueInDailyData(DailyShiftData.DailyOperationalDataId);

            }
        }


        private void UpdateActualValueInDailyData(long DailyOperationalDataId)
        {
            var model = (from p in _dailyOperationalDataRepository.Table
                         where p.DailyOperationalDataId == DailyOperationalDataId
                         select p).FirstOrDefault();
            if (model != null)
            {
                if (model.SiteMetrics.Metric.Unit.UOM.Trim() == "%")
                {
                    var Average = (from x in _dailyShiftOperationalDataRepository.Table
                                   where x.DailyOperationalDataId == DailyOperationalDataId
                                   select x).ToList().Average(x => x.ActualValue);
                    model.ActualValue = Average;
                }
                else
                {
                    var TotalActualValue = (from x in _dailyShiftOperationalDataRepository.Table
                                            where x.DailyOperationalDataId == DailyOperationalDataId
                                            select x).ToList().Sum(x => x.ActualValue);
                    model.ActualValue = TotalActualValue;
                }
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
                _dailyOperationalDataRepository.Update(model);

            }



        }
        #endregion
    }
}
