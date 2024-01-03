using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public static class RoleRightsService
    {
        /// <summary>
        /// Gets the access rights.
        /// </summary>
        /// <returns>List&lt;AccessRights&gt;.</returns>
        public static List<AccessRights> GetAccessRights()
        {
            using (var _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
            {
                return _dbContext.AccessRights.Where(r => r.IsActive).ToList();
            }
        }

        /// <summary>
        /// Gets the access rights for role.
        /// </summary>
        /// <param name="RoleId">The role identifier.</param>
        /// <returns>List&lt;AccessRightsForRole&gt;.</returns>
        public static List<AccessRightsForRole> GetAccessRightsForRole(int RoleId)
        {
            using (var _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
            {
                object[] parameters = {
                            new SqlParameter("RoleId", RoleId)
                         };

                var rights = _dbContext.ExecuteStoredProcedureList<AccessRightsForRole>("GEN_AccessRightsForRole", parameters).ToList();
                return rights;
            }
        }

        /// <summary>
        /// Gets the child menus.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>List&lt;Menus&gt;.</returns>
        public static List<Menus> GetChildMenus(int parentId)
        {
            using (var _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
            {
                return _dbContext.Menus.Where(r => r.ParentMenuId == parentId && r.IsActive == true).OrderBy(role => role.DispalyOrder).ToList();
            }
        }

        /// <summary>
        /// Gets the menu access rights.
        /// </summary>
        /// <param name="menudId">The menud identifier.</param>
        /// <returns>List&lt;MenuAccessRights&gt;.</returns>
        public static List<MenuAccessRights> GetMenuAccessRights(int menudId)
        {
            using (var _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
            {
                return _dbContext.MenuAccessRights.Where(r => r.MenuID == menudId).ToList();
            }
        }

        /// <summary>
        /// Deletes the role menu access rights.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        public static void DeleteRoleMenuAccessRights(int roleId)
        {
            try
            {
                using (var _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
                {
                    List<RoleMenuAccessRights> roleData = _dbContext.RoleMenuAccessRights.Where(d => d.RoleID == roleId).ToList();

                    foreach (var roleMenuAccessRight in roleData)
                    {
                        _dbContext.RoleMenuAccessRights.Attach(roleMenuAccessRight);
                        _dbContext.Entry(roleMenuAccessRight).State = EntityState.Deleted;
                    }
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Roles the menu access rights insert.
        /// </summary>
        /// <param name="roleMenuAccessRight">The role menu access right.</param>
        /// <returns>System.String.</returns>
        public static string RoleMenuAccessRightsInsert(RoleMenuAccessRights roleMenuAccessRight)
        {
            try
            {
                using (var _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
                {
                    _dbContext.RoleMenuAccessRights.Add(roleMenuAccessRight);
                    _dbContext.SaveChanges();
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the role references.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        public static void DeleteRoleReferences(int roleId)
        {
            try
            {
                using (var _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
                {
                    List<RoleMenuAccessRights> roleData = _dbContext.RoleMenuAccessRights.Where(d => d.RoleID == roleId).ToList();

                    foreach (var roleMenuAccessRight in roleData)
                    {
                        _dbContext.RoleMenuAccessRights.Attach(roleMenuAccessRight);
                        _dbContext.Entry(roleMenuAccessRight).State = EntityState.Deleted;
                    }

                    _dbContext.SaveChanges();
                }
            }
            catch (Exception) { throw; }
        }


        public static List<UserSiteRights> GetSiteRights(int UserID)
        {
            if (UserID == 0)
                throw new ArgumentNullException("UserSiteRightsID");

            using (var _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
            {
                List<UserSiteRights> CompanySiteData = _dbContext.UserSiteRights.Where(d => d.UserID == UserID).ToList();
                return CompanySiteData;
            }
        }

        public static List<UserSiteRights> GetCompanyRights(int UserID)
        {
            if (UserID == 0)
                throw new ArgumentNullException("UserCompanyRightsID");

            using (var _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
            {
                List<UserSiteRights> CompanyRightsData = _dbContext.UserSiteRights.Where(d => d.UserID == UserID).ToList();
                return CompanyRightsData;
            }
        }
    }
}
