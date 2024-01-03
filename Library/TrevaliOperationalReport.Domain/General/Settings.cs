using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.General
{
    [Table("GEN_Settings", Schema = "dbo")]
    public class Settings
    {
        /// <summary>
        /// Get or set SettingID
        /// </summary>
        [Key]
        public int SettingID { get; set; }

        /// <summary>
        /// Get or set ParentIDSetting
        /// </summary>
        public int? ParentIDSetting { get; set; }

        /// <summary>
        /// Get or set DataTypesId
        /// </summary>
        public int DataTypesId { get; set; }

        /// <summary>
        /// Get or set SettingKey
        /// </summary>
        public string SettingKey { get; set; }

        /// <summary>
        /// Get or set SettingValue
        /// </summary>
        [Required]
        public string SettingValue { get; set; }

        /// <summary>
        /// Get or set DefaultValue
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Get or set MinValue
        /// </summary>
        public string MinValue { get; set; }

        /// <summary>
        /// Get or set MaxValue
        /// </summary>
        public string MaxValue { get; set; }

        /// <summary>
        /// Get or set Comment
        /// </summary>
        public string Comment { get; set; }


        [NotMapped]
        public string ParentKey { get; set; }

    }

}
