using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.Report;

namespace TrevaliOperationalReport.Service.Report
{
    public interface IMonthlyForecastDataService
    {
        IList<SelectListItem> GetSitesSelectList(int siteId = 0, bool IsFullRightsRequired = false);
        IList<SelectListItem> GetMonthsSelectList();
        IList<SelectListItem> GetMetricsSelectList(int siteId = 0, int ReportId = 0, bool IsDataDownload = false);
        IList<MonthlyForecastData> GetMonthlyForecastData(int SiteId, string forecast);
        IList<SelectListItem> GetForecastListFormatted(int siteId = 0, bool IsDownload = false);
        IList<SelectListItem> GetMetricsSelectList(int siteId = 0);
        List<InvalidMonthlyForecastData> UploadMonthlyForecastData(List<MonthlyForecastUploadModel> lstMonthlyForecastData);
        //List<InvalidAnnualForecastData> UploadAnnualForecastData(List<AnnualForecastUploadModel> lstMonthlyForecastData);

        /// <summary>
        /// Updates Monthly Forecast Data
        /// </summary>
        /// <param name="ForecastData"></param>
        void UpdateMonthlyForecastData(MonthlyForecastData ForecastData);

        /// <summary>
        /// Approves Forecast
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="forecast"></param>
        /// <returns></returns>
        bool ApproveMonthlyForecastData(int SiteId, string forecast);

        /// <summary>
        /// checks forecast approval status
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="forecast"></param>
        /// <returns></returns>
        bool CheckApproveStatus(int SiteId, string forecast);

        int GetSiteIdFromSiteName(string sitename);
    }
}
