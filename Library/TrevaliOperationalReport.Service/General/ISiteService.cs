using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface ISiteService
    {
        /// <summary>
        /// Searches the sites.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns>IList&lt;Site&gt;.</returns>
        IList<Site> SearchSites(string site);

        /// <summary>
        /// Inserts the site.
        /// </summary>
        /// <param name="site">The site.</param>
        int InsertSite(Site site);

        /// <summary>
        /// Updates the site.
        /// </summary>
        /// <param name="site">The site.</param>
        int UpdateSite(Site site);

        /// <summary>
        /// Gets the site by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Site.</returns>
        Site GetSiteById(int id);

        /// <summary>
        /// Deletes the site.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteSite(Site site);

        /// <summary>
        /// Gets sites  selectlist.
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetSitesSelectList();

        IList<SelectListItem> GetCompanyRightsSelectList();
        /// <summary>
        /// Searches the site metricss.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <returns>IList&lt;SiteMetrics&gt;.</returns>
        IList<SiteMetrics> SearchSiteMetrics(int siteId);

        /// <summary>
        /// Inserts the site metrics.
        /// </summary>
        /// <param name="siteMetrics">The site metrics.</param>
        int InsertSiteMetrics(SiteMetrics siteMetrics);

        /// <summary>
        /// Deletes the site metrics.
        /// </summary>
        /// <param name="siteMetrics">The site metrics.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteSiteMetrics(SiteMetrics siteMetrics, bool IsForceDelete = false);

        SiteMetrics GetSiteMetricById(int siteMetricId);

        int UpdateSiteMetrics(SiteMetrics siteMetrics);

        SiteParameters GetSiteParameters(int siteid, int year);

        int InsertSiteParams(SiteParameters siteParams);

        int UpdateSiteParams(SiteParameters siteParams);

    }
}
