using System.Collections.Generic;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service.Projects
{
    public interface IProjectResourceService
    {
        /// <summary>
        /// Searches the project resources.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="resourceId">The resource identifier.</param>
        /// <returns>
        /// IList&lt;ProjectResource&gt;.
        /// </returns>
        IList<ProjectResource> SearchProjectResources(int projectId, int resourceId);

        /// <summary>
        /// Inserts the project resource.
        /// </summary>
        /// <param name="projectresource">The projectresource.</param>
        /// <returns></returns>
        int InsertProjectResource(ProjectResource projectresource);

        /// <summary>
        /// Deletes the project resource.
        /// </summary>
        /// <param name="projectresource">The projectresource.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        bool DeleteProjectResource(ProjectResource projectresource);

        /// <summary>
        /// Deletes the resource.
        /// </summary>
        /// <param name="projectresource">The projectresource.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        bool DeleteResource(ProjectResource projectresource);

        /// <summary>
        /// Actives the deactive resource.
        /// </summary>
        /// <param name="modelResource">The model resource.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        bool ActiveDeactiveResource(ProjectResource modelResource);

        /// <summary>
        /// Gets the resource by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        ProjectResource GetResourceById(int id);

        /// <summary>
        /// Gets the resource by user identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        List<ProjectResource> GetResourceByUserId(int id);

        /// <summary>
        /// Updates the resource.
        /// </summary>
        /// <param name="ProjectResource">The project resource.</param>
        void UpdateResource(ProjectResource ProjectResource);
    }
}