using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_Distribution", Schema = "dbo")]
    public class Distribution : BaseEntity
    {
        /// <summary>
        /// Gets or sets the distribution identifier.
        /// </summary>
        /// <value>
        /// The distribution identifier.
        /// </value>
        [Key]
        public int DistributionID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "*")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email identifier.
        /// </summary>
        /// <value>
        /// The email identifier.
        /// </value>
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$")]
        [StringLength(400, ErrorMessage = "*")]
        public string EmailID { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "*")]
        public string Surname { get; set; }
    }
}
