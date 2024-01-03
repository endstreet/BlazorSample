using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.Report;

namespace TrevaliOperationalReport.Service.Report
{
    public interface IMonthlyPlanBudgetDataService
    {
        /// <summary>
        /// Gets the MonthlyBudgetPlanData by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>MonthlyBudgetPlanData.</returns>
        MonthlyBudgetPlanData GetMonthlyBudgetPlanDataById(long id);

        /// <summary>
        /// search monthly budget plan
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="metricId"></param>
        /// <returns></returns>
        IList<MonthlyBudgetPlanData> SearchMonthlyBudgetPlan(int siteId, int reportId, int metricId, int month, int year);

        /// <summary>
        /// Upload monthly plan data
        /// </summary>
        /// <param name="lstMontlhyPlanData"></param>
        /// <returns></returns>
        List<InvalidMonthlyPlanData> UploadMonthlyPlanData(List<MonthlyBudgetPlanData> lstMontlhyPlanData, int reportid);

        /// <summary>
        ///   Gets sites  selectlist.
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetSitesSelectList(int siteId = 0, bool IsFullRequired = false);

        /// <summary>
        /// Gets metrics  selectlist.
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetMetricsSelectList(int siteId = 0, int reportId = 0, bool IsDataDownload = false);

        /// <summary>
        /// gets reports selectlist
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetReportsSelectList(int reportId = 0);

        /// <summary>
        /// Updates the monthly plan data.
        /// </summary>
        /// <param name="Budgetmodel">The budgetmodel.</param>
        /// <returns>System.Int32.</returns>
        long UpdateMonthlyPlanData(MonthlyBudgetPlanData Budgetmodel);

        /// <summary>
        /// Inserts the monthly plan data.
        /// </summary>
        /// <param name="Budgetmodel">The budgetmodel.</param>
        /// <returns>System.Int32.</returns>
        long InsertMonthlyPlanData(MonthlyBudgetPlanData Budgetmodel);

        /// <summary>
        /// Upload monthly data
        /// </summary>
        /// <param name="lstMontlhyPlanData"></param>
        /// <returns></returns>
        List<InvalidMonthlyPlanData> UploadMonthlyData(List<MonthlyUploadModel> lstMontlhyPlanData, int reportid);

        /// <summary>
        /// Gets monthly budget data for whole year
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="reportId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        List<BudgetYearView> GetBudgetYearViewData(int siteId, int reportId, int year);

        /// <summary>
        /// gets monthly data by sitemetricid 
        /// </summary>
        /// <param name="SiteMetricId"></param>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        MonthlyBudgetPlanData GetMonthlyBudgetPlanDataBySiteMetricId(int SiteMetricId, int Year, int Month);

        /// <summary>
        /// Get status of approved months in a year for a site
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        List<MonthlyBudgetPlanDataApprove> GetMonthlyBudgetPlanDataApprovedStatus(int siteId, int year);

        /// <summary>
        /// Approves budget plan data for the month
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        bool ApproveBudgetPlanData(int siteId, int year, int month);


        IList<ApprovalDetails> GetApprovalDetails(int year, bool IsWeekly, int[] site = null);
    }
}
