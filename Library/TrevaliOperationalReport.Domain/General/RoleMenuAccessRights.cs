using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_RoleMenuAccessRights", Schema = "dbo")]
    public class RoleMenuAccessRights
    {
        /// <summary>
        /// Get or set RoleMenuAccessRightID
        /// </summary>
        [Key]
        public int RoleMenuAccessRightID { get; set; }

        /// <summary>
        /// Get or set MenuAccessRightID
        /// </summary>
        public int MenuAccessRightID { get; set; }

        /// <summary>
        /// Get or set RoleID
        /// </summary>
        public int RoleID { get; set; }
    }

}
