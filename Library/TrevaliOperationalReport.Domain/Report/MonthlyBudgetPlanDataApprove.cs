using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.Report
{
    [Table("RPT_MonthlyBudgetPlanDataApprove", Schema = "dbo")]
    public class MonthlyBudgetPlanDataApprove : BaseEntity
    {
        /// <summary>
        /// Get or set MonthlyBudgetPlanDataApproveId
        /// </summary>
        [Key]
        public int MonthlyBudgetPlanDataApproveId { get; set; }

        /// <summary>
        /// Get or set SiteId
        /// </summary>
        public int SiteId { get; set; }

        [ForeignKey("SiteId")]
        public virtual General.Site Site { get; set; }

        /// <summary>
        /// Get or set Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Get or set Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Get or set IsApprove
        /// </summary>
        public bool IsApprove { get; set; }


    }

}
