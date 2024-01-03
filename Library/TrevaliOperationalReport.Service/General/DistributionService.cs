using System;
using System.Collections.Generic;
using System.Linq;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public class DistributionService : IDistributionService
    {
        #region Fields
        private readonly IRepository<Distribution> _distributionRepository;
        #endregion
        #region Ctor
        public DistributionService(IRepository<Distribution> distributionRepository)
        {
            _distributionRepository = distributionRepository;


        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets all distribute users.
        /// </summary>
        /// <returns></returns>
        public IList<Distribution> GetAllDistributeUsers()
        {
            var query = from p in _distributionRepository.Table
                        orderby p.Name descending
                        select p;

            return query.ToList();
        }
        /// <summary>
        /// Inserts the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        public void InsertUser(Distribution user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (checkExistingEmail(user))
            {
                user.DistributionID = -1;
                return;
            }

            _distributionRepository.Insert(user);
        }

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Distribution GetUserById(int id)
        {
            return _distributionRepository.GetById(id);
        }
        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        public void UpdateUser(Distribution user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (checkExistingEmail(user))
            {
                user.DistributionID = -1;
                return;
            }


            var model = GetUserById(user.DistributionID);
            if (model != null)
            {
                model.Name = user.Name;
                model.Surname = user.Surname;
                model.EmailID = user.EmailID;
                model.ModifiedBy = user.ModifiedBy;
                model.ModifiedDate = DateTime.Now;

            }
            _distributionRepository.Update(model);
        }
        /// <summary>
        /// Deletes the unit.
        /// </summary>
        /// <param name="duser">The duser.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">unit</exception>
        public bool DeleteUnit(Distribution duser)
        {
            if (duser == null)
                throw new ArgumentNullException("unit");

            try
            {
                _distributionRepository.Delete(duser);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Checks the existing email.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private bool checkExistingEmail(Distribution model)
        {
            try
            {
                var query = from p in _distributionRepository.Table
                            where p.DistributionID != model.DistributionID && ((p.EmailID).Equals(model.EmailID))
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
        /// Searches the users.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="surname">The surname.</param>
        /// <param name="Active">The active.</param>
        /// <returns></returns>
        public IList<Distribution> SearchUsers(string name, string surname, string Email)
        {

            var query = from p in _distributionRepository.Table
                        where ((p.Name.Contains(name) || name == "") &&
                        (p.Surname.Contains(surname) || surname == "") &&
                        (p.EmailID.Contains(Email) || Email == ""))
                        orderby p.DistributionID descending
                        select p;
            return query.ToList();

        }
        #endregion
    }
}
