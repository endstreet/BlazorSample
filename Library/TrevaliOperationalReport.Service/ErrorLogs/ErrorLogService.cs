using System;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.ErrorLogs;

namespace TrevaliOperationalReport.Service.ErrorLogs
{
    public class ErrorLogService
    {

        #region Fields

        private static IRepository<ErrorLog> _errorLogRepository;

        #endregion Fields

        #region Ctor

        public ErrorLogService()
        {
            _errorLogRepository = new EfRepository<ErrorLog>(new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName));
        }

        #endregion Ctor

        #region Methods

        /// <summary>
        /// Inserts the errorlog.
        /// </summary>
        /// <param name="errorlog">The errorlog.</param>
        /// <exception cref="System.ArgumentNullException">errorlog</exception>
        public int AddErrorLog(ErrorLog errorlog)
        {
            if (errorlog == null)
                throw new ArgumentNullException("errorlog");

            _errorLogRepository.Insert(errorlog);
            return errorlog.ErrorLogId;
        }

        #endregion Methods
    }
}
