using System.Configuration;

namespace TrevaliOperationalReport.Common
{
    public class ConfigItems
    {
        /// <summary>
        /// get value of connection string name
        /// </summary>
        public static string ConnectionStringName
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionStringName"];
            }
        }

        ///// <summary>
        ///// Gets the Configuration From Email Address
        ///// </summary>
        ///// <value>From email address.</value>
        //public static string FromEmailAddress
        //{
        //    get
        //    {
        //        return ConvertTo.String(ConfigurationManager.AppSettings["FromEmailAddress"]);
        //    }
        //}

        /// <summary>
        /// Gets a value indicating whether [test mode].
        /// </summary>
        /// <value><c>true</c> if [test mode]; otherwise, <c>false</c>.</value>
        public static bool TestMode
        {
            get
            {
                return ConvertTo.Boolean(ConfigurationManager.AppSettings["TestMode"]);
            }
        }

        /// <summary>
        /// Gets the test email address.
        /// </summary>
        /// <value>The test email address.</value>
        public static string TestEmailAddress
        {
            get
            {
                return ConvertTo.String(ConfigurationManager.AppSettings["TestEmailAddress"]);
            }
        }

        public static string DefaultEmailAddress
        {
            get
            {
                return ConvertTo.String(ConfigurationManager.AppSettings["DefaultEmailAddress"]);
            }
        }

        public static string RPZCYearlyBudgetRefresh
        {
            get
            {
                return ConvertTo.String(ConfigurationManager.AppSettings["RPZCYearlyBudgetRefresh"]);
            }
        }

        public static string NANTOUYearlyBudgetRefresh
        {
            get
            {
                return ConvertTo.String(ConfigurationManager.AppSettings["NANTOUYearlyBudgetRefresh"]);
            }
        }

        public static string CARIBOUYearlyBudgetRefresh
        {
            get
            {
                return ConvertTo.String(ConfigurationManager.AppSettings["CARIBOUYearlyBudgetRefresh"]);
            }
        }


        ///// <summary>
        ///// Gets the cookies validity.
        ///// </summary>
        ///// <value>The cookies validity.</value>
        //public static int CookiesValidity
        //{
        //    get
        //    {
        //        return ConvertTo.Integer(ConfigurationManager.AppSettings["CookiesValidity"]);
        //    }
        //}

        //public static string HostURL
        //{
        //    get
        //    {
        //        return ConvertTo.String(ConfigurationManager.AppSettings["HostURL"]);
        //    }
        //}

        ///// <summary>
        ///// Gets the SiteName.
        ///// </summary>
        ///// <value>The SiteName.</value>
        //public static string SiteName
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings.Get("SiteName");
        //    }
        //}

        ///// <summary>
        ///// Gets the size of the site logo.
        ///// </summary>
        ///// <value>The size of the site logo.</value>
        //public static int SiteLogoSize
        //{
        //    get
        //    {
        //        return ConvertTo.Integer(ConfigurationManager.AppSettings.Get("SiteLogoSize"));
        //    }
        //}

        ///// <summary>
        ///// Gets the upload file format.
        ///// </summary>
        ///// <value>The upload file format.</value>
        //public static string UploadFileFormat
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings.Get("UploadFileFormat");
        //    }
        //}
    }
}
