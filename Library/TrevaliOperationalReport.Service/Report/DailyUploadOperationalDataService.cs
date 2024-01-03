using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;
using TrevaliOperationalReport.Domain.Report;
using TrevaliOperationalReport.Service.General;

namespace TrevaliOperationalReport.Service.Report
{
    public class DailyUploadOperationalDataService : IDailyUploadOperationalDataService
    {
        private readonly IRepository<DailyUploadOperationalData> _dailyUploadOperationalDataRepository;
        public static TrevaliOperationalReportObjectContext _dbContext;
        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<Metrics> _metricsRepository;
        private readonly IRepository<Reports> _reportRepository;
        private readonly IRepository<UploadSheetData> _uploadSheetRepository;

        public DailyUploadOperationalDataService(IRepository<DailyUploadOperationalData> dailyUploadOperationalDataRepository, IRepository<Site> siteRepository,
            IRepository<Metrics> metricsRepository, IRepository<Reports> reportRepository, IRepository<UploadSheetData> uploadSheetRepository)
        {
            _dailyUploadOperationalDataRepository = dailyUploadOperationalDataRepository;
            _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
            _siteRepository = siteRepository;
            _metricsRepository = metricsRepository;
            _reportRepository = reportRepository;
            _uploadSheetRepository = uploadSheetRepository;


        }

        public void UpdateDailylyData(DailyUploadMetricsData DailyData, DateTime? Date, decimal? millAvailability, decimal? millOre)
        {

            ////Unable to save add/edit daily grid data due to day difference.
            var model = (from x in _dailyUploadOperationalDataRepository.Table
                         where x.DailyOperationalDataId == DailyData.DailyOperationalDataId
                         //&& x.SiteMetricId == DailyData.SiteMetricId
                         ////&& x.CreatedDate == DailyData.CreatedDate
                         //&& x.DailyUploadDate.Value.Year == DailyData.DailyUploadDate.Year
                         //  && x.DailyUploadDate.Value.Month == DailyData.DailyUploadDate.Month
                         //  && x.DailyUploadDate.Value.Day == DailyData.DailyUploadDate.Day
                         select x).FirstOrDefault();

            if (model != null)
            {
                model.Comment = (string.IsNullOrEmpty(DailyData.Comment) ? null : DailyData.Comment);
                var oldActualValue = model.ActualValue;
                model.ActualValue = 0;
                if (DailyData.ActualValue != null)
                {
                    model.ActualValue = DailyData.ActualValue;
                }
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;

                try
                {
                    if (model.SiteMetrics.MetricsName == "Mill Availability" || model.SiteMetrics.MetricsName == "Ore Milled")
                    {
                        object[] xparams =
                        {
                            new SqlParameter("SiteId", DailyData.SiteId),
                            new SqlParameter("SiteMetricId", DailyData.SiteMetricId),
                            new SqlParameter("ActualValue", DailyData.ActualValue),
                            new SqlParameter("MTDValue", DailyData.ActualValue),
                            new SqlParameter("MetricId", DailyData.Metric.MetricId),
                            new SqlParameter("DailyUploadDate", DailyData.DailyUploadDate),
                            new SqlParameter("Year", DailyData.DailyUploadDate.Year),
                            new SqlParameter("Month", DailyData.DailyUploadDate.Month),
                            new SqlParameter("Week", 1),
                            new SqlParameter("PeriodMetric", 1),
                            new SqlParameter("UserId", ProjectSession.UserID)
                        };

                        if ((millAvailability > 0 && millOre > 0) || (millAvailability == 0 && millOre == 0))
                        {
                            _dailyUploadOperationalDataRepository.Update(model);

                            _dbContext.ExecuteStoredProcedureList<long>("InsertOrUpdateMillThroughPut", xparams);
                        }
                    }
                    else
                    {
                        _dailyUploadOperationalDataRepository.Update(model);
                    }

                    if (oldActualValue != DailyData.ActualValue)
                    {
                        UpdateOrInsertToInsightsTable(model.SiteMetricId, model.DailyUploadDate.Value);
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                }
            }
        }

        public long InsertDailyData(DailyUploadOperationalData modelDaily, decimal? millAvailability, decimal? millOre)
        {
            if (modelDaily == null)
                throw new ArgumentNullException("WeeklyOperationalData");

            var model = (from x in _dailyUploadOperationalDataRepository.Table
                         where x.DailyOperationalDataId == modelDaily.DailyOperationalDataId
                         //&& x.SiteMetricId == modelDaily.SiteMetricId
                         //&& x.DailyUploadDate.Value.Year == modelDaily.DailyUploadDate.Value.Year
                         //   && x.DailyUploadDate.Value.Month == modelDaily.DailyUploadDate.Value.Month
                         //   && x.DailyUploadDate.Value.Day == modelDaily.DailyUploadDate.Value.Day
                         select x).FirstOrDefault();

            var metricId = (from p in _dbContext.SiteMetrics
                            where p.SiteMetricId == modelDaily.SiteMetricId
                            select p.MetricId).SingleOrDefault();

            var metricName = (from p in _dbContext.Metrics
                              where p.MetricId == metricId
                              select p.MetricsName).SingleOrDefault();

            if (model != null)
            {

                if (!string.IsNullOrEmpty(modelDaily.Comment))
                    model.Comment = modelDaily.Comment;

                model.ActualValue = modelDaily.ActualValue ?? 0;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;

                if (metricName == "Mill Availability" || metricName == "Ore Milled")
                {
                    object[] xparams =
                    {
                        new SqlParameter("SiteId", modelDaily.SiteId),
                        new SqlParameter("SiteMetricId", modelDaily.SiteMetricId),
                        new SqlParameter("ActualValue", modelDaily.ActualValue),
                        new SqlParameter("MTDValue", modelDaily.ActualValue),
                        new SqlParameter("MetricId", metricId),
                        new SqlParameter("DailyUploadDate", modelDaily.DailyUploadDate),
                        new SqlParameter("Year", modelDaily.DailyUploadDate.Value.Year),
                        new SqlParameter("Month", modelDaily.DailyUploadDate.Value.Month),
                        new SqlParameter("Week", 1),
                        new SqlParameter("PeriodMetric", 1),
                        new SqlParameter("UserId", ProjectSession.UserID)
                    };

                    if (millAvailability > 0 && millOre > 0)
                    {
                        _dailyUploadOperationalDataRepository.Update(model);
                    }
                    _dbContext.ExecuteStoredProcedureList<long>("InsertOrUpdateMillThroughPut", xparams);
                }
                else
                {
                    _dailyUploadOperationalDataRepository.Update(model);
                }

                UpdateOrInsertToInsightsTable(model.SiteMetrics.SiteMetricId, model.DailyUploadDate.Value);

                return model.DailyOperationalDataId;
            }
            else
            {
                modelDaily.CreatedBy = ProjectSession.UserID;
                //modelDaily.CreatedDate = Convert.ToDateTime(modelDaily.CreatedDate);
                modelDaily.DailyUploadDate = Convert.ToDateTime(modelDaily.DailyUploadDate);

                if (metricName == "Mill Availability" || metricName == "Ore Milled")
                {
                    object[] xparams =
                    {
                        new SqlParameter("SiteId", modelDaily.SiteId),
                        new SqlParameter("SiteMetricId", modelDaily.SiteMetricId),
                        new SqlParameter("ActualValue", modelDaily.ActualValue),
                        new SqlParameter("MTDValue", modelDaily.ActualValue),
                        new SqlParameter("MetricId", metricId),
                        new SqlParameter("DailyUploadDate", modelDaily.DailyUploadDate),
                        new SqlParameter("Year", modelDaily.DailyUploadDate.Value.Year),
                        new SqlParameter("Month", modelDaily.DailyUploadDate.Value.Month),
                        new SqlParameter("Week", 1),
                        new SqlParameter("PeriodMetric", 1),
                        new SqlParameter("UserId", ProjectSession.UserID)
                    };

                    if (millAvailability > 0 && millOre > 0)
                    {
                        _dbContext.ExecuteStoredProcedureList<long>("InsertOrUpdateMillThroughPut", xparams);

                        _dailyUploadOperationalDataRepository.Insert(modelDaily);
                    }

                }
                else
                {
                    _dailyUploadOperationalDataRepository.Insert(modelDaily);
                }

                UpdateOrInsertToInsightsTable(modelDaily.SiteMetricId, modelDaily.DailyUploadDate.Value);

                return modelDaily.DailyOperationalDataId;
            }
        }


        public IList<DailyUploadMetricsData> GetDailyOperationalDataMetrics(int SiteId, int ReportId, DateTime? Date)
        {
            try
            {

                DateTime now;
                //if (Date)
                //{
                //     now = DateTime.Now.Date;
                //}
                //else
                //{
                //now = DateTime.ParseExact(Date, "yyyy-MM-dd", null);
                now = Convert.ToDateTime(Date).Date;
                //}

                var DailyMetricsData = new List<DailyUploadMetricsData>();
                object[] xparams = {
                         new SqlParameter("SiteId", SiteId),
                            new SqlParameter("ReportId", ReportId),
                            new SqlParameter("Date",now ),


                         };
                var DailyGridData = _dbContext.ExecuteStoredProcedureList<RPT_DailyData_Result>("RPT_DailyData", xparams);
                DailyGridData = DailyGridData.ToList().OrderBy(z => z.SectionId).ToList();
                if (DailyGridData != null && DailyGridData.Count > 0)
                {
                    foreach (RPT_DailyData_Result result in DailyGridData)
                    {

                        var objmetric = (from p in _dbContext.SiteMetrics
                                         where p.SiteMetricId == result.SiteMetricId
                                         select
                                             p.Metric
                                      ).FirstOrDefault();
                        if (result.DailyOperationalDataId != 0)
                        {
                            DailyMetricsData.Add(new DailyUploadMetricsData
                            {
                                DailyOperationalDataId = result.DailyOperationalDataId,
                                SiteMetricId = result.SiteMetricId,
                                SiteId = SiteId,
                                SectionId = result.SectionId,
                                SectionName = result.SectionName,
                                Metric = objmetric,
                                DailyUploadDate = now,
                                Comment = result.Comment ?? "",
                                //ActualValue = CommonHelper.SetRounding(result.ActualValue, objmetric.Unit.UOM.Trim()),
                                ActualValue = result.ActualValue,
                                IsDefault = false

                            });
                        }
                        else
                        {
                            long r = 0;
                            do
                            {
                                Random generator = new Random();
                                r = generator.Next(0, 999999);
                            }
                            while (DailyMetricsData.Any(x => x.DailyOperationalDataId == r) != false);

                            DailyMetricsData.Add(new DailyUploadMetricsData
                            {

                                DailyOperationalDataId = r,
                                SiteMetricId = result.SiteMetricId,
                                SiteId = SiteId,
                                SectionId = result.SectionId,
                                SectionName = result.SectionName,
                                Metric = objmetric,
                                Comment = "",
                                ActualValue = 0,
                                DailyUploadDate = now,
                                IsDefault = true
                                // MTD = decimal.Round(MTDVal, 4, MidpointRounding.AwayFromZero),

                            });
                        }
                    }
                }
                return DailyMetricsData;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool CheckExistingSectionsData(DailyUploadOperationalData model, int siteId)
        {
            var data = (from p in _dailyUploadOperationalDataRepository.Table
                        where
                            // p.CreatedDate ==model.CreatedDate
                            p.DailyUploadDate.Value.Year == model.DailyUploadDate.Value.Year
                            && p.DailyUploadDate.Value.Month == model.DailyUploadDate.Value.Month
                            && p.DailyUploadDate.Value.Day == model.DailyUploadDate.Value.Day
                        select p).ToList();
            var sitemeteic = _dbContext.SiteMetrics.ToList();
            var data2 = (from x in data
                         join od in _dbContext.SiteMetrics on x.SiteMetricId equals od.SiteMetricId
                         select new
                         {
                             x,

                         }).ToList();

            var metrics = GetMetricsSelectList(siteId, 0);
            //(metrics.Any(x => x.Text == model.SectionName + "^" + model.MetricName)
            if (data2 != null && data2.Count > 0)
            {
                if (metrics.Any(x => x.Text == model.SectionName + "^" + model.MetricName))
                {
                    if (data2.Any(z => z.x.SiteMetrics.SiteMetricId == Convert.ToInt32(metrics.Where(x => x.Text == model.SectionName + "^" + model.MetricName).FirstOrDefault().Value)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public bool SendWeeklyOperationalUploadData(int siteId, DateTime date, int reportId)
        {
            //Create Weekly Report PDF
            byte[] DailyReportPDF = CreateDailyOperationalReportPDF(siteId, date, reportId);

            List<byte[]> lstAttachments = new List<byte[]>() { DailyReportPDF };
            string DialySummaryReportName = "DailyOperationalData_" + date.ToString("MM/dd/yyyy") + ".pdf";

            List<string> attachmentName = new List<string>() { DialySummaryReportName };
            var settings = SettingService.SelectSettingList();
            string Body = "Please Find attachment of sites daily operational reports.";
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string Name = ProjectSession.UserName;
            //string MailCC = "";
            string Subject = "Sites Daily Operational Reports: " + "Date-" + date.ToString("MM/dd/yyyy");
            string emailTemplatePath = System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplates/DistributionOperationalReports.html");
            //string emailTemplatePath = Path.Combine(Directory.GetParent(Directory.GetParent(path).ToString()).ToString(), "EmailTempletes\\PMNotification.html"); ;
            using (StreamReader sReader = new StreamReader(emailTemplatePath))
            {
                string siteName = settings.Where(x => x.SettingKey == "SiteNameKey").Select(x => x.SettingValue).FirstOrDefault();
                string imglogo = settings.Where(x => x.SettingKey == "HostURLKey").Select(x => x.SettingValue).FirstOrDefault() + "/content/images/trevali.png";

                string htmlTemplate = sReader.ReadToEnd();
                htmlTemplate = htmlTemplate.Replace("$TYPE$", Subject);
                htmlTemplate = htmlTemplate.Replace("$SITEURL$", settings.Where(x => x.SettingKey == "HostURLKey").Select(x => x.SettingValue).FirstOrDefault());
                htmlTemplate = htmlTemplate.Replace("$LOGOURL$", imglogo);
                htmlTemplate = htmlTemplate.Replace("$SITENAME$", siteName);
                htmlTemplate = htmlTemplate.Replace("$NAME$", Name);
                htmlTemplate = htmlTemplate.Replace("$HEADERTEXT$", Body);
                bool success = Send(ProjectSession.Email, null, null, Subject, htmlTemplate, lstAttachments, attachmentName);
                return success;
            }
        }

        public bool Send(string mailTo, string mailCC, string mailBCC, string subject, string body, List<byte[]> attachmentFile = null, List<string> attachmentName = null)
        {
            Boolean issent = true;
            string mailFrom;
            var settings = SettingService.SelectSettingList();
            mailFrom = settings.Where(x => x.SettingKey == "FromEmailAddress").Select(x => x.SettingValue).FirstOrDefault();

            try
            {
                //if (ValidateEmail(mailFrom, mailTo) && (string.IsNullOrEmpty(mailCC) || IsEmail(mailCC)) && (string.IsNullOrEmpty(mailBCC) || IsEmail(mailBCC)))
                //{
                MailMessage mailMesg = new MailMessage();
                //SmtpClient objSMTP = new SmtpClient();
                SmtpClient objSMTP = new SmtpClient
                {
                    Host = settings.Where(x => x.SettingKey == "SMTP").Select(x => x.SettingValue).FirstOrDefault(),
                    Credentials = new NetworkCredential(settings.Where(x => x.SettingKey == "MailUserName").Select(x => x.SettingValue).FirstOrDefault(), settings.Where(x => x.SettingKey == "MailPassword").Select(x => x.SettingValue).FirstOrDefault()),
                    Port = ConvertTo.Integer(settings.Where(x => x.SettingKey == "SMTPPort").Select(x => x.SettingValue).FirstOrDefault()),
                    EnableSsl = ConvertTo.Boolean(settings.Where(x => x.SettingKey == "EnableSSL").Select(x => x.SettingValue).FirstOrDefault())
                };

                if (ConfigItems.TestMode)
                {
                    mailFrom = ConfigItems.TestEmailAddress;
                    mailTo = ConfigItems.TestEmailAddress;
                    mailCC = string.Empty;
                    mailBCC = string.Empty;

                    //objSMTP.UseDefaultCredentials = true;
                }

                mailMesg.From = new System.Net.Mail.MailAddress(mailFrom);
                mailMesg.To.Add(mailTo);

                //Include Default Addresses.
                if (!string.IsNullOrWhiteSpace(ConfigItems.DefaultEmailAddress))
                {
                    if (ConfigItems.DefaultEmailAddress.Contains(';'))
                    {
                        foreach (var address in ConfigItems.DefaultEmailAddress.Split(';'))
                        {
                            if (string.IsNullOrWhiteSpace(address))
                            {
                                continue;
                            }
                            mailMesg.To.Add(address);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(mailCC))
                {
                    string[] mailCCArray = mailCC.Split(';');
                    foreach (string email in mailCCArray)
                    {
                        mailMesg.CC.Add(email);
                    }
                }

                if (!string.IsNullOrEmpty(mailBCC))
                {
                    mailBCC = mailBCC.Replace(";", ",");
                    mailMesg.Bcc.Add(mailBCC);
                }

                if (attachmentFile != null && attachmentName != null)
                {
                    byte[][] Files = attachmentFile.ToArray();
                    string[] Names = attachmentName.ToArray();
                    if (Files != null)
                    {
                        for (int i = 0; i < Files.Length; i++)
                        {
                            mailMesg.Attachments.Add(new Attachment(new MemoryStream(Files[i]), Names[i]));
                        }
                    }
                }

                mailMesg.Subject = subject;
                mailMesg.Body = body;
                mailMesg.IsBodyHtml = true;

                try
                {
                    objSMTP.Send(mailMesg);
                    issent = true;
                    return issent;
                }
                catch (Exception e)
                {
                    mailMesg.Dispose();
                    mailMesg = null;
                    issent = false;
                    return issent;
                }
                finally
                {
                    mailMesg.Dispose();
                }
                //}
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public byte[] CreateDailyOperationalReportPDF(int siteId, DateTime date, int reportId)
        {
            //Render Report
            TrevaliOperationalReportObjectContext _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
            //bool IsShowReport = false;

            var localReport = new LocalReport
            {
                ReportPath = HttpContext.Current.Server.MapPath("~/Reports/rptDailyOperational.rdlc")
            };
            object[] xparams = {
                            new SqlParameter("SiteId", siteId),
                            new SqlParameter("ReportId", reportId),
                              new SqlParameter("Date", date),
                         };

            var reportdata = _dbContext.ExecuteStoredProcedureList<RPT_DailyOperationalReport_Result>("RPT_DailyOperationalReport", xparams);

            var excludeSections = new string[] { "MINING", "SILO STATUS" };

            localReport.DataSources.Add(new ReportDataSource("DailyOperational", ConvertListToDataTableforDailyOperationalReport(reportdata, excludeSections:excludeSections)));
            localReport.DataSources.Add(new ReportDataSource("DailyOperationalMining", ConvertListToDataTableforDailyOperationalReport(reportdata, forSection: "MINING")));
            localReport.DataSources.Add(new ReportDataSource("DailyOperationalSilo", ConvertListToDataTableforDailyOperationalReport(reportdata, forSection: "SILO STATUS")));

            localReport.DisplayName = "Daily Operational Report";
            return localReport.Render("pdf");
        }

        static DataTable ConvertListToDataTableforDailyOperationalReport(IList<RPT_DailyOperationalReport_Result> listWeekly, string forSection = null, string[] excludeSections = null)
        {
            //new Table

            DataTable table = new DataTable();

            table.Columns.Add("MetricId");
            table.Columns.Add("SectionName");
            table.Columns.Add("MetricsName");
            table.Columns.Add("Units");
            table.Columns.Add("Date");
            table.Columns.Add("Actual");
            table.Columns.Add("Forecast");
            table.Columns.Add("MTDActual");
            table.Columns.Add("MTDBudget");
            table.Columns.Add("MTDForcast");
            table.Columns.Add("Comment");

            if (forSection != null)
                listWeekly = listWeekly.Where(x => x.SectionName.Equals(forSection)).ToList();

            if (excludeSections != null)
                listWeekly = listWeekly.Where(x => !excludeSections.Contains(x.SectionName)).ToList();

            for (int i = 0; i < listWeekly.Count; i++)
                table.Rows.Add(listWeekly[i].MetricId, listWeekly[i].SectionName, listWeekly[i].MetricsName, listWeekly[i].Units, listWeekly[i].Date,
                     listWeekly[i].Actual ?? Decimal.Zero, listWeekly[i].Forecast ?? Decimal.Zero, listWeekly[i].MTDActual ?? Decimal.Zero,
                    listWeekly[i].MTDBudget ?? Decimal.Zero, listWeekly[i].MTDForcast ?? Decimal.Zero, listWeekly[i].Comment);
            return table;
        }

        public List<InvalidDailyData> UploadDailyOperationalData(List<DailyUploadOperationalData> lstDailyData, int reportId)
        {
            var InvalidCount = 0;
            List<InvalidDailyData> lstInvalid = new List<InvalidDailyData>();
            var sitename = lstDailyData[0].SiteName;
            var site = from p in _dbContext.Site where p.SiteName == sitename select p;
            int siteId = site.FirstOrDefault().SiteId;


            var sites = GetSitesSelectList(0, -1, true);
            var metrics = GetMetricsSelectList(siteId, reportId);
            var reports = GetReportsSelectList();
            var oreMilledData = lstDailyData.Where(x => x.MetricName == "Ore Milled (DMT)").FirstOrDefault();
            var millAvailabilityData = lstDailyData.Where(x => x.MetricName == "Mill Availability (%)").FirstOrDefault();
            //var comments = GetCommentsSelectList();
            if (lstDailyData != null && lstDailyData.Count > 0)
            {
                var oreMilled =
                    lstDailyData.Where(x => x.MetricName == "Ore Milled (DMT)")
                        .Select(x => x.ActualValue)
                        .FirstOrDefault();
                var millAvailability =
                    lstDailyData.Where(x => x.MetricName == "Mill Availability (%)")
                        .Select(x => x.ActualValue)
                        .FirstOrDefault();

                //Mill ThroughPut Calculation
                var millThroughPut = (millAvailability == 0 || millAvailability == null) ? 0 : (oreMilled / (1 * 24 * (millAvailability / 100)));
                if (oreMilled == 0 || millAvailability == 0)
                {
                    //lstDailyData.Remove(lstDailyData.Single(x => x.MetricName == "Ore Milled (DMT)"));
                    //lstDailyData.Remove(lstDailyData.Single(x => x.MetricName == "Mill Availability (%)"));
                    lstDailyData.Remove(lstDailyData.Single(x => x.MetricName == "Mill Throughput (TPH)"));
                }

                if (oreMilledData != null && millAvailability == 0 && oreMilled > 0)
                {
                    lstInvalid.Add(new InvalidDailyData
                    {
                        SiteName = oreMilledData.SiteName,
                        SectionName = oreMilledData.SectionName,
                        ReportName = oreMilledData.ReportName,
                        MetricName = oreMilledData.MetricName,
                        CreatedDate = oreMilledData.DailyUploadDate.Value.Month.ToString() + '/' + oreMilledData.DailyUploadDate.Value.Day.ToString() + '/' + oreMilledData.DailyUploadDate.Value.Year.ToString(),
                        ActualValue = oreMilledData.ActualValue,
                        Remarks = "Mill Availability value should be greater than 0."
                        //Remarks = "Year must be between " + (DateTime.Now.Year - 10).ToString() + " and " + (DateTime.Now.Year + 10).ToString()
                    });
                    InvalidCount++;
                }

                if (millAvailabilityData != null && oreMilled == 0 && millAvailability > 0)
                {
                    lstInvalid.Add(new InvalidDailyData
                    {
                        SiteName = millAvailabilityData.SiteName,
                        SectionName = millAvailabilityData.SectionName,
                        ReportName = millAvailabilityData.ReportName,
                        MetricName = millAvailabilityData.MetricName,
                        CreatedDate = millAvailabilityData.DailyUploadDate.Value.Month.ToString() + '/' + millAvailabilityData.DailyUploadDate.Value.Day.ToString() + '/' + millAvailabilityData.DailyUploadDate.Value.Year.ToString(),
                        ActualValue = millAvailabilityData.ActualValue,
                        Remarks = "Ore Milled value should be greater than 0."
                        //Remarks = "Year must be between " + (DateTime.Now.Year - 10).ToString() + " and " + (DateTime.Now.Year + 10).ToString()
                    });
                    InvalidCount++;
                }

                    foreach (DailyUploadOperationalData dailydata in lstDailyData)
                    {
                        if (!sites.Any(x => x.Text == dailydata.SiteName))
                        {
                            lstInvalid.Add(new InvalidDailyData
                            {
                                SiteName = dailydata.SiteName,
                                SectionName = dailydata.SectionName,
                                ReportName = dailydata.ReportName,
                                MetricName = dailydata.MetricName,
                                CreatedDate = dailydata.DailyUploadDate.Value.Month.ToString() + '/' + dailydata.DailyUploadDate.Value.Day.ToString() + '/' + dailydata.DailyUploadDate.Value.Year.ToString(),
                                ActualValue = dailydata.ActualValue,
                                Remarks = "Invalid site OR permission denied for site"
                            });
                            InvalidCount++;
                            continue;

                        }
                        //else if (!reports.Any(x => x.Value == weeklydata.ReportName))
                        //{

                        //    lstInvalid.Add(new InvalidWeeklyData
                        //    {
                        //        SiteName = weeklydata.SiteName,
                        //        ReportName = weeklydata.ReportName,
                        //        MetricName = weeklydata.MetricName,
                        //        Year = weeklydata.Year,
                        //        Week = weeklydata.Week,
                        //        ActualValue = weeklydata.ActualValue,
                        //        Remarks = "Invalid Report"
                        //    });
                        //    InvalidCount++;
                        //    continue;
                        //}
                        else if (!metrics.Any(x => x.Text == dailydata.SectionName + "^" + dailydata.MetricName))
                        {

                            lstInvalid.Add(new InvalidDailyData
                            {
                                SiteName = dailydata.SiteName,
                                SectionName = dailydata.SectionName,
                                ReportName = dailydata.ReportName,
                                MetricName = dailydata.MetricName,
                                CreatedDate = dailydata.DailyUploadDate.Value.Month.ToString() + '/' + dailydata.DailyUploadDate.Value.Day.ToString() + '/' + dailydata.DailyUploadDate.Value.Year.ToString(),
                                ActualValue = dailydata.ActualValue,
                                Remarks = "Invalid Metric/Section name OR Permission denied for section"
                            });
                            InvalidCount++;
                            continue;
                        }
                        //else if (!string.IsNullOrEmpty(weeklydata.CommentString) && !comments.Any(x => x.Text == weeklydata.CommentString))
                        //{

                        //    lstInvalid.Add(new InvalidWeeklyData
                        //    {
                        //        SiteName = weeklydata.SiteName,
                        //        ReportName = weeklydata.ReportName,
                        //        MetricName = weeklydata.MetricName,
                        //        Year = weeklydata.Year,
                        //        Week = weeklydata.Week,
                        //        ActualValue = weeklydata.ActualValue,
                        //        Remarks = "Invalid Comment"
                        //    });
                        //    InvalidCount++;
                        //    continue;
                        //}
                        else if (dailydata.ActualValue < 0)
                        {
                            lstInvalid.Add(new InvalidDailyData
                            {
                                SiteName = dailydata.SiteName,
                                SectionName = dailydata.SectionName,
                                ReportName = dailydata.ReportName,
                                MetricName = dailydata.MetricName,
                                CreatedDate = dailydata.DailyUploadDate.Value.Month.ToString() + '/' + dailydata.DailyUploadDate.Value.Day.ToString() + '/' + dailydata.DailyUploadDate.Value.Year.ToString(),
                                ActualValue = dailydata.ActualValue,
                                Remarks = "Actual value must be positive number"
                            });
                            InvalidCount++;
                            continue;
                        }
                        else if (string.IsNullOrEmpty(dailydata.DailyUploadDate.ToString()))
                        //else if ((weeklydata.Year < DateTime.Now.Year - 10) || (weeklydata.Year > DateTime.Now.Year + 10))
                        {
                            lstInvalid.Add(new InvalidDailyData
                            {
                                SiteName = dailydata.SiteName,
                                SectionName = dailydata.SectionName,
                                ReportName = dailydata.ReportName,
                                MetricName = dailydata.MetricName,
                                CreatedDate = dailydata.DailyUploadDate.Value.Month.ToString() + '/' + dailydata.DailyUploadDate.Value.Day.ToString() + '/' + dailydata.DailyUploadDate.Value.Year.ToString(),
                                ActualValue = dailydata.ActualValue,
                                Remarks = "Date value is missing."
                                //Remarks = "Year must be between " + (DateTime.Now.Year - 10).ToString() + " and " + (DateTime.Now.Year + 10).ToString()
                            });
                            InvalidCount++;
                            continue;
                        }


                        //if (!string.IsNullOrEmpty(weeklydata.YearString))
                        //{
                        //    try
                        //    {

                        //        weeklydata.Year = Convert.ToInt32(weeklydata.YearString);
                        //        if (((weeklydata.Year < DateTime.Now.Year - 10) || (weeklydata.Year > DateTime.Now.Year + 10)))
                        //        {
                        //            lstInvalid.Add(new InvalidWeeklyData
                        //            {
                        //                SiteName = weeklydata.SiteName,
                        //                SectionName = weeklydata.SectionName,
                        //                ReportName = weeklydata.ReportName,
                        //                MetricName = weeklydata.MetricName,
                        //                Year = weeklydata.YearString,
                        //                Week = weeklydata.WeekString,
                        //                ActualValue = weeklydata.ActualValue,
                        //                Remarks = "Year must be between " + (DateTime.Now.Year - 10).ToString() + " and " + (DateTime.Now.Year + 10).ToString()
                        //            });
                        //            InvalidCount++;
                        //            continue;
                        //        }
                        //    }
                        //    catch
                        //    {
                        //        lstInvalid.Add(new InvalidWeeklyData
                        //        {
                        //            SiteName = weeklydata.SiteName,
                        //            SectionName = weeklydata.SectionName,
                        //            ReportName = weeklydata.ReportName,
                        //            MetricName = weeklydata.MetricName,
                        //            Year = weeklydata.YearString,
                        //            Week = weeklydata.WeekString,
                        //            ActualValue = weeklydata.ActualValue,
                        //            Remarks = "Invalid Year value."
                        //        });
                        //        InvalidCount++;
                        //        continue;
                        //    }
                        //}



                        //if (!string.IsNullOrEmpty(weeklydata.WeekString))
                        //{
                        //    try
                        //    {


                        //        weeklydata.Week = Convert.ToInt32(weeklydata.WeekString);
                        //        if (weeklydata.Week < 0 || weeklydata.Week > 52)
                        //        {
                        //            lstInvalid.Add(new InvalidWeeklyData
                        //            {
                        //                SiteName = weeklydata.SiteName,
                        //                SectionName = weeklydata.SectionName,
                        //                ReportName = weeklydata.ReportName,
                        //                MetricName = weeklydata.MetricName,
                        //                Year = weeklydata.YearString,
                        //                Week = weeklydata.WeekString,
                        //                ActualValue = weeklydata.ActualValue,
                        //                Remarks = "Week must be between 1-52."
                        //            });
                        //            InvalidCount++;
                        //            continue;
                        //        }
                        //    }
                        //    catch
                        //    {
                        //        lstInvalid.Add(new InvalidWeeklyData
                        //        {
                        //            SiteName = weeklydata.SiteName,
                        //            SectionName = weeklydata.SectionName,
                        //            ReportName = weeklydata.ReportName,
                        //            MetricName = weeklydata.MetricName,
                        //            Year = weeklydata.YearString,
                        //            Week = weeklydata.WeekString,
                        //            ActualValue = weeklydata.ActualValue,
                        //            Remarks = "Invalid Week value."
                        //        });
                        //        InvalidCount++;
                        //        continue;
                        //    }
                        //}


                        if (metrics.Any(x => x.Text == dailydata.SectionName + "^" + dailydata.MetricName))
                        {
                            dailydata.SiteMetricId = Convert.ToInt32(metrics.Where(x => x.Text == dailydata.SectionName + "^" + dailydata.MetricName).FirstOrDefault().Value);
                        }
                        else
                        {
                            lstInvalid.Add(new InvalidDailyData
                            {
                                SiteName = dailydata.SiteName,
                                SectionName = dailydata.SectionName,
                                ReportName = dailydata.ReportName,
                                MetricName = dailydata.MetricName,
                                CreatedDate = dailydata.DailyUploadDate.Value.Month.ToString() + '/' + dailydata.DailyUploadDate.Value.Day.ToString() + '/' + dailydata.DailyUploadDate.Value.Year.ToString(),
                                ActualValue = dailydata.ActualValue,
                                Remarks = "Metric is not assigned to any section."
                            });
                            InvalidCount++;
                            continue;
                        }
                        //weeklydata.MetricId = Convert.ToInt32(metrics.Where(x => x.Text == weeklydata.MetricName).FirstOrDefault().Value);
                        //weeklydata.SiteId = Convert.ToInt32(sites.Where(x => x.Text == weeklydata.SiteName).FirstOrDefault().Value);
                        //weeklydata.ReportId = Convert.ToInt32(weeklydata.ReportName);// Convert.ToInt32(reports.Where(x => x.Value == weeklydata.ReportName).FirstOrDefault().Value);


                        if (!string.IsNullOrEmpty(dailydata.Comment))
                        {
                            dailydata.Comment = dailydata.Comment;
                        }
                        dailydata.CreatedBy = ProjectSession.UserID;


                        var model = (from x in _dailyUploadOperationalDataRepository.Table
                                     where x.SiteMetricId == dailydata.SiteMetricId &&
                                     //x.CreatedDate == dailydata.CreatedDate
                                      x.DailyUploadDate.Value.Year == dailydata.DailyUploadDate.Value.Year
                                    && x.DailyUploadDate.Value.Month == dailydata.DailyUploadDate.Value.Month
                                    && x.DailyUploadDate.Value.Day == dailydata.DailyUploadDate.Value.Day
                                     select x).FirstOrDefault();
                        if (model != null)
                        {
                            if (!string.IsNullOrEmpty(dailydata.Comment))
                                model.Comment = dailydata.Comment;
                            var oldActualValue = model.ActualValue;
                            if (model.SiteMetrics.MetricsName == "Mill Throughput")
                            {
                                model.ActualValue = Math.Round(Convert.ToDecimal(millThroughPut), 2);
                            }
                            else
                            {
                                if (dailydata.ActualValue != 0)
                                    model.ActualValue = dailydata.ActualValue;
                            }

                            model.ModifiedBy = ProjectSession.UserID;
                            model.ModifiedDate = DateTime.Now;

                            _dailyUploadOperationalDataRepository.Update(model);

                            if (oldActualValue != model.ActualValue)
                            {
                                UpdateOrInsertToInsightsTable(model.SiteMetricId, model.DailyUploadDate.Value);
                            }
                        }
                        else
                        {
                            dailydata.CreatedDate = DateTime.Now.Date;
                            if (dailydata.MetricName == "Mill Throughput (TPH)")
                            {
                                dailydata.ActualValue = Math.Round(Convert.ToDecimal(millThroughPut), 2);
                            }

                            _dailyUploadOperationalDataRepository.Insert(dailydata);
                            UpdateOrInsertToInsightsTable(dailydata.SiteMetricId, dailydata.DailyUploadDate.Value);

                    }
                }
            }
            return lstInvalid;

        }
        public IList<SelectListItem> GetSitesSelectList(int siteId = 0, int IsSync = -1, bool IsFullRightsRequired = false)
        {
            bool BoolIsSync = false;
            if (IsSync != -1)
                BoolIsSync = Convert.ToBoolean(IsSync);

            List<int> SiteIds = new List<int>();
            if (IsFullRightsRequired)
                SiteIds = ProjectSession.UserSiteRightsDynamic.Where(x => x.IsFullRights == true).Select(x => x.SiteId).ToList();
            else
                SiteIds = ProjectSession.UserSiteRightsDynamic.Where(x => x.IsFullRights == true || x.IsView == true).Select(x => x.SiteId).ToList();



            if (siteId == 0)
            {
                var query = from p in _siteRepository.Table
                            where ((p.IsSync == BoolIsSync || (p.IsSync == null && !BoolIsSync)) || IsSync == -1)
                            && (SiteIds.Contains(p.SiteId) || ProjectSession.IsAdmin == true)
                            orderby p.SiteName ascending
                            select new SelectListItem
                            {
                                Text = p.SiteName,
                                Value = p.SiteId.ToString()
                            };
                return query.ToList();
            }
            else
            {
                var query = from p in _siteRepository.Table
                            where p.SiteId == siteId &&
                            ((p.IsSync == BoolIsSync || (p.IsSync == null && !BoolIsSync)) || IsSync == -1)
                             && (SiteIds.Contains(p.SiteId) || ProjectSession.IsAdmin == true)
                            orderby p.SiteName ascending
                            select new SelectListItem
                            {
                                Text = p.SiteName,
                                Value = p.SiteId.ToString()
                            };
                return query.ToList();
            }
        }
        public IList<SelectListItem> GetMetricsSelectList(int siteId = 0, int ReportId = 0)
        {
            if (siteId > 0)
            {

                var list = new List<SelectListItem>();
                var metrics = (from p in _dbContext.SiteMetrics
                               where p.SiteId == siteId
                               && (p.ReportId == ReportId || ReportId == 0)
                               && p.IsDaily == true
                               select p).OrderBy(a => a.SectionId).ToList();

                if (metrics != null && metrics.Count() > 0)
                {
                    metrics = metrics.AsEnumerable().OrderBy(x => x.DisplayOrder).OrderBy(a => a.SectionId).ToList();

                    foreach (var itm in metrics)
                    {
                        if (ProjectSession.SectionAccessPermissionsDynamic.Any(x => x.SectionId == itm.SectionId))
                        {
                            var listItem = new SelectListItem { Text = itm.Section.SectionName + "^" + itm.Metric.MetricsName + " (" + itm.Metric.Unit.UOM + ")", Value = itm.SiteMetricId.ToString() };

                            list.Add(listItem);
                        }
                    }


                    //var query = from p in _metricsRepository.Table
                    //            where metrics.Contains(p.MetricId)
                    //            orderby p.MetricsName ascending
                    //            select new SelectListItem
                    //            {
                    //                Text = p.MetricsName,
                    //                Value = p.MetricId.ToString()
                    //            };
                    //return query.ToList();
                    return list;
                }
                else
                {
                    return new List<SelectListItem>();
                }

            }
            else
            {
                var query = from p in _metricsRepository.Table
                            orderby p.MetricsName ascending
                            select new SelectListItem
                            {
                                Text = p.MetricsName,
                                Value = p.MetricId.ToString()
                            };
                return query.ToList();
            }
        }
        public IList<SelectListItem> GetReportsSelectList()
        {
            var query = from p in _reportRepository.Table
                        orderby p.Name ascending
                        select new SelectListItem
                        {
                            Text = p.Name,
                            Value = p.ReportId.ToString()
                        };
            return query.ToList();
        }
        public void InsertSheet(UploadSheetData upload)
        {
            _uploadSheetRepository.Insert(upload);
        }

        private void UpdateOrInsertToInsightsTable(int siteMetricId, DateTime uploadDate)
        {
            var endOfMonth = new DateTime(uploadDate.Year, uploadDate.Month, 1).AddMonths(1).AddDays(-1);
            for (DateTime date = uploadDate.Date; date <= endOfMonth; date = date.AddDays(1))
            {
                object[] xparams = {
                    new SqlParameter("SiteMetricId", siteMetricId),
                    new SqlParameter("Date", date),
                };

                _dbContext.ExecuteStoredProcedureList<int>("GEN_UpdateInsightsTable", xparams);
            }
        }
    }
}
