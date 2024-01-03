using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_Metrics", Schema = "dbo")]
    public class Metrics : BaseEntity
    {
        /// <summary>
        /// Get or set MetricId
        /// </summary>
        [Key]
        public int MetricId { get; set; }

        /// <summary>
        /// Get or set MetricsName
        /// </summary>
        [Required]
        public string MetricsName { get; set; }

        /// <summary>
        /// Get or set MetricsMappingName
        /// </summary>
        public string MetricsMappingName { get; set; }

        /// <summary>
        /// Get or set UnitId
        /// </summary>
        [Required]
        public int UnitId { get; set; }

        /// <summary>
        /// Get or sets the Unit value
        /// </summary>
        /// <value>The Unit.</value>
        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }

        /// <summary>
        /// Get or sets the Unit Name
        /// </summary>
        /// <value>The name of the Unit.</value>
        [NotMapped]
        public string UnitName => Unit != null ? Unit.UOM : "";
    }
}
