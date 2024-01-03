using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_Site", Schema = "dbo")]
    public class Site : BaseEntity
    {
        /// <summary>
        /// Get or set SiteId
        /// </summary>
        [Key]
        public int SiteId { get; set; }

        /// <summary>
        /// Get or set SiteName
        /// </summary>
        [Required]
        public string SiteName { get; set; }

        /// <summary>
        /// Get or set SiteMappingName
        /// </summary>
        public string SiteMappingName { get; set; }

        /// <summary>
        /// Get or set Logo
        /// </summary>
        public byte[] Logo { get; set; }

        /// <summary>
        /// Get or set IsSync
        /// </summary>
        public bool? IsSync { get; set; }

        [NotMapped]
        public string LogoImage => Logo != null ? Convert.ToBase64String(Logo) : null;

        [NotMapped]

        public SiteParameters SiteParams { get; set; }
    }
}
