using System.Collections.Generic;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface IUnitService
    {
        /// <summary>
        /// Searches the units.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>IList&lt;Unit&gt;.</returns>
        IList<Unit> SearchUnits(string unit);

        /// <summary>
        /// Inserts the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        int InsertUnit(Unit unit);

        /// <summary>
        /// Updates the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        int UpdateUnit(Unit unit);

        /// <summary>
        /// Gets the unit by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Unit.</returns>
        Unit GetUnitById(int id);

        /// <summary>
        /// Deletes the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteUnit(Unit unit);

        /// <summary>
        /// Gets the unit select list.
        /// </summary>
        /// <returns>IList&lt;SelectListItem&gt;.</returns>
        IList<SelectListItem> GetUnitSelectList();
    }
}
