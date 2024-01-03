using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_Section", Schema = "dbo")]
    public class Section : BaseEntity
    {
        /// <summary>
        /// Get or set SectionId
        /// </summary>
        [Key]
        public int SectionId { get; set; }

        /// <summary>
        /// Get or set HideInReports
        /// </summary>
        public int HideInReports { get; set; }

        /// <summary>
        /// Get or set SectionName
        /// </summary>
        [Required]
        public string SectionName { get; set; }



        /// <summary>
        /// Get or set SectionMappingName
        /// </summary>
        public string SectionMappingName { get; set; }

    }
}
