using System.Collections.Generic;
using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service
{
    public class GeneralService
    {
        #region Methods
        public string GetLatestProjectId()
        {
            var Id = 0;
            var query = new List<Project>();
            using (TrevaliOperationalReportObjectContext objContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
            {
                query = (from p in objContext.Project
                         select p).OrderByDescending(m => m.ProjectId).ToList();
            }
            Project model = query.FirstOrDefault();
            if (model == null)
            {
                Id = 1;
            }
            else
            {
                Id = model.ProjectId + 1;
            }
            return (Id.ToString().PadLeft(4, '0'));
        }
        #endregion
    }
}
