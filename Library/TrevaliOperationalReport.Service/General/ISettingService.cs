using System.Collections.Generic;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public interface ISettingService
    {
        List<Settings> GetAllSettingsList();

        /// <summary>
        /// Gets the setting by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Settings.</returns>
        Settings GetSettingsById(int id);

        int UpdateSetting(Settings setting);
    }


}
