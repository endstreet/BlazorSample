using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_Menus", Schema = "dbo")]
    public class Menus
    {
        /// <summary>
        /// Get or set MenuId
        /// </summary>
        [Key]
        public int MenuId { get; set; }

        /// <summary>
        /// Get or set ParentMenuId
        /// </summary>
        public int? ParentMenuId { get; set; }

        /// <summary>
        /// Get or set Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set ImagePath
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Get or set Controller
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Get or set Action
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Get or set IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or set DispalyOrder
        /// </summary>
        public int? DispalyOrder { get; set; }

        /// <summary>
        /// Get or set Area
        /// </summary>
        public string Area { get; set; }
    }

    public partial class GEN_UserAccessPermissions_Result
    {
        public int MenuId { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentMenuId { get; set; }
        public Nullable<int> DispalyOrder { get; set; }
        public string AccessRight { get; set; }
        public string Area { get; set; }
    }

}
