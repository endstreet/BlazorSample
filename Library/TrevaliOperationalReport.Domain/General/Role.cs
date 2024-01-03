using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_Role", Schema = "dbo")]
    public class Role : BaseEntity
    {
        /// <summary>
        /// Get or set RoleId
        /// </summary>
        [Key]
        public int RoleId { get; set; }

        /// <summary>
        /// Get or set RoleName
        /// </summary>
        [Required]
        public string RoleName { get; set; }

        /// <summary>
        /// Get or set Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get or set IsActive
        /// </summary>
        public bool IsActive { get; set; }

        [NotMapped]
        public string AccessRight { get; set; }

        [NotMapped]
        public List<Menus> LstMenuDynamic { get; set; }
    }


    [Table("GEN_ReportRoles", Schema = "dbo")]
    public class PBIReportRoles
    {
        /// <summary>
        /// Get or set ReportRoleId
        /// </summary>
        [Key]
        public int ReportRoleId { get; set; }

        /// <summary>
        /// Get or set RoleId
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Get or set PBIReportID
        /// </summary>
        public int PBIReportID { get; set; }
    }


    public class PBIReportRole
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public List<PBIReportRolePopup> PBIReportRoles { get; set; }
    }
    public class PBIReportRolePopup
    {
        #region Public Property
        public int PBIReportId { get; set; }
        public string ReportName { get; set; }
        public bool IsChecked { get; set; }
        #endregion
    }

    public class PBIReportsRights
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int UserID { get; set; }
        public int PBIReportID { get; set; }
        public string ReportGUID { get; set; }
        public string DataSetGUID { get; set; }
        public string ReportName { get; set; }

    }

    public class SectionRoles
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public List<SectionRolePopup> SectionRole { get; set; }
    }
    public class SectionRolePopup
    {
        #region Public Property
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public bool IsChecked { get; set; }
        #endregion
    }



}
