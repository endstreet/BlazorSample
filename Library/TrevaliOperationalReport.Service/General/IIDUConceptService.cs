namespace TrevaliOperationalReport.Service.General
{
    interface IIDUConceptService
    {

        /// <summary>
        /// Get RPZC Update Records.
        /// </summary>
        /// <returns></returns>
        int GetRPZCRecords();

        /// <summary>
        /// Get NANTOU Update Records.
        /// </summary>
        /// <returns></returns>
        int GetNANTOURecords();

        /// <summary>
        /// Get CARIBOU Update Records.
        /// </summary>
        /// <returns></returns>
        int GetCARIBOURecords();
    }
}
