using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface ISectionService
    {
        /// <summary>
        /// Searches the sections.
        /// </summary>
        /// <param name="sectionname">The sectionname.</param>
        /// <returns>IList&lt;Section&gt;.</returns>
        IList<Section> SearchSections(string sectionname);

        /// <summary>
        /// Inserts the section.
        /// </summary>
        /// <param name="section">The section.</param>
        int InsertSection(Section section);

        /// <summary>
        /// Updates the section.
        /// </summary>
        /// <param name="section">The section.</param>
        int UpdateSection(Section section);

        /// <summary>
        /// Gets the section by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Section.</returns>
        Section GetSectionById(int id);

        /// <summary>
        /// Deletes the section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteSection(Section section);

        /// <summary>
        /// Gets the section select list.
        /// </summary>
        /// <returns>IList&lt;SelectListItem&gt;.</returns>
        IList<SelectListItem> GetSectionSelectList();

        /// <summary>
        /// Gets the Section roles.
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        IList<Section> GetSectionRoles(int RoleId);

        /// <summary>
        /// Deletes the section roles.
        /// </summary>
        /// <param name="RoleId"></param>
        void DeleteUserRoles(int RoleId);

        /// <summary>
        /// Inserts the section role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        int InsertSectionRole(SectionRole role);

        /// <summary>
        /// gets all section roles
        /// </summary>
        /// <returns></returns>
        List<SectionRole> GetAllSectionRoles();

        /// <summary>
        /// Gets sections list for site and report
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="ReportId"></param>
        /// <returns></returns>
        IList<SelectListItem> GetSectionSelectList(int SiteId, int ReportId);
    }
}
