using System.Collections.Generic;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface IPBIReportService
    {

        /// <summary>
        /// Serches the PBIreports
        /// </summary>
        /// <param name="ReportName"></param>
        /// <param name="ReportGUID"></param>
        /// <param name="DataSetGUID"></param>
        /// <returns></returns>
        IList<PBIReports> SearchPBIReports(string ReportGUID, string DataSetGUID);

        /// <summary>
        /// Inserts the PBIReports.
        /// </summary>
        /// <param name="pbireport">The pbireport.</param>
        int InsertPBIReports(PBIReports pbireport);

        /// <summary>
        /// Updates the PBIReports.
        /// </summary>
        /// <param name="pbireport">The pbireport.</param>
        int UpdatePBIReports(PBIReports pbireport);

        /// <summary>
        /// Gets All the PBIreports
        /// </summary>
        /// <returns></returns>
        IList<PBIReports> GetAllPBIReports();

        /// <summary>
        /// Deactivates All reports
        /// </summary>
        /// <returns></returns>
        void DeActivateAllReports(List<int> ActiveIds);

        /// <summary>
        /// Save user Reports (pinned)
        /// </summary>
        /// <param name="reportguid"></param>
        void SaveUserReport(string reportguid, bool IsPinned);

        PBIUserReports GetUserReport(string reportguid, int UserID);

        IList<PBIReports> GetReportRoles(int RoleId);

        /// <summary>
        /// Deletes the Report roles.
        /// </summary>
        /// <param name="RoleId">The role identifier.</param>
        void DeleteReportRoles(int RoleId);

        /// <summary>
        /// Inserts the report role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        int InsertReportRole(PBIReportRoles role);

        List<PBIReportsRights> GetPBIReportRights(int UserID);

        bool GetMenuReport(string reportguid, int MenuId = 0);
    }
}
