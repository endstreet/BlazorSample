using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_Unit", Schema = "dbo")]
    public class Unit : BaseEntity
    {
        /// <summary>
        /// Get or set UnitId
        /// </summary>
        [Key]
        public int UnitId { get; set; }

        /// <summary>
        /// Get or set UOM
        /// </summary>
        [Required]
        public string UOM { get; set; }
    }
}
