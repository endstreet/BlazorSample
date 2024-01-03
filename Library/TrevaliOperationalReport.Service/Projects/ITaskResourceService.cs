using System.Collections.Generic;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service.Projects
{
    public interface ITaskResourceService
    {
        /// <summary>
        /// Searches the task resource.
        /// </summary>
        /// <param name="TaskId">The task identifier.</param>
        /// <returns>
        /// IList&lt;TaskResource&gt;.
        /// </returns>
        IList<TaskResource> searchTaskResource(int TaskId);

        /// <summary>
        /// Deletes the resource.
        /// </summary>
        /// <param name="taskresource">The taskresource.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        bool DeleteResource(TaskResource taskresource);

        /// <summary>
        /// Inserts the task resource.
        /// </summary>
        /// <param name="taskresource">The projectresource.</param>
        /// <returns></returns>
        int InsertTaskResource(TaskResource taskresource);

        /// <summary>
        /// Gets the resource by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TaskResource GetResourceById(int id);
    }
}