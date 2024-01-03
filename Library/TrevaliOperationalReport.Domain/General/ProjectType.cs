using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_ProjectType", Schema = "dbo")]
    public class ProjectType : BaseEntity
    {
        /// <summary>
        /// Get or set ProjectTypeId
        /// </summary>
        [Key]
        public int ProjectTypeId { get; set; }

        /// <summary>
        /// Get or set Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Get or set IsActive
        /// </summary>
        public bool IsActive { get; set; }
    }
}
