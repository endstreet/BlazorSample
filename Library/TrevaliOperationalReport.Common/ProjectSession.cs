using System.Collections.Generic;
using System.Web;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Common
{
    public class ProjectSession
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public static int UserID
        {
            get
            {
                if (HttpContext.Current.Session["UserID"] == null)
                {
                    return 0;
                }
                else
                {
                    return ConvertTo.Integer(HttpContext.Current.Session["UserID"]);
                }
            }

            set
            {
                HttpContext.Current.Session["UserID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        public static string Email
        {
            get
            {
                if (HttpContext.Current.Session["Email"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return HttpContext.Current.Session["Email"].ToString();
                }
            }
            set
            {
                HttpContext.Current.Session["Email"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the isAdmin.
        /// </summary>
        /// <value>The isSuperAdmin.</value>
        public static bool IsAdmin
        {
            get
            {
                if (HttpContext.Current.Session["IsAdmin"] == null)
                {
                    return false;
                }
                else
                {
                    return ConvertTo.Boolean(HttpContext.Current.Session["IsAdmin"]);
                }
            }

            set
            {
                HttpContext.Current.Session["IsAdmin"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session["UserName"] == null)
                {
                    return "User";
                }
                else
                {
                    return ConvertTo.String(HttpContext.Current.Session["UserName"]);
                }
            }

            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }
        public static List<Section> SectionAccessPermissionsDynamic
        {
            get
            {
                return HttpContext.Current.Session["SectionAccessPermissionsDynamic"] == null ? new List<Section>() : HttpContext.Current.Session["SectionAccessPermissionsDynamic"] as List<Section>;
            }

            set
            {
                HttpContext.Current.Session["SectionAccessPermissionsDynamic"] = value;
            }
        }


        public static Dictionary<string, object> SaveSearchFilters
        {
            get
            {
                return HttpContext.Current.Session["StrSaveSearchFilters"] == null
                           ? new Dictionary<string, object>()
                           : (Dictionary<string, object>)HttpContext.Current.Session["StrSaveSearchFilters"];
            }
            set
            {
                HttpContext.Current.Session["StrSaveSearchFilters"] = value;
            }
        }

        public static List<UserSiteRights> UserSiteRightsDynamic
        {
            get
            {
                return HttpContext.Current.Session["UserSiteRightsDynamic"] == null ? new List<UserSiteRights>() : HttpContext.Current.Session["UserSiteRightsDynamic"] as List<UserSiteRights>;
            }
            set
            {
                HttpContext.Current.Session["UserSiteRightsDynamic"] = value;
            }
        }

        public static List<UserSiteRights> UserCompanyRightsDynamic
        {
            get
            {
                return HttpContext.Current.Session["UserCompanyRightsDynamic"] == null ? new List<UserSiteRights>() : HttpContext.Current.Session["UserCompanyRightsDynamic"] as List<UserSiteRights>;
            }
            set
            {
                HttpContext.Current.Session["UserCompanyRightsDynamic"] = value;
            }
        }

        public static bool IsUploadBudgetAllowed
        {
            get
            {
                if (HttpContext.Current.Session["IsUploadBudgetAllowed"] == null)
                {
                    return false;
                }
                else
                {
                    return ConvertTo.Boolean(HttpContext.Current.Session["IsUploadBudgetAllowed"]);
                }
            }

            set
            {
                HttpContext.Current.Session["IsUploadBudgetAllowed"] = value;
            }
        }

        public static bool IsEditBudgetAllowed
        {
            get
            {
                if (HttpContext.Current.Session["IsEditBudgetAllowed"] == null)
                {
                    return false;
                }
                else
                {
                    return ConvertTo.Boolean(HttpContext.Current.Session["IsEditBudgetAllowed"]);
                }
            }

            set
            {
                HttpContext.Current.Session["IsEditBudgetAllowed"] = value;
            }
        }
        public static bool IsMonthlyForecastApproveAllowed
        {
            get
            {
                if (HttpContext.Current.Session["IsMonthlyForecastApproveAllowed"] == null)
                {
                    return false;
                }
                else
                {
                    return ConvertTo.Boolean(HttpContext.Current.Session["IsMonthlyForecastApproveAllowed"]);
                }
            }

            set
            {
                HttpContext.Current.Session["IsMonthlyForecastApproveAllowed"] = value;
            }
        }


        public static bool IsMonthlyReconApproveAllowed
        {
            get
            {
                if (HttpContext.Current.Session["IsMonthlyReconApproveAllowed"] == null)
                {
                    return false;
                }
                else
                {
                    return ConvertTo.Boolean(HttpContext.Current.Session["IsMonthlyReconApproveAllowed"]);
                }
            }

            set
            {
                HttpContext.Current.Session["IsMonthlyReconApproveAllowed"] = value;
            }
        }
        ///// <summary>
        ///// Gets or sets the UserRolePermissions.
        ///// </summary>
        ///// <value>List of Permissions which are applicable for logged in User.</value>
        //public static PermissionAccess UserRolePermissions
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UserRolePermissions"] == null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return (PermissionAccess)HttpContext.Current.Session["UserRolePermissions"];
        //        }
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UserRolePermissions"] = value;
        //    }
        //}
    }
}
