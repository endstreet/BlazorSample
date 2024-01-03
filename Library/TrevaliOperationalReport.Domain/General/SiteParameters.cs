using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_SiteParameters", Schema = "dbo")]
    public class SiteParameters : BaseEntity
    {
        /// <summary>
        /// Get or set SiteParameterId
        /// </summary>
        [Key]
        public int SiteParameterId { get; set; }

        /// <summary>
        /// Get or set SiteId
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Get or set Year
        /// </summary>
        [Required]
        public int Year { get; set; }

        /// <summary>
        /// Get or set LowFactor
        /// </summary>
        [Required]
        public decimal ZnLowGuide { get; set; }

        /// <summary>
        /// Get or set HighFactor
        /// </summary>
        [Required]
        public decimal ZnHighGuide { get; set; }

        /// <summary>
        /// Get or set LowFactor
        /// </summary>
        [Required]
        public decimal PbLowPayable { get; set; }

        /// <summary>
        /// Get or set HighFactor
        /// </summary>
        [Required]
        public decimal PbHighPayable { get; set; }

        /// <summary>
        /// Get or set LowFactor
        /// </summary>
        [Required]
        public decimal AgLowPayable { get; set; }

        /// <summary>
        /// Get or set HighFactor
        /// </summary>
        [Required]
        public decimal AgHighPayable { get; set; }

        /// <summary>
        /// Get or set ZnPayableValue
        /// </summary>

        [Required]
        public decimal ZnPayableValue { get; set; }

        /// <summary>
        /// Get or set ZnPayablePercentage
        /// </summary>
        [Required]
        public decimal ZnPayablePercentage { get; set; }

        /// <summary>
        /// Get or set PbPayableValue
        /// </summary>
        [Required]
        public decimal PbPayableValue { get; set; }

        /// <summary>
        /// Get or set PbPayablePercentage
        /// </summary>
        [Required]
        public decimal PbPayablePercentage { get; set; }

        /// <summary>
        /// Get or set AgPayableValue1
        /// </summary>
        [Required]
        public decimal AgPayableValue1 { get; set; }

        /// <summary>
        /// Get or set AgPayableValue2
        /// </summary>
        [Required]
        public decimal AgPayableValue2 { get; set; }


    }

}
