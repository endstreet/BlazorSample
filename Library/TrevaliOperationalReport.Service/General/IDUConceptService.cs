using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data.Repository;

namespace TrevaliOperationalReport.Service.General
{
    public class IDUConceptService : IIDUConceptService
    {

        #region Fields

        private readonly IRepository<string> _IDUConceptServiceRepository;
        private readonly TrevaliOperationalReport.Data.TrevaliOperationalReportObjectContext _dbContext;

        #endregion

        #region Ctor
        public IDUConceptService(IRepository<string> IDUConceptServiceRepository)
        {
            _IDUConceptServiceRepository = IDUConceptServiceRepository;
            _dbContext = new TrevaliOperationalReport.Data.TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);
        }
        #endregion

        #region Methods

        public int GetRPZCRecords()
        {
            var lst = _IDUConceptServiceRepository.ExecuteStoredProcedureList<int>(ConfigItems.RPZCYearlyBudgetRefresh).ToList()[0];
            return lst;
        }

        public int GetNANTOURecords()
        {
            var lst = _IDUConceptServiceRepository.ExecuteStoredProcedureList<int>(ConfigItems.NANTOUYearlyBudgetRefresh).ToList()[0];
            return lst;
        }

        public int GetCARIBOURecords()
        {
            var lst = _IDUConceptServiceRepository.ExecuteStoredProcedureList<int>(ConfigItems.CARIBOUYearlyBudgetRefresh).ToList()[0];
            return lst;
        }
        #endregion
    }
}
