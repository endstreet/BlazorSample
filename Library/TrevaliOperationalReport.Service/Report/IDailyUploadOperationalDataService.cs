using System;
using System.Collections.Generic;
using TrevaliOperationalReport.Domain.Report;

namespace TrevaliOperationalReport.Service.Report
{
    public interface IDailyUploadOperationalDataService
    {
        IList<DailyUploadMetricsData> GetDailyOperationalDataMetrics(int SiteId, int ReportId, DateTime? Date);
        void UpdateDailylyData(DailyUploadMetricsData DailyData, DateTime? Date, decimal? millAvailability, decimal? millOre);
        long InsertDailyData(DailyUploadOperationalData model, decimal? millAvailability, decimal? millOre);
        bool CheckExistingSectionsData(DailyUploadOperationalData model, int SiteId);
        void InsertSheet(UploadSheetData upload);
        bool SendWeeklyOperationalUploadData(int siteId, DateTime date, int reportId);
    }

}
