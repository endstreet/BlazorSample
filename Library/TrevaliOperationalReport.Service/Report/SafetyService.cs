using System;
using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.Report;

namespace TrevaliOperationalReport.Service.Report
{
    public class SafetyService : ISafetyService
    {
        #region Fields

        private readonly IRepository<Safety> _safetyRepository;
        private readonly IRepository<SafetyIncident> _safetyIncidentRepository;

        #endregion

        #region Ctor

        public SafetyService(IRepository<Safety> safetyRepository, IRepository<SafetyIncident> safetyIncidentRepository)
        {
            _safetyRepository = safetyRepository;
            _safetyIncidentRepository = safetyIncidentRepository;
        }

        #endregion


        /// <summary>
        /// Search safety data
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="siteId"></param>
        /// <param name="week"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public Safety SearchSafety(int reportId, int siteId, int? week, int year, int? month)
        {
            var query = from p in _safetyRepository.Table
                        where (p.SiteId == siteId) &&
                        (p.ReportId == reportId) &&
                        (p.Week == week) &&
                         (p.Month == month) &&
                        (p.Year == year)
                        select p;
            var safety = query.ToList().FirstOrDefault();

            if (safety != null)
            {
                GetSafetyIncidentBySafety(safety);
            }
            return safety;
        }


        /// <summary>
        /// Gets the safety by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Safety GetSafetyById(int id)
        {
            var safety = _safetyRepository.GetById(id);

            if (safety != null)
            {
                GetSafetyIncidentBySafety(safety);
            }

            return safety;
        }

        /// <summary>
        /// Updates safety 
        /// </summary>
        /// <param name="safetyModel"></param>
        /// <returns></returns>
        public int UpdateSafety(Safety safetyModel)
        {
            if (safetyModel == null)
                throw new ArgumentNullException("Safety");
            if (checkExistingRecord(safetyModel))
            {
                return -1;
            }
            var model = GetSafetyById(safetyModel.SafetyId);
            if (model != null)
            {
                model.SiteId = safetyModel.SiteId;
                model.ReportId = safetyModel.ReportId;
                model.Week = safetyModel.Week;
                model.Year = safetyModel.Year;
                model.Actual = safetyModel.Actual;
                model.Target = safetyModel.Target;
                model.Rating = safetyModel.Rating;
                model.InitiativesSummary = safetyModel.InitiativesSummary;
                model.IncidentSummary = safetyModel.IncidentSummary;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
            }
            _safetyRepository.Update(model);

            if (safetyModel.WeeklyIncidentSummary != null)
                UpdateSafetyIncident(safetyModel.WeeklyIncidentSummary);
            //if (safetyModel.MTDIncidentSummary != null)
            //    UpdateSafetyIncident(safetyModel.MTDIncidentSummary);
            //if (safetyModel.YTDIncidentSummary != null)
            //    UpdateSafetyIncident(safetyModel.YTDIncidentSummary);

            return safetyModel.SafetyId;
        }



        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingRecord(Safety safetyModel)
        {
            try
            {
                var query = from p in _safetyRepository.Table
                            where p.ReportId == safetyModel.ReportId &&
                            p.SiteId == safetyModel.SiteId &&
                            p.Week == safetyModel.Week &&
                            p.Month == safetyModel.Month &&
                            p.Year == safetyModel.Year &&
                            p.SafetyId != safetyModel.SafetyId
                            select p;
                if (query.ToList().Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inserts the safety
        /// </summary>
        /// <param name="safety"></param>
        /// <returns></returns>
        public int InsertSafety(Safety safety)
        {
            if (safety == null)
                throw new ArgumentNullException("safety");
            if (checkExistingRecord(safety))
            {
                return -1;
            }


            safety.CreatedBy = ProjectSession.UserID;
            safety.CreatedDate = DateTime.Now;
            _safetyRepository.Insert(safety);


            if (safety.WeeklyIncidentSummary != null)
            {
                safety.WeeklyIncidentSummary.SafetyId = safety.SafetyId;
                safety.WeeklyIncidentSummary.SafetyIncidentTypeId = Convert.ToInt32(EnumHelp.IncidentType.WeeklyIncidentSummary);
                InsertSafetyIncident(safety.WeeklyIncidentSummary);
            }
            //if (safety.MTDIncidentSummary != null)
            //{
            //    safety.MTDIncidentSummary.SafetyId = safety.SafetyId;
            //    safety.MTDIncidentSummary.SafetyIncidentTypeId = Convert.ToInt32(EnumHelp.IncidentType.MTDIncidentSummary);
            //    InsertSafetyIncident(safety.MTDIncidentSummary);
            //}
            //if (safety.YTDIncidentSummary != null)
            //{
            //    safety.YTDIncidentSummary.SafetyId = safety.SafetyId;
            //    safety.YTDIncidentSummary.SafetyIncidentTypeId = Convert.ToInt32(EnumHelp.IncidentType.YTDSummary);
            //    InsertSafetyIncident(safety.YTDIncidentSummary);
            //}

            return safety.SafetyId;
        }


        #region Safety Incident

        /// <summary>
        /// Gets the safety incident by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public SafetyIncident GetSafetyIncidentById(int id)
        {

            return _safetyIncidentRepository.GetById(id);
        }

        private Safety GetSafetyIncidentBySafety(Safety safety)
        {
            var safetyincident = (from p in _safetyIncidentRepository.Table
                                  where p.SafetyId == safety.SafetyId
                                  select p).ToList();

            safety.WeeklyIncidentSummary = new SafetyIncident();
            //safety.MTDIncidentSummary = new SafetyIncident();
            //safety.YTDIncidentSummary = new SafetyIncident();


            if (safetyincident != null && safetyincident.Count > 0)
            {
                if (safetyincident.Any(x => x.SafetyIncidentTypeId == Convert.ToInt32(EnumHelp.IncidentType.WeeklyIncidentSummary)))
                {
                    safety.WeeklyIncidentSummary = safetyincident.Where(x => x.SafetyIncidentTypeId == Convert.ToInt32(EnumHelp.IncidentType.WeeklyIncidentSummary)).FirstOrDefault();
                }
                //if (safetyincident.Any(x => x.SafetyIncidentTypeId == Convert.ToInt32(EnumHelp.IncidentType.MTDIncidentSummary)))
                //{
                //    safety.MTDIncidentSummary = safetyincident.Where(x => x.SafetyIncidentTypeId == Convert.ToInt32(EnumHelp.IncidentType.MTDIncidentSummary)).FirstOrDefault();
                //}
                //if (safetyincident.Any(x => x.SafetyIncidentTypeId == Convert.ToInt32(EnumHelp.IncidentType.YTDSummary)))
                //{
                //    safety.YTDIncidentSummary = safetyincident.Where(x => x.SafetyIncidentTypeId == Convert.ToInt32(EnumHelp.IncidentType.YTDSummary)).FirstOrDefault();
                //}
            }

            return safety;
        }

        public int UpdateSafetyIncident(SafetyIncident incident)
        {
            if (incident == null)
                throw new ArgumentNullException("incident");
            if (checkExistingIncident(incident))
            {
                return -1;
            }
            var model = GetSafetyIncidentById(incident.SafetyIncidentId);
            if (model != null)
            {
                model.SafetyId = incident.SafetyId;
                model.SafetyIncidentTypeId = incident.SafetyIncidentTypeId;
                model.FirstAidInjury = incident.FirstAidInjury;
                model.RestrictedWorkInjury = incident.RestrictedWorkInjury;
                model.LostTimeInjury = incident.LostTimeInjury;
                model.NearHit = incident.NearHit;
                model.EquipmentDamage = incident.EquipmentDamage;
                model.BusinessImpact = incident.BusinessImpact;
                model.EnviroIncident = incident.EnviroIncident;
                model.MedicalCases = incident.MedicalCases;

                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
                _safetyIncidentRepository.Update(model);
            }


            return incident.SafetyIncidentId;
        }


        /// <summary>
        /// Inserts the safety incident
        /// </summary>
        /// <param name="safetyincident"></param>
        /// <returns></returns>
        public int InsertSafetyIncident(SafetyIncident safetyincident)
        {
            if (safetyincident == null)
                throw new ArgumentNullException("safetyincident");
            if (checkExistingIncident(safetyincident))
            {
                return -1;
            }


            safetyincident.CreatedBy = ProjectSession.UserID;
            safetyincident.CreatedDate = DateTime.Now;
            _safetyIncidentRepository.Insert(safetyincident);

            return safetyincident.SafetyIncidentId;
        }

        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingIncident(SafetyIncident incidentmodel)
        {
            try
            {
                var query = from p in _safetyIncidentRepository.Table
                            where p.SafetyId == incidentmodel.SafetyId &&
                            p.SafetyIncidentTypeId == incidentmodel.SafetyIncidentTypeId &&
                            p.SafetyIncidentId != incidentmodel.SafetyIncidentId
                            select p;
                if (query.ToList().Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
