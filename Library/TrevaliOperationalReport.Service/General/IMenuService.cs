using System.Collections.Generic;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface IMenuService
    {
        /// <summary>
        /// Gets Menu items for userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<GEN_UserAccessPermissions_Result> GetMenu(int userId);

        /// <summary>
        /// Gets the menu list dynamic.
        /// </summary>
        /// <returns>List&lt;Menus&gt;.</returns>
        List<Menus> GetMenuListDynamic();

        List<Section> GetSectionRoles(int UserId);

        Menus GetMenuById(int id);
    }
}
