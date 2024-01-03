using System.Collections.Generic;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface IDistributionService
    {
        /// <summary>
        /// Gets all distribute users.
        /// </summary>
        /// <returns></returns>
        IList<Distribution> GetAllDistributeUsers();

        /// <summary>
        /// Inserts the user.
        /// </summary>
        /// <param name="user">The user.</param>
        void InsertUser(Distribution user);
        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Distribution GetUserById(int id);
        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        void UpdateUser(Distribution user);
        /// <summary>
        /// Deletes the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        bool DeleteUnit(Distribution unit);
    }
}
