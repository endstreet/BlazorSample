using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("Mining_Equipment", Schema = "dbo")]
    public class Equipment
    {
        /// <summary>
        /// Get or set EquipmentId
        /// </summary>
        [Key]
        public int EquipmentId { get; set; }

        /// <summary>
        /// Get or set EquipmentName
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// Get or set EquipmentTypeId
        /// </summary>
        public int EquipmentTypeId { get; set; }

        /// <summary>
        /// Get or set DrillType
        /// </summary>
        public string DrillType { get; set; }

        /// <summary>
        /// Get or set Capacity
        /// </summary>
        public int? Capacity { get; set; }

        /// <summary>
        /// Get or set IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or set EquipmentDownTime
        /// </summary>
        public bool EquipmentDownTime { get; set; }


    }

}
