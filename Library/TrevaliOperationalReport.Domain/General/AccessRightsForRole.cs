using System;

namespace TrevaliOperationalReport.Domain.General
{
    public class AccessRightsForRole
    {
        public int MenuAccessRightId { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string AccessRight { get; set; }
        public Nullable<int> RoleMenuAccessRightId { get; set; }
        public Nullable<int> RoleId { get; set; }
    }
}
