using System.Collections.Generic;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service.Projects
{
    public interface ITaskFeedbackService
    {
        /// <summary>
        /// Inserts the task feedback.
        /// </summary>
        /// <param name="taskFeedback">The task feedback.</param>
        void InsertTaskFeedback(TaskFeedback taskFeedback);

        /// <summary>
        /// Searches the task feedback.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        IList<FeedbackRead> SearchTaskFeedback(int taskId);

        /// <summary>
        /// Deletes the feedback.
        /// </summary>
        /// <param name="TaskFeedbackId">The task feedback identifier.</param>
        /// <returns></returns>
        bool DeleteFeedback(int TaskFeedbackId);

        /// <summary>
        /// Gets the feedback by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        TaskFeedback GetFeedbackById(int Id);
    }
}