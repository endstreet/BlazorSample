using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.Projects
{
    [Table("PRJ_TaskFeedback", Schema = "dbo")]
    public class TaskFeedback
    {
        #region Properties

        /// <summary>
        /// Gets or sets the TaskFeedbackId value.
        /// </summary>
        [Key]
        public int TaskFeedbackId { get; set; }

        /// <summary>
        /// Gets or sets the TaskId value.
        /// </summary>
        [Required]
        public int TaskId { get; set; }

        public int? FeedbackPercentage { get; set; }

        /// <summary>
        /// Gets or sets the FeedbackText value.
        /// </summary>
        public string FeedbackText { get; set; }

        /// <summary>
        /// Gets or sets the FeedbackDocument value.
        /// </summary>
        public byte[] FeedbackDocument { get; set; }

        public string DocumentName { get; set; }
        /// <summary>
        /// Gets or sets the CreatedDate value.
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }

        [NotMapped]
        public string CreatedDateString => CreatedDate != null ? CreatedDate.ToString("MM/dd/yyyy") : "";
        /// <summary>
        /// Gets or sets the CreatedBy value.
        /// </summary>
        [Required]
        public int CreatedBy { get; set; }

        [NotMapped]
        public int OwnerId { get; set; }

        [NotMapped]
        public int ProjectId { get; set; }

        [NotMapped]
        public bool IsFullRights { get; set; }

        [NotMapped]
        public bool IsCompanyRights { get; set; }

        [NotMapped]
        public List<int> TaskResource { get; set; }
        #endregion
    }
    public class FeedbackRead
    {
        public int TaskFeedbackId { get; set; }
        public int TaskId { get; set; }
        public int? FeedbackPercentage { get; set; }
        public string FeedbackText { get; set; }
        public string DocumentName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString => CreatedDate != null ? CreatedDate.ToString("MM/dd/yyyy") : "";
    }
}
