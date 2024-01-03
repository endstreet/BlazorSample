using System;
using System.Collections.Generic;
using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service.Projects
{
    public class TaskFeedbackService : ITaskFeedbackService
    {
        #region Fields

        private readonly IRepository<TaskFeedback> _taskFeedbackRepository;

        #endregion

        #region Ctor

        public TaskFeedbackService(IRepository<TaskFeedback> taskFeedbackRepository)
        {
            _taskFeedbackRepository = taskFeedbackRepository;
        }

        #endregion

        #region Method
        /// <summary>
        /// Inserts the task feedback.
        /// </summary>
        /// <param name="TaskFeedback">The task feedback.</param>
        /// <exception cref="System.ArgumentNullException">TaskFeedback</exception>
        public void InsertTaskFeedback(TaskFeedback TaskFeedback)
        {
            if (TaskFeedback == null)
                throw new ArgumentNullException("TaskFeedback");

            TaskFeedback.CreatedBy = ProjectSession.UserID;
            TaskFeedback.CreatedDate = DateTime.Now;
            _taskFeedbackRepository.Insert(TaskFeedback);
        }

        /// <summary>
        /// Searches the task feedback.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        public IList<FeedbackRead> SearchTaskFeedback(int taskId)
        {
            var query = from p in _taskFeedbackRepository.Table
                        where (p.TaskId == taskId)
                        orderby p.TaskFeedbackId descending
                        select new FeedbackRead { TaskFeedbackId = p.TaskFeedbackId, TaskId = p.TaskId, DocumentName = p.DocumentName, FeedbackText = p.FeedbackText, FeedbackPercentage = p.FeedbackPercentage, CreatedDate = p.CreatedDate };
            return query.ToList();
        }

        /// <summary>
        /// Deletes the feedback.
        /// </summary>
        /// <param name="TaskFeedbackId">The task feedback identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">feedback</exception>
        public bool DeleteFeedback(int TaskFeedbackId)
        {
            if (TaskFeedbackId == 0)
                throw new ArgumentNullException("feedback");
            try
            {
                TaskFeedback delete = _taskFeedbackRepository.GetById(TaskFeedbackId);
                _taskFeedbackRepository.Delete(delete);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the feedback by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">feedback</exception>
        public TaskFeedback GetFeedbackById(int Id)
        {
            if (Id == 0)
                throw new ArgumentNullException("feedback");
            else
                return _taskFeedbackRepository.GetById(Id);
        }
        #endregion
    }
}