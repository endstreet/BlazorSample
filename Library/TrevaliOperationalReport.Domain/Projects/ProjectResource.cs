using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Domain.Projects
{
    [Table("PRJ_ProjectResource", Schema = "dbo")]
    public class ProjectResource
    {
        /// <summary>
		/// Gets or sets the ProjectResourceId value.
		/// </summary>
		[Key]
        public int ProjectResourceId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId value.
        /// </summary>
        [Required]
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the UserId value.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the ResourceType value.
        /// </summary>
        [Required]
        public int ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the IsActive value.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the Users.
        /// </summary>
        /// <value>The Users.</value>
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }

        /// <summary>
        /// Gets the name of the User.
        /// </summary>
        /// <value>The name of the User.</value>
        [NotMapped]
        public string Name => Users != null ? Users.Name + " " + Users.Surname : "";

        /// <summary>
        /// Gets the Email of the User.
        /// </summary>
        /// <value>The Email of the User.</value>
        [NotMapped]
        public string Email => Users != null ? Users.EmailID : "";

        /// <summary>
        /// Gets the Designation of the User.
        /// </summary>
        /// <value>The Designation of the User.</value>
        [NotMapped]
        public string Designation => Users != null ? Users.Designation : "";
    }
}
