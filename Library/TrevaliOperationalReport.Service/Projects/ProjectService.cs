using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service.Projects
{
    public class ProjectService : IProjectService
    {
        #region Fields

        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<ProjectResource> _projectResourceRepository;
        private readonly TrevaliOperationalReport.Data.TrevaliOperationalReportObjectContext _dbContext;

        #endregion

        #region Ctor

        public ProjectService(IRepository<Project> projectRepository, IRepository<ProjectResource> projectResourceRepository)
        {
            _projectRepository = projectRepository;
            _projectResourceRepository = projectResourceRepository;
            _dbContext = new TrevaliOperationalReport.Data.TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
        }

        #endregion

        #region Method
        /// <summary>
        /// Searches the projects.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="projectType"></param>
        /// <param name="company"></param>
        /// <param name="status"></param>
        /// <param name="AllProject"></param>
        /// <returns>
        /// IList&lt;Project&gt;.
        /// </returns>
        public IList<Project> SearchProjects(string projectName, int projectType, int company, int status, bool AllProject, bool hasRights = false)
        {
            if (AllProject == true)
            {
                var query = from p in _projectRepository.Table
                            join pr in _projectResourceRepository.Table on p.ProjectId equals pr.ProjectId
                            where ((p.ProjectName.Contains(projectName) || projectName == "")
                            && (p.ProjectTypeId == projectType || projectType == 0)
                            && (p.SiteId == company || company == 0)
                            && (p.ProjectStatus == status || status == 0))
                            orderby p.ProjectId descending
                            select p;
                foreach (var item in query)
                {
                    item.IsRoleRights = true;
                }
                return query.Distinct().ToList();
            }
            else
            {
                var query = from p in _projectRepository.Table
                            join pr in _projectResourceRepository.Table on p.ProjectId equals pr.ProjectId
                            where ((p.ProjectName.Contains(projectName) || projectName == "")
                            && (p.ProjectTypeId == projectType || projectType == 0)
                            && (p.SiteId == company || company == 0)
                            && (p.ProjectStatus == status || status == 0)
                            && pr.ResourceType == 1
                            && pr.UserId == ProjectSession.UserID)
                            orderby p.ProjectId descending
                            select p;
                foreach (var item in query)
                {
                    item.IsRoleRights = true;
                }
                return query.Distinct().ToList();
            }
        }
        public IList<Project> ProjectList()
        {
            if (ProjectSession.IsAdmin == true)
            {
                var query = from p in _projectRepository.Table
                            join pr in _projectResourceRepository.Table on p.ProjectId equals pr.ProjectId
                            orderby p.ProjectId descending
                            select p;
                foreach (var item in query)
                {
                    item.IsRoleRights = true;
                }
                return query.Distinct().ToList();
            }
            else
            {
                var query = from p in _projectRepository.Table
                            join pr in _projectResourceRepository.Table on p.ProjectId equals pr.ProjectId
                            where (pr.UserId == ProjectSession.UserID)
                            orderby p.ProjectId descending
                            select p;
                foreach (var item in query)
                {
                    item.IsRoleRights = true;
                }
                return query.Distinct().ToList();
            }
        }

        /// <summary>
        /// Inserts the project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">project</exception>
        public int InsertProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            project.CreatedBy = ProjectSession.UserID;
            project.CreatedDate = DateTime.Now;
            _projectRepository.Insert(project);
            return project.ProjectId;
        }

        /// <summary>
        /// Updates the project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">project</exception>
        public int UpdateProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            var model = GetProjectById(project.ProjectId);
            if (model != null)
            {
                model.ProjectName = project.ProjectName;
                model.ProjectCode = project.ProjectCode;
                model.Description = project.Description;
                model.ProjectStatus = project.ProjectStatus;
                model.SiteId = project.SiteId;
                model.ProjectTypeId = project.ProjectTypeId;
                model.ProjectStartDate = project.ProjectStartDate;
                model.ProjectEndDate = project.ProjectEndDate;
                model.InternalRateofReturn = project.InternalRateofReturn;
                model.AnnualizedBenefit = project.AnnualizedBenefit;
                model.NetPresentValue = project.NetPresentValue;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
                model.SafetyRisk = project.SafetyRisk;
                model.EaseOfImplementation = project.EaseOfImplementation;
                model.CapitalIntensity = project.CapitalIntensity;
                model.ResourceIntensity = project.ResourceIntensity;
                model.Return = project.Return;
                model.RiskIfNotImplemented = project.RiskIfNotImplemented;
                model.PriorityRatings = project.PriorityRatings;
            }
            _projectRepository.Update(model);
            return project.ProjectId;
        }

        /// <summary>
        /// Gets the project by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Project.
        /// </returns>
        public Project GetProjectById(int id)
        {
            return _projectRepository.GetById(id);
        }

        /// <summary>
        /// Deletes the project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">project</exception>
        public bool DeleteProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            try
            {
                using (TrevaliOperationalReportObjectContext dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
                {
                    object[] Projectparams = {
                            new System.Data.SqlClient.SqlParameter("ProjectID", project.ProjectId),
                         };

                    var Success = dbContext.ExcecuteSqlCommand("PRJ_DeleteProject", Projectparams);
                    if (Success > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the project status select list.
        /// </summary>
        /// <returns>
        /// IList&lt;SelectListItem&gt;.
        /// </returns>
        public IList<SelectListItem> GetProjectStatusSelectList()
        {
            var StatusList = Enum.GetValues(typeof(ProjectEnum.ProjectStatus)).Cast<ProjectEnum.ProjectStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            return StatusList;
        }

        /// <summary>
        /// Gets the project status select list.
        /// </summary>
        /// <returns>
        /// IList&lt;SelectListItem&gt;.
        /// </returns>
        public IList<SelectListItem> GetPrioritySelectList()
        {
            var PriorityList = Enum.GetValues(typeof(ProjectEnum.Priority)).Cast<ProjectEnum.Priority>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            return PriorityList;
        }

        /// <summary>
        /// Gets the projectt list.
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetProjecttList()
        {
            var project = from p in _projectRepository.Table
                          select p;
            var ProjecttList = project.Select(v => new SelectListItem
            {
                Text = v.ProjectName.ToString() + " (" + v.ProjectCode + ")",
                Value = v.ProjectId.ToString()
            }).ToList();

            return ProjecttList;
        }

        /// <summary>
        /// Copies the project.
        /// </summary>
        /// <param name="ProjectID">The project identifier.</param>
        /// <param name="UserID">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">taskType</exception>
        public bool CopyProject(int ProjectID, int UserID)
        {
            if (ProjectID == 0 || UserID == 0)
                throw new ArgumentNullException("taskType");

            try
            {
                using (TrevaliOperationalReportObjectContext dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
                {
                    object[] Projectparams = {
                            new System.Data.SqlClient.SqlParameter("ProjectID", ProjectID),
                            new System.Data.SqlClient.SqlParameter("UserID", UserID)
                         };

                    var result = dbContext.ExcecuteSqlCommand("PRJ_CopyProject", Projectparams);
                    if (result > 0)
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
        /// Gets the prior list.
        /// </summary>
        /// <param name="TotalValue">The total value.</param>
        /// <returns></returns>
        public IList<SelectListItem> GetPriorList(int TotalValue)
        {
            List<SelectListItem> DdList = new List<SelectListItem>();
            for (int i = 0; i <= TotalValue; i++)
            {
                DdList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return DdList;
        }
        #endregion
    }
}