using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public class ProjectTypeService : IProjectTypeService
    {
        #region Fields

        private readonly IRepository<ProjectType> _projectTypeRepository;

        #endregion

        #region Ctor

        public ProjectTypeService(IRepository<ProjectType> projectTypeRepository)
        {
            _projectTypeRepository = projectTypeRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches the project types.
        /// </summary>
        /// <param name="projectTypeName">Name of the project type.</param>
        /// <returns>
        /// IList&lt;ProjectType&gt;.
        /// </returns>
        public IList<ProjectType> SearchProjectTypes(string projectTypeName)
        {
            var query = from p in _projectTypeRepository.Table
                        where ((p.Name.Contains(projectTypeName) || projectTypeName == ""))
                        orderby p.ProjectTypeId descending
                        select p;
            return query.ToList();
        }

        /// <summary>
        /// Inserts the type of the project.
        /// </summary>
        /// <param name="projectType">Type of the project.</param>
        /// <returns>
        /// System.Int32.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">projectType</exception>
        public int InsertProjectType(ProjectType projectType)
        {
            if (projectType == null)
                throw new ArgumentNullException("projectType");

            if (checkExistingRecord(projectType))
            {
                return -1;
            }
            projectType.CreatedBy = ProjectSession.UserID;
            projectType.CreatedDate = DateTime.Now;
            _projectTypeRepository.Insert(projectType);
            return projectType.ProjectTypeId;
        }

        /// <summary>
        /// Updates the type of the project.
        /// </summary>
        /// <param name="projectType">Type of the project.</param>
        /// <returns>
        /// System.Int32.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">projectType</exception>
        public int UpdateProjectType(ProjectType projectType)
        {
            if (projectType == null)
                throw new ArgumentNullException("projectType");

            if (checkExistingRecord(projectType))
            {
                return -1;
            }
            var model = GetProjectTypeById(projectType.ProjectTypeId);
            if (model != null)
            {
                model.Name = projectType.Name;
                model.IsActive = projectType.IsActive;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
            }
            _projectTypeRepository.Update(model);
            return projectType.ProjectTypeId;
        }

        /// <summary>
        /// Gets the project type by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// ProjectType.
        /// </returns>
        public ProjectType GetProjectTypeById(int id)
        {
            return _projectTypeRepository.GetById(id);
        }

        /// <summary>
        /// Deletes the type of the project.
        /// </summary>
        /// <param name="projectType">Type of the project.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">projectType</exception>
        public bool DeleteProjectType(ProjectType projectType)
        {
            if (projectType == null)
                throw new ArgumentNullException("projectType");

            try
            {
                _projectTypeRepository.Delete(projectType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the project type select list.
        /// </summary>
        /// <returns>
        /// IList&lt;SelectListItem&gt;.
        /// </returns>
        public IList<SelectListItem> GetProjectTypeSelectList()
        {
            var query = from p in _projectTypeRepository.Table
                        where p.IsActive
                        orderby p.Name ascending
                        select new SelectListItem
                        {
                            Text = p.Name,
                            Value = p.ProjectTypeId.ToString()
                        };
            return query.ToList();
        }

        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        private bool checkExistingRecord(ProjectType model)
        {
            var query = from p in _projectTypeRepository.Table
                        where p.ProjectTypeId != model.ProjectTypeId &&
                        ((p.Name).Equals(model.Name))
                        select p;
            if (query.ToList().Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}