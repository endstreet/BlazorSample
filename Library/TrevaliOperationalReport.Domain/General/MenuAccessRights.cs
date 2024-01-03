using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_MenuAccessRights", Schema = "dbo")]
    public class MenuAccessRights
    {
        /// <summary>
        /// Get or set MenuAccessRightID
        /// </summary>
        [Key]
        public int MenuAccessRightID { get; set; }

        /// <summary>
        /// Get or set MenuID
        /// </summary>
        public int MenuID { get; set; }

        /// <summary>
        /// Get or set AccessRightID
        /// </summary>
        public int AccessRightID { get; set; }
    }

}
