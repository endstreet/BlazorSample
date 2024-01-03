using System.Collections.Generic;
using TrevaliOperationalReport.Common.Paging;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface IUserService
    {
        /// <summary>
        /// Inserts the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        void InsertUser(Users user);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        void UpdateUser(Users user);

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        void DeleteUser(Users user);

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Users GetUserById(int id);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        IList<Users> GetAllUsers();

        /// <summary>
        /// Searches the users.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        /// <returns>IList&lt;Users&gt;.</returns>
        IList<Users> SearchUsers(string Name, string Surname, string Active);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        IPagedList<Users> GetAllUsers(int pageIndex, int pageSize);

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Users GetUserByEmail(string email);

        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        Users GetUserByUserName(string username);

        /// <summary>
        /// validate employee logins
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Users CheckEmployeeCredential(string username, string password);

        /// <summary>
        /// check user exist or not.
        /// </summary>
        /// <param name="username">The Username.</param>
        /// <returns></returns>
        Users CheckEmployeeExist(string username);

        /// <summary>
        /// Inserts reset password record
        /// </summary>
        /// <param name="modelreset"></param>
        /// <returns></returns>
        int InsertUserResetPwd(UserResetPassword modelreset);

        /// <summary>
        /// get reset password link details
        /// </summary>
        /// <param name="ResetLinkID"></param>
        /// <returns></returns>
        UserResetPassword GetResetPasswordDetailsByID(int ResetLinkID);

        /// <summary>
        /// Update reset link data
        /// </summary>
        /// <param name="userresetpwd"></param>
        void UpdateUserResetPwd(UserResetPassword userresetpwd);


        /// <summary>
        /// Activates/Deactivates the users.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        bool ActiveDeactiveUser(Users modelUser);

        /// <summary>
        /// Changes user password
        /// </summary>
        /// <param name="modelChangePass"></param>
        void ChangeUserPassword(ChangePassword modelChangePass);

        void DeleteSiteRights(int UserID, int siteId);

        void InsertSiteRights(List<UserSiteRights> SiteRightsList);
    }
}
