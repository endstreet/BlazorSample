using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_PBIReports", Schema = "dbo")]
    public class PBIReports
    {
        /// <summary>
        /// Get or set PBIReportID
        /// </summary>
        [Key]
        public int PBIReportID { get; set; }

        /// <summary>
        /// Get or set ReportName
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// Get or set ReportGUID
        /// </summary>
        public string ReportGUID { get; set; }

        /// <summary>
        /// Get or set DataSetGUID
        /// </summary>
        public string DataSetGUID { get; set; }

        /// <summary>
        /// Get or set IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or set MenuId
        /// </summary>
        public int? MenuId { get; set; }


        /// <summary>
        /// Get or set CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Get or set CreatedBy
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Get or set ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Get or set ModifiedBy
        /// </summary>
        public int? ModifiedBy { get; set; }
    }

    [Table("GEN_PBIUserReports", Schema = "dbo")]
    public class PBIUserReports
    {
        /// <summary>
        /// Get or set PBIUserReportID
        /// </summary>
        [Key]
        public int PBIUserReportID { get; set; }

        /// <summary>
        /// Get or set PBIReportID
        /// </summary>
        public int PBIReportID { get; set; }

        /// <summary>
        /// Get or set UserID
        /// </summary>
        public int UserID { get; set; }
    }

}
