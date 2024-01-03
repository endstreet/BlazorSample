using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrevaliOperationalReport.Domain.General;
using static TrevaliOperationalReport.Domain.ProjectEnum;

namespace TrevaliOperationalReport.Domain.Projects
{
    [Table("PRJ_Project", Schema = "dbo")]
    public class Project : BaseEntity
    {
        /// <summary>
		/// Gets or sets the ProjectId value.
		/// </summary>
		[Key]
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectName value.
        /// </summary>
        [Required]
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the Description value.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ProjectCode value.
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// Gets or sets the ProjectTypeId value.
        /// </summary>
        [Required]
        public int ProjectTypeId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectStatus value.
        /// </summary>
        [Required]
        public int ProjectStatus { get; set; }

        [NotMapped]
        public string ProjectStatusName => Enum.GetName(typeof(ProjectStatus), ProjectStatus);

        /// <summary>
        /// Gets or sets the Priority value.
        /// </summary>

        /// <summary>
        /// Gets or sets the ProjectStartDate value.
        /// </summary>
        public DateTime? ProjectStartDate { get; set; }

        /// <summary>
        /// Gets the start date string.
        /// </summary>
        /// <value>The start date string.</value>
        [NotMapped]
        public string StartDateString => ProjectStartDate != null ? ProjectStartDate.Value.ToString("MM/dd/yyyy") : "";

        /// <summary>
        /// Gets or sets the ProjectEndDate value.
        /// </summary>
        public DateTime? ProjectEndDate { get; set; }

        /// <summary>
        /// Gets the end date string.
        /// </summary>
        /// <value>The end date string.</value>
        [NotMapped]
        public string EndDateString => ProjectEndDate != null ? ProjectEndDate.Value.ToString("MM/dd/yyyy") : "";

        /// <summary>
        /// Gets or sets the SiteId value.
        /// </summary>
        [Required]
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the type of the project.
        /// </summary>
        /// <value>The type of the project.</value>
        [ForeignKey("ProjectTypeId")]
        public virtual ProjectType ProjectType { get; set; }

        /// <summary>
        /// Gets the name of the project type.
        /// </summary>
        /// <value>The name of the project type.</value>
        [NotMapped]
        public string ProjectTypeName => ProjectType != null ? ProjectType.Name : "";

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        [ForeignKey("SiteId")]
        public virtual Site Company { get; set; }

        /// <summary>
        /// Gets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        [NotMapped]
        public string CompanyName => Company != null ? Company.SiteName : "";

        /// <summary>
        /// Gets or sets the ProjectResource.
        /// </summary>
        /// <value>The ProjectResource.</value>
        [NotMapped]
        public ProjectResource OwnerResource { get; set; }

        /// <summary>
        /// Gets or sets the ResrourceType value.
        /// </summary>
        [NotMapped]
        public int ResrourceType { get; set; }

        /// <summary>
        /// Gets or sets the OwnerId value.
        /// </summary>
        [NotMapped]
        public int OwnerId { get; set; }

        [NotMapped]
        public bool IsFullRights { get; set; }

        public double? AnnualizedBenefit { get; set; }

        public double? NetPresentValue { get; set; }

        [RegularExpression(@"^100(\.[0]{1,2})?|([0-9]|[1-9][0-9])(\.[0-9]{1,2})?$")]
        public double? InternalRateofReturn { get; set; }

        public Nullable<int> SafetyRisk { get; set; }
        public int? EaseOfImplementation { get; set; }
        public int? CapitalIntensity { get; set; }
        public int? ResourceIntensity { get; set; }
        public int? Return { get; set; }
        public int? RiskIfNotImplemented { get; set; }
        public int? PriorityRatings { get; set; }

        [NotMapped]
        public bool allProject { get; set; }

        [NotMapped]
        public bool IsRoleRights { get; set; }
    }
}
