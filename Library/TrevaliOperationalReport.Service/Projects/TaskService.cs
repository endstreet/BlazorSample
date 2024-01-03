using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain;
using TrevaliOperationalReport.Domain.General;
using TrevaliOperationalReport.Domain.Projects;
using static TrevaliOperationalReport.Domain.ProjectEnum;

namespace TrevaliOperationalReport.Service.Projects
{
    public class TaskService : ITaskService
    {
        #region Fields

        private readonly IRepository<ProjectTask> _taskRepository;
        private readonly IRepository<TaskResource> _taskResourceRepository;
        private readonly IRepository<Users> _userRepository;
        private readonly IRepository<Project> _projectRepository;
        private readonly TrevaliOperationalReport.Data.TrevaliOperationalReportObjectContext _dbContext;

        #endregion

        #region Ctor

        public TaskService(IRepository<ProjectTask> taskRepository, IRepository<TaskResource> taskResourceRepository, IRepository<Users> userRepository, IRepository<Project> projectRepository)
        {
            _taskRepository = taskRepository;
            _taskResourceRepository = taskResourceRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _dbContext = new TrevaliOperationalReport.Data.TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
        }

        #endregion

        #region Method

        /// <summary>
        /// Gets the task type select list.
        /// </summary>
        /// <returns>
        /// IList&lt;SelectListItem&gt;.
        /// </returns>
        public IList<SelectListItem> GetTaskTypeSelectList()
        {
            var TaskTypeList = Enum.GetValues(typeof(ProjectEnum.TaskType)).Cast<ProjectEnum.TaskType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            return TaskTypeList;
        }

        /// <summary>
        /// Gets the feddback sedule list.
        /// </summary>
        /// <returns>
        /// IList&lt;SelectListItem&gt;.
        /// </returns>
        public IList<SelectListItem> GetFeddbackSeduleList()
        {
            var FeddbackSedule = Enum.GetValues(typeof(ProjectEnum.FeddbackSedule)).Cast<ProjectEnum.FeddbackSedule>().Select(v => new SelectListItem
            {
                Text = v.ToString().Replace('_', '-'),
                Value = ((int)v).ToString()
            }).ToList();

            return FeddbackSedule;
        }

        /// <summary>
        /// Searches the task.
        /// </summary>
        /// <param name="taskName">Name of the project.</param>
        /// <param name="projectId"></param>
        /// <param name="taskType">Type of the task.</param>
        /// <param name="status">The status.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="IsAllTask"></param>
        /// <returns>
        /// IList&lt;Project&gt;.
        /// </returns>
        public IList<ProjectTask> SearchTasks(string taskName, int projectId, int taskType, int status, int priority, int IsAllTask, bool hasRights = true)
        {
            if (projectId == 0)
            {
                if (IsAllTask == 1)
                {

                    var query = from p in _taskRepository.Table
                                join pr in _taskResourceRepository.Table on p.TaskId equals pr.TaskId
                                where ((p.ProjectId == projectId || projectId == 0)
                                && (p.TaskName.Contains(taskName) || taskName == "")
                                && (p.TaskType == taskType || taskType == 0)
                                && (p.TaskStatus == status || status == 0)
                                && (p.Priority == priority || priority == 0))
                                orderby p.TaskId descending
                                select p;
                    foreach (var item in query)
                    {
                        item.IsRoleRights = true;
                    }
                    return query.Distinct().ToList();

                }
                else
                {
                    var query = from p in _taskRepository.Table
                                join pr in _taskResourceRepository.Table on p.TaskId equals pr.TaskId
                                where ((p.ProjectId == projectId || projectId == 0)
                                && (p.TaskName.Contains(taskName) || taskName == "")
                                && (p.TaskType == taskType || taskType == 0)
                                && (p.TaskStatus == status || status == 0)
                                && (p.Priority == priority || priority == 0)
                                && pr.UserId == ProjectSession.UserID)
                                orderby p.TaskId descending
                                select p;
                    foreach (var item in query)
                    {
                        item.IsRoleRights = true;
                    }
                    return query.Distinct().ToList();
                }
            }
            else
            {
                var query = from p in _taskRepository.Table
                            where (p.ProjectId == projectId)
                            orderby p.TaskId descending
                            select p;
                return query.Distinct().ToList();
            }
        }

        /// <summary>
        /// Inserts the task.
        /// </summary>
        /// <param name="projectTask">The project task.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">task</exception>
        public int InsertTask(ProjectTask projectTask)
        {
            if (projectTask == null)
                throw new ArgumentNullException("task");

            projectTask.CreatedBy = ProjectSession.UserID;
            projectTask.CreatedDate = DateTime.Now;
            _taskRepository.Insert(projectTask);
            return projectTask.TaskId;
        }

        /// <summary>
        /// Updates the task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>
        /// System.Int32.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">task</exception>
        public int UpdateTask(ProjectTask task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            var model = GetTaskById(task.TaskId);
            if (model != null)
            {
                model.TaskName = task.TaskName;
                model.Description = task.Description;
                model.ProjectId = task.ProjectId;
                model.TaskStatus = task.TaskStatus;
                model.Priority = task.Priority;
                model.TaskType = task.TaskType;
                model.TaskStartDate = task.TaskStartDate;
                model.TaskEndDate = task.TaskEndDate;
                model.Percentage = task.Percentage;
                model.FeedbackType = task.FeedbackType;
                model.ParentTaskId = task.ParentTaskId;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
            }
            _taskRepository.Update(model);
            return task.TaskId;
        }

        /// <summary>
        /// Gets the task by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public ProjectTask GetTaskById(int id)
        {
            return _taskRepository.GetById(id);
        }

        /// <summary>
        /// Deletes the task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">task</exception>
        public bool DeleteTask(ProjectTask task)
        {
            if (task == null)
                throw new ArgumentNullException("taskType");

            try
            {
                using (TrevaliOperationalReportObjectContext dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
                {
                    object[] Taskparams = {
                            new System.Data.SqlClient.SqlParameter("TaskID", task.TaskId),
                         };

                    var Success = dbContext.ExcecuteSqlCommand("PRJ_DeleteTask", Taskparams);
                    if (Success > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the parent task.
        /// </summary>
        /// <param name="ProjectTask">The project task.</param>
        /// <returns>
        /// ProjectTask.
        /// </returns>
        public ProjectTask GetParentTask(ProjectTask ProjectTask)
        {
            var Query = (from p in _taskRepository.Table
                         where p.TaskId == ProjectTask.ParentTaskId
                         select p).FirstOrDefault();
            return Query;
        }

        /// <summary>
        /// Gets the child task.
        /// </summary>
        /// <param name="ParentTaskId">The parent task identifier.</param>
        /// <returns></returns>
        public IList<ProjectTask> GetChildTask(int ParentTaskId)
        {
            var Query = (from p in _taskRepository.Table
                         where p.ParentTaskId == ParentTaskId
                         select p);
            return Query.Distinct().ToList();
        }

        /// <summary>
        /// Shifts the task date.
        /// </summary>
        /// <param name="TaskId">The task identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">taskType</exception>
        public bool ShiftTaskDate(int TaskId)
        {
            if (TaskId == 0)
                throw new ArgumentNullException("taskType");

            try
            {
                using (TrevaliOperationalReportObjectContext dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
                {
                    object[] ShiftTaskDateparams = {
                            new System.Data.SqlClient.SqlParameter("TaskId", TaskId),
                         };

                    var Success = dbContext.ExcecuteSqlCommand("PRJ_ShiftTaskDate", ShiftTaskDateparams);
                    if (Success > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the child task list.
        /// </summary>
        /// <param name="TaskId">The task identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">taskType</exception>
        public IList<ChildtaskId> GetChildTaskList(int TaskId)
        {
            if (TaskId == 0)
                throw new ArgumentNullException("taskType");

            using (TrevaliOperationalReportObjectContext dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
            {
                object[] ShiftTaskDateparams = {
                            new System.Data.SqlClient.SqlParameter("TaskId", TaskId),
                         };

                IList<ChildtaskId> Success = dbContext.ExecuteStoredProcedureList<ChildtaskId>("PRJ_ShiftTaskDate", ShiftTaskDateparams);
                return Success;
            }
        }

        public string GetTaskResource(int TaskId)
        {
            if (TaskId == 0)
                throw new ArgumentNullException("taskType");

            var Query = (from p in _taskResourceRepository.Table
                         where p.TaskId == TaskId
                         select p.UserId);
            return string.Join(",", Query.Select(n => n.ToString()).ToArray());
        }

        //for Export
        /// <summary>
        /// Searches the export task.
        /// </summary>
        /// <param name="IsAdmin">if set to <c>true</c> [is admin].</param>
        /// <returns></returns>
        public IList<ExportModel> SearchExportTask(bool IsAllProject, bool IsViewRights)
        {
            IList<GetExportFromSP> ExportList = new List<GetExportFromSP>();
            using (TrevaliOperationalReportObjectContext dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
            {
                object[] Exportparams = {
                            new System.Data.SqlClient.SqlParameter("IsAllProject", IsAllProject),
                            new System.Data.SqlClient.SqlParameter("UserId", ProjectSession.UserID),
                            new System.Data.SqlClient.SqlParameter("IsAdmin", ProjectSession.IsAdmin)
                         };
                ExportList = dbContext.ExecuteStoredProcedureList<GetExportFromSP>("PRJ_ProjectExport", Exportparams);
            }
            int ProjectId = 0;
            int TaskId = 0;
            int UserId = 0;
            string TaskResources = "";
            List<ExportModel> exportData = new List<ExportModel>();
            foreach (GetExportFromSP model in ExportList)
            {
                ExportModel data = new ExportModel();
                if (model.ProjectId == ProjectId)
                {
                    data.ProjectName = null;
                    data.ProjectType = null;
                    data.Score = null;
                    data.AnnualizedBenefit = null;
                    data.NetPresentValue = null;
                    data.InternalRateofReturn = null;
                }
                else
                {
                    data.ProjectName = model.ProjectName;
                    data.ProjectType = model.ProjectType;
                    data.Score = ConvertTo.Integer(model.PriorityRatings);
                    data.AnnualizedBenefit = Convert.ToString(model.AnnualizedBenefit);
                    data.NetPresentValue = Convert.ToString(model.NetPresentValue);
                    data.InternalRateofReturn = Convert.ToString(model.InternalRateofReturn);
                }

                if (model.TaskId != null)
                {
                    if (model.TaskId == TaskId)
                    {
                        data.TaskName = null;
                        data.StartDate = null;
                        data.DueDate = null;
                        data.FeedbackSchedule = null;
                        data.Progress = null;
                        data.TaskStatus = null;
                    }
                    else
                    {
                        UserId = 0;
                        data.TaskName = model.TaskName;
                        data.StartDate = model.TaskStartDate.Value.ToString("MM/dd/yyyy");
                        data.DueDate = model.TaskEndDate.Value.ToString("MM/dd/yyyy");
                        data.FeedbackSchedule = Enum.GetName(typeof(FeddbackSedule), model.FeedbackSchedule);
                        data.Progress = Convert.ToString(model.Percentage);
                        data.TaskStatus = Enum.GetName(typeof(ProjectStatus), model.TaskStatus);
                        TaskResources = GetTaskResource(Convert.ToInt32(model.TaskId));
                    }
                    if (model.UserId != null)
                    {
                        int Id = ConvertTo.Integer(model.UserId);
                        if (TaskResources.Contains(Convert.ToString(model.UserId)))
                        {
                            var UserData = _userRepository.Table.Where(p => p.UserID == Id).FirstOrDefault();
                            data.TaskMembers = UserData.Name + " " + UserData.Surname;
                        }
                        else
                        {
                            int OwnerId = Convert.ToInt32((from p in _projectRepository.Table
                                                           where p.ProjectId == model.ProjectId
                                                           select p.CreatedBy).FirstOrDefault());
                            if (model.UserId == OwnerId)
                            {
                                var UserData = _userRepository.Table.Where(p => p.UserID == Id).FirstOrDefault();
                                data.TaskMembers = UserData.Name + " " + UserData.Surname + " (Project Owner)";
                            }
                            else
                            {
                                var UserData = _userRepository.Table.Where(p => p.UserID == Id).FirstOrDefault();
                                data.TaskMembers = UserData.Name + " " + UserData.Surname + " (Admin)";
                            }
                        }
                    }
                    if (model.FeedbackText != null)
                    {
                        int Id = ConvertTo.Integer(model.UserId);
                        if (UserId == Id)
                        {
                            data.TaskMembers = null;
                        }
                        else
                        {
                            if (TaskResources.Contains(Convert.ToString(model.UserId)))
                            {
                                var UserData = _userRepository.Table.Where(p => p.UserID == Id).FirstOrDefault();
                                data.TaskMembers = UserData.Name + " " + UserData.Surname;
                            }
                            else
                            {
                                int OwnerId = Convert.ToInt32((from p in _projectRepository.Table
                                                               where p.ProjectId == model.ProjectId
                                                               select p.CreatedBy).FirstOrDefault());
                                if (model.UserId == OwnerId)
                                {
                                    var UserData = _userRepository.Table.Where(p => p.UserID == Id).FirstOrDefault();
                                    data.TaskMembers = UserData.Name + " " + UserData.Surname + "(Project Owner)";
                                }
                                else
                                {
                                    var UserData = _userRepository.Table.Where(p => p.UserID == Id).FirstOrDefault();
                                    data.TaskMembers = UserData.Name + " " + UserData.Surname + "(Admin)";
                                }
                            }
                        }
                        data.FeedbackText = model.FeedbackText;
                        UserId = Id;
                    }
                    TaskId = ConvertTo.Integer(model.TaskId);
                }
                exportData.Add(data);
                ProjectId = model.ProjectId;
            }
            return exportData;
        }
        #endregion
    }
}