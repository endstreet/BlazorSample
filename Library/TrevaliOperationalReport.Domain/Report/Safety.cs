using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.Report
{
    [Table("RPT_Safety", Schema = "dbo")]
    public class Safety : BaseEntity
    {
        /// <summary>
        /// Get or set SafetyId
        /// </summary>
        [Key]
        public int SafetyId { get; set; }

        /// <summary>
        /// Get or set SiteId
        /// </summary>
        [Required(ErrorMessage = "*")]
        public int SiteId { get; set; }

        /// <summary>
        /// Get or set ReportId
        /// </summary>
        [Required(ErrorMessage = "*")]
        public int ReportId { get; set; }

        /// <summary>
        /// Get or set Week
        /// </summary>
        //[Required(ErrorMessage = "*")]
        public int? Week { get; set; }

        /// <summary>
        /// Get or set Month
        /// </summary>
        //[Required(ErrorMessage = "*")]
        public int? Month { get; set; }

        /// <summary>
        /// Get or set Year
        /// </summary>
        [Required(ErrorMessage = "*")]
        public int Year { get; set; }

        /// <summary>
        /// Get or set Actual
        /// </summary>
        [Required(ErrorMessage = "*")]
        public int Actual { get; set; }

        /// <summary>
        /// Get or set Target
        /// </summary>
        [Required(ErrorMessage = "*")]
        public int Target { get; set; }

        /// <summary>
        /// Get or set Rating
        /// </summary>
        public int? Rating { get; set; }

        /// <summary>
        /// Get or set InitiativesSummary
        /// </summary>
        public string InitiativesSummary { get; set; }

        /// <summary>
        /// Get or set IncidentSummary
        /// </summary>
        public string IncidentSummary { get; set; }

        /// <summary>
        /// Get or sets the sites 
        /// </summary>
        [ForeignKey("SiteId")]
        public virtual General.Site Site { get; set; }

        /// <summary>
        /// Get or sets the reports 
        /// </summary>
        [ForeignKey("ReportId")]
        public virtual General.Reports Report { get; set; }

        [NotMapped]
        public SafetyIncident WeeklyIncidentSummary { get; set; }

        //[NotMapped]
        //public SafetyIncident MTDIncidentSummary { get; set; }

        //[NotMapped]
        //public SafetyIncident YTDIncidentSummary { get; set; }
    }


    [Table("RPT_SafetyIncident", Schema = "dbo")]
    public class SafetyIncident : BaseEntity
    {
        /// <summary>
        /// Get or set SafetyIncidentId
        /// </summary>
        [Key]
        public int SafetyIncidentId { get; set; }

        /// <summary>
        /// Get or set SafetyId
        /// </summary>
        public int SafetyId { get; set; }

        /// <summary>
        /// Get or set SafetyIncidentTypeId
        /// </summary>
        public int SafetyIncidentTypeId { get; set; }

        /// <summary>
        /// Get or set FirstAidInjury
        /// </summary>
        public int FirstAidInjury { get; set; }

        /// <summary>
        /// Get or set RestrictedWorkInjury
        /// </summary>
        public int RestrictedWorkInjury { get; set; }

        /// <summary>
        /// Get or set LostTimeInjury
        /// </summary>
        public int LostTimeInjury { get; set; }

        /// <summary>
        /// Get or set NearHit
        /// </summary>
        public int NearHit { get; set; }

        /// <summary>
        /// Get or set EquipmentDamage
        /// </summary>
        public int EquipmentDamage { get; set; }

        /// <summary>
        /// Get or set BusinessImpact
        /// </summary>
        public int BusinessImpact { get; set; }

        /// <summary>
        /// Get or set EnviroIncident
        /// </summary>
        public int EnviroIncident { get; set; }

        /// <summary>
        /// Get or set MedicalCases
        /// </summary>
        public int MedicalCases { get; set; }

    }


    [Table("RPT_SafetyIncidentType", Schema = "dbo")]
    public class SafetyIncidentType
    {
        /// <summary>
        /// Get or set SafetyIncidentTypeId
        /// </summary>
        [Key]
        public int SafetyIncidentTypeId { get; set; }

        /// <summary>
        /// Get or set SafetyIncidentType
        /// </summary>
        public string SafetyIncidentTypeName { get; set; }
    }


    public partial class EnumHelp
    {
        public enum IncidentType
        {
            WeeklyIncidentSummary = 1
            //MTDIncidentSummary = 2,
            //YTDSummary = 3
        }
    }


}
