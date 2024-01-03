using Kendo.Mvc.UI;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TrevaliOperationalReport.Domain.ProjectEnum;

namespace TrevaliOperationalReport.Domain.Projects
{
    [Table("PRJ_Task", Schema = "dbo")]
    public class ProjectTask : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the TaskId value.
        /// </summary>
        [Key]
        public int TaskId { get; set; }

        /// <summary>
        /// Gets or sets the ParentTaskId value.
        /// </summary>
        public int? ParentTaskId { get; set; }

        /// <summary>
        /// Gets or sets the TaskName value.
        /// </summary>
        [Required]
        public string TaskName { get; set; }

        /// <summary>
        /// Gets or sets the Description value.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId value.
        /// </summary>
        [Required]
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the TaskStatus value.
        /// </summary>
        [Required]
        public int TaskStatus { get; set; }

        [NotMapped]
        public string TaskStatusName => Enum.GetName(typeof(ProjectStatus), TaskStatus);
        /// <summary>
        /// Gets or sets the TaskType value.
        /// </summary>
        [Required]
        public int TaskType { get; set; }

        [NotMapped]
        public string TaskTypeName => Enum.GetName(typeof(TaskType), TaskType);
        /// <summary>
        /// Gets or sets the Priority value.
        /// </summary>
        [Required]
        public int Priority { get; set; }

        [NotMapped]
        public string PriorityName => Enum.GetName(typeof(Priority), Priority);

        [RegularExpression(@"^(100|[1-9]?[0-9])$")]
        public int Percentage { get; set; }

        /// <summary>
        /// Gets or sets the TaskStartDate value.
        /// </summary>
        [Required]
        public DateTime? TaskStartDate { get; set; }

        [NotMapped]
        public string StartDateString => TaskStartDate != null ? TaskStartDate.Value.ToString("MM/dd/yyyy") : "";

        /// <summary>
        /// Gets or sets the TaskEndDate value.
        /// </summary>
        [Required]
        public DateTime? TaskEndDate { get; set; }

        [NotMapped]
        public string EndDateString => TaskEndDate != null ? TaskEndDate.Value.ToString("MM/dd/yyyy") : "";

        /// <summary>
        /// Gets or sets the FeedbackSchedule value.
        /// </summary>
        [Required]
        public int FeedbackSchedule { get; set; }

        [NotMapped]
        public string FeedbackScheduleName => Enum.GetName(typeof(FeddbackSedule), FeedbackSchedule);

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        /// <summary>
        /// Gets the name of the ProjectName.
        /// </summary>
        /// <value>The name of the ProjectName.</value>
        [NotMapped]
        public string ProjectName => Project != null ? Project.ProjectName : "";

        [NotMapped]
        public string ProjectNameDisplay { get; set; }

        [NotMapped]
        public string ProjectTypeName => Project != null ? Project.ProjectTypeName : "";

        [NotMapped]
        public string ProjectCode => Project != null ? Project.ProjectCode : "";

        [NotMapped]
        public int ProjectManagerId => Project != null ? Convert.ToInt32(Project.CreatedBy) : 0;

        /// <summary>
        /// Gets or sets the OwnerId value.
        /// </summary>
        [NotMapped]
        public int OwnerId { get; set; }

        [NotMapped]
        public string AssignedUserIds { get; set; }

        [NotMapped]
        public string AssignedUserName { get; set; }

        /// <summary>
        /// Gets or sets the ProjectStartDate value.
        /// </summary>
        [NotMapped]
        public DateTime? ProjectStartDate { get; set; }

        /// <summary>
        /// Gets or sets the ProjectEndDate value.
        /// </summary>
        [NotMapped]
        public DateTime? ProjectEndDate { get; set; }

        /// <summary>
        /// Gets or sets the ParentTaskStartDate value.
        /// </summary>
        [NotMapped]
        public DateTime? ParentTaskStartDate { get; set; }

        /// <summary>
        /// Gets or sets the ParentTaskEndDate value.
        /// </summary>
        [NotMapped]
        public DateTime? ParentTaskEndDate { get; set; }

        [NotMapped]
        public int[] SponserIds { get; set; }

        [NotMapped]
        public int Days { get; set; }

        public string FeedbackType { get; set; }

        [NotMapped]
        public int IsProject { get; set; }

        [NotMapped]
        public int allTask { get; set; }

        [NotMapped]
        public string TaskStatusList { get; set; }

        [NotMapped]
        public bool IsFullRights { get; set; }

        [NotMapped]
        public bool IsRoleRights { get; set; }

        #endregion
    }

    public class TaskGanttKendo : IGanttTask
    {
        #region Properties
        public int ProjectId { get; set; }
        public bool Expanded { get; set; }
        public int OrderId { get; set; }
        public decimal PercentComplete { get; set; }
        public bool Summary { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Days { get; set; }
        [NotMapped]
        public int OwnerId { get; set; }
        [NotMapped]
        public int? flag { get; set; }
        [NotMapped]
        public bool IsFullRights { get; set; }
        [NotMapped]
        public bool IsRoleRights { get; set; }
        #endregion
    }

    public class TaskDependencyGanttKendo : IGanttDependency
    {
        public int ID { get; set; }
        public DependencyType Type { get; set; }
        public int DependencyID { get; set; }
        public int PredecessorID { get; set; }
        public int SuccessorID { get; set; }
        [NotMapped]
        public int OwnerId { get; set; }
    }

    public class ChildtaskId
    {
        public int TaskId { get; set; }
    }
}
