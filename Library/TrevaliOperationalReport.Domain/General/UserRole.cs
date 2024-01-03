using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_UserRole", Schema = "dbo")]
    public class UserRole
    {
        /// <summary>
        /// Get or set UserRoleId
        /// </summary>
        [Key]
        public int UserRoleId { get; set; }

        /// <summary>
        /// Get or set UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Get or set RoleId
        /// </summary>
        public int RoleId { get; set; }
    }

}
