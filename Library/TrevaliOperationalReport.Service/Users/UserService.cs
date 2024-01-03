using System;
using System.Collections.Generic;
using System.Linq;
using TrevaliOperationalReport.Common.Paging;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.Users;
using TrevaliOperationalReport.Service.Caching;

namespace TrevaliOperationalReport.Service.Users
{
    public class UserService : IUserService
    {
        #region Constants

        private const string TATVA_USERS_KEY = "TrevaliOperationalReport.users.key.";
        private const string TATVA_USERS_LIST = "TrevaliOperationalReport.users.key.list";

        #endregion

        #region Fields

        private readonly IRepository<User> _userRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public UserService(IRepository<User> userRepository,
                           ICacheManager cacheManager)
        {
            _userRepository = userRepository;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        public void InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.CreatedDate = DateTime.UtcNow;
            user.ModifiedDate = DateTime.UtcNow;
            _userRepository.Insert(user);
            _cacheManager.RemoveByPattern(TATVA_USERS_KEY);
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.ModifiedDate = DateTime.UtcNow;
            _userRepository.Update(user);
            _cacheManager.RemoveByPattern(TATVA_USERS_KEY);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">User</exception>
        public void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Delete(user);
            _cacheManager.RemoveByPattern(TATVA_USERS_KEY);
        }

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public User GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public IList<User> GetAllUsers()
        {
            return _cacheManager.Get(TATVA_USERS_LIST, () =>
                {
                    var query = from p in _userRepository.Table
                               // orderby p.user descending
                                select p;

                    return query.ToList();
                });
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public IPagedList<User> GetAllUsers(int pageIndex, int pageSize)
        {
            var query = from p in _userRepository.Table
                        //orderby p.Id descending
                        select p;

            return new PagedList<User>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public User GetUserByEmail(string email)
        {
            var query = from p in _userRepository.Table
                        where p.Email == email
                       // orderby p.Id descending
                        select p;

            return query.FirstOrDefault();
        }

        #endregion
    }
}
