using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface IMetricsService
    {
        /// <summary>
        /// Searches the metricss.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <returns>IList&lt;Metrics&gt;.</returns>
        IList<Metrics> SearchMetricss(string metrics);

        /// <summary>
        /// Inserts the metrics.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        int InsertMetrics(Metrics metrics);

        /// <summary>
        /// Updates the metrics.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        int UpdateMetrics(Metrics metrics);

        /// <summary>
        /// Gets the metrics by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Metrics.</returns>
        Metrics GetMetricsById(int id);

        /// <summary>
        /// Deletes the metrics.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteMetrics(Metrics metrics);

        /// <summary>
        /// Gets metrics  selectlist.
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetMetricsSelectList(bool IsWithUnits = false);
    }
}
