using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.Report
{
    [Table("RPT_WeeklyOperationalDataApprove", Schema = "dbo")]
    public class WeeklyOperationalDataApprove : BaseEntity
    {
        /// <summary>
        /// Get or set WeeklyOperationalDataApproveId
        /// </summary>
        [Key]
        public int WeeklyOperationalDataApproveId { get; set; }

        /// <summary>
        /// Get or set SiteId
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Get or set Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Get or set Week
        /// </summary>
        public int Week { get; set; }

        /// <summary>
        /// Get or set IsApprove
        /// </summary>
        public bool IsApprove { get; set; }


    }

}
