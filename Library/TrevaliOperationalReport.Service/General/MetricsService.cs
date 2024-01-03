using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public class MetricsService : IMetricsService
    {
        #region Fields

        private readonly IRepository<Metrics> _metricsRepository;
        private readonly IRepository<Unit> _unitRepository;

        #endregion

        #region Ctor

        public MetricsService(IRepository<Metrics> metricsRepository, IRepository<Unit> unitRepository)
        {
            _metricsRepository = metricsRepository;
            _unitRepository = unitRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches the metricss.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <returns>IList&lt;Metrics&gt;.</returns>
        public IList<Metrics> SearchMetricss(string metrics)
        {
            var query = from p in _metricsRepository.Table
                        where ((p.MetricsName.Contains(metrics) || metrics == ""))
                        orderby p.MetricId descending
                        select p;

            return query.ToList();

        }

        /// <summary>
        /// Inserts the metrics.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <exception cref="System.ArgumentNullException">metrics</exception>
        public int InsertMetrics(Metrics metrics)
        {
            if (metrics == null)
                throw new ArgumentNullException("metrics");
            if (checkExistingRecord(metrics))
            {
                return -1;
            }
            metrics.CreatedBy = ProjectSession.UserID;
            metrics.CreatedDate = DateTime.Now;
            _metricsRepository.Insert(metrics);
            return metrics.MetricId;
        }

        public int UpdateMetrics(Metrics metrics)
        {
            if (metrics == null)
                throw new ArgumentNullException("metrics");
            if (checkExistingRecord(metrics))
            {
                return -1;
            }
            var model = GetMetricsById(metrics.MetricId);
            if (model != null)
            {
                model.UnitId = metrics.UnitId;
                model.MetricsName = metrics.MetricsName;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
            }
            _metricsRepository.Update(model);
            return metrics.MetricId;
        }

        /// <summary>
        /// Gets the metrics by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Metrics.</returns>
        public Metrics GetMetricsById(int id)
        {
            return _metricsRepository.GetById(id);
        }

        /// <summary>
        /// Deletes the metrics.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">metrics</exception>
        public bool DeleteMetrics(Metrics metrics)
        {
            if (metrics == null)
                throw new ArgumentNullException("metrics");

            try
            {
                _metricsRepository.Delete(metrics);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets metrics  selectlist.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetMetricsSelectList(bool IsWithUnits = false)
        {
            if (IsWithUnits)
            {
                var query = from p in _metricsRepository.Table
                            orderby p.MetricsName ascending
                            select new SelectListItem
                            {
                                Text = p.MetricsName + " (" + p.Unit.UOM + ")",
                                Value = p.MetricId.ToString()
                            };

                return query.ToList();

            }
            else
            {
                var query = from p in _metricsRepository.Table
                            orderby p.MetricsName ascending
                            select new SelectListItem
                            {
                                Text = p.MetricsName,
                                Value = p.MetricId.ToString()
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingRecord(Metrics model)
        {
            try
            {
                var query = from p in _metricsRepository.Table
                            where p.MetricId != model.MetricId &&
                            ((p.MetricsName).Equals(model.MetricsName))
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
        #endregion
    }
}
