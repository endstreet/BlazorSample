using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;
using TrevaliOperationalReport.Domain.Report;

namespace TrevaliOperationalReport.Service.General
{
    public class SiteService : ISiteService
    {
        #region Fields

        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<SiteMetrics> _siteMetricsRepository;
        private readonly IRepository<SiteParameters> _siteParamsRepository;
        public static TrevaliOperationalReportObjectContext _dbContext;
        public static IRepository<WeeklyOperationalData> _weeklyOperationalRepository;
        public static IRepository<MonthlyBudgetPlanData> _MonthlyBudgetPlanDataRepository;
        #endregion

        #region Ctor

        public SiteService(IRepository<Site> siteRepository, IRepository<SiteMetrics> siteMetricsRepository, IRepository<SiteParameters> siteParamsRepository, IRepository<WeeklyOperationalData> weeklyOperationalRepository, IRepository<MonthlyBudgetPlanData> MonthlyBudgetPlanDataRepository)
        {
            _siteRepository = siteRepository;
            _siteMetricsRepository = siteMetricsRepository;
            _siteParamsRepository = siteParamsRepository;
            _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
            _weeklyOperationalRepository = weeklyOperationalRepository;
            _MonthlyBudgetPlanDataRepository = MonthlyBudgetPlanDataRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches the sites.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns>IList&lt;Site&gt;.</returns>
        public IList<Site> SearchSites(string site)
        {
            var query = from p in _siteRepository.Table
                        where ((p.SiteName.Contains(site) || site == ""))
                        orderby p.SiteId descending
                        select p;

            return query.ToList();

        }

        /// <summary>
        /// Inserts the site.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <exception cref="System.ArgumentNullException">site</exception>
        public int InsertSite(Site site)
        {
            if (site == null)
                throw new ArgumentNullException("site");
            if (checkExistingRecord(site))
            {
                return -1;
            }
            site.CreatedBy = ProjectSession.UserID;
            site.CreatedDate = DateTime.Now;
            _siteRepository.Insert(site);
            return site.SiteId;
        }

        /// <summary>
        /// Updates the site.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <exception cref="System.ArgumentNullException">site</exception>
        public int UpdateSite(Site site)
        {
            if (site == null)
                throw new ArgumentNullException("site");
            if (checkExistingRecord(site))
            {
                return -1;
            }
            var model = GetSiteById(site.SiteId);
            if (model != null)
            {
                if (site.Logo != null)
                {
                    model.Logo = site.Logo;
                }
                model.SiteName = site.SiteName;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
            }
            _siteRepository.Update(model);
            return site.SiteId;
        }

        /// <summary>
        /// Gets the site by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Site.</returns>
        public Site GetSiteById(int id)
        {
            return _siteRepository.GetById(id);
        }

        /// <summary>
        /// Deletes the site.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">site</exception>
        public bool DeleteSite(Site site)
        {
            if (site == null)
                throw new ArgumentNullException("site");

            try
            {
                _siteRepository.Delete(site);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets sites  selectlist.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetSitesSelectList()
        {
            var query = from p in _siteRepository.Table
                        orderby p.SiteName ascending
                        select new SelectListItem
                        {
                            Text = p.SiteName,
                            Value = p.SiteId.ToString()
                        };

            return query.ToList();

        }

        public IList<SelectListItem> GetCompanyRightsSelectList()
        {
            List<int> SiteIds = ProjectSession.UserCompanyRightsDynamic.Where(x => x.IsFullRights == true).Select(x => x.SiteId).ToList();

            var query = from p in _siteRepository.Table
                        where SiteIds.Contains(p.SiteId)
                        orderby p.SiteName ascending
                        select new SelectListItem
                        {
                            Text = p.SiteName,
                            Value = p.SiteId.ToString()
                        };
            return query.ToList();
        }

        /// <summary>
        /// Searches the site metricss.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <returns>IList&lt;SiteMetrics&gt;.</returns>
        public IList<SiteMetrics> SearchSiteMetrics(int siteId)
        {
            var query = from p in _siteMetricsRepository.Table
                        where (p.SiteId == siteId)
                        select p;

            return query.ToList().OrderBy(x => x.DisplayOrder).ToList();
        }

        /// <summary>
        /// Inserts the site metrics.
        /// </summary>
        /// <param name="siteMetrics">The site metrics.</param>
        /// <exception cref="System.ArgumentNullException">siteMetrics</exception>
        public int InsertSiteMetrics(SiteMetrics siteMetrics)
        {
            if (siteMetrics == null)
                throw new ArgumentNullException("siteMetrics");
            if (checkSiteMetrics(siteMetrics))
            {
                return -1;
            }
            siteMetrics.CreatedBy = ProjectSession.UserID;
            siteMetrics.CreatedDate = DateTime.Now;
            _siteMetricsRepository.Insert(siteMetrics);
            return siteMetrics.SiteMetricId;
        }

        /// <summary>
        /// Deletes the site metrics.
        /// </summary>
        /// <param name="siteMetrics">The site metrics.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">siteMetrics</exception>
        public bool DeleteSiteMetrics(SiteMetrics siteMetrics, bool IsForceDelete = false)
        {
            if (siteMetrics == null)
                throw new ArgumentNullException("siteMetrics");

            try
            {
                if (IsForceDelete)
                {
                    var weeklyData = (from p in _weeklyOperationalRepository.Table where p.SiteMetricId == siteMetrics.SiteMetricId select p).ToList();
                    if (weeklyData != null)
                    {
                        foreach (var weekdata in weeklyData)
                        {
                            _weeklyOperationalRepository.Delete(weekdata);
                        }
                    }
                    var monthlyData = (from p in _MonthlyBudgetPlanDataRepository.Table where p.SiteMetricId == siteMetrics.SiteMetricId select p).ToList();
                    if (monthlyData != null)
                    {
                        foreach (var monthdata in monthlyData)
                        {
                            _MonthlyBudgetPlanDataRepository.Delete(monthdata);
                        }
                    }

                }

                _siteMetricsRepository.Delete(siteMetrics);
                return true;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingRecord(Site model)
        {
            try
            {
                var query = from p in _siteRepository.Table
                            where p.SiteId != model.SiteId &&
                            ((p.SiteName).Equals(model.SiteName))
                            select p;
                if (query.ToList().Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks the site metrics.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkSiteMetrics(SiteMetrics model)
        {
            try
            {
                var query = from p in _siteMetricsRepository.Table
                            where (p.SiteMetricId != model.SiteMetricId) &&
                            (p.ReportId).Equals(model.ReportId) &&
                            (p.SectionId).Equals(model.SectionId) &&
                            (p.MetricId).Equals(model.MetricId) &&
                            (p.SiteId).Equals(model.SiteId)
                            select p;
                if (query.ToList().Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public SiteMetrics GetSiteMetricById(int siteMetricId)
        {
            return _siteMetricsRepository.GetById(siteMetricId);
        }


        public int UpdateSiteMetrics(SiteMetrics siteMetrics)
        {
            var model = (from x in _siteMetricsRepository.Table
                         where x.SiteMetricId == siteMetrics.SiteMetricId
                         select x).FirstOrDefault();

            if (model != null)
            {
                model.DisplayOrder = siteMetrics.DisplayOrder;
                model.IsYearly = siteMetrics.IsYearly;
                model.IsWeekly = siteMetrics.IsWeekly;

                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
                _siteMetricsRepository.Update(model);
                return model.SiteMetricId;
            }
            else
            {
                return -1;
            }
        }

        public IList<Site> GetAllSites(bool IsFullRightsRequired = false)
        {

            List<int> SiteIds = new List<int>();
            if (IsFullRightsRequired)
                SiteIds = ProjectSession.UserSiteRightsDynamic.Where(x => x.IsFullRights == true).Select(x => x.SiteId).ToList();
            else
                SiteIds = ProjectSession.UserSiteRightsDynamic.Where(x => x.IsFullRights == true || x.IsView == true).Select(x => x.SiteId).ToList();


            var query = from p in _siteRepository.Table
                        where (SiteIds.Contains(p.SiteId) || ProjectSession.IsAdmin == true)
                        select p;
            return query.ToList();

        }


        public SiteParameters GetSiteParameters(int siteid, int year)
        {
            return (from p in _siteParamsRepository.Table
                    where p.SiteId == siteid && p.Year == year
                    select p).FirstOrDefault();
        }


        public int InsertSiteParams(SiteParameters siteParams)
        {
            if (siteParams == null)
                throw new ArgumentNullException("siteParams");

            siteParams.CreatedBy = ProjectSession.UserID;
            siteParams.CreatedDate = DateTime.Now;
            _siteParamsRepository.Insert(siteParams);
            return siteParams.SiteParameterId;
        }

        public int UpdateSiteParams(SiteParameters siteParams)
        {
            if (siteParams == null)
                throw new ArgumentNullException("siteParams");

            var model = (from f in _siteParamsRepository.Table
                         where f.SiteParameterId == siteParams.SiteParameterId
                         select f).FirstOrDefault();

            if (model != null)
            {
                model.Year = siteParams.Year;
                model.ZnLowGuide = siteParams.ZnLowGuide;
                model.ZnHighGuide = siteParams.ZnHighGuide;
                model.PbLowPayable = siteParams.PbLowPayable;
                model.PbHighPayable = siteParams.PbHighPayable;
                model.PbPayablePercentage = siteParams.PbPayablePercentage;
                model.PbPayableValue = siteParams.PbPayableValue;
                model.ZnPayablePercentage = siteParams.ZnPayablePercentage;
                model.ZnPayableValue = siteParams.ZnPayableValue;
                model.AgLowPayable = siteParams.AgLowPayable;
                model.AgHighPayable = siteParams.AgHighPayable;
                model.AgPayableValue2 = siteParams.AgPayableValue2;
                model.AgPayableValue1 = siteParams.AgPayableValue1;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;

            }
            _siteParamsRepository.Update(model);
            return siteParams.SiteParameterId;
        }



        #endregion
    }
}
