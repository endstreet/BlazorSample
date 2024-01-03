using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service.Projects
{
    public interface ITaskService
    {
        /// <summary>
        /// Gets the task type select list.
        /// </summary>
        /// <returns>
        /// IList&lt;SelectListItem&gt;.
        /// </returns>
        IList<SelectListItem> GetTaskTypeSelectList();

        /// <summary>
        /// Gets the feddback sedule list.
        /// </summary>
        /// <returns>
        /// IList&lt;SelectListItem&gt;.
        /// </returns>
        IList<SelectListItem> GetFeddbackSeduleList();

        /// <summary>
        /// Searches the task.
        /// </summary>
        /// <param name="taskName">Name of the project.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="taskType">Type of the task.</param>
        /// <param name="status">The status.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="IsAllTask">The is all task.</param>
        /// <returns>
        /// IList&lt;Project&gt;.
        /// </returns>
        IList<ProjectTask> SearchTasks(string taskName, int projectId, int taskType, int status, int priority, int IsAllTask, bool hasRights = true);

        /// <summary>
        /// Inserts the task.
        /// </summary>
        /// <param name="task">The project.</param>
        /// <returns></returns>
        int InsertTask(ProjectTask task);

        /// <summary>
        /// Updates the task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        int UpdateTask(ProjectTask task);

        /// <summary>
        /// Gets the task by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// task.
        /// </returns>
        ProjectTask GetTaskById(int id);

        /// <summary>
        /// Deletes the task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>
        ///   <c>true</c> if XXXX,  <c>false</c> otherwise.
        /// </returns>
        bool DeleteTask(ProjectTask task);

        /// <summary>
        /// Gets the parent task.
        /// </summary>
        /// <param name="ProjectTask">The project task.</param>
        /// <returns>
        /// ProjectTask.
        /// </returns>
        ProjectTask GetParentTask(ProjectTask ProjectTask);

        /// <summary>
        /// Gets the child task.
        /// </summary>
        /// <param name="ParentTaskId">The parent task identifier.</param>
        /// <returns></returns>
        IList<ProjectTask> GetChildTask(int ParentTaskId);

        /// <summary>
        /// Shifts the task date.
        /// </summary>
        /// <param name="TaskId">The task identifier.</param>
        /// <returns></returns>
        bool ShiftTaskDate(int TaskId);

        /// <summary>
        /// Gets the child task list.
        /// </summary>
        /// <param name="TaskId">The task identifier.</param>
        /// <returns></returns>
        IList<ChildtaskId> GetChildTaskList(int TaskId);

        /// <summary>
        /// Searches the export task.
        /// </summary>
        /// <param name="IsAdmin">if set to <c>true</c> [is admin].</param>
        /// <returns></returns>
        IList<ExportModel> SearchExportTask(bool IsAdmin, bool IsViewRights);
    }
}