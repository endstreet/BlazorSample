using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.Report;

namespace TrevaliOperationalReport.Service.Report
{
    public interface IMonthlyPlanDataService
    {
        IList<SelectListItem> GetSitesSelectList(int siteId = 0, bool IsFullRightsRequired = false);

        IList<SelectListItem> GetMetricsSelectList(int siteId = 0, int ReportId = 0);

        IList<SelectListItem> GetNext24MonthsSelectList();

        IList<MonthlyPlanData> GetMonthlyPlanData(int SiteId, string month, string year);

        List<MonthlyPlanDataInvalid> UploadMonthlyPlanData(List<MonthlyPlanUploadModel> lstMonthlyPlanData);

        void UpdateMonthlyPlanData(MonthlyPlanData PlanData);
    }
}
