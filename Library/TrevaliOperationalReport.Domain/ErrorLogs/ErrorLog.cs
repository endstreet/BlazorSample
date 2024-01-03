using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.ErrorLogs
{
    [Table("ErrorLog", Schema = "dbo")]
    public class ErrorLog
    {
        /// <summary>
        /// Get or set ErrorLogId
        /// </summary>
        [Key]

        public int ErrorLogId { get; set; }

        /// <summary>
        /// Get or set ErrorDate
        /// </summary>
        public DateTime? ErrorDate { get; set; }

        /// <summary>
        /// Get or set IPAddress
        /// </summary>
        public string IPAddress { get; set; }

        ///// <summary>
        ///// Get or set ClientCode
        ///// </summary>
        //public string ClientCode { get; set; }

        /// <summary>
        /// Get or set ClientBrowser
        /// </summary>
        public string ClientBrowser { get; set; }

        /// <summary>
        /// Get or set ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Get or set ErrorStackTrace
        /// </summary>
        public string ErrorStackTrace { get; set; }

        /// <summary>
        /// Get or set URL
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Get or set URLReferrer
        /// </summary>
        public string URLReferrer { get; set; }

        /// <summary>
        /// Get or set ErrorSource
        /// </summary>
        public string ErrorSource { get; set; }

        /// <summary>
        /// Get or set ErrorTargetSite
        /// </summary>
        public string ErrorTargetSite { get; set; }

        /// <summary>
        /// Get or set QueryString
        /// </summary>
        public string QueryString { get; set; }

        /// <summary>
        /// Get or set PostData
        /// </summary>
        public string PostData { get; set; }

        /// <summary>
        /// Get or set SessionInfo
        /// </summary>
        public string SessionInfo { get; set; }
    }
}
