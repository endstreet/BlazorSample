using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_UserSiteRights", Schema = "dbo")]
    public class UserSiteRights
    {
        /// <summary>
        /// Get or set UserSiteRightsID
        /// </summary>
        public int UserSiteRightsID { get; set; }

        /// <summary>
        /// Get or set UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Get or set SiteId
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Get or set IsView
        /// </summary>
        public bool IsView { get; set; }

        /// <summary>
        /// Get or set IsFullRights
        /// </summary>
        public bool IsFullRights { get; set; }
    }

    public class TrevaliAccessRight
    {
        public string AccessRightName { get; set; }
    }
}
