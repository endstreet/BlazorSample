using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_Users", Schema = "dbo")]
    public class Users : BaseEntity
    {
        /// <summary>
        /// Get or set UserID
        /// </summary>
        [Key]
        public int UserID { get; set; }

        /// <summary>
        /// Get or set Name
        /// </summary>
        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "*")]
        public string Name { get; set; }

        /// <summary>
        /// Get or set Surname
        /// </summary>
        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "*")]
        public string Surname { get; set; }

        /// <summary>
        /// Get or set EmailID
        /// </summary>
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$")]
        [StringLength(400, ErrorMessage = "*")]
        public string EmailID { get; set; }

        /// <summary>
        /// Get or set Designation
        /// </summary>
        [StringLength(100, ErrorMessage = "*")]
        public string Designation { get; set; }

        /// <summary>
        /// Get or set UserName
        /// </summary>
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "*")]
        public string UserName { get; set; }

        /// <summary>
        /// Get or set Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Get or set IsSuperAdmin
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// Get or set PasswordSalt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Get or set IsActive
        /// </summary>
        public bool? IsActive { get; set; }

        [NotMapped]
        public bool? IsRoleAssigned { get; set; }

        [NotMapped]
        public IList<Site> LstSiteDynamic { get; set; }

        [NotMapped]
        public string SiteRights { get; set; }

        [NotMapped]
        public string FullName { get; set; }

    }

    public class Login
    {
        /// <summary>
        /// Get or set UserName
        /// </summary>
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "*")]
        public string UserName { get; set; }

        /// <summary>
        /// Get or set Password
        /// </summary>
        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "*")]
        public string Password { get; set; }

        /// <summary>
        /// Get or set IsSuperAdmin
        /// </summary>
        public bool RememberMe { get; set; }

    }

    public class UserRoles
    {
        public int UserID { get; set; }
        public string UserName { get; set; }

        public List<RolePopup> Role { get; set; }
    }
    public class RolePopup
    {
        #region Public Property
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsChecked { get; set; }
        #endregion
    }
    //public class ChangePassword
    //{

    //    /// <summary>
    //    /// Get or set UserID
    //    /// </summary>
    //    public int UserID { get; set; }

    //    /// <summary>
    //    /// Get or set OldPassword
    //    /// </summary>
    //    [Required(ErrorMessage = "*")]
    //    [StringLength(100, ErrorMessage = "*")]
    //    public string OldPassword { get; set; }


    //    [Required]
    //    [StringLength(100)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "New password")]
    //    public string NewPassword { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm new password")]
    //    [Compare("NewPassword")]
    //    public string ConfirmPassword { get; set; }

    //}

}
