using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service.Projects
{
    public interface IProjectService
    {
        /// <summary>
        /// Searches the projects.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="projectType">Type of the project.</param>
        /// <param name="company">The company.</param>
        /// <param name="status">The status.</param>
        /// <param name="AllProject">if set to <c>true</c> [all project].</param>
        /// <returns>
        /// IList&lt;Project&gt;.
        /// </returns>
        IList<Project> SearchProjects(string projectName, int projectType, int company, int status, bool AllProject, bool hasRights = false);

        /// <summary>
        /// Inserts the project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        int InsertProject(Project project);

        /// <summary>
        /// Updates the project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        int UpdateProject(Project project);

        /// <summary>
        /// Gets the project by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Project.
        /// </returns>
        Project GetProjectById(int id);

        /// <summary>
        /// Deletes the project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        bool DeleteProject(Project project);

        /// <summary>
        /// Gets the project status select list.
        /// </summary>
        /// <returns>
        /// IList&lt;SelectListItem&gt;.
        /// </returns>
        IList<SelectListItem> GetProjectStatusSelectList();

        /// <summary>
        /// Gets the priority select list.
        /// </summary>
        /// <returns>
        /// IList&lt;SelectListItem&gt;.
        /// </returns>
        IList<SelectListItem> GetPrioritySelectList();
        /// <summary>
        /// Gets the projectt list.
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetProjecttList();
        /// <summary>
        /// Copies the project.
        /// </summary>
        /// <param name="ProjectID">The project identifier.</param>
        /// <param name="UserID">The user identifier.</param>
        /// <returns></returns>
        bool CopyProject(int ProjectID, int UserID);
        /// <summary>
        /// Gets the prior list.
        /// </summary>
        /// <param name="TotalValue">The total value.</param>
        /// <returns></returns>
        IList<SelectListItem> GetPriorList(int TotalValue);

        IList<Project> ProjectList();
    }
}