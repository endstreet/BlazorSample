using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public class MenuService : IMenuService
    {
        #region Fields

        private readonly IRepository<Menus> _menuRepository;
        private static TrevaliOperationalReportObjectContext _dbContext;
        #endregion

        #region Ctor

        public MenuService(IRepository<Menus> menuRepository)
        {
            _menuRepository = menuRepository;
            _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets menu items for userid
        /// </summary>
        /// <param name="userid">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        public List<GEN_UserAccessPermissions_Result> GetMenu(int UserId)
        {
            if (UserId <= 0)
                throw new ArgumentNullException("user");
            object[] xparams = {
                            new SqlParameter("UserID", UserId)
                         };

            var lst = _menuRepository.ExecuteStoredProcedureList<GEN_UserAccessPermissions_Result>("GEN_UserAccessPermissions", xparams).ToList().OrderBy(p => p.DispalyOrder).ToList();

            return lst;
        }

        public List<Menus> GetMenuListDynamic()
        {
            var query = (from p in _menuRepository.Table
                         where p.IsActive && p.ParentMenuId == null
                         orderby p.DispalyOrder
                         select p);
            return query.ToList();
        }


        public List<Section> GetSectionRoles(int UserId)
        {
            if (UserId <= 0)
                throw new ArgumentNullException("user");

            if (ProjectSession.IsAdmin)
            {
                return (from s in _dbContext.Section select s).ToList();
            }

            var sections = (from s in _dbContext.Section
                            join sr in _dbContext.SectionRoles
                            on s.SectionId equals sr.SectionId
                            join ur in _dbContext.UserRoles
                            on sr.RoleId equals ur.RoleId
                            where ur.UserId == UserId
                            select new
                            {
                                SectionId = s.SectionId,
                                SectionName = s.SectionName
                            }).ToList().Distinct()
                           .Select(x => new Section
                           {
                               SectionId = x.SectionId,
                               SectionName = x.SectionName
                           }).ToList();

            return sections;
        }
        public Menus GetMenuById(int id)
        {
            return _menuRepository.GetById(id);
        }
        #endregion

    }
}
