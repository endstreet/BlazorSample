using System;
using System.Collections.Generic;
using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public class PBIReportService : IPBIReportService
    {
        #region Fields

        private readonly IRepository<PBIReports> _PBIReportRepository;
        private readonly IRepository<PBIUserReports> _PBIUserReportRepository;
        private readonly IRepository<PBIReportRoles> _ReportRoleRepository;
        private readonly IRepository<UserRole> _UserRoleRepository;


        #endregion

        #region Ctor

        public PBIReportService(IRepository<PBIReports> PBIReportRepository, IRepository<PBIUserReports> PBIUserReportRepository, IRepository<PBIReportRoles> ReportRoleRepository,
            IRepository<UserRole> UserRoleRepository)
        {
            _PBIReportRepository = PBIReportRepository;
            _PBIUserReportRepository = PBIUserReportRepository;
            _ReportRoleRepository = ReportRoleRepository;
            _UserRoleRepository = UserRoleRepository;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Serches the PBIreports
        /// </summary>
        /// <param name="ReportName"></param>
        /// <param name="ReportGUID"></param>
        /// <param name="DataSetGUID"></param>
        /// <returns></returns>
        public IList<PBIReports> SearchPBIReports(string ReportGUID, string DataSetGUID)
        {

            var query = from p in _PBIReportRepository.Table
                        where ((p.ReportGUID == ReportGUID))
                        orderby p.PBIReportID descending
                        select p;

            return query.ToList();
        }

        /// <summary>
        /// Inserts the PBIReports.
        /// </summary>
        /// <param name="pbireport">The pbireport.</param>
        public int InsertPBIReports(PBIReports pbireport)
        {
            if (pbireport == null)
                throw new ArgumentNullException("PBIReports");
            if (checkExistingRecord(pbireport))
            {
                return -1;
            }
            pbireport.CreatedBy = ProjectSession.UserID;
            pbireport.CreatedDate = DateTime.Now;
            _PBIReportRepository.Insert(pbireport);

            return pbireport.PBIReportID;

        }


        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingRecord(PBIReports model)
        {
            try
            {
                var query = from p in _PBIReportRepository.Table
                            where p.PBIReportID != model.PBIReportID &&
                            (p.ReportGUID == model.ReportGUID)
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
        /// Gets All the PBIreports
        /// </summary>
        /// <returns></returns>
        public IList<PBIReports> GetAllPBIReports()
        {
            var query = (from x in _PBIReportRepository.Table
                         select x);

            return query.ToList();

        }

        /// <summary>
        /// Updates the PBIReports.
        /// </summary>
        /// <param name="pbireport">The pbireport.</param>
        public int UpdatePBIReports(PBIReports pbireport)
        {
            if (pbireport == null)
                throw new ArgumentNullException("PBIReports");

            var model = (from x in _PBIReportRepository.Table
                         where x.PBIReportID == pbireport.PBIReportID
                         select x).FirstOrDefault();

            if (model != null)
            {
                model.ReportName = pbireport.ReportName;
                model.IsActive = pbireport.IsActive;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
            }

            _PBIReportRepository.Update(model);
            return pbireport.PBIReportID;


        }

        /// <summary>
        /// Deactivates All reports
        /// </summary>
        /// <returns></returns>
        public void DeActivateAllReports(List<int> ActiveIds)
        {

            var reports = GetAllPBIReports();

            foreach (PBIReports Report in reports)
            {
                if (!ActiveIds.Contains(Report.PBIReportID))
                {
                    Report.IsActive = false;
                    Report.ModifiedBy = ProjectSession.UserID;
                    Report.ModifiedDate = DateTime.Now;
                    _PBIReportRepository.Update(Report);
                }
            }
        }

        /// <summary>
        /// Save User Reports
        /// </summary>
        /// <returns></returns>
        public void SaveUserReport(string reportguid, bool IsPinned)
        {
            var report = (from x in _PBIReportRepository.Table
                          where x.ReportGUID == reportguid
                          select x).FirstOrDefault();
            if (IsPinned)
            {

                if (report != null)
                {
                    PBIUserReports userreport = new PBIUserReports()
                    {
                        PBIReportID = report.PBIReportID,
                        UserID = ProjectSession.UserID
                    };

                    _PBIUserReportRepository.Insert(userreport);
                }
            }
            else
            {
                if (report != null)
                {
                    var model = (from x in _PBIUserReportRepository.Table
                                 where x.PBIReportID == report.PBIReportID && x.UserID == ProjectSession.UserID
                                 select x).FirstOrDefault();
                    if (model != null)
                        _PBIUserReportRepository.Delete(model);
                }
            }
        }


        public PBIUserReports GetUserReport(string reportguid, int UserID)
        {

            var query = (from x in _PBIUserReportRepository.Table
                         join z in _PBIReportRepository.Table
                         on x.PBIReportID equals z.PBIReportID
                         where z.ReportGUID == reportguid && x.UserID == UserID
                         select new
                         {
                             PBIReportID = z.PBIReportID,
                             UserID = x.UserID,
                             PBIUserReportID = x.PBIUserReportID

                         }).ToList().Select(c => new PBIUserReports()
                         {
                             PBIReportID = c.PBIReportID,
                             PBIUserReportID = c.PBIUserReportID,
                             UserID = c.UserID
                         }).FirstOrDefault();

            return query;

        }



        /// <summary>
        /// Gets the Report roles.
        /// </summary>
        /// <param name="RoleId">The Role identifier.</param>
        /// <returns>IList&lt;Role&gt;.</returns>
        public IList<PBIReports> GetReportRoles(int RoleId)
        {
            var query = (from ur in _ReportRoleRepository.Table
                         join r in _PBIReportRepository.Table on
                         ur.PBIReportID equals r.PBIReportID
                         where ur.RoleId == RoleId
                         select new
                         {
                             PBIReportID = ur.PBIReportID,
                             ReportName = r.ReportName,
                         }).ToList()
                         .Select(x => new PBIReports
                         {
                             PBIReportID = x.PBIReportID,
                             ReportName = x.ReportName,

                         }).ToList();
            return query;
        }



        /// <summary>
        /// Deletes the Report roles.
        /// </summary>
        /// <param name="RoleId">The role identifier.</param>
        public void DeleteReportRoles(int RoleId)
        {
            var query = (from p in _ReportRoleRepository.Table
                         where p.RoleId == RoleId
                         select p).ToList();

            if (query != null && query.Count > 0)
            {
                foreach (PBIReportRoles entry in query)
                {
                    _ReportRoleRepository.Delete(entry);
                }
            }

        }


        /// <summary>
        /// Inserts the report role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public int InsertReportRole(PBIReportRoles role)
        {
            if (role == null)
                throw new ArgumentNullException("reportrole");
            if (checkExistingRecordReportRole(role))
            {
                return -1;
            }
            _ReportRoleRepository.Insert(role);
            return role.RoleId;
        }


        private bool checkExistingRecordReportRole(PBIReportRoles model)
        {
            try
            {
                var query = from p in _ReportRoleRepository.Table
                            where p.RoleId == model.RoleId &&
                           p.PBIReportID == model.PBIReportID &&
                           p.ReportRoleId != model.ReportRoleId
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

        //public int RoleID { get; set; }
        //public string RoleName { get; set; }
        //public int UserID { get; set; }
        //public int PBIReportID { get; set; }
        //public int ReportGUID { get; set; }
        //public int DataSetGUID { get; set; }
        //public int ReportName { get; set; }

        public List<PBIReportsRights> GetPBIReportRights(int UserID)
        {
            List<PBIReportsRights> lstRights = new List<PBIReportsRights>();
            try
            {

                var query = (from rr in _ReportRoleRepository.Table
                             join r in _PBIReportRepository.Table on rr.PBIReportID equals r.PBIReportID
                             join u in _UserRoleRepository.Table on rr.RoleId equals u.RoleId
                             where u.UserId == UserID
                             select new
                             {
                                 RoleID = rr.RoleId,
                                 UserID = u.UserId,
                                 PBIReportID = r.PBIReportID,
                                 ReportGUID = r.ReportGUID,
                                 DataSetGUID = r.DataSetGUID,
                                 ReportName = r.ReportName

                             }).ToList()
                            .Select(x => new PBIReportsRights
                            {
                                RoleID = x.RoleID,
                                UserID = x.UserID,
                                PBIReportID = x.PBIReportID,
                                ReportGUID = x.ReportGUID,
                                DataSetGUID = x.DataSetGUID,
                                ReportName = x.ReportName
                            }).ToList();

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool GetMenuReport(string reportguid, int MenuId = 0)
        {
            try
            {
                if (MenuId == 0)
                {
                    return _PBIReportRepository.Table.Any(x => x.ReportGUID == reportguid && x.MenuId != null);
                }
                else
                {
                    return _PBIReportRepository.Table.Any(x => x.ReportGUID == reportguid && x.MenuId == MenuId);
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
