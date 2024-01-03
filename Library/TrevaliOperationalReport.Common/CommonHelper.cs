using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrevaliOperationalReport.Domain.Report;

namespace TrevaliOperationalReport.Common
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public class CommonHelper
    {

        #region Public Variables

        public const string DateFormat = "dd/MM/yyyy";
        public const string MonthLongFormat = "MMMM yyyy";
        public const string MonthFormat = "MMM yyyy";
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
        public const string DateFormatChart = "dd MMM yy";
        public const string DateFormatDatemonthChart = "dd";
        public const string TimeFormate = "HH:mm";
        public const int PageSize = 15;

        #endregion

        static object _locker = new object();

        /// <summary>
        /// Generate15s the unique digits.
        /// </summary>
        /// <returns></returns>
        public static long Generate15UniqueDigits()
        {
            lock (_locker)
            {
                Thread.Sleep(100);
                var number = DateTime.Now.ToString("yyyyMMddHHmmssf");
                return Convert.ToInt64(number);
            }
        }

        /// <summary>
        /// Deletes the files and folders recursively.
        /// </summary>
        /// <param name="targetDir">The target_dir.</param>
        public static void DeleteFilesAndFoldersRecursively(string targetDir)
        {
            foreach (string file in Directory.GetFiles(targetDir))
            {
                File.Delete(file);
            }

            foreach (string subDir in Directory.GetDirectories(targetDir))
            {
                DeleteFilesAndFoldersRecursively(subDir);
            }

            Thread.Sleep(1); // This makes the difference between whether it works or not. Sleep(0) is not enough.
            Directory.Delete(targetDir);
        }

        /// <summary>
        /// Ensures the subscriber email or throw.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            string output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!IsValidEmail(output))
            {
                throw new Exception("Email is not valid.");
            }

            return output;
        }

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }

        /// <summary>
        /// Returns an random interger number within a specified rage
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }
            return str;
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            if (str == null)
                return string.Empty;

            return str;
        }

        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            bool result = false;
            Array.ForEach(stringsToValidate, str =>
            {
                if (string.IsNullOrEmpty(str)) result = true;
            });
            return result;
        }

        /// <summary>
        /// Compare two arrasy
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="a1">Array 1</param>
        /// <param name="a2">Array 2</param>
        /// <returns>Result</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            //also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            return !a1.Where((t, i) => !comparer.Equals(t, a2[i])).Any();
        }

        //private static AspNetHostingPermissionLevel? _trustLevel;
        ///// <summary>
        ///// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        ///// </summary>
        ///// <returns>The current trust level.</returns>
        //public static AspNetHostingPermissionLevel GetTrustLevel()
        //{
        //    if (!_trustLevel.HasValue)
        //    {
        //        //set minimum
        //        _trustLevel = AspNetHostingPermissionLevel.None;

        //        //determine maximum
        //        foreach (AspNetHostingPermissionLevel trustLevel in
        //                new[] {
        //                        AspNetHostingPermissionLevel.Unrestricted,
        //                        AspNetHostingPermissionLevel.High,
        //                        AspNetHostingPermissionLevel.Medium,
        //                        AspNetHostingPermissionLevel.Low,
        //                        AspNetHostingPermissionLevel.Minimal
        //                    })
        //        {
        //            try
        //            {
        //                new AspNetHostingPermission(trustLevel).Demand();
        //                _trustLevel = trustLevel;
        //                break; //we've set the highest permission we can
        //            }
        //            catch (System.Security.SecurityException)
        //            {
        //            }
        //        }
        //    }
        //    return _trustLevel.Value;
        //}

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            string result = string.Empty;
            char[] letters = str.ToCharArray();
            foreach (char c in letters)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c;
                else
                    result += c.ToString();
            return result;
        }


        /// <summary>
        /// Sets alert message and displays on page.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="IsConfirmation"></param>
        /// <param name="YesResponseMethod"></param>
        /// <param name="NoResponseMethod"></param>
        /// <returns></returns>
        public static string ShowAlertMessageToastr(string type, string message, bool IsConfirmation = false, string YesResponseMethod = "", string NoResponseMethod = "")
        {
            message = message.Replace("'", " ");
            var strString = @"<script type='text/javascript' language='javascript'>$(function() { ShowMessageToastr('" + type + "','" + message + "','" + IsConfirmation.ToString().ToLower() + "','" + YesResponseMethod + "','" + NoResponseMethod + "') ; })</script>";
            return strString;
        }

        public static string GetNameWithspace(string name)
        {
            try
            {
                string name2 = "";
                int nameCount = name.Count();
                for (int i = 0; i < nameCount; i++)
                {
                    char car = name[i];
                    if (char.IsUpper(car))
                    {
                        if ((i + 1) == nameCount)
                        {
                            if (!char.IsUpper(name[i - 1]))
                                name2 = name2 + " " + car;
                            else
                                name2 = name2 + car;
                        }
                        else if (((i + 1) < nameCount && char.IsUpper(name[i + 1])) && ((i - 1) >= 0 && !char.IsUpper(name[i - 1])))
                        {
                            name2 = name2 + " " + car;
                        }
                        else if (((i + 1) < nameCount && char.IsUpper(name[i + 1])) && ((i - 1) >= 0 && char.IsUpper(name[i - 1])))
                        {
                            name2 = name2 + car;
                        }
                        else if (((i + 1) < nameCount && !char.IsUpper(name[i + 1])) && ((i - 1) >= 0 && char.IsUpper(name[i - 1])))
                        {
                            name2 = name2 + car;
                        }
                        else
                        {
                            name2 = name2 + " " + car;
                        }
                    }
                    else
                        name2 = name2 + car;
                }

                name2 = name2.Replace("_", " ");

                if (name == "RECOOrders") name = "RECO Orders";
                return name2.Trim();
            }
            catch (Exception)
            {
                return name;
            }

        }

        public static int GetMonth(int Year, int Week)
        {
            DateTime tDt = new DateTime(Year, 1, 1);

            tDt.AddDays((Week - 1) * 7);

            for (int i = 0; i <= 365; ++i)
            {
                int tWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                    tDt,
                    CalendarWeekRule.FirstDay,
                    DayOfWeek.Monday);
                if (tWeek == Week)
                    return tDt.Month;

                tDt = tDt.AddDays(1);
            }
            return 0;
        }

        public static int WeeksInMonth(int month, int year)
        {
            int mondays = 0;
            int daysThisMonth = DateTime.DaysInMonth(year, month);
            DateTime beginingOfThisMonth = new DateTime(year, month, 1);
            for (int i = 0; i < daysThisMonth; i++)
                if (beginingOfThisMonth.AddDays(i).DayOfWeek == DayOfWeek.Monday)
                    mondays++;
            return mondays;
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }

        public static decimal CalculateBudgetWeeklyFromMonthly(DateTime FirstDayOfWeek, DateTime LastDayOfWeek, decimal FirstMonthBudget, decimal SecondMonthBudget, bool IsPercentage = false)
        {
            if (IsPercentage)
            {
                if (FirstDayOfWeek.Month == LastDayOfWeek.Month)
                {
                    return FirstMonthBudget;
                }
                else
                {
                    if (LastDayOfWeek.Day >= 4)
                    {
                        return SecondMonthBudget;
                    }
                    else
                    {
                        return FirstMonthBudget;
                    }
                }
            }
            else
            {

                if (FirstDayOfWeek.Month == LastDayOfWeek.Month)
                {
                    return (FirstMonthBudget / DateTime.DaysInMonth(FirstDayOfWeek.Year, FirstDayOfWeek.Month) * 7);
                }
                else
                {
                    int FirstMonthDays = (DateTime.DaysInMonth(FirstDayOfWeek.Year, FirstDayOfWeek.Month) - FirstDayOfWeek.Day) + 1;
                    int SecondMonthDays = LastDayOfWeek.Day;//(DateTime.DaysInMonth(LastDayOfWeek.Year, LastDayOfWeek.Month) - LastDayOfWeek.Day);

                    return ((FirstMonthBudget / DateTime.DaysInMonth(FirstDayOfWeek.Year, FirstDayOfWeek.Month) * FirstMonthDays) + (SecondMonthBudget / DateTime.DaysInMonth(LastDayOfWeek.Year, LastDayOfWeek.Month) * SecondMonthDays));
                }
            }
        }
        public static decimal CalculateMTDVal(List<WeeklyOperationalData> lstWeeklyData, WeeklyOperationalData currentWeek)
        {
            decimal MTDVal = 0;
            var FirstDay = CommonHelper.FirstDateOfWeek(currentWeek.Year, currentWeek.Week);
            var LastDay = FirstDay.AddDays(6);
            if (FirstDay.Month == LastDay.Month)
            {
                List<WeeklyOperationalData> ActualWeekly = new List<WeeklyOperationalData>();
                foreach (WeeklyOperationalData item in lstWeeklyData)
                {
                    var FDay = CommonHelper.FirstDateOfWeek(item.Year, item.Week);
                    if (FDay.Month == FirstDay.Month || FDay.AddDays(6).Month == FirstDay.Month)
                    {
                        ActualWeekly.Add(item);
                    }
                }
                if (ActualWeekly.Count > 0)
                {
                    //MTDVal = (decimal)ActualWeekly.Sum(x => x.ActualValue);
                    foreach (WeeklyOperationalData item in ActualWeekly)
                    {
                        if (FirstDateOfWeek(item.Year, item.Week).Month != FirstDateOfWeek(item.Year, item.Week).AddDays(6).Month)
                        {
                            MTDVal += (decimal)((item.ActualValue / 7) * FirstDateOfWeek(item.Year, item.Week).AddDays(6).Day);
                        }
                        else
                        {
                            MTDVal += (decimal)item.ActualValue;
                        }
                    }
                }

            }

            return MTDVal;


        }

        public static decimal SetRounding(decimal amount, string UOM = "", bool NoRounding = false)
        {
            return decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
            //if (NoRounding)
            //    return decimal.Round(amount, 2, MidpointRounding.AwayFromZero);

            //if (UOM == "%" || UOM.ToLower().Contains("troy oz"))
            //{
            //    return decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
            //}
            //else {
            //    return decimal.Round(amount, 0, MidpointRounding.AwayFromZero);
            //}
        }

        public static List<SelectListItem> GetYearsList()
        {
            List<SelectListItem> YearsList = new List<SelectListItem>();
            for (int i = 9; i >= -21; i--)
            {
                bool IsSelected = false;
                if (GetWeekNumber() > 52 && DateTime.Now.Year + 1 == (DateTime.Now.Year - i))
                {
                    IsSelected = true;
                }
                else if (DateTime.Now.Year == (DateTime.Now.Year - i))
                {
                    IsSelected = true;
                }
                YearsList.Add(new SelectListItem
                {

                    Text = (DateTime.Now.Year - i).ToString(),
                    Value = (DateTime.Now.Year - i).ToString(),
                    //Selected = (DateTime.Now.Year == (DateTime.Now.Year - i))
                    Selected = IsSelected
                });
            }
            return YearsList;
        }

        public static List<T> GetYearsList<T>()
        {
            List<SelectListItem> YearsList = new List<SelectListItem>();
            for (int i = 9; i >= -21; i--)
            {
                bool IsSelected = false;
                if (GetWeekNumber() > 52 && DateTime.Now.Year + 1 == (DateTime.Now.Year - i))
                {
                    IsSelected = true;
                }
                else if (DateTime.Now.Year == (DateTime.Now.Year - i))
                {
                    IsSelected = true;
                }
                YearsList.Add(new SelectListItem
                {

                    Text = (DateTime.Now.Year - i).ToString(),
                    Value = (DateTime.Now.Year - i).ToString(),
                    //Selected = (DateTime.Now.Year == (DateTime.Now.Year - i))
                    Selected = IsSelected
                });
            }
            return YearsList as List<T>;
        }
        public static int GetWeekNumber()
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        public static List<SelectListItem> GetMonthsList()
        {
            List<SelectListItem> MonthsList = new List<SelectListItem>();
            for (int j = 0; j < 12; j++)
            {
                MonthsList.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(Months), (j + 1)),
                    Value = (j + 1).ToString(),
                    Selected = (DateTime.Now.Month == (j + 1))
                });
            }
            return MonthsList;
        }

        public static List<T> GetMonthsList<T>()
        {
            List<SelectListItem> MonthsList = new List<SelectListItem>();
            for (int j = 0; j < 12; j++)
            {
                MonthsList.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(Months), (j + 1)),
                    Value = (j + 1).ToString(),
                    Selected = (DateTime.Now.Month == (j + 1))
                });
            }
            return MonthsList as List<T>;
        }

        public static List<SelectListItem> GetWeekList(int? year = null)
        {
            List<SelectListItem> WeekList = new List<SelectListItem>();
            for (int j = 0; j < (year.HasValue ?
                CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(new DateTime((int)year, 12, 31),
            CultureInfo.InvariantCulture.DateTimeFormat.CalendarWeekRule,
            CultureInfo.InvariantCulture.DateTimeFormat.FirstDayOfWeek) : 53); j++)
            {
                WeekList.Add(new SelectListItem
                {
                    Text = (j + 1).ToString(),
                    Value = (j + 1).ToString()
                });
            }
            return WeekList;
        }

        public static List<T> GetWeekList<T>(int? year = null)
        {
            List<SelectListItem> WeekList = new List<SelectListItem>();
            for (int j = 0; j < (year.HasValue ?
                CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(new DateTime((int)year, 12, 31),
            CultureInfo.InvariantCulture.DateTimeFormat.CalendarWeekRule,
            CultureInfo.InvariantCulture.DateTimeFormat.FirstDayOfWeek) : 53); j++)
            {
                WeekList.Add(new SelectListItem
                {
                    Text = (j + 1).ToString(),
                    Value = (j + 1).ToString()
                });
            }
            return WeekList as List<T>;
        }

        public enum Months
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12

        }

        private static List<int> _leapYears = new List<int>();
        public static List<int> LeapYears()
        {
            if(_leapYears.Count == 0)
            {
                for (int year = 1900; year <= 2100; year++)
                {
                    CultureInfo ci = CultureInfo.InvariantCulture;
                    DateTimeFormatInfo dtfi = ci.DateTimeFormat;
                    Calendar cal = ci.Calendar;

                    int weeks = cal.GetWeekOfYear(new DateTime(year, 12, 31), dtfi.CalendarWeekRule, dtfi.FirstDayOfWeek);

                    if (weeks == 53)
                    {
                        _leapYears.Add(year);
                    }
                }
            }
            return _leapYears;

        }
        public static string PowerBI_AutoLogin(string username, string password, string clientid, string API, string clientsecret, string loginUrl)
        {
            string response = "";

            //using (HttpClient httpClient = new HttpClient())
            //{
            //    HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, loginUrl);

            //    var values = new List<KeyValuePair<string, string>>();
            //    values.Add(new KeyValuePair<string, string>("grant_type", "password"));
            //    values.Add(new KeyValuePair<string, string>("scope", "openid"));
            //    values.Add(new KeyValuePair<string, string>("resource", API));
            //    values.Add(new KeyValuePair<string, string>("client_id", clientid));
            //    values.Add(new KeyValuePair<string, string>("username", username));
            //    values.Add(new KeyValuePair<string, string>("password", password));
            //    values.Add(new KeyValuePair<string, string>("client_secret", clientsecret));

            //    var content = new FormUrlEncodedContent(values);

            //    var resp = await httpClient.PostAsync("https://login.microsoftonline.com/common/oauth2/token", content);

            //    response = await resp.Content.ReadAsStringAsync();

            //}
            return response;
        }


        public static string GetForecastMonthSequence(int month, int year)
        {
            if (month > 12 && month <= 24)
            {
                month = month - 12;
                year = year + 1;
            }
            if (month > 24)
            {
                month = month - 24;
                year = year + 2;
            }

            string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string[] OutMonths = new string[12];

            for (int i = 0; i < 12; i++)
            {
                OutMonths[i] = months[i] + "-" + year;
                month = month + 1;
                if (month > 12)
                {
                    month = month - 12;
                }
            }
            return OutMonths[month - 1];

        }

        public static string GetNextForecastYear(int year)
        {
            return ("Year-" + year);
        }

        public static string GetMonthDaysSequence(int day, string month)
        {
            string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string[] days = new string[31];

            for (int i = 1; i <= 31; i++)
            {
                days[i - 1] = string.Format("{0} {1}", i, months[Int32.Parse(month) - 1]);
            }
            return days[day - 1];

        }
    }
}
