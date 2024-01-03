using TrevaliOperationalReport.Domain.Report;


namespace TrevaliOperationalReport.Service.Report
{
    public interface ISafetyService
    {
        /// <summary>
        /// Search safety data
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="siteId"></param>
        /// <param name="week"></param>
        /// <param name="year"></param>
        Safety SearchSafety(int reportId, int siteId, int? week, int year, int? month);

        /// <summary>
        /// gets safety by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Safety GetSafetyById(int id);

        /// <summary>
        /// updates safety
        /// </summary>
        /// <param name="safetyModel"></param>
        /// <returns></returns>
        int UpdateSafety(Safety safetyModel);

        /// <summary>
        /// gets safety incident by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SafetyIncident GetSafetyIncidentById(int id);

        /// <summary>
        /// updates safety incident
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        int UpdateSafetyIncident(SafetyIncident incident);


        /// <summary>
        /// inserts safety
        /// </summary>
        /// <param name="safety"></param>
        /// <returns></returns>
        int InsertSafety(Safety safety);


        /// <summary>
        /// inserts safety incident
        /// </summary>
        /// <param name="safetyincident"></param>
        /// <returns></returns>
        int InsertSafetyIncident(SafetyIncident safetyincident);
    }
}
