using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.Report;

namespace TrevaliOperationalReport.Service.Report
{
    public interface IDailyOperationalDataService
    {
        /// <summary>
        ///   Gets sites  selectlist.
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetSitesSelectList(int siteId = 0, int IsSync = -1);

        /// <summary>
        /// Gets metrics  selectlist.
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetMetricsSelectList(int siteId = 0, int ReportId = 0);

        /// <summary>
        /// Gets reports  selectlist.
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetReportsSelectList();

        /// <summary>
        /// Get daily data for grid
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="ReportId"></param>
        /// <param name="Month"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        IList<DailyMetricsData> GetDailyOperationalDataMetrics(int SiteId, int ReportId, int Month, int Year, int Day);

        /// <summary>
        /// updates daily data
        /// </summary>
        /// <param name="DailyData"></param>
        void UpdateDailyData(DailyMetricsData DailyData);

        /// <summary>
        /// binds daily shift wise data
        /// </summary>
        /// <param name="DailyOperationalDataId"></param>
        /// <returns></returns>
        IList<DailyShiftMetricsData> GetDailyShiftOperationalDataList(int DailyOperationalDataId);

        /// <summary>
        /// updates daily shift data
        /// </summary>
        /// <param name="DailyShiftData"></param>
        void UpdateDailyShiftData(DailyShiftMetricsData DailyShiftData);

    }
}
