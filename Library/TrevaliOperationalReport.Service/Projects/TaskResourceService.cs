using System;
using System.Collections.Generic;
using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service.Projects
{
    public class TaskResourceService : ITaskResourceService
    {

        #region Fields

        private readonly IRepository<TaskResource> _taskResourceRepository;

        #endregion

        #region Ctor

        public TaskResourceService(IRepository<TaskResource> taskResourceRepository)
        {
            _taskResourceRepository = taskResourceRepository;
        }

        #endregion

        #region Method

        /// <summary>
        /// Searches the task resource.
        /// </summary>
        /// <param name="TaskId">The task identifier.</param>
        /// <returns>
        /// IList&lt;TaskResource&gt;.
        /// </returns>
        public IList<TaskResource> searchTaskResource(int TaskId)
        {
            var query = from p in _taskResourceRepository.Table
                        where (p.TaskId == TaskId || TaskId == 0)
                        orderby p.TaskId descending
                        select p;
            return query.Distinct().ToList();
        }

        /// <summary>
        /// Deletes the resource.
        /// </summary>
        /// <param name="taskresource">The taskresource.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">taskresource</exception>
        public bool DeleteResource(TaskResource taskresource)
        {
            if (taskresource == null)
                throw new ArgumentNullException("taskresource");

            try
            {
                using (TrevaliOperationalReportObjectContext context = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
                {
                    var data = context.TaskResource.Where(p => p.TaskId == taskresource.TaskId && p.UserId == taskresource.UserId).ToList();
                    foreach (TaskResource obj in data)
                    {
                        context.TaskResource.Remove(obj);
                    }
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Inserts the project resource.
        /// </summary>
        /// <param name="taskresource">The projectresource.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">projectresource</exception>
        public int InsertTaskResource(TaskResource taskresource)
        {
            if (taskresource == null)
                throw new ArgumentNullException("projectresource");

            if (checkTaskResource(taskresource))
            {
                return -1;
            }
            _taskResourceRepository.Insert(taskresource);

            return taskresource.TaskResourceId;
        }

        /// <summary>
        /// Gets the resource by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public TaskResource GetResourceById(int id)
        {
            return _taskResourceRepository.GetById(id);
        }

        /// <summary>
        /// Checks the task resource.
        /// </summary>
        /// <param name="taskresource">The taskresource.</param>
        /// <returns></returns>
        public bool checkTaskResource(TaskResource taskresource)
        {
            var exist = false;
            var model = _taskResourceRepository.Table.Where(p => p.TaskId == taskresource.TaskId && p.UserId == taskresource.UserId).FirstOrDefault();
            if (model != null)
            {
                exist = true;
            }
            return exist;
        }
        #endregion
    }
}