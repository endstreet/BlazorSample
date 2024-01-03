using System;

namespace TrevaliOperationalReport.Domain.Projects
{
    public class ExportModel
    {
        public string ProjectName { get; set; }
        public string ProjectType { get; set; }
        public int? Score { get; set; }
        public string AnnualizedBenefit { get; set; }
        public string NetPresentValue { get; set; }
        public string InternalRateofReturn { get; set; }
        public string TaskName { get; set; }
        public string TaskMembers { get; set; }
        public string StartDate { get; set; }
        public string DueDate { get; set; }
        public string TaskStatus { get; set; }
        public string FeedbackSchedule { get; set; }
        public string Progress { get; set; }
        public string FeedbackText { get; set; }
    }

    public class GetExportFromSP
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectType { get; set; }
        public int? PriorityRatings { get; set; }
        public double? AnnualizedBenefit { get; set; }
        public double? NetPresentValue { get; set; }
        public double? InternalRateofReturn { get; set; }
        public int? TaskId { get; set; }
        public string TaskName { get; set; }
        public int? UserId { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public int? TaskStatus { get; set; }
        public int? FeedbackSchedule { get; set; }
        public int? Percentage { get; set; }
        public string FeedbackText { get; set; }
    }
}