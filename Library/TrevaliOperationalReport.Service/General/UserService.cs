using System;
using System.Collections.Generic;
using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Common.Paging;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public class UserService : IUserService
    {
        #region Fields

        private readonly IRepository<Users> _userRepository;
        private readonly IRepository<UserResetPassword> _userResetPasswordRepository;

        #endregion

        #region Ctor

        public UserService(IRepository<Users> userRepository,
                           IRepository<UserResetPassword> userResetPasswordRepository)
        {
            _userRepository = userRepository;
            _userResetPasswordRepository = userResetPasswordRepository;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        public void InsertUser(Users user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (checkExistingEmail(user))
            {
                user.UserID = -1;
                return;
            }
            if (checkExistingEmail(user, true))
            {
                user.UserID = -2;
                return;
            }
            _userRepository.Insert(user);
        }

        /// <summary>
        /// Checks the existing email.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingEmail(Users model, bool IsCheckusername = false)
        {
            try
            {
                var query = from p in _userRepository.Table
                            where p.UserID != model.UserID &&
                            ((p.EmailID).Equals(model.EmailID))
                            select p;
                if (IsCheckusername)
                {
                    query = from p in _userRepository.Table
                            where p.UserID != model.UserID &&
                            ((p.UserName).Equals(model.UserName))
                            select p;
                }

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
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        public void UpdateUser(Users user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (checkExistingEmail(user))
            {
                user.UserID = -1;
                return;
            }
            if (checkExistingEmail(user, true))
            {
                user.UserID = -2;
                return;
            }

            var model = GetUserById(user.UserID);
            if (model != null)
            {
                model.Name = user.Name;
                model.Surname = user.Surname;
                model.UserName = user.UserName;
                model.Designation = user.Designation;
                if (ProjectSession.IsAdmin)
                {
                    model.IsSuperAdmin = user.IsSuperAdmin;
                }
                model.EmailID = user.EmailID;
                model.ModifiedBy = user.ModifiedBy;
                model.ModifiedDate = DateTime.Now;
            }
            _userRepository.Update(model);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        public void DeleteUser(Users user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Delete(user);
        }

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Users GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public IList<Users> GetAllUsers()
        {
            var query = from p in _userRepository.Table
                        orderby p.UserID descending
                        select p;

            return query.ToList();
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public IList<Users> SearchUsers(string name, string surname, string Active)
        {
            bool status;
            if (Active == "true")
                status = true;
            else
                status = false;
            if (Active == "")
            {
                var query = from p in _userRepository.Table
                            where ((p.Name.Contains(name) || name == "") &&
                            (p.Surname.Contains(surname) || surname == ""))
                            orderby p.UserID descending
                            select p;
                return query.ToList();
            }
            else
            {
                var query = from p in _userRepository.Table
                            where ((p.Name.Contains(name) || name == "") &&
                            (p.Surname.Contains(surname) || surname == "") &&
                            (p.IsActive == status))
                            orderby p.UserID descending
                            select p;
                return query.ToList();
            }
            //var query = from p in _userRepository.Table
            //            where ((p.UserName.Contains(username) || username == "") &&
            //            (p.EmailID == email || email == ""))
            //            orderby p.UserID descending
            //            select p;

            //return query.ToList();

        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public IPagedList<Users> GetAllUsers(int pageIndex, int pageSize)
        {
            var query = from p in _userRepository.Table
                        orderby p.UserID descending
                        select p;

            return new PagedList<Users>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public Users GetUserByEmail(string email)
        {
            var query = from p in _userRepository.Table
                        where p.EmailID == email
                        orderby p.UserID descending
                        select p;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Users.</returns>
        public Users GetUserByUserName(string username)
        {
            try
            {


                var query = from p in _userRepository.Table
                            where p.UserName == username
                            || p.EmailID == username
                            orderby p.UserID descending
                            select p;
                //var query = from p in _userRepository.Table select p;

                //var test = query.FirstOrDefault();

                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new Users() { Name = ex.ToString() };
            }
        }

        /// <summary>
        /// validate employee logins
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>Users.</returns>
        public Users CheckEmployeeCredential(string username, string password)
        {
            //TODO:&& t.Password == password
            var userdata = _userRepository.Table.Where(t => (t.UserName.ToLower() == username.ToLower() || t.EmailID.ToLower() == username.ToLower()) ).FirstOrDefault();
            //var userdata = _userRepository.Table.Where(t => (t.UserName.ToLower() == username.ToLower() || t.EmailID.ToLower() == username.ToLower()) && t.Password == password).FirstOrDefault();
            return userdata;
        }

        /// <summary>
        /// Check employee based on user name.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public Users CheckEmployeeExist(string username)
        {
            var userdata = _userRepository.Table.Where(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefault();
            return userdata;
        }

        /// <summary>
        /// Inserts reset password record
        /// </summary>
        /// <param name="modelreset">The modelreset.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.ArgumentNullException">UserResetPassword</exception>
        public int InsertUserResetPwd(UserResetPassword modelreset)
        {
            if (modelreset == null)
                throw new ArgumentNullException("UserResetPassword");


            modelreset.CreatedDate = DateTime.Now;
            _userResetPasswordRepository.Insert(modelreset);
            int id = modelreset.ResetLinkID;
            return id;
        }

        /// <summary>
        /// get reset password link details
        /// </summary>
        /// <param name="ResetLinkID">The reset link identifier.</param>
        /// <returns>UserResetPassword.</returns>
        public UserResetPassword GetResetPasswordDetailsByID(int ResetLinkID)
        {
            var model = _userResetPasswordRepository.GetById(ResetLinkID);
            return model;
        }

        /// <summary>
        /// Update reset link data
        /// </summary>
        /// <param name="userresetpwd">The userresetpwd.</param>
        /// <exception cref="System.ArgumentNullException">UserResetPassword</exception>
        public void UpdateUserResetPwd(UserResetPassword userresetpwd)
        {
            if (userresetpwd == null)
                throw new ArgumentNullException("UserResetPassword");
            userresetpwd.ModifiedDate = DateTime.Now;
            _userResetPasswordRepository.Update(userresetpwd);
        }

        /// <summary>
        /// Activates/Deactivates the users.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        public bool ActiveDeactiveUser(Users modelUser)
        {
            if (modelUser == null)
                throw new ArgumentNullException("UsersModel");

            try
            {
                var model = GetUserById(modelUser.UserID);
                if (model != null)
                {
                    model.IsActive = modelUser.IsActive;
                    model.ModifiedBy = modelUser.ModifiedBy;
                    model.ModifiedDate = modelUser.ModifiedDate;

                    _userRepository.Update(model);
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
        /// Changes user password
        /// </summary>
        /// <param name="modelChangePass"></param>
        public void ChangeUserPassword(ChangePassword modelChangePass)
        {
            if (modelChangePass == null)
                throw new ArgumentNullException("UsersChangePasswordModel");

            var model = GetUserById(modelChangePass.UserID);
            if (model != null)
            {
                model.Password = ConvertTo.HashSHA1(modelChangePass.NewPassword + model.PasswordSalt);
                model.ModifiedBy = modelChangePass.UserID;
                model.ModifiedDate = DateTime.Now;
            }

            _userRepository.Update(model);

        }


        public void DeleteSiteRights(int UserID, int SiteId = 0)
        {
            if (UserID == 0)
                throw new ArgumentNullException("UserSiteRightsID");


            using (var _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
            {
                _dbContext.UserSiteRights.RemoveRange((_dbContext.UserSiteRights.Where(c => c.UserID == UserID && (c.SiteId == SiteId || SiteId == 0))).ToList());
                _dbContext.SaveChanges();
            }
        }

        public void InsertSiteRights(List<UserSiteRights> SiteRightsList)
        {
            if (SiteRightsList.Count == 0)
                throw new ArgumentNullException("userSiteRights");

            using (var _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
            {
                _dbContext.UserSiteRights.AddRange((SiteRightsList));
                _dbContext.SaveChanges();
            }
        }
        #endregion
    }
}
