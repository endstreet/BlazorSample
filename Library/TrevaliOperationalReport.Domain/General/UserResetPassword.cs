using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_UserResetPassword", Schema = "dbo")]
    public class UserResetPassword : BaseEntity
    {
        /// <summary>
        /// Get or set ResetLinkID
        /// </summary>
        [Key]
        public int ResetLinkID { get; set; }

        /// <summary>
        /// Get or set UserID
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// Get or set ResetType
        /// </summary>
        public int? ResetType { get; set; }

        /// <summary>
        /// Get or set ResetDone
        /// </summary>
        public bool? ResetDone { get; set; }

        /// <summary>
        /// Get or set IsActive
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the changepwd.
        /// </summary>
        /// <value>The changepwd.</value>
        [NotMapped]
        public SetPassword changepwd { get; set; }

    }

    public class SetPassword : BaseEntity
    {
        #region Properties
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        #endregion
    }

    public class ChangePassword
    {
        #region Properties

        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        #endregion
    }
}
