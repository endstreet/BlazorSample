using System.Collections.Generic;
using TrevaliOperationalReport.Common.Paging;
using TrevaliOperationalReport.Domain.Users;

namespace TrevaliOperationalReport.Service.Users
{
    public interface IUserService
    {
        /// <summary>
        /// Inserts the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        void InsertUser(User user);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        void UpdateUser(User user);

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        void DeleteUser(User user);

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        User GetUserById(int id);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        IList<User> GetAllUsers();

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        IPagedList<User> GetAllUsers(int pageIndex, int pageSize);

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        User GetUserByEmail(string email);
    }
}
