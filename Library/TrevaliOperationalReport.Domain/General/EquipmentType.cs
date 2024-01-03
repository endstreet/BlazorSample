using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("Mining_EquipmentType", Schema = "dbo")]
    public class EquipmentTypes
    {
        /// <summary>
        /// Get or set EquipmentTypeId
        /// </summary>
        [Key]
        public int EquipmentTypeId { get; set; }

        /// <summary>
        /// Get or set EquipmentType
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// Get or set DisplayOrderInEquipmentSheet
        /// </summary>
        public int DisplayOrderInEquipmentSheet { get; set; }

        /// <summary>
        /// Get or set HaveCapacity
        /// </summary>
        public bool? HaveCapacity { get; set; }

        /// <summary>
        /// Get or set IsTargetRequired
        /// </summary>
        public bool? IsTargetRequired { get; set; }
    }

}
