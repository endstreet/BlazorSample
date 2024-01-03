using System;

namespace TrevaliOperationalReport.Domain
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Get or set CreatedDate
        /// </summary>
        public Nullable<DateTime> CreatedDate { get; set; }

        /// <summary>
        /// Get or set CreatedBy
        /// </summary>
        public Nullable<int> CreatedBy { get; set; }

        /// <summary>
        /// Get or set ModifiedDate
        /// </summary>
        public Nullable<DateTime> ModifiedDate { get; set; }

        /// <summary>
        /// Get or set ModifiedBy
        /// </summary>
        public Nullable<int> ModifiedBy { get; set; }
    }
}
