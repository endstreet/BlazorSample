using System;
using System.Collections.Generic;
using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service.Projects
{
    public class ProjectResourceService : IProjectResourceService
    {
        #region Fields

        private readonly IRepository<ProjectResource> _projectResourceRepository;
        private readonly IRepository<Users> _userRepository;

        #endregion

        #region Ctor

        public ProjectResourceService(IRepository<ProjectResource> projectResourceRepository, IRepository<Users> userRepository)
        {
            _projectResourceRepository = projectResourceRepository;
            _userRepository = userRepository;
        }

        #endregion

        #region Method

        /// <summary>
        /// Searches the project resources.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="resourceId">The resource identifier.</param>
        /// <returns>
        /// IList&lt;ProjectResource&gt;.
        /// </returns>
        public IList<ProjectResource> SearchProjectResources(int projectId, int resourceId)
        {
            if (resourceId > 0)
            {
                var query = from p in _projectResourceRepository.Table
                            where (p.ProjectId == projectId && p.ResourceType == resourceId)
                            orderby p.ProjectResourceId descending
                            select p;
                return query.ToList();
            }
            else
            {
                var query = (from p in _projectResourceRepository.Table
                             where (p.ProjectId == projectId)
                             orderby p.ProjectResourceId descending
                             select p);
                return query.ToList();
            }
        }

        /// <summary>
        /// Inserts the project resource.
        /// </summary>
        /// <param name="projectresource">The projectresource.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">projectresource</exception>
        public int InsertProjectResource(ProjectResource projectresource)
        {
            if (projectresource == null)
                throw new ArgumentNullException("projectresource");

            _projectResourceRepository.Insert(projectresource);
            return projectresource.ProjectResourceId;
        }

        /// <summary>
        /// Deletes the project resource.
        /// </summary>
        /// <param name="projectresource">The projectresource.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">projectresource</exception>
        public bool DeleteProjectResource(ProjectResource projectresource)
        {
            if (projectresource == null)
                throw new ArgumentNullException("projectresource");

            try
            {
                using (TrevaliOperationalReportObjectContext context = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
                {
                    var data = context.ProjectResource.Where(p => p.ProjectId == projectresource.ProjectId).ToList();
                    foreach (ProjectResource obj in data)
                    {
                        context.ProjectResource.Remove(obj);
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
        /// Deletes the resource.
        /// </summary>
        /// <param name="projectresource">The projectresource.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">projectresource</exception>
        public bool DeleteResource(ProjectResource projectresource)
        {
            if (projectresource == null)
                throw new ArgumentNullException("projectresource");

            try
            {
                _projectResourceRepository.Delete(projectresource);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Actives the deactive resource.
        /// </summary>
        /// <param name="modelResource">The model resource.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">modelResource</exception>
        public bool ActiveDeactiveResource(ProjectResource modelResource)
        {
            if (modelResource == null)
                throw new ArgumentNullException("modelResource");

            var model = GetResourceById(modelResource.ProjectResourceId);
            if (model != null)
            {
                model.IsActive = modelResource.IsActive;
                _projectResourceRepository.Update(model);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the resource by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// ProjectResource.
        /// </returns>
        public ProjectResource GetResourceById(int id)
        {
            var query = (from p in _projectResourceRepository.Table
                         join u in _userRepository.Table on p.UserId equals u.UserID
                         where (p.ProjectResourceId == id)
                         select p).FirstOrDefault();
            return query;
        }

        /// <summary>
        /// Gets the resource by user identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public List<ProjectResource> GetResourceByUserId(int id)
        {
            var query = (from p in _projectResourceRepository.Table
                         where (p.UserId == id)
                         select p);
            return query.ToList();
        }

        /// <summary>
        /// Updates the resource.
        /// </summary>
        /// <param name="ProjectResource">The project resource.</param>
        /// <exception cref="System.ArgumentNullException">task</exception>
        public void UpdateResource(ProjectResource ProjectResource)
        {
            if (ProjectResource == null)
                throw new ArgumentNullException("task");

            var model = GetResourceById(ProjectResource.ProjectResourceId);
            if (model != null)
            {
                model.ProjectId = ProjectResource.ProjectId;
                model.UserId = ProjectResource.UserId;
                model.ResourceType = ProjectResource.ResourceType;
                model.IsActive = ProjectResource.IsActive;

            }
            _projectResourceRepository.Update(model);
        }
        #endregion
    }
}