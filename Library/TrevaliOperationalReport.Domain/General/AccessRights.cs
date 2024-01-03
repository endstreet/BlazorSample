using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_AccessRights", Schema = "dbo")]
    public class AccessRights
    {
        /// <summary>
        /// Get or set AccessRightID
        /// </summary>
        [Key]
        public int AccessRightID { get; set; }

        /// <summary>
        /// Get or set AccessRightName
        /// </summary>
        public string AccessRightName { get; set; }

        /// <summary>
        /// Get or set Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get or set IsActive
        /// </summary>
        public bool IsActive { get; set; }
    }

}
