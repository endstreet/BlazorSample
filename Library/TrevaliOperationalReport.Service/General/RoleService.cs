using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public class RoleService : IRoleService
    {

        #region Fields

        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole> _userroleRepository;

        #endregion

        #region Ctor

        public RoleService(IRepository<Role> roleRepository, IRepository<UserRole> userroleRepository)
        {
            _roleRepository = roleRepository;
            _userroleRepository = userroleRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches the roles.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>IList&lt;Role&gt;.</returns>
        public IList<Role> SearchRoles(string role)
        {
            var query = from p in _roleRepository.Table
                        where ((p.RoleName.Contains(role) || role == ""))
                        orderby p.RoleId descending
                        select p;

            return query.ToList();

        }

        /// <summary>
        /// Inserts the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <exception cref="System.ArgumentNullException">role</exception>
        public int InsertRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            if (checkExistingRecord(role))
            {
                return -1;
            }
            role.CreatedBy = ProjectSession.UserID;
            role.CreatedDate = DateTime.Now;
            _roleRepository.Insert(role);
            return role.RoleId;
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.ArgumentNullException">role</exception>
        public int UpdateRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            if (checkExistingRecord(role))
            {
                return -1;
            }
            var model = GetRoleById(role.RoleId);
            if (model != null)
            {
                model.RoleName = role.RoleName;
                model.Description = role.Description;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
            }
            _roleRepository.Update(model);
            return role.RoleId;
        }

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Role.</returns>
        public Role GetRoleById(int id)
        {
            return _roleRepository.GetById(id);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">role</exception>
        public bool DeleteRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            try
            {
                _roleRepository.Delete(role);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the role select list.
        /// </summary>
        /// <returns>IList&lt;SelectListItem&gt;.</returns>
        public IList<SelectListItem> GetRoleSelectList()
        {
            var query = from p in _roleRepository.Table
                        orderby p.RoleName ascending
                        select new SelectListItem
                        {
                            Text = p.RoleName,
                            Value = p.RoleId.ToString()
                        };

            return query.ToList();
        }

        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingRecord(Role model)
        {
            try
            {
                var query = from p in _roleRepository.Table
                            where p.RoleId != model.RoleId &&
                            ((p.RoleName).Equals(model.RoleName))
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Searches the roles.
        /// </summary>
        /// <param name="RoleId">The role identifier.</param>
        /// <returns>IList&lt;Role&gt;.</returns>
        public IList<Role> SearchRoles(int RoleId)
        {
            var query = from p in _roleRepository.Table
                        where p.RoleId == RoleId
                        orderby p.RoleId descending
                        select p;

            return query.ToList();

        }


        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        /// <returns>IList&lt;Role&gt;.</returns>
        public IList<Role> GetUserRoles(int UserID)
        {
            var query = (from ur in _userroleRepository.Table
                         join r in _roleRepository.Table on
                         ur.RoleId equals r.RoleId
                         where ur.UserId == UserID
                         select new
                         {
                             RoleId = ur.RoleId,
                             RoleName = r.RoleName,
                         }).ToList()
                         .Select(x => new Role
                         {
                             RoleId = x.RoleId,
                             RoleName = x.RoleName,

                         }).ToList();
            return query;
        }

        /// <summary>
        /// Deletes the user roles.
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        public void DeleteUserRoles(int UserID)
        {
            var query = (from p in _userroleRepository.Table
                         where p.UserId == UserID
                         select p).ToList();

            if (query != null && query.Count > 0)
            {
                foreach (UserRole entry in query)
                {
                    _userroleRepository.Delete(entry);
                }
            }

        }

        /// <summary>
        /// Inserts the user role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <exception cref="System.ArgumentNullException">role</exception>
        public int InsertRole(UserRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            if (checkExistingRecordUserRole(role))
            {
                return -1;
            }
            _userroleRepository.Insert(role);
            return role.RoleId;
        }

        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingRecordUserRole(UserRole model)
        {
            try
            {
                var query = from p in _userroleRepository.Table
                            where p.RoleId == model.RoleId &&
                           p.UserId == model.UserId &&
                           p.UserRoleId != model.UserRoleId
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
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// gets all user roles
        /// </summary>
        /// <returns></returns>
        public List<UserRole> GetAllUserRoles()
        {
            try
            {
                var query = from p in _userroleRepository.Table
                            select p;
                return query.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
