using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Domain.Projects
{
    [Table("PRJ_TaskResource", Schema = "dbo")]
    public class TaskResource
    {
        /// <summary>
        /// Gets or sets the TaskResourceId value.
        /// </summary>
        [Key]
        public int TaskResourceId { get; set; }

        /// <summary>
        /// Gets or sets the TaskId value.
        /// </summary>
        [Required]
        public int TaskId { get; set; }

        /// <summary>
        /// Gets or sets the UserId value.
        /// </summary>
        [Required]
        public int UserId { get; set; }

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

        [ForeignKey("TaskId")]
        public virtual ProjectTask ProjectTask { get; set; }

        /// <summary>
        /// Gets the name of the User.
        /// </summary>
        /// <value>The name of the User.</value>
        [NotMapped]
        public string TaskName => ProjectTask != null ? ProjectTask.TaskName : "";
    }
}
