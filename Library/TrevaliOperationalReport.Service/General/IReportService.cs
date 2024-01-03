using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface IReportService
    {
        /// <summary>
        /// Searches the reports.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns>IList&lt;Report&gt;.</returns>
        IList<Reports> SearchReports(string report);

        /// <summary>
        /// Inserts the report.
        /// </summary>
        /// <param name="report">The report.</param>
        int InsertReport(Reports report);

        /// <summary>
        /// Updates the report.
        /// </summary>
        /// <param name="report">The report.</param>
        int UpdateReport(Reports report);

        /// <summary>
        /// Gets the report by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Report.</returns>
        Reports GetReportById(int id);

        /// <summary>
        /// Deletes the report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteReport(Reports report);

        /// <summary>
        /// Gets the report select list.
        /// </summary>
        /// <returns>IList&lt;SelectListItem&gt;.</returns>
        IList<SelectListItem> GetReportSelectList();
    }
}
