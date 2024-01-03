using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.Report;

namespace TrevaliOperationalReport.Service.Report
{
    public interface IWeeklyOperationalDataService
    {

        /// <summary>
        /// search Weekly Operational data
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="metricId"></param>
        /// <returns></returns>
        IList<WeeklyOperationalData> SearchWeeklyOperationalData(int siteId, int reportId, int metricId, int week, int year);

        /// <summary>
        /// Upload weekly operational data
        /// </summary>
        /// <param name="lstWeeklyData"></param>
        /// <returns></returns>
        List<InvalidWeeklyData> UploadWeeklyOperationalData(List<WeeklyOperationalData> lstWeeklyData, int reportId);

        /// <summary>
        ///   Gets sites  selectlist.
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetSitesSelectList(int siteId = 0, int IsSync = -1, bool IsFullRightsRequired = false);

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
        /// Gets reports  selectlist.
        /// </summary>
        /// <returns> List with Actual Value Mtd Value Comment</returns>
        /*IList<WeeklyMetricsData> GetCommentsDownloadSample(int SiteId, int ReportId, int Week, int Year);*/



        IList<WeeklyMetricsData> GetWeeklyOperationalDataMetrics(int SiteId, int ReportId, int Week, int year);

        /// <summary>
        /// Updates WeeklyData
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="ReportId"></param>
        /// <returns></returns>
        void UpdateWeeklyData(WeeklyMetricsData WeeklyData);

        /// <summary>
        /// Get daily operational data list
        /// </summary>
        /// <param name="WeeklyOperationalDataId"></param>
        /// <returns></returns>
        IList<DailyOperationalData> GetDailyOperationalDataList(int WeeklyOperationalDataId);


        /// <summary>
        /// Get metrics list for dropdown list
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="reportId"></param>
        /// <returns></returns>
        List<SelectListItem> GetMeticsList(int siteId, int reportId);

        /// <summary>
        /// Add weekly data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        long InsertWeeklyData(WeeklyOperationalData model);

        bool CheckExistingSectionsData(WeeklyOperationalData model);


        decimal GetYTDFiguresCalculatedForWeek(int week, int year, int sitemetricId, ref bool IsPercentage);

        bool ApproveWeeklyData(int SiteId, int Year, int Week);

        bool CheckApproveStatus(int SiteId, int Year, int Week);

        int GetSiteIdFromSiteName(string sitename);

        bool SendWeeklyOperationalUploadData(int SiteId, int Year, int Week);
        IList<WeeklyMetricsData> GetCommentsDownloadSample(WeeklyMetricsData weeklyMetricsData);
    }
}
