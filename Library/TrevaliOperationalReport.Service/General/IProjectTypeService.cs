using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface IProjectTypeService
    {
        /// <summary>
        /// Searches the project types.
        /// </summary>
        /// <param name="projectTypeName">Name of the project type.</param>
        /// <returns>
        /// IList&lt;ProjectType&gt;.
        /// </returns>
        IList<ProjectType> SearchProjectTypes(string projectTypeName);

        /// <summary>
        /// Inserts the type of the project.
        /// </summary>
        /// <param name="projectType">Type of the project.</param>
        /// <returns>
        /// System.Int32.
        /// </returns>
        int InsertProjectType(ProjectType projectType);

        /// <summary>
        /// Updates the type of the project.
        /// </summary>
        /// <param name="projectType">Type of the project.</param>
        /// <returns>
        /// System.Int32.
        /// </returns>
        int UpdateProjectType(ProjectType projectType);

        /// <summary>
        /// Gets the project type by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// ProjectType.
        /// </returns>
        ProjectType GetProjectTypeById(int id);

        /// <summary>
        /// Deletes the type of the project.
        /// </summary>
        /// <param name="projectType">Type of the project.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        bool DeleteProjectType(ProjectType projectType);

        /// <summary>
        /// Gets the project type select list.
        /// </summary>
        /// <returns>
        /// IList&lt;SelectListItem&gt;.
        /// </returns>
        IList<SelectListItem> GetProjectTypeSelectList();
    }
}