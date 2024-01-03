using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public class UnitService : IUnitService
    {
        #region Fields

        private readonly IRepository<Unit> _unitRepository;

        #endregion

        #region Ctor

        public UnitService(IRepository<Unit> unitRepository)
        {
            _unitRepository = unitRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches the units.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>IList&lt;Unit&gt;.</returns>
        public IList<Unit> SearchUnits(string unit)
        {
            var query = from p in _unitRepository.Table
                        where ((p.UOM.Contains(unit) || unit == ""))
                        orderby p.UnitId descending
                        select p;

            return query.ToList();

        }

        /// <summary>
        /// Inserts the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <exception cref="System.ArgumentNullException">unit</exception>
        public int InsertUnit(Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");
            if (checkExistingRecord(unit))
            {
                return -1;
            }
            unit.CreatedBy = ProjectSession.UserID;
            unit.CreatedDate = DateTime.Now;
            _unitRepository.Insert(unit);
            return unit.UnitId;
        }

        /// <summary>
        /// Updates the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.ArgumentNullException">unit</exception>
        public int UpdateUnit(Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");
            if (checkExistingRecord(unit))
            {
                return -1;
            }
            var model = GetUnitById(unit.UnitId);
            if (model != null)
            {
                model.UOM = unit.UOM;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
            }
            _unitRepository.Update(model);
            return unit.UnitId;
        }

        /// <summary>
        /// Gets the unit by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Unit.</returns>
        public Unit GetUnitById(int id)
        {
            return _unitRepository.GetById(id);
        }

        /// <summary>
        /// Deletes the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">unit</exception>
        public bool DeleteUnit(Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");

            try
            {
                _unitRepository.Delete(unit);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the unit select list.
        /// </summary>
        /// <returns>IList&lt;SelectListItem&gt;.</returns>
        public IList<SelectListItem> GetUnitSelectList()
        {
            var query = from p in _unitRepository.Table
                        orderby p.UOM ascending
                        select new SelectListItem
                        {
                            Text = p.UOM,
                            Value = p.UnitId.ToString()
                        };

            return query.ToList();
        }

        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingRecord(Unit model)
        {
            try
            {
                var query = from p in _unitRepository.Table
                            where p.UnitId != model.UnitId &&
                            ((p.UOM).Equals(model.UOM))
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
        #endregion
    }
}
