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
    public class MonthlyPlanBudgetDataService : IMonthlyPlanBudgetDataService
    {
        #region Fields

        private readonly IRepository<MonthlyBudgetPlanData> _montlhyBudgetPlanDataRepository;
        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<Metrics> _metricsRepository;
        private readonly IRepository<Reports> _reportRepository;
        private readonly IRepository<SiteMetrics> _siteMetricsRepository;
        private readonly IRepository<MonthlyBudgetPlanDataApprove> _montlhyBudgetPlanDataApproveRepository;
        public static TrevaliOperationalReportObjectContext _dbContext;
        #endregion

        #region Ctor

        public MonthlyPlanBudgetDataService(IRepository<MonthlyBudgetPlanData> montlhyBudgetPlanDataRepository, IRepository<Site> siteRepository,
            IRepository<Metrics> metricsRepository, IRepository<Reports> reportRepository, IRepository<SiteMetrics> siteMetricsRepository, IRepository<MonthlyBudgetPlanDataApprove> montlhyBudgetPlanDataApproveRepository)
        {
            _montlhyBudgetPlanDataRepository = montlhyBudgetPlanDataRepository;
            _siteRepository = siteRepository;
            _metricsRepository = metricsRepository;
            _siteMetricsRepository = siteMetricsRepository;
            _reportRepository = reportRepository;
            _montlhyBudgetPlanDataApproveRepository = montlhyBudgetPlanDataApproveRepository;
            _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets the MonthlyBudgetPlanData by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>MonthlyBudgetPlanData.</returns>
        public MonthlyBudgetPlanData GetMonthlyBudgetPlanDataById(long id)
        {
            return _montlhyBudgetPlanDataRepository.GetById(id);
        }


        /// <summary>
        /// search the MonthlyBudgetPlanData .
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>MonthlyBudgetPlanData.</returns>
        public IList<MonthlyBudgetPlanData> SearchMonthlyBudgetPlan(int siteId, int reportId, int metricId, int month, int year)
        {
            var query = from p in _montlhyBudgetPlanDataRepository.Table
                        where (p.SiteMetrics.SiteId == siteId || siteId == 0) &&
                        (p.SiteMetrics.Report.ReportId == reportId || reportId == 0) &&
                        (p.SiteMetrics.MetricId == metricId || metricId == 0) &&
                        (p.Month == month || month == 0) &&
                        (p.Year == year || year == 0)
                        select p;
            return query.ToList().OrderBy(x => x.SiteMetrics.DisplayOrder).ToList();

        }

        /// <summary>
        /// Uploads monthly plan data
        /// </summary>
        /// <param name="lstMontlhyPlanData"></param>
        /// <returns></returns>
        public List<InvalidMonthlyPlanData> UploadMonthlyPlanData(List<MonthlyBudgetPlanData> lstMontlhyPlanData, int reportid)
        {
            //var InvalidCount = 0;
            //List<InvalidMonthlyPlanData> lstInvalid = new List<InvalidMonthlyPlanData>();
            //var sitename = lstMontlhyPlanData[0].SiteName;
            //var site = from p in _dbContext.Site where p.SiteName == sitename select p;
            //int siteId = site.FirstOrDefault().SiteId;

            //var sites = GetSitesSelectList(siteId);
            //var metrics = GetMetricsSelectList(siteId, reportid);
            //if (lstMontlhyPlanData != null && lstMontlhyPlanData.Count > 0)
            //{
            //    foreach (MonthlyBudgetPlanData monthlydata in lstMontlhyPlanData)
            //    {
            //        if (!sites.Any(x => x.Text == monthlydata.SiteName))
            //        {
            //            lstInvalid.Add(new InvalidMonthlyPlanData
            //            {
            //                SiteName = monthlydata.SiteName,
            //                SectionName = monthlydata.SectionName,
            //                MetricName = monthlydata.MetricName,
            //                Year = monthlydata.YearString,
            //                Month = monthlydata.MonthString,
            //                Plan = monthlydata.Plan,
            //                Budget = monthlydata.Budget,
            //                Recon = monthlydata.Recon,
            //                Comment = "Invalid Site"
            //            });
            //            InvalidCount++;
            //            continue;

            //        }
            //        else if (!metrics.Any(x => x.Text == monthlydata.SectionName + "-" + monthlydata.MetricName))
            //        {

            //            lstInvalid.Add(new InvalidMonthlyPlanData
            //            {
            //                SiteName = monthlydata.SiteName,
            //                SectionName = monthlydata.SectionName,
            //                MetricName = monthlydata.MetricName,
            //                Year = monthlydata.YearString,
            //                Month = monthlydata.MonthString,
            //                Plan = monthlydata.Plan,
            //                Budget = monthlydata.Budget,
            //                Recon = monthlydata.Recon,
            //                Comment = "Invalid Metric or Section name."
            //            });
            //            InvalidCount++;
            //            continue;
            //        }
            //        else if (monthlydata.Budget < 0)
            //        {
            //            lstInvalid.Add(new InvalidMonthlyPlanData
            //            {
            //                SiteName = monthlydata.SiteName,
            //                SectionName = monthlydata.SectionName,
            //                MetricName = monthlydata.MetricName,
            //                Year = monthlydata.YearString,
            //                Month = monthlydata.MonthString,
            //                Plan = monthlydata.Plan,
            //                Budget = monthlydata.Budget,
            //                Recon = monthlydata.Recon,
            //                Comment = "Budget value must be positive number"
            //            });
            //            InvalidCount++;
            //            continue;
            //        }
            //        else if (monthlydata.Plan < 0)
            //        {
            //            lstInvalid.Add(new InvalidMonthlyPlanData
            //            {
            //                SiteName = monthlydata.SiteName,
            //                SectionName = monthlydata.SectionName,
            //                MetricName = monthlydata.MetricName,
            //                Year = monthlydata.YearString,
            //                Month = monthlydata.MonthString,
            //                Plan = monthlydata.Plan,
            //                Budget = monthlydata.Budget,
            //                Recon = monthlydata.Recon,
            //                Comment = "Plan value must be positive number"
            //            });
            //            InvalidCount++;
            //            continue;
            //        }
            //        else if (monthlydata.Recon < 0)
            //        {
            //            lstInvalid.Add(new InvalidMonthlyPlanData
            //            {
            //                SiteName = monthlydata.SiteName,
            //                SectionName = monthlydata.SectionName,
            //                MetricName = monthlydata.MetricName,
            //                Year = monthlydata.YearString,
            //                Month = monthlydata.MonthString,
            //                Plan = monthlydata.Plan,
            //                Budget = monthlydata.Budget,
            //                Recon = monthlydata.Recon,
            //                Comment = "Recon value must be positive number"
            //            });
            //            InvalidCount++;
            //            continue;
            //        }
            //        else if (string.IsNullOrEmpty(monthlydata.YearString))
            //        //else if ((monthlydata.Year < DateTime.Now.Year - 10) || (monthlydata.Year > DateTime.Now.Year + 10))
            //        {
            //            lstInvalid.Add(new InvalidMonthlyPlanData
            //            {
            //                SiteName = monthlydata.SiteName,
            //                SectionName = monthlydata.SectionName,
            //                MetricName = monthlydata.MetricName,
            //                Year = monthlydata.YearString,
            //                Month = monthlydata.MonthString,
            //                Plan = monthlydata.Plan,
            //                Budget = monthlydata.Budget,
            //                Recon = monthlydata.Recon,
            //                Comment = "Year value is missing."
            //            });
            //            InvalidCount++;
            //            continue;
            //        }
            //        else if (string.IsNullOrEmpty(monthlydata.MonthString))
            //        {
            //            lstInvalid.Add(new InvalidMonthlyPlanData
            //            {
            //                SiteName = monthlydata.SiteName,
            //                SectionName = monthlydata.SectionName,
            //                MetricName = monthlydata.MetricName,
            //                Year = monthlydata.YearString,
            //                Month = monthlydata.MonthString,
            //                Plan = monthlydata.Plan,
            //                Budget = monthlydata.Budget,
            //                Recon = monthlydata.Recon,
            //                Comment = "Month value is missing."
            //            });
            //            InvalidCount++;
            //            continue;
            //        }




            //        if (!string.IsNullOrEmpty(monthlydata.YearString))
            //        {
            //            try
            //            {

            //                monthlydata.Year = Convert.ToInt32(monthlydata.YearString);
            //                if (((monthlydata.Year < DateTime.Now.Year - 10) || (monthlydata.Year > DateTime.Now.Year + 10)))
            //                {
            //                    lstInvalid.Add(new InvalidMonthlyPlanData
            //                    {
            //                        SiteName = monthlydata.SiteName,
            //                        SectionName = monthlydata.SectionName,
            //                        MetricName = monthlydata.MetricName,
            //                        Year = monthlydata.YearString,
            //                        Month = monthlydata.MonthString,
            //                        Plan = monthlydata.Plan,
            //                        Budget = monthlydata.Budget,
            //                        Recon = monthlydata.Recon,
            //                        Comment = "Year must be between " + (DateTime.Now.Year - 10).ToString() + " and " + (DateTime.Now.Year + 10).ToString()
            //                    });
            //                    InvalidCount++;
            //                    continue;
            //                }
            //            }
            //            catch
            //            {
            //                lstInvalid.Add(new InvalidMonthlyPlanData
            //                {
            //                    SiteName = monthlydata.SiteName,
            //                    SectionName = monthlydata.SectionName,
            //                    MetricName = monthlydata.MetricName,
            //                    Year = monthlydata.YearString,
            //                    Month = monthlydata.MonthString,
            //                    Plan = monthlydata.Plan,
            //                    Budget = monthlydata.Budget,
            //                    Recon = monthlydata.Recon,
            //                    Comment = "Year must be between " + (DateTime.Now.Year - 10).ToString() + " and " + (DateTime.Now.Year + 10).ToString()
            //                });
            //                InvalidCount++;
            //                continue;
            //            }
            //        }


            //        if (!string.IsNullOrEmpty(monthlydata.MonthString))
            //        //else if (monthlydata.Month < 0 || monthlydata.Month > 12)
            //        {
            //            try
            //            {

            //                //monthlydata.Month = Convert.ToInt32(monthlydata.MonthString);
            //                monthlydata.Month = (int)((CommonHelper.Months)Enum.Parse(typeof(CommonHelper.Months), monthlydata.MonthString));
            //                if (monthlydata.Month < 0 || monthlydata.Month > 12)
            //                {
            //                    lstInvalid.Add(new InvalidMonthlyPlanData
            //                    {
            //                        SiteName = monthlydata.SiteName,
            //                        SectionName = monthlydata.SectionName,
            //                        MetricName = monthlydata.MetricName,
            //                        Year = monthlydata.YearString,
            //                        Month = monthlydata.MonthString,
            //                        Plan = monthlydata.Plan,
            //                        Budget = monthlydata.Budget,
            //                        Recon = monthlydata.Recon,
            //                        Comment = "Month Value is invalid."
            //                    });
            //                    InvalidCount++;
            //                    continue;
            //                }
            //            }
            //            catch
            //            {
            //                lstInvalid.Add(new InvalidMonthlyPlanData
            //                {
            //                    SiteName = monthlydata.SiteName,
            //                    SectionName = monthlydata.SectionName,
            //                    MetricName = monthlydata.MetricName,
            //                    Year = monthlydata.YearString,
            //                    Month = monthlydata.MonthString,
            //                    Plan = monthlydata.Plan,
            //                    Budget = monthlydata.Budget,
            //                    Recon = monthlydata.Recon,
            //                    Comment = "Month must be between January-December."
            //                });
            //                InvalidCount++;
            //                continue;
            //            }
            //        }

            //        if (metrics.Any(x => x.Text == monthlydata.SectionName + "-" + monthlydata.MetricName))
            //        {
            //            monthlydata.SiteMetricId = Convert.ToInt32(metrics.Where(x => x.Text == monthlydata.SectionName + "-" + monthlydata.MetricName).FirstOrDefault().Value);
            //        }
            //        else
            //        {
            //            lstInvalid.Add(new InvalidMonthlyPlanData
            //            {
            //                SiteName = monthlydata.SiteName,
            //                SectionName = monthlydata.SectionName,
            //                MetricName = monthlydata.MetricName,
            //                Year = monthlydata.YearString,
            //                Month = monthlydata.MonthString,
            //                Plan = monthlydata.Plan,
            //                Budget = monthlydata.Budget,
            //                Recon = monthlydata.Recon,
            //                Comment = "Invalid Metric or Section name."
            //            });
            //            InvalidCount++;
            //            continue;
            //        }

            //        monthlydata.CreatedBy = ProjectSession.UserID;
            //        monthlydata.CreatedDate = DateTime.Now;


            //        var model = (from x in _montlhyBudgetPlanDataRepository.Table
            //                     where x.SiteMetricId == monthlydata.SiteMetricId &&
            //                     x.Month == monthlydata.Month &&
            //                     x.Year == monthlydata.Year
            //                     select x).FirstOrDefault();
            //        if (model != null)
            //        {

            //            if (ProjectSession.IsAdmin)
            //            {
            //                model.Budget = monthlydata.Budget ?? 0;
            //            }
            //            model.Plan = monthlydata.Plan ?? 0;
            //            model.Recon = monthlydata.Recon ?? 0;
            //            model.ModifiedBy = ProjectSession.UserID;
            //            model.ModifiedDate = DateTime.Now;
            //            _montlhyBudgetPlanDataRepository.Update(model);
            //            //_montlhyBudgetPlanDataRepository.
            //        }
            //        else
            //        {
            //            if (!ProjectSession.IsAdmin)
            //            {
            //                monthlydata.Budget = 0;
            //            }

            //            _montlhyBudgetPlanDataRepository.Insert(monthlydata);
            //        }
            //    }
            //}
            //return lstInvalid;
            return null;
        }

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
                        //Causes calculated metrics to not upload
                        //if (IsDataDownload == false && itm.Section.SectionMappingName == "Finance" && CalculatedMetrics.Contains(itm.Metric.MetricsMappingName))
                        //    continue;

                        if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == itm.SectionId))
                        {
                            var listItem = new SelectListItem { Text = itm.Section.SectionName + "^" + itm.Metric.MetricsName + " (" + itm.Metric.Unit.UOM + ")", Value = itm.SiteMetricId.ToString() };

                            list.Add(listItem);
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
        public IList<SelectListItem> GetReportsSelectList(int reportId = 0)
        {
            if (reportId == 0)
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
            else
            {
                var query = from p in _reportRepository.Table
                            where p.ReportId == reportId
                            orderby p.Name ascending
                            select new SelectListItem
                            {
                                Text = p.Name,
                                Value = p.ReportId.ToString()
                            };
                return query.ToList();

            }
        }


        /// <summary>
        /// Updates the monthly plan data.
        /// </summary>
        /// <param name="Budgetmodel">The budgetmodel.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.ArgumentNullException">MonthlyBudgetPlanData</exception>
        public long UpdateMonthlyPlanData(MonthlyBudgetPlanData Budgetmodel)
        {
            try
            {
                if (Budgetmodel == null)
                    throw new ArgumentNullException("MonthlyBudgetPlanData");

                var metricId = (from p in _dbContext.SiteMetrics
                                where p.SiteMetricId == Budgetmodel.SiteMetricId
                                select p.MetricId).SingleOrDefault();

                var metricName = (from p in _dbContext.Metrics
                                  where p.MetricId == metricId
                                  select p.MetricsName).SingleOrDefault();

                var siteId = (from p in _dbContext.SiteMetrics
                              where p.SiteMetricId == Budgetmodel.SiteMetricId
                              select p.SiteId).SingleOrDefault();
                var model = GetMonthlyBudgetPlanDataById(Budgetmodel.MonthlyPlanBudgetDataId);
                if (model != null)
                {
                    if (ProjectSession.IsAdmin || model.Budget == null || ProjectSession.IsUploadBudgetAllowed || ProjectSession.IsEditBudgetAllowed)
                        model.Budget = Budgetmodel.Budget;
                    //model.Forecast = Budgetmodel.Forecast;
                    model.Actual = Budgetmodel.Actual;
                    model.ModifiedBy = ProjectSession.UserID;
                    model.ModifiedDate = DateTime.Now;

                    _montlhyBudgetPlanDataRepository.Update(model);
                    if (metricName == "Mill Availability" || metricName == "Ore Milled")
                    {
                        object[] xparams = {
                         new SqlParameter("SiteId", siteId),
                            new SqlParameter("SiteMetricId", model.SiteMetricId),
                            new SqlParameter("ActualValue", model.Actual),
                            new SqlParameter("MTDValue", model.Budget),
                            new SqlParameter("MetricId", metricId),
                            new SqlParameter("DailyUploadDate", DateTime.Now),
                            new SqlParameter("Year", model.Year),
                            new SqlParameter("Month", model.Month),
                            new SqlParameter("Week", 1),
                            new SqlParameter("PeriodMetric", 3),
                            new SqlParameter("UserId", ProjectSession.UserID)
                         };

                        _dbContext.ExecuteStoredProcedureList<long>("InsertOrUpdateMillThroughPut", xparams);
                    }

                    UpdateFinanceMetricsData(model.SiteMetrics.SiteId, model.Month, model.Year);
                    UpdateFinanceMetricsData_Budget(model.SiteMetrics.SiteId, model.Month, model.Year);
                    //UpdateFinanceMetricsData_Forecast(model.SiteMetrics.SiteId, model.Month, model.Year);
                }

                return model.MonthlyPlanBudgetDataId;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        private void UpdateFinanceMetricsData(int siteId, int month, int year)
        {

            object[] xparams = {
                            new SqlParameter("SiteId", siteId),
                              new SqlParameter("Month", month),
                                new SqlParameter("Year", year),
                         };

            var Result = _dbContext.ExecuteStoredProcedureList<RPT_CalculateFinanceMetrics_Result>("RPT_CalculateFinanceMetrics", xparams);
        }
        private void UpdateFinanceMetricsData_Budget(int siteId, int month, int year)
        {

            object[] xparams = {
                            new SqlParameter("SiteId", siteId),
                              new SqlParameter("Month", month),
                                new SqlParameter("Year", year),
                         };

            var Result = _dbContext.ExecuteStoredProcedureList<RPT_CalculateFinanceMetrics_Result>("RPT_CalculateFinanceMetrics_Budget", xparams);
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


        /// <summary>
        /// Inserts monthly plan data
        /// </summary>
        /// <param name="Budgetmodel"></param>
        /// <returns></returns>
        public long InsertMonthlyPlanData(MonthlyBudgetPlanData Budgetmodel)
        {
            try
            {
                if (Budgetmodel == null)
                    throw new ArgumentNullException("MonthlyBudgetPlanData");


                Budgetmodel.CreatedBy = ProjectSession.UserID;
                Budgetmodel.CreatedDate = DateTime.Now;

                var metricId = (from p in _dbContext.SiteMetrics
                                where p.SiteMetricId == Budgetmodel.SiteMetricId
                                select p.MetricId).SingleOrDefault();

                var metricName = (from p in _dbContext.Metrics
                                  where p.MetricId == metricId
                                  select p.MetricsName).SingleOrDefault();

                var siteId = (from p in _dbContext.SiteMetrics
                              where p.SiteMetricId == Budgetmodel.SiteMetricId
                              select p.SiteId).SingleOrDefault();

                _montlhyBudgetPlanDataRepository.Insert(Budgetmodel);
                if (metricName == "Mill Availability" || metricName == "Ore Milled")
                {
                    object[] xparams = {
                         new SqlParameter("SiteId", siteId),
                            new SqlParameter("SiteMetricId", Budgetmodel.SiteMetricId),
                            new SqlParameter("ActualValue", Budgetmodel.Actual),
                            new SqlParameter("MTDValue", Budgetmodel.Budget),
                            new SqlParameter("MetricId", metricId),
                            new SqlParameter("DailyUploadDate", DateTime.Now),
                            new SqlParameter("Year", Budgetmodel.Year),
                            new SqlParameter("Month", Budgetmodel.Month),
                            new SqlParameter("Week", 1),
                            new SqlParameter("PeriodMetric", 3),
                            new SqlParameter("UserId", ProjectSession.UserID)
                         };

                    _dbContext.ExecuteStoredProcedureList<long>("InsertOrUpdateMillThroughPut", xparams);
                }
                var model = (from p in _dbContext.MonthlyBudgetPlanData where p.MonthlyPlanBudgetDataId == Budgetmodel.MonthlyPlanBudgetDataId select p).FirstOrDefault();
                if (model != null && model.SiteMetrics != null)
                {
                    UpdateFinanceMetricsData(model.SiteMetrics.SiteId, model.Month, model.Year);
                    UpdateFinanceMetricsData_Budget(model.SiteMetrics.SiteId, model.Month, model.Year);
                    //UpdateFinanceMetricsData_Forecast(model.SiteMetrics.SiteId, model.Month, model.Year);
                }
                return Budgetmodel.MonthlyPlanBudgetDataId;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Uploads monthly plan data
        /// </summary>
        /// <param name="lstMontlhyPlanData"></param>
        /// <returns></returns>
        public List<InvalidMonthlyPlanData> UploadMonthlyData(List<MonthlyUploadModel> lstMontlhyPlanData, int reportid)
        {
            var InvalidCount = 0;
            List<InvalidMonthlyPlanData> lstInvalid = new List<InvalidMonthlyPlanData>();
            var sitename = lstMontlhyPlanData[0].SiteName;
            var site = from p in _dbContext.Site where p.SiteName == sitename select p;
            int siteId = site.FirstOrDefault().SiteId;

            var sites = GetSitesSelectList(siteId, true);
            int year = Convert.ToInt32(lstMontlhyPlanData[0].YearString);
            var ApprovedMonths = (from x in _montlhyBudgetPlanDataApproveRepository.Table
                                  where x.SiteId == siteId && x.Year == year && x.IsApprove == true
                                  select x.Month).ToList();

            var metrics = GetMetricsSelectList(siteId, reportid, false);
            if (lstMontlhyPlanData != null && lstMontlhyPlanData.Count > 0)
            {
                int metricYear = Convert.ToInt32(lstMontlhyPlanData[0].YearString);
                //Mill ThroughPut Calculation
                var millThroughPutJan = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Jan).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.Jan).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 1) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Jan).FirstOrDefault()) / 100)));
                var millThroughPutFeb = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Feb).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.Feb).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 2) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Feb).FirstOrDefault()) / 100)));
                var millThroughPutMar = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Mar).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.Mar).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 3) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Mar).FirstOrDefault()) / 100)));
                var millThroughPutApr = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Apr).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.Apr).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 4) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Apr).FirstOrDefault()) / 100)));
                var millThroughPutMay = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.May).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.May).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 5) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.May).FirstOrDefault()) / 100)));
                var millThroughPutJun = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Jun).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.Jun).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 6) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Jun).FirstOrDefault()) / 100)));
                var millThroughPutJul = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Jul).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.Jul).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 7) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Jul).FirstOrDefault()) / 100)));
                var millThroughPutAug = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Aug).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.Aug).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 8) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Aug).FirstOrDefault()) / 100)));
                var millThroughPutSep = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Sep).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.Sep).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 9) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Sep).FirstOrDefault()) / 100)));
                var millThroughPutOct = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Oct).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.Oct).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 10) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Oct).FirstOrDefault()) / 100)));
                var millThroughPutNov = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Nov).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.Nov).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 11) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Nov).FirstOrDefault()) / 100)));
                var millThroughPutDec = (lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Dec).FirstOrDefault()) == 0 ? 0 : ((lstMontlhyPlanData.Where(x => x.MetricName == "Ore Milled (DMT)").Select(x => x.Dec).FirstOrDefault()) / (DateTime.DaysInMonth(metricYear, 12) * 24 * ((lstMontlhyPlanData.Where(x => x.MetricName == "Mill Availability (%)").Select(x => x.Dec).FirstOrDefault()) / 100)));
                foreach (MonthlyUploadModel monthlydata in lstMontlhyPlanData)
                {

                    if (!sites.Any(x => x.Text == monthlydata.SiteName))
                    {
                        lstInvalid.Add(new InvalidMonthlyPlanData
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            Year = monthlydata.YearString,
                            Type = monthlydata.Type,
                            Jan = monthlydata.Jan,
                            Feb = monthlydata.Feb,
                            Mar = monthlydata.Mar,
                            Apr = monthlydata.Apr,
                            May = monthlydata.May,
                            Jun = monthlydata.Jun,
                            Jul = monthlydata.Jul,
                            Aug = monthlydata.Aug,
                            Sep = monthlydata.Sep,
                            Oct = monthlydata.Oct,
                            Nov = monthlydata.Nov,
                            Dec = monthlydata.Dec,
                            Comment = "Invalid site OR permission denied for site"
                        });
                        InvalidCount++;
                        continue;

                    }
                    else if (!metrics.Any(x => x.Text == monthlydata.SectionName + "^" + monthlydata.MetricName))
                    {

                        lstInvalid.Add(new InvalidMonthlyPlanData
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            Year = monthlydata.YearString,
                            Type = monthlydata.Type,
                            Jan = monthlydata.Jan,
                            Feb = monthlydata.Feb,
                            Mar = monthlydata.Mar,
                            Apr = monthlydata.Apr,
                            May = monthlydata.May,
                            Jun = monthlydata.Jun,
                            Jul = monthlydata.Jul,
                            Aug = monthlydata.Aug,
                            Sep = monthlydata.Sep,
                            Oct = monthlydata.Oct,
                            Nov = monthlydata.Nov,
                            Dec = monthlydata.Dec,
                            Comment = "Invalid Metric/Section name OR Permission denied for section"
                        });
                        InvalidCount++;
                        continue;
                    }
                    else if (monthlydata.Type != "PLAN" && monthlydata.Type != "BUDGET" && monthlydata.Type != "RECON" && monthlydata.Type != "FORECAST" && monthlydata.Type != "ACTUAL")
                    {
                        lstInvalid.Add(new InvalidMonthlyPlanData
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            Year = monthlydata.YearString,
                            Type = monthlydata.Type,
                            Jan = monthlydata.Jan,
                            Feb = monthlydata.Feb,
                            Mar = monthlydata.Mar,
                            Apr = monthlydata.Apr,
                            May = monthlydata.May,
                            Jun = monthlydata.Jun,
                            Jul = monthlydata.Jul,
                            Aug = monthlydata.Aug,
                            Sep = monthlydata.Sep,
                            Oct = monthlydata.Oct,
                            Nov = monthlydata.Nov,
                            Dec = monthlydata.Dec,
                            Comment = "Invalid Type"
                        });
                        InvalidCount++;
                        continue;
                    }

                    else if (string.IsNullOrEmpty(monthlydata.YearString))
                    {
                        lstInvalid.Add(new InvalidMonthlyPlanData
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            Year = monthlydata.YearString,
                            Type = monthlydata.Type,
                            Jan = monthlydata.Jan,
                            Feb = monthlydata.Feb,
                            Mar = monthlydata.Mar,
                            Apr = monthlydata.Apr,
                            May = monthlydata.May,
                            Jun = monthlydata.Jun,
                            Jul = monthlydata.Jul,
                            Aug = monthlydata.Aug,
                            Sep = monthlydata.Sep,
                            Oct = monthlydata.Oct,
                            Nov = monthlydata.Nov,
                            Dec = monthlydata.Dec,
                            Comment = "Year value is missing."
                        });
                        InvalidCount++;
                        continue;
                    }

                    InvalidMonthlyPlanData invalid = new InvalidMonthlyPlanData();
                    bool invalidfound = false;
                    if (monthlydata.SectionName != "SALES ADJUSTMENTS")
                    {
                        for (int m = 1; m <= 12; m++)
                        {
                            invalidfound = false;
                            switch (m)
                            {
                                case 1:
                                    if (monthlydata.Jan < 0 || monthlydata.Jan > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for January.";
                                        invalidfound = true;
                                    }
                                    break;
                                case 2:
                                    if (monthlydata.Feb < 0 || monthlydata.Feb > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for February.";
                                        invalidfound = true;
                                    }
                                    break;
                                case 3:
                                    if (monthlydata.Mar < 0 || monthlydata.Mar > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for March.";
                                        invalidfound = true;
                                    }
                                    break;
                                case 4:
                                    if (monthlydata.Apr < 0 || monthlydata.Apr > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for April.";
                                        invalidfound = true;
                                    }
                                    break;
                                case 5:
                                    if (monthlydata.May < 0 || monthlydata.May > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for May.";
                                        invalidfound = true;
                                    }
                                    break;
                                case 6:
                                    if (monthlydata.Jun < 0 || monthlydata.Jun > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for June.";
                                        invalidfound = true;
                                    }
                                    break;
                                case 7:
                                    if (monthlydata.Jul < 0 || monthlydata.Jul > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for July.";
                                        invalidfound = true;
                                    }
                                    break;
                                case 8:
                                    if (monthlydata.Aug < 0 || monthlydata.Aug > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for August.";
                                        invalidfound = true;
                                    }
                                    break;
                                case 9:
                                    if (monthlydata.Sep < 0 || monthlydata.Sep > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for September.";
                                        invalidfound = true;
                                    }
                                    break;
                                case 10:
                                    if (monthlydata.Oct < 0 || monthlydata.Oct > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for October.";
                                        invalidfound = true;
                                    }
                                    break;
                                case 11:
                                    if (monthlydata.Nov < 0 || monthlydata.Nov > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for November.";
                                        invalidfound = true;
                                    }
                                    break;
                                case 12:
                                    if (monthlydata.Dec < 0 || monthlydata.Dec > 99999999)
                                    {
                                        invalid.Comment = "Invalid value for December.";
                                        invalidfound = true;
                                    }
                                    break;
                            }
                            if (invalidfound)
                                break;
                        }
                    }
                    if (invalidfound)
                    {
                        invalid.SiteName = monthlydata.SiteName;
                        invalid.SectionName = monthlydata.SectionName;
                        invalid.MetricName = monthlydata.MetricName;
                        invalid.Year = monthlydata.YearString;
                        invalid.Type = monthlydata.Type;
                        invalid.Jan = monthlydata.Jan;
                        invalid.Feb = monthlydata.Feb;
                        invalid.Mar = monthlydata.Mar;
                        invalid.Apr = monthlydata.Apr;
                        invalid.May = monthlydata.May;
                        invalid.Jun = monthlydata.Jun;
                        invalid.Jul = monthlydata.Jul;
                        invalid.Aug = monthlydata.Aug;
                        invalid.Sep = monthlydata.Sep;
                        invalid.Oct = monthlydata.Oct;
                        invalid.Nov = monthlydata.Nov;
                        invalid.Dec = monthlydata.Dec;
                        continue;
                    }


                    if (!string.IsNullOrEmpty(monthlydata.YearString))
                    {
                        try
                        {

                            monthlydata.Year = Convert.ToInt32(monthlydata.YearString);
                            if (((monthlydata.Year < DateTime.Now.Year - 10) || (monthlydata.Year > DateTime.Now.Year + 21)))
                            {
                                lstInvalid.Add(new InvalidMonthlyPlanData
                                {
                                    SiteName = monthlydata.SiteName,
                                    SectionName = monthlydata.SectionName,
                                    MetricName = monthlydata.MetricName,
                                    Year = monthlydata.YearString,
                                    Type = monthlydata.Type,
                                    Jan = monthlydata.Jan,
                                    Feb = monthlydata.Feb,
                                    Mar = monthlydata.Mar,
                                    Apr = monthlydata.Apr,
                                    May = monthlydata.May,
                                    Jun = monthlydata.Jun,
                                    Jul = monthlydata.Jul,
                                    Aug = monthlydata.Aug,
                                    Sep = monthlydata.Sep,
                                    Oct = monthlydata.Oct,
                                    Nov = monthlydata.Nov,
                                    Dec = monthlydata.Dec,
                                    Comment = "Year must be between " + (DateTime.Now.Year - 10).ToString() + " and " + (DateTime.Now.Year + 21).ToString()
                                });
                                InvalidCount++;
                                continue;
                            }
                        }
                        catch
                        {
                            lstInvalid.Add(new InvalidMonthlyPlanData
                            {
                                SiteName = monthlydata.SiteName,
                                SectionName = monthlydata.SectionName,
                                MetricName = monthlydata.MetricName,
                                Year = monthlydata.YearString,
                                Type = monthlydata.Type,
                                Jan = monthlydata.Jan,
                                Feb = monthlydata.Feb,
                                Mar = monthlydata.Mar,
                                Apr = monthlydata.Apr,
                                May = monthlydata.May,
                                Jun = monthlydata.Jun,
                                Jul = monthlydata.Jul,
                                Aug = monthlydata.Aug,
                                Sep = monthlydata.Sep,
                                Oct = monthlydata.Oct,
                                Nov = monthlydata.Nov,
                                Dec = monthlydata.Dec,
                                Comment = "Year must be between " + (DateTime.Now.Year - 10).ToString() + " and " + (DateTime.Now.Year + 21).ToString()
                            });
                            InvalidCount++;
                            continue;
                        }
                    }




                    if (metrics.Any(x => x.Text == monthlydata.SectionName + "^" + monthlydata.MetricName))
                    {
                        monthlydata.SiteMetricId = Convert.ToInt32(metrics.Where(x => x.Text == monthlydata.SectionName + "^" + monthlydata.MetricName).FirstOrDefault().Value);
                    }
                    else
                    {
                        lstInvalid.Add(new InvalidMonthlyPlanData
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            Year = monthlydata.YearString,
                            Type = monthlydata.Type,
                            Jan = monthlydata.Jan,
                            Feb = monthlydata.Feb,
                            Mar = monthlydata.Mar,
                            Apr = monthlydata.Apr,
                            May = monthlydata.May,
                            Jun = monthlydata.Jun,
                            Jul = monthlydata.Jul,
                            Aug = monthlydata.Aug,
                            Sep = monthlydata.Sep,
                            Oct = monthlydata.Oct,
                            Nov = monthlydata.Nov,
                            Dec = monthlydata.Dec,
                            Comment = "Invalid Metric/Section name OR Permission denied for section"
                        });
                        InvalidCount++;
                        continue;
                    }

                    var modellist = (from x in _montlhyBudgetPlanDataRepository.Table
                                     where x.SiteMetricId == monthlydata.SiteMetricId &&
                                     x.Year == monthlydata.Year
                                     select x).ToList();

                    for (int m = 1; m <= 12; m++)
                    {
                        bool IsApproved = ApprovedMonths.Any(x => x == m);
                        var IsExist = modellist.Any(x => x.Month == m);
                        decimal? value = null;
                        switch (m)
                        {
                            case 1:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutJan : monthlydata.Jan;
                                break;
                            case 2:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutFeb : monthlydata.Feb;
                                break;
                            case 3:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutMar : monthlydata.Mar;
                                break;
                            case 4:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutApr : monthlydata.Apr;
                                break;
                            case 5:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutMay : monthlydata.May;
                                break;
                            case 6:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutJun : monthlydata.Jun;
                                break;
                            case 7:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutJul : monthlydata.Jul;
                                break;
                            case 8:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutAug : monthlydata.Aug;
                                break;
                            case 9:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutSep : monthlydata.Sep;
                                break;
                            case 10:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutOct : monthlydata.Oct;
                                break;
                            case 11:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutNov : monthlydata.Nov;
                                break;
                            case 12:
                                value = monthlydata.MetricName == "Mill Throughput (TPH)" ? millThroughPutDec : monthlydata.Dec;
                                break;
                            default:
                                value = 0;
                                break;
                        }
                        if (IsApproved == false || ProjectSession.IsAdmin == true || ProjectSession.IsMonthlyReconApproveAllowed == true)
                        {
                            if (IsExist)
                                updateMonthlyRecord(monthlydata, modellist.Where(x => x.Month == m).FirstOrDefault(), value, m);
                            else
                                insertMonthlyRecord(monthlydata, value, m);
                        }
                    }



                }
                if (!string.IsNullOrEmpty(lstMontlhyPlanData[0].YearString))
                {
                    UpdateFinanceMetricsData(siteId, 0, Convert.ToInt32(lstMontlhyPlanData[0].YearString));
                    UpdateFinanceMetricsData_Budget(siteId, 0, Convert.ToInt32(lstMontlhyPlanData[0].YearString));
                    //UpdateFinanceMetricsData_Forecast(siteId, 0, Convert.ToInt32(lstMontlhyPlanData[0].YearString));
                }


            }
            return lstInvalid;
            //return null;
        }

        /// <summary>
        /// inserts monthly data
        /// </summary>
        /// <param name="modelUpload"></param>
        /// <param name="value"></param>
        /// <param name="month"></param>
        private void insertMonthlyRecord(MonthlyUploadModel modelUpload, Decimal? value, int month)
        {
            if (value != null)
            {
                var model = new MonthlyBudgetPlanData()
                {

                    Budget = (modelUpload.Type == "BUDGET") ? value : null,
                    //Forecast = (modelUpload.Type == "PLAN" || modelUpload.Type == "FORECAST" ? value : 0),
                    Actual = modelUpload.Type == "RECON" || modelUpload.Type == "ACTUAL" ? value : 0,
                    Month = month,
                    Year = modelUpload.Year,
                    SiteMetricId = modelUpload.SiteMetricId,
                    CreatedBy = ProjectSession.UserID,
                    CreatedDate = DateTime.Now
                };
                _montlhyBudgetPlanDataRepository.Insert(model);

                if (modelUpload.Type == "BUDGET")
                {
                    var budgetDate = new DateTime(model.Year, model.Month, 1);
                    UpdateOrInsertToInsightsTable(model.SiteMetricId, budgetDate);
                }
            }
        }

        /// <summary>
        /// updates monthly data
        /// </summary>
        /// <param name="modelUpload"></param>
        /// <param name="monthlydata"></param>
        /// <param name="value"></param>
        /// <param name="month"></param>
        private void updateMonthlyRecord(MonthlyUploadModel modelUpload, MonthlyBudgetPlanData monthlydata, Decimal? value, int month)
        {
            if (value != null)
            {
                if (ProjectSession.IsAdmin || monthlydata.Budget == null || ProjectSession.IsUploadBudgetAllowed || ProjectSession.IsEditBudgetAllowed)
                {
                    monthlydata.Budget = (modelUpload.Type == "BUDGET") ? value : monthlydata.Budget;
                }
                //monthlydata.Forecast = modelUpload.Type == "PLAN" || modelUpload.Type == "FORECAST" ? value : monthlydata.Forecast;

                monthlydata.Actual = modelUpload.Type == "RECON" || modelUpload.Type == "ACTUAL" ? value : monthlydata.Actual;


                monthlydata.ModifiedBy = ProjectSession.UserID;
                monthlydata.ModifiedDate = DateTime.Now;

                _montlhyBudgetPlanDataRepository.Update(monthlydata);

                if (modelUpload.Type == "BUDGET")
                {
                    var budgetDate = new DateTime(monthlydata.Year, monthlydata.Month, 1);
                    UpdateOrInsertToInsightsTable(monthlydata.SiteMetrics.SiteMetricId, budgetDate);
                }
            }
        }

        /// <summary>
        /// get monthly data for whole year
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="reportId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<BudgetYearView> GetBudgetYearViewData(int siteId, int reportId, int year)
        {

            var query1 = (from p in _montlhyBudgetPlanDataRepository.Table
                          where (p.SiteMetrics.SiteId == siteId || siteId == 0) &&
                          (p.SiteMetrics.Report.ReportId == reportId || reportId == 0) &&
                          (p.Year == year || year == 0)
                          select p).ToList().Where(x => x.SiteMetrics.HideInReports == 0).OrderBy(x => x.SiteMetrics.DisplayOrder).ToList();
            // hello

            var query = (from p in _siteMetricsRepository.Table
                         where p.SiteId == siteId && p.IsYearly == true && (p.ReportId == reportId || reportId == 0)
                         select p).ToList().Where(x => x.HideInReports == 0).ToList();

            List<BudgetYearView> lstbudget = new List<BudgetYearView>();

            if (query != null && query.Count > 0)
            {
                var uSiteMetricId = query.Select(x => x.SiteMetricId).Distinct().ToList();

                for (int i = 0; i < uSiteMetricId.Count; i++)
                {

                    var metricsdata = query.Where(x => x.SiteMetricId == uSiteMetricId[i]).ToList();

                    if (metricsdata != null && metricsdata.Count > 0)
                    {
                        if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == metricsdata[0].Section.SectionId))
                        {

                            BudgetYearView model = new BudgetYearView();
                            model.SiteMetricId = uSiteMetricId[i];
                            model.SiteName = metricsdata[0].Site.SiteName;
                            model.SiteId = metricsdata[0].Site.SiteId;
                            model.SectionName = metricsdata[0].Section.SectionName;
                            model.SectionMappingName = metricsdata[0].Section.SectionMappingName;
                            model.SectionId = metricsdata[0].Section.SectionId;
                            model.DisplayOrder = metricsdata[0].DisplayOrder;
                            model.MetricsName = metricsdata[0].Metric.MetricsName + " (" + metricsdata[0].Metric.Unit.UOM + ")";
                            model.MetricsMappingName = metricsdata[0].Metric.MetricsMappingName;
                            model.Year = year;
                            var monthlydata = query1.Where(x => x.SiteMetricId == uSiteMetricId[i]).ToList();
                            if (query1 != null && query1.Count > 0 && monthlydata != null && monthlydata.Count > 0)
                            {


                                for (int j = 0; j < monthlydata.Count; j++)
                                {

                                    switch (monthlydata[j].Month)
                                    {
                                        case 1:
                                            model.Jan_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.Jan_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.Jan_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        case 2:
                                            model.Feb_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.Feb_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.Feb_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        case 3:
                                            model.Mar_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.Mar_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.Mar_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        case 4:
                                            model.Apr_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.Apr_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.Apr_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        case 5:
                                            model.May_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.May_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.May_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        case 6:
                                            model.Jun_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.Jun_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.Jun_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        case 7:
                                            model.Jul_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.Jul_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.Jul_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        case 8:
                                            model.Aug_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.Aug_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.Aug_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        case 9:
                                            model.Sep_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.Sep_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.Sep_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        case 10:
                                            model.Oct_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.Oct_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.Oct_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        case 11:
                                            model.Nov_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.Nov_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.Nov_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        case 12:
                                            model.Dec_Budget = monthlydata[j].Budget == null ? monthlydata[j].Budget : CommonHelper.SetRounding(monthlydata[j].Budget ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            model.Dec_Forecast = CommonHelper.SetRounding(monthlydata[j].Forecast ?? 0, metricsdata[0].Metric.Unit.UOM.Trim());
                                            model.Dec_Actual = CommonHelper.SetRounding(monthlydata[j].Actual ?? 0, metricsdata[0].Metric.Unit.UOM.Trim(), (metricsdata[0].Section.SectionMappingName == "Finance"));
                                            break;
                                        default:
                                            model.Dec_Budget = null;
                                            model.Dec_Forecast = 0;
                                            model.Dec_Actual = 0;
                                            break;
                                    }


                                }
                            }
                            lstbudget.Add(model);
                        }

                    }
                }
            }
            return lstbudget.OrderBy(x => x.SectionId).ToList().OrderBy(x => x.DisplayOrder).ToList();
        }

        /// <summary>
        /// gets monthly data by sitemetricId
        /// </summary>
        /// <param name="SiteMetricId"></param>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public MonthlyBudgetPlanData GetMonthlyBudgetPlanDataBySiteMetricId(int SiteMetricId, int Year, int Month)
        {
            var query = (from p in _montlhyBudgetPlanDataRepository.Table
                         where (p.SiteMetricId == SiteMetricId || SiteMetricId == 0) &&
                         (p.Month == Month || Month == 0) &&
                         (p.Year == Year || Year == 0)
                         select p).FirstOrDefault();
            if (query == null)
            {
                var sitemetrics = _siteMetricsRepository.GetById(SiteMetricId);
                if (sitemetrics != null)
                {
                    query = new MonthlyBudgetPlanData();
                    query.SiteMetricId = sitemetrics.SiteMetricId;
                    query.SiteMetrics = sitemetrics;
                    //query.Budget = 0;
                    query.Forecast = 0;
                    query.Actual = 0;
                    query.Month = Month;
                    query.Year = Year;
                }
            }
            return query;
        }

        public List<MonthlyBudgetPlanDataApprove> GetMonthlyBudgetPlanDataApprovedStatus(int siteId, int year)
        {
            List<MonthlyBudgetPlanDataApprove> lstApprove = new List<MonthlyBudgetPlanDataApprove>();
            try
            {
                var query = (from p in _montlhyBudgetPlanDataApproveRepository.Table
                             where (p.SiteId == siteId && p.Year == year)
                             select p).ToList();
                for (int i = 0; i < 12; i++)
                {
                    var obj = query.Where(s => s.Month == (i + 1)).FirstOrDefault();
                    lstApprove.Add(new MonthlyBudgetPlanDataApprove
                    {
                        Month = (i + 1),
                        Year = year,
                        SiteId = siteId,
                        IsApprove = obj == null ? false : obj.IsApprove
                    });
                }
                return lstApprove;

            }
            catch (Exception)
            {
                return lstApprove;
            }

        }

        public bool ApproveBudgetPlanData(int siteId, int year, int month)
        {
            try
            {
                var model = (from p in _montlhyBudgetPlanDataApproveRepository.Table
                             where p.SiteId == siteId && p.Month == month && p.Year == year
                             select p).FirstOrDefault();
                if (model == null)
                {
                    _montlhyBudgetPlanDataApproveRepository.Insert(new MonthlyBudgetPlanDataApprove
                    {
                        SiteId = siteId,
                        Month = month,
                        Year = year,
                        IsApprove = true,
                        CreatedBy = ProjectSession.UserID,
                        CreatedDate = DateTime.Now
                    });
                }
                else
                {
                    model.IsApprove = true;
                    model.ModifiedBy = ProjectSession.UserID;
                    model.ModifiedDate = DateTime.Now;
                    _montlhyBudgetPlanDataApproveRepository.Update(model);

                }
                return true;

            }
            catch (Exception)
            { return false; }

        }


        public IList<ApprovalDetails> GetApprovalDetails(int year, bool IsWeekly, int[] site = null)
        {
            try
            {
                var lstApproved = new List<ApprovalDetails>();

                if (site == null)
                {
                    object[] xparams = {
                            new SqlParameter("Year", year),
                              new SqlParameter("SiteId", 0),
                                new SqlParameter("IsWeekly", IsWeekly),
                         };
                    var Result = _dbContext.ExecuteStoredProcedureList<ApprovalDetails>("RPT_GetApprovalDetails", xparams);
                    if (Result != null && Result.Count > 0)
                        lstApproved.AddRange(Result);
                }
                else
                {
                    foreach (int siteid in site)
                    {
                        object[] xparams = {
                            new SqlParameter("Year", year),
                              new SqlParameter("SiteId", siteid),
                                new SqlParameter("IsWeekly", IsWeekly),
                         };
                        var Result = _dbContext.ExecuteStoredProcedureList<ApprovalDetails>("RPT_GetApprovalDetails", xparams);
                        if (Result != null && Result.Count > 0)
                            lstApproved.AddRange(Result);
                    }
                }

                return lstApproved;

            }
            catch (Exception)
            {
                return null;
            }

        }

        private void UpdateOrInsertToInsightsTable(int siteMetricId, DateTime forecastDate)
        {
            var startOfMonth = new DateTime(forecastDate.Year, forecastDate.Month, 1);
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
