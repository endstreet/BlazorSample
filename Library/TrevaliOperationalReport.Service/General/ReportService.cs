using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public class ReportService : IReportService
    {
        #region Fields

        private readonly IRepository<Reports> _reportRepository;

        #endregion

        #region Ctor

        public ReportService(IRepository<Reports> reportRepository)
        {
            _reportRepository = reportRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches the reports.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns>IList&lt;Report&gt;.</returns>
        public IList<Reports> SearchReports(string report)
        {
            var query = from p in _reportRepository.Table
                        where ((p.Name.Contains(report) || report == ""))
                        orderby p.ReportId descending
                        select p;

            return query.ToList();

        }

        /// <summary>
        /// Inserts the report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <exception cref="System.ArgumentNullException">report</exception>
        public int InsertReport(Reports report)
        {
            if (report == null)
                throw new ArgumentNullException("report");
            if (checkExistingRecord(report))
            {
                return -1;
            }
            report.CreatedBy = ProjectSession.UserID;
            report.CreatedDate = DateTime.Now;
            _reportRepository.Insert(report);
            return report.ReportId;
        }

        /// <summary>
        /// Updates the report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <exception cref="System.ArgumentNullException">report</exception>
        public int UpdateReport(Reports report)
        {
            if (report == null)
                throw new ArgumentNullException("report");
            if (checkExistingRecord(report))
            {
                return -1;
            }
            var model = GetReportById(report.ReportId);
            if (model != null)
            {
                model.Name = report.Name;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
            }
            _reportRepository.Update(model);
            return report.ReportId;
        }

        /// <summary>
        /// Gets the report by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Report.</returns>
        public Reports GetReportById(int id)
        {
            return _reportRepository.GetById(id);
        }

        /// <summary>
        /// Deletes the report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">report</exception>
        public bool DeleteReport(Reports report)
        {
            if (report == null)
                throw new ArgumentNullException("report");

            try
            {
                _reportRepository.Delete(report);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the report select list.
        /// </summary>
        /// <returns>IList&lt;SelectListItem&gt;.</returns>
        public IList<SelectListItem> GetReportSelectList()
        {
            var query = from p in _reportRepository.Table
                        orderby p.Name ascending
                        select new SelectListItem
                        {
                            Text = p.Name,
                            Value = p.ReportId.ToString()
                        };
            return query.ToList();
        }

        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingRecord(Reports model)
        {
            try
            {
                var query = from p in _reportRepository.Table
                            where p.ReportId != model.ReportId &&
                            ((p.Name).Equals(model.Name))
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
