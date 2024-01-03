namespace TrevaliOperationalReport.Domain
{
    public class ProjectEnum
    {
        public enum Priority
        {
            High = 1,
            Medium = 2,
            Low = 3
        }

        public enum ProjectStatus
        {
            New = 1,
            InProgress = 2,
            OnHold = 3,
            InActive = 4,
            Closed = 5
        }

        public enum ProjectResource
        {
            Owner = 1,
            Sponsor = 2,
            TeamMember = 3
        }

        public enum TaskType
        {
            Task = 1,
            Defect = 2,
            Enhancement = 3,
            Requirement = 4
        }

        public enum FeddbackSedule
        {
            Weekly = 1,
            Bi_Weekly = 2,
            Monthly = 3
        }

        public enum FeedbackType
        {
            Feedback_Percentage = 1,
            Feedback_Document = 2,
            Feedback_Text = 3
        }
    }
}
