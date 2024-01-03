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
    public class MonthlyPlanDataService : IMonthlyPlanDataService
    {
        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<SiteMetrics> _siteMetricsRepository;
        private readonly IRepository<Metrics> _metricsRepository;
        private readonly IRepository<MonthlyPlanData> _montlhyPlanDataRepository;
        private readonly IRepository<MonthlyForecastData> _montlhyForecastDataRepository;
        public static TrevaliOperationalReportObjectContext _dbContext;

        public MonthlyPlanDataService(IRepository<Site> siteRepository, IRepository<SiteMetrics> siteMetricsRepository, IRepository<MonthlyPlanData> montlhyPlanDataRepository, IRepository<MonthlyForecastData> montlhyForecastDataRepository, IRepository<Metrics> metricsRepository)
        {
            _siteRepository = siteRepository;
            _metricsRepository = metricsRepository;
            _siteMetricsRepository = siteMetricsRepository;
            _montlhyPlanDataRepository = montlhyPlanDataRepository;
            _montlhyForecastDataRepository = montlhyForecastDataRepository;
            _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
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
        /// Gets next 24 months from current month.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetNext24MonthsSelectList()
        {
            List<SelectListItem> monthsList = new List<SelectListItem>();
            string currentMonthYear = DateTime.Now.ToString("MMM yyyy");
            string currentMonthYearValue = DateTime.Now.ToString("MM") + "_" + DateTime.Now.ToString("yyyy"); ;

            SelectListItem monthItem = new SelectListItem()
            {
                Text = currentMonthYear,
                Value = currentMonthYearValue
            };
            monthsList.Add(monthItem);

            string nextMonthYear = currentMonthYear;
            string nextMonthYearValue = currentMonthYearValue;

            for (int i = 2; i <= 24; i++)
            {
                nextMonthYearValue = DateTime.Parse(nextMonthYear).AddMonths(1).ToString("MM") + "_" + DateTime.Parse(nextMonthYear).AddMonths(1).ToString("yyyy");
                nextMonthYear = DateTime.Parse(nextMonthYear).AddMonths(1).ToString("MMM yyyy");

                SelectListItem nextMonthItem = new SelectListItem()
                {
                    Text = nextMonthYear,
                    Value = nextMonthYearValue
                };

                monthsList.Add(nextMonthItem);
            }

            return monthsList;
        }

        /// <summary>
        /// Gets monthly plan data for display on web page
        /// </summary>
        /// <param name="siteId">The site id.</param>
        /// <param name="month">The search month.</param>
        /// <param name="year">The serach year.</param>
        /// <returns></returns>
        public IList<MonthlyPlanData> GetMonthlyPlanData(int SiteId, string month, string year)
        {
            List<MonthlyPlanData> data = new List<MonthlyPlanData>();

            object[] xparams = {
                            new SqlParameter("SiteId", SiteId),
                              new SqlParameter("Month", month),
                                new SqlParameter("Year", year),
                                new SqlParameter("UserId", ProjectSession.UserID)
                         };

            var planData = _dbContext.ExecuteStoredProcedureList<MonthlyPlanData>("RPT_GetMonthlyPlanData", xparams);

            if (planData != null && planData.Count > 0)
            {
                foreach (var item in planData)
                {
                    var sitemetric = (from s in _siteMetricsRepository.Table
                                      where s.SiteMetricId == item.SiteMetricId
                                      select s).FirstOrDefault();

                    item.Total = item.D1.GetValueOrDefault(0) + item.D2.GetValueOrDefault(0) + item.D3.GetValueOrDefault(0) +
                        item.D4.GetValueOrDefault(0) + item.D5.GetValueOrDefault(0) + item.D6.GetValueOrDefault(0) + item.D7.GetValueOrDefault(0) +
                        item.D8.GetValueOrDefault(0) + item.D9.GetValueOrDefault(0) + item.D10.GetValueOrDefault(0) + item.D11.GetValueOrDefault(0) +
                        item.D12.GetValueOrDefault(0) + item.D13.GetValueOrDefault(0) + item.D14.GetValueOrDefault(0) + item.D15.GetValueOrDefault(0) +
                        item.D16.GetValueOrDefault(0) + item.D17.GetValueOrDefault(0) + item.D18.GetValueOrDefault(0) + item.D19.GetValueOrDefault(0) +
                        item.D20.GetValueOrDefault(0) + item.D21.GetValueOrDefault(0) + item.D22.GetValueOrDefault(0) + item.D23.GetValueOrDefault(0) +
                        item.D24.GetValueOrDefault(0) + item.D25.GetValueOrDefault(0) + item.D26.GetValueOrDefault(0) + item.D27.GetValueOrDefault(0) +
                        item.D28.GetValueOrDefault(0) + item.D29.GetValueOrDefault(0) + item.D30.GetValueOrDefault(0) + item.D31.GetValueOrDefault(0);

                    item.SiteMetrics = sitemetric;

                    if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == item.SiteMetrics.SectionId))
                    {
                        data.Add(item);
                    }
                }
            }

            return data;

        }

        /// <summary>
        /// Update monthly plan data. Updated by user from web page.
        /// </summary>
        /// <param name="PlanData">The changed model.</param>
        public void UpdateMonthlyPlanData(MonthlyPlanData PlanData)
        {
            var model = (from x in _montlhyPlanDataRepository.Table
                         where x.MonthlyPlanDataId == PlanData.MonthlyPlanDataId
                         select x).FirstOrDefault();

            if (model != null)
            {
                model.D1 = PlanData.D1;
                model.D2 = PlanData.D2;
                model.D3 = PlanData.D3;
                model.D4 = PlanData.D4;
                model.D5 = PlanData.D5;
                model.D6 = PlanData.D6;
                model.D7 = PlanData.D7;
                model.D8 = PlanData.D8;
                model.D9 = PlanData.D9;
                model.D10 = PlanData.D10;
                model.D11 = PlanData.D11;
                model.D12 = PlanData.D12;
                model.D13 = PlanData.D13;
                model.D14 = PlanData.D14;
                model.D15 = PlanData.D15;
                model.D16 = PlanData.D16;
                model.D17 = PlanData.D17;
                model.D18 = PlanData.D18;
                model.D19 = PlanData.D19;
                model.D20 = PlanData.D20;
                model.D21 = PlanData.D21;
                model.D22 = PlanData.D22;
                model.D23 = PlanData.D23;
                model.D24 = PlanData.D24;
                model.D25 = PlanData.D25;
                model.D26 = PlanData.D26;
                model.D27 = PlanData.D27;
                model.D28 = PlanData.D28;
                model.D29 = PlanData.D29;
                model.D30 = PlanData.D30;
                model.D31 = PlanData.D31;

                model.ModifiedDate = DateTime.Now;
                model.ModifiedBy = ProjectSession.UserID;

                _montlhyPlanDataRepository.Update(model);
            }
        }

        /// <summary>
        /// Gets metrics  selectlist.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetMetricsSelectList(int siteId = 0, int ReportId = 0)
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
        /// Uploads monthly plan data by excel sheet.
        /// </summary>
        /// <param name="lstMonthlyPlanData">The list of uploaded monthly plan data.</param>
        /// <returns></returns>
        public List<MonthlyPlanDataInvalid> UploadMonthlyPlanData(List<MonthlyPlanUploadModel> lstMonthlyPlanData)
        {
            var InvalidCount = 0;
            List<MonthlyPlanDataInvalid> lstInvalid = new List<MonthlyPlanDataInvalid>();
            var sitename = lstMonthlyPlanData[0].SiteName;
            var site = from p in _dbContext.Site where p.SiteName == sitename select p;
            int siteId = site.FirstOrDefault().SiteId;

            string monthYear = lstMonthlyPlanData[0].PlanDataId;
            int month = Convert.ToInt32(monthYear.Split('_')[0]);
            int year = Convert.ToInt32(monthYear.Split('_')[1]);

            var sites = GetSitesSelectList(siteId, true);
            var metrics = GetMetricsSelectList(siteId, 0);

            if (lstMonthlyPlanData != null && lstMonthlyPlanData.Count > 0)
            {
                foreach (MonthlyPlanUploadModel monthlydata in lstMonthlyPlanData)
                {
                    if (month < 1 || month > 12)
                    {
                        lstInvalid.Add(new MonthlyPlanDataInvalid
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            PlanDataId = monthlydata.PlanDataId,
                            Month = monthlydata.Month,
                            Year = monthlydata.Year,
                            D1 = monthlydata.D1,
                            D2 = monthlydata.D2,
                            D3 = monthlydata.D3,
                            D4 = monthlydata.D4,
                            D5 = monthlydata.D5,
                            D6 = monthlydata.D6,
                            D7 = monthlydata.D7,
                            D8 = monthlydata.D8,
                            D9 = monthlydata.D9,
                            D10 = monthlydata.D10,
                            D11 = monthlydata.D11,
                            D12 = monthlydata.D12,
                            D13 = monthlydata.D13,
                            D14 = monthlydata.D14,
                            D15 = monthlydata.D15,
                            D16 = monthlydata.D16,
                            D17 = monthlydata.D17,
                            D18 = monthlydata.D18,
                            D19 = monthlydata.D19,
                            D20 = monthlydata.D20,
                            D21 = monthlydata.D21,
                            D22 = monthlydata.D22,
                            D23 = monthlydata.D23,
                            D24 = monthlydata.D24,
                            D25 = monthlydata.D25,
                            D26 = monthlydata.D26,
                            D27 = monthlydata.D27,
                            D28 = monthlydata.D28,
                            D29 = monthlydata.D29,
                            D30 = monthlydata.D30,
                            D31 = monthlydata.D31,
                            Comment = "invalid month"
                        });
                        InvalidCount++;
                        continue;
                    }
                    else if (!sites.Any(x => x.Text == monthlydata.SiteName))
                    {
                        lstInvalid.Add(new MonthlyPlanDataInvalid
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            PlanDataId = monthlydata.PlanDataId,
                            Month = monthlydata.Month,
                            Year = monthlydata.Year,
                            D1 = monthlydata.D1,
                            D2 = monthlydata.D2,
                            D3 = monthlydata.D3,
                            D4 = monthlydata.D4,
                            D5 = monthlydata.D5,
                            D6 = monthlydata.D6,
                            D7 = monthlydata.D7,
                            D8 = monthlydata.D8,
                            D9 = monthlydata.D9,
                            D10 = monthlydata.D10,
                            D11 = monthlydata.D11,
                            D12 = monthlydata.D12,
                            D13 = monthlydata.D13,
                            D14 = monthlydata.D14,
                            D15 = monthlydata.D15,
                            D16 = monthlydata.D16,
                            D17 = monthlydata.D17,
                            D18 = monthlydata.D18,
                            D19 = monthlydata.D19,
                            D20 = monthlydata.D20,
                            D21 = monthlydata.D21,
                            D22 = monthlydata.D22,
                            D23 = monthlydata.D23,
                            D24 = monthlydata.D24,
                            D25 = monthlydata.D25,
                            D26 = monthlydata.D26,
                            D27 = monthlydata.D27,
                            D28 = monthlydata.D28,
                            D29 = monthlydata.D29,
                            D30 = monthlydata.D30,
                            D31 = monthlydata.D31,
                            Comment = "Invalid site OR permission denied for site"
                        });
                        InvalidCount++;
                        continue;

                    }
                    else if (!metrics.Any(x => x.Text == monthlydata.SectionName + "^" + monthlydata.MetricName))
                    {
                        lstInvalid.Add(new MonthlyPlanDataInvalid
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            PlanDataId = monthlydata.PlanDataId,
                            Month = monthlydata.Month,
                            Year = monthlydata.Year,
                            D1 = monthlydata.D1,
                            D2 = monthlydata.D2,
                            D3 = monthlydata.D3,
                            D4 = monthlydata.D4,
                            D5 = monthlydata.D5,
                            D6 = monthlydata.D6,
                            D7 = monthlydata.D7,
                            D8 = monthlydata.D8,
                            D9 = monthlydata.D9,
                            D10 = monthlydata.D10,
                            D11 = monthlydata.D11,
                            D12 = monthlydata.D12,
                            D13 = monthlydata.D13,
                            D14 = monthlydata.D14,
                            D15 = monthlydata.D15,
                            D16 = monthlydata.D16,
                            D17 = monthlydata.D17,
                            D18 = monthlydata.D18,
                            D19 = monthlydata.D19,
                            D20 = monthlydata.D20,
                            D21 = monthlydata.D21,
                            D22 = monthlydata.D22,
                            D23 = monthlydata.D23,
                            D24 = monthlydata.D24,
                            D25 = monthlydata.D25,
                            D26 = monthlydata.D26,
                            D27 = monthlydata.D27,
                            D28 = monthlydata.D28,
                            D29 = monthlydata.D29,
                            D30 = monthlydata.D30,
                            D31 = monthlydata.D31,
                            Comment = "Invalid Metric/Section name OR Permission denied for section"
                        });
                        InvalidCount++;
                        continue;
                    }

                    MonthlyPlanDataInvalid invalid = new MonthlyPlanDataInvalid();
                    bool invalidfound = false;
                    for (int m = 1; m <= 31; m++)
                    {
                        invalidfound = false;
                        switch (m)
                        {
                            case 1:
                                if (monthlydata.D1 < 0 || monthlydata.D1 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(1, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 2:
                                if (monthlydata.D2 < 0 || monthlydata.D2 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(2, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 3:
                                if (monthlydata.D3 < 0 || monthlydata.D3 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(3, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 4:
                                if (monthlydata.D4 < 0 || monthlydata.D4 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(4, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 5:
                                if (monthlydata.D5 < 0 || monthlydata.D5 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(5, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 6:
                                if (monthlydata.D6 < 0 || monthlydata.D6 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(6, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 7:
                                if (monthlydata.D7 < 0 || monthlydata.D7 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(7, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 8:
                                if (monthlydata.D8 < 0 || monthlydata.D8 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(8, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 9:
                                if (monthlydata.D9 < 0 || monthlydata.D9 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(9, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 10:
                                if (monthlydata.D10 < 0 || monthlydata.D10 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(10, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 11:
                                if (monthlydata.D11 < 0 || monthlydata.D11 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(11, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 12:
                                if (monthlydata.D12 < 0 || monthlydata.D12 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(12, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 13:
                                if (monthlydata.D13 < 0 || monthlydata.D13 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(13, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 14:
                                if (monthlydata.D14 < 0 || monthlydata.D14 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(14, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 15:
                                if (monthlydata.D15 < 0 || monthlydata.D15 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(15, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 16:
                                if (monthlydata.D16 < 0 || monthlydata.D16 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(16, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 17:
                                if (monthlydata.D17 < 0 || monthlydata.D17 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(17, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 18:
                                if (monthlydata.D18 < 0 || monthlydata.D18 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(18, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 19:
                                if (monthlydata.D19 < 0 || monthlydata.D19 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(19, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 20:
                                if (monthlydata.D20 < 0 || monthlydata.D20 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(20, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 21:
                                if (monthlydata.D21 < 0 || monthlydata.D21 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(21, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 22:
                                if (monthlydata.D22 < 0 || monthlydata.D22 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(22, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 23:
                                if (monthlydata.D23 < 0 || monthlydata.D23 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(23, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 24:
                                if (monthlydata.D24 < 0 || monthlydata.D24 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(24, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 25:
                                if (monthlydata.D25 < 0 || monthlydata.D25 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(25, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 26:
                                if (monthlydata.D26 < 0 || monthlydata.D26 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(26, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 27:
                                if (monthlydata.D27 < 0 || monthlydata.D27 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(27, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 28:
                                if (monthlydata.D28 < 0 || monthlydata.D28 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(28, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 29:
                                if (monthlydata.D29 < 0 || monthlydata.D29 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(29, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 30:
                                if (monthlydata.D30 < 0 || monthlydata.D30 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(30, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                            case 31:
                                if (monthlydata.D31 < 0 || monthlydata.D31 > 99999999)
                                {
                                    invalid.Comment = "Invalid value for " + CommonHelper.GetMonthDaysSequence(31, month.ToString()) + ".";
                                    invalidfound = true;
                                }
                                break;
                        }
                        if (invalidfound)
                            break;
                    }
                    if (invalidfound)
                    {
                        invalid.SiteName = monthlydata.SiteName;
                        invalid.SectionName = monthlydata.SectionName;
                        invalid.MetricName = monthlydata.MetricName;
                        invalid.PlanDataId = monthlydata.PlanDataId;
                        invalid.Month = monthlydata.Month;
                        invalid.Year = monthlydata.Year;
                        invalid.D1 = monthlydata.D1;
                        invalid.D2 = monthlydata.D2;
                        invalid.D3 = monthlydata.D3;
                        invalid.D4 = monthlydata.D4;
                        invalid.D5 = monthlydata.D5;
                        invalid.D6 = monthlydata.D6;
                        invalid.D7 = monthlydata.D7;
                        invalid.D8 = monthlydata.D8;
                        invalid.D9 = monthlydata.D9;
                        invalid.D10 = monthlydata.D10;
                        invalid.D11 = monthlydata.D11;
                        invalid.D12 = monthlydata.D12;
                        invalid.D13 = monthlydata.D13;
                        invalid.D14 = monthlydata.D14;
                        invalid.D15 = monthlydata.D15;
                        invalid.D16 = monthlydata.D16;
                        invalid.D17 = monthlydata.D17;
                        invalid.D18 = monthlydata.D18;
                        invalid.D19 = monthlydata.D19;
                        invalid.D20 = monthlydata.D20;
                        invalid.D21 = monthlydata.D21;
                        invalid.D22 = monthlydata.D22;
                        invalid.D23 = monthlydata.D23;
                        invalid.D24 = monthlydata.D24;
                        invalid.D25 = monthlydata.D25;
                        invalid.D26 = monthlydata.D26;
                        invalid.D27 = monthlydata.D27;
                        invalid.D28 = monthlydata.D28;
                        invalid.D29 = monthlydata.D29;
                        invalid.D30 = monthlydata.D30;
                        invalid.D31 = monthlydata.D31;
                        continue;
                    }

                    if (metrics.Any(x => x.Text == monthlydata.SectionName + "^" + monthlydata.MetricName))
                    {
                        monthlydata.SiteMetricId = Convert.ToInt32(metrics.Where(x => x.Text == monthlydata.SectionName + "^" + monthlydata.MetricName).FirstOrDefault().Value);
                    }
                    else
                    {
                        lstInvalid.Add(new MonthlyPlanDataInvalid
                        {
                            SiteName = monthlydata.SiteName,
                            SectionName = monthlydata.SectionName,
                            MetricName = monthlydata.MetricName,
                            PlanDataId = monthlydata.PlanDataId,
                            Month = monthlydata.Month,
                            Year = monthlydata.Year,
                            D1 = monthlydata.D1,
                            D2 = monthlydata.D2,
                            D3 = monthlydata.D3,
                            D4 = monthlydata.D4,
                            D5 = monthlydata.D5,
                            D6 = monthlydata.D6,
                            D7 = monthlydata.D7,
                            D8 = monthlydata.D8,
                            D9 = monthlydata.D9,
                            D10 = monthlydata.D10,
                            D11 = monthlydata.D11,
                            D12 = monthlydata.D12,
                            D13 = monthlydata.D13,
                            D14 = monthlydata.D14,
                            D15 = monthlydata.D15,
                            D16 = monthlydata.D16,
                            D17 = monthlydata.D17,
                            D18 = monthlydata.D18,
                            D19 = monthlydata.D19,
                            D20 = monthlydata.D20,
                            D21 = monthlydata.D21,
                            D22 = monthlydata.D22,
                            D23 = monthlydata.D23,
                            D24 = monthlydata.D24,
                            D25 = monthlydata.D25,
                            D26 = monthlydata.D26,
                            D27 = monthlydata.D27,
                            D28 = monthlydata.D28,
                            D29 = monthlydata.D29,
                            D30 = monthlydata.D30,
                            D31 = monthlydata.D31,
                            Comment = "Invalid Metric/Section name OR Permission denied for section"
                        });
                        InvalidCount++;
                        continue;
                    }

                    var modellist = (from x in _montlhyPlanDataRepository.Table
                                     where x.SiteMetricId == monthlydata.SiteMetricId &&
                                   x.Year == year &&
                                   x.Month == month
                                     select x).FirstOrDefault();

                    if (modellist != null)
                    {
                        modellist.D1 = monthlydata.D1 ?? modellist.D1;
                        modellist.D2 = monthlydata.D2 ?? modellist.D2;
                        modellist.D3 = monthlydata.D3 ?? modellist.D3;
                        modellist.D4 = monthlydata.D4 ?? modellist.D4;
                        modellist.D5 = monthlydata.D5 ?? modellist.D5;
                        modellist.D6 = monthlydata.D6 ?? modellist.D6;
                        modellist.D7 = monthlydata.D7 ?? modellist.D7;
                        modellist.D8 = monthlydata.D8 ?? modellist.D8;
                        modellist.D9 = monthlydata.D9 ?? modellist.D9;
                        modellist.D10 = monthlydata.D10 ?? modellist.D10;
                        modellist.D11 = monthlydata.D11 ?? modellist.D11;
                        modellist.D12 = monthlydata.D12 ?? modellist.D12;
                        modellist.D13 = monthlydata.D13 ?? modellist.D13;
                        modellist.D14 = monthlydata.D14 ?? modellist.D14;
                        modellist.D15 = monthlydata.D15 ?? modellist.D15;
                        modellist.D16 = monthlydata.D16 ?? modellist.D16;
                        modellist.D17 = monthlydata.D17 ?? modellist.D17;
                        modellist.D18 = monthlydata.D18 ?? modellist.D18;
                        modellist.D19 = monthlydata.D19 ?? modellist.D19;
                        modellist.D20 = monthlydata.D10 ?? modellist.D10;
                        modellist.D21 = monthlydata.D21 ?? modellist.D21;
                        modellist.D22 = monthlydata.D22 ?? modellist.D22;
                        modellist.D23 = monthlydata.D23 ?? modellist.D23;
                        modellist.D24 = monthlydata.D24 ?? modellist.D24;
                        modellist.D25 = monthlydata.D25 ?? modellist.D25;
                        modellist.D26 = monthlydata.D26 ?? modellist.D26;
                        modellist.D27 = monthlydata.D27 ?? modellist.D27;
                        modellist.D28 = monthlydata.D28 ?? modellist.D28;
                        modellist.D29 = monthlydata.D29 ?? modellist.D29;
                        modellist.D30 = monthlydata.D30 ?? modellist.D30;
                        modellist.D31 = monthlydata.D31 ?? modellist.D31;
                        modellist.ModifiedBy = ProjectSession.UserID;
                        modellist.ModifiedDate = DateTime.Now;

                        _montlhyPlanDataRepository.Update(modellist);

                    }
                    else
                    {
                        var objmonthlyPlandata = new MonthlyPlanData
                        {
                            SiteMetricId = monthlydata.SiteMetricId,
                            Month = month,
                            Year = year,
                            D1 = monthlydata.D1,
                            D2 = monthlydata.D2,
                            D3 = monthlydata.D3,
                            D4 = monthlydata.D4,
                            D5 = monthlydata.D5,
                            D6 = monthlydata.D6,
                            D7 = monthlydata.D7,
                            D8 = monthlydata.D8,
                            D9 = monthlydata.D9,
                            D10 = monthlydata.D10,
                            D11 = monthlydata.D11,
                            D12 = monthlydata.D12,
                            D13 = monthlydata.D13,
                            D14 = monthlydata.D14,
                            D15 = monthlydata.D15,
                            D16 = monthlydata.D16,
                            D17 = monthlydata.D17,
                            D18 = monthlydata.D18,
                            D19 = monthlydata.D19,
                            D20 = monthlydata.D20,
                            D21 = monthlydata.D21,
                            D22 = monthlydata.D22,
                            D23 = monthlydata.D23,
                            D24 = monthlydata.D24,
                            D25 = monthlydata.D25,
                            D26 = monthlydata.D26,
                            D27 = monthlydata.D27,
                            D28 = monthlydata.D28,
                            D29 = monthlydata.D29,
                            D30 = monthlydata.D30,
                            D31 = monthlydata.D31,
                            CreatedBy = ProjectSession.UserID,
                            CreatedDate = DateTime.Now
                        };

                        _montlhyPlanDataRepository.Insert(objmonthlyPlandata);

                    }

                }

            }
            return lstInvalid;
        }

    }

}
