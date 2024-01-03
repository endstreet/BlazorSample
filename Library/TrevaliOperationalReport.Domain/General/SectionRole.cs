using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_SectionRole", Schema = "dbo")]
    public class SectionRole
    {
        /// <summary>
        /// Get or set SectionRoleId
        /// </summary>
        [Key]
        public int SectionRoleId { get; set; }

        /// <summary>
        /// Get or set SectionId
        /// </summary>
        public int SectionId { get; set; }

        /// <summary>
        /// Get or set RoleId
        /// </summary>
        public int RoleId { get; set; }
    }

}
