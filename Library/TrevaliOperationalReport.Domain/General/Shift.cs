using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("Mining_Shift", Schema = "dbo")]
    public class Shift
    {
        /// <summary>
        /// Get or set ShiftId
        /// </summary>
        [Key]
        public int ShiftId { get; set; }

        /// <summary>
        /// Get or set Shift
        /// </summary>
        public string ShiftName { get; set; }

        /// <summary>
        /// Get or set IsActive
        /// </summary>
        public bool IsActive { get; set; }
    }

}
