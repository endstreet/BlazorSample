using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrevaliOperationalReport.Domain.Projects
{
    [Table("PRJ_Documents", Schema = "dbo")]
    public class Documents
    {
        /// <summary>
		/// Gets or sets the DocumentId value.
		/// </summary>
		[Key]
        public int DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId value.
        /// </summary>
        [Required]
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the TaskId value.
        /// </summary>
        public int? TaskId { get; set; }

        /// <summary>
        /// Gets or sets the DocumentName value.
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the Document value.
        /// </summary>
        [Required]
        public byte[] Document { get; set; }
    }

    public class DownloadDocument
    {
        public int DocumentId { get; set; }
        public int ProjectId { get; set; }
        public string DocumentName { get; set; }
    }
}
