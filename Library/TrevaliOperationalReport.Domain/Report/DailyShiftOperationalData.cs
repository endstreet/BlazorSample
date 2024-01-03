using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.Report
{
    [Table("RPT_DailyShiftOperationalData", Schema = "dbo")]
    public class DailyShiftOperationalData : BaseEntity
    {
        /// <summary>
        /// Get or set DailyShiftOperationalDataId
        /// </summary>
        [Key]
        public long DailyShiftOperationalDataId { get; set; }

        /// <summary>
        /// Get or set DailyOperationalDataId
        /// </summary>
        public long DailyOperationalDataId { get; set; }

        /// <summary>
        /// Get or set ShiftId
        /// </summary>
        public int? ShiftId { get; set; }

        /// <summary>
        /// Get or set EquipmentId
        /// </summary>
        public int? EquipmentId { get; set; }

        /// <summary>
        /// Get or set ActualValue
        /// </summary>
        public decimal? ActualValue { get; set; }

        [ForeignKey("ShiftId")]
        public virtual General.Shift Shift { get; set; }

        [ForeignKey("EquipmentId")]
        public virtual General.Equipment Equipment { get; set; }

    }

}
