using System;
using System.Collections.Generic;
using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public class SettingService : ISettingService
    {

        #region Fields

        private readonly IRepository<Settings> _settingRepository;

        #endregion

        #region Ctor

        public SettingService(IRepository<Settings> settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public static string Configitems { get; private set; }

        #endregion

        #region Methods
        /// <summary>
        /// Selects the setting list.
        /// </summary>
        /// <returns>List&lt;Settings&gt;.</returns>
        public static List<Settings> SelectSettingList()
        {

            var DbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
            var query = from p in DbContext.Settings
                        select p;

            return query.ToList();

        }


        /// <summary>
        /// Selects the setting list.
        /// </summary>
        /// <returns>List&lt;Settings&gt;.</returns>
        public List<Settings> GetAllSettingsList()
        {
            var query = (from p in _settingRepository.Table
                         join p1 in _settingRepository.Table on p.SettingID equals p1.ParentIDSetting
                         select new
                         {
                             SettingKey = p1.SettingKey,
                             SettingValue = p1.SettingValue,
                             SettingID = p1.SettingID,
                             ParentIDSetting = p1.ParentIDSetting,
                             DefaultValue = p1.DefaultValue,
                             MinValue = p1.MinValue,
                             MaxValue = p1.MaxValue,
                             Comment = p1.Comment,
                             ParentKey = p.SettingKey,
                         }).ToList().Select(t => new Settings
                         {
                             SettingID = t.SettingID,
                             ParentIDSetting = t.ParentIDSetting,
                             SettingKey = t.SettingKey,
                             SettingValue = t.SettingValue,
                             DefaultValue = t.DefaultValue,
                             MinValue = t.MinValue,
                             MaxValue = t.MaxValue,
                             Comment = t.Comment,
                             ParentKey = t.ParentKey
                         }).ToList();
            return query;

        }

        public Settings GetSettingsById(int id)
        {
            return _settingRepository.GetById(id);
        }



        /// <summary>
        /// Updates the setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.ArgumentNullException">setting</exception>
        public int UpdateSetting(Settings setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            var model = GetSettingsById(setting.SettingID);
            if (model != null)
            {
                model.SettingValue = setting.SettingValue;
                model.Comment = setting.Comment;

            }
            _settingRepository.Update(model);
            return setting.SettingID;
        }
        #endregion
    }
}
