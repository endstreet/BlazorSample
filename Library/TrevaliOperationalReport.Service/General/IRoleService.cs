using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface IRoleService
    {
        /// <summary>
        /// Searches the roles.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>IList&lt;GEN_Role&gt;.</returns>
        IList<Role> SearchRoles(string role);

        /// <summary>
        /// Inserts the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>System.Int32.</returns>
        int InsertRole(Role role);

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>System.Int32.</returns>
        int UpdateRole(Role role);

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>GEN_Role.</returns>
        Role GetRoleById(int id);

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteRole(Role role);

        /// <summary>
        /// Gets the role select list.
        /// </summary>
        /// <returns>IList&lt;SelectListItem&gt;.</returns>
        IList<SelectListItem> GetRoleSelectList();

        /// <summary>
        /// Searches the roles.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;GEN_Role&gt;.</returns>
        IList<Role> SearchRoles(int Id);

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        /// <returns>IList&lt;Role&gt;.</returns>
        IList<Role> GetUserRoles(int UserID);

        /// <summary>
        /// Deletes the user roles.
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        void DeleteUserRoles(int UserID);

        /// <summary>
        /// gets all user roles
        /// </summary>
        /// <returns></returns>
        List<UserRole> GetAllUserRoles();

    }
}
