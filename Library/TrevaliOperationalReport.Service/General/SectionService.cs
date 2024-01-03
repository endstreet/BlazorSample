using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.General;

namespace TrevaliOperationalReport.Service.General
{
    public class SectionService : ISectionService
    {
        #region Fields

        private readonly IRepository<Section> _sectionRepository;
        private readonly IRepository<SectionRole> _sectionRoleRepository;
        public static TrevaliOperationalReportObjectContext _dbContext;


        #endregion

        #region Ctor

        public SectionService(IRepository<Section> sectionRepository, IRepository<SectionRole> sectionRoleRepository)
        {
            _sectionRepository = sectionRepository;
            _sectionRoleRepository = sectionRoleRepository;
            _dbContext = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName);

        }

        #endregion

        #region Methods

        #region Section
        /// <summary>
        /// Searches the sections.
        /// </summary>
        /// <param name="sectionname">The sectionname.</param>
        /// <returns>IList&lt;Section&gt;.</returns>
        public IList<Section> SearchSections(string sectionname)
        {
            var query = from p in _sectionRepository.Table
                        where ((p.SectionName.Contains(sectionname) || sectionname == ""))
                        orderby p.SectionId descending
                        select p;

            return query.ToList();

        }

        /// <summary>
        /// Inserts the section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <exception cref="System.ArgumentNullException">section</exception>
        public int InsertSection(Section section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            if (checkExistingRecord(section))
            {
                return -1;
            }
            section.CreatedBy = ProjectSession.UserID;
            section.CreatedDate = DateTime.Now;
            _sectionRepository.Insert(section);
            return section.SectionId;
        }

        /// <summary>
        /// Updates the section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.ArgumentNullException">section</exception>
        public int UpdateSection(Section section)
        {
            if (section == null)
                throw new ArgumentNullException("section");
            if (checkExistingRecord(section))
            {
                return -1;
            }
            var model = GetSectionById(section.SectionId);
            if (model != null)
            {
                model.SectionName = section.SectionName;
                model.ModifiedBy = ProjectSession.UserID;
                model.ModifiedDate = DateTime.Now;
            }
            _sectionRepository.Update(model);
            return section.SectionId;
        }

        /// <summary>
        /// Gets the section by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Section.</returns>
        public Section GetSectionById(int id)
        {
            return _sectionRepository.GetById(id);
        }

        /// <summary>
        /// Deletes the section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">section</exception>
        public bool DeleteSection(Section section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            try
            {
                _sectionRepository.Delete(section);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the section select list.
        /// </summary>
        /// <returns>IList&lt;SelectListItem&gt;.</returns>
        public IList<SelectListItem> GetSectionSelectList()
        {
            var query = from p in _sectionRepository.Table
                        orderby p.SectionName ascending
                        select new SelectListItem
                        {
                            Text = p.SectionName,
                            Value = p.SectionId.ToString()
                        };

            return query.ToList();
        }

        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingRecord(Section model)
        {
            try
            {
                var query = from p in _sectionRepository.Table
                            where p.SectionId != model.SectionId &&
                            ((p.SectionName).Equals(model.SectionName))
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

        public IList<SelectListItem> GetSectionSelectList(int SiteId, int ReportId)
        {
            //var query = from p in _dbContext.SiteMetrics
            //            join s in _dbContext.Section 
            //            on p.SectionId equals s.SectionId
            //            where p.ReportId == ReportId && p.SiteId == SiteId 
            //            select new SelectListItem
            //            {
            //                Text = s.SectionName,
            //                Value = s.SectionId.ToString()
            //            };

            var query = (from p in _dbContext.SiteMetrics
                         where p.ReportId == ReportId && p.SiteId == SiteId
                         select p.SectionId).Distinct().ToList();

            var query1 = from s in _dbContext.Section
                         where query.Contains(s.SectionId)
                         select new SelectListItem
                         {
                             Text = s.SectionName,
                             Value = s.SectionId.ToString()
                         };

            return query1.ToList();
        }
        #endregion

        #region SectionRole

        /// <summary>
        /// Gets the Section roles.
        /// </summary>
        /// <param name="RoleId">The Role identifier.</param>
        /// <returns>IList&lt;Role&gt;.</returns>
        public IList<Section> GetSectionRoles(int RoleId)
        {
            var query = (from ur in _sectionRoleRepository.Table
                         join r in _sectionRepository.Table on
                         ur.SectionId equals r.SectionId
                         where ur.RoleId == RoleId
                         select new
                         {
                             SectionId = ur.SectionId,
                             SectionName = r.SectionName,
                         }).ToList()
                         .Select(x => new Section
                         {
                             SectionId = x.SectionId,
                             SectionName = x.SectionName,

                         }).ToList();
            return query;
        }

        /// <summary>
        /// Deletes the section roles.
        /// </summary>
        /// <param name="RoleId">The role identifier.</param>
        public void DeleteUserRoles(int RoleId)
        {
            var query = (from p in _sectionRoleRepository.Table
                         where p.RoleId == RoleId
                         select p).ToList();

            if (query != null && query.Count > 0)
            {
                foreach (SectionRole entry in query)
                {
                    _sectionRoleRepository.Delete(entry);
                }
            }

        }

        /// <summary>
        /// Inserts the section role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <exception cref="System.ArgumentNullException">role</exception>
        public int InsertSectionRole(SectionRole role)
        {
            if (role == null)
                throw new ArgumentNullException("sectionrole");
            if (checkExistingRecordSectionRole(role))
            {
                return -1;
            }
            _sectionRoleRepository.Insert(role);
            return role.RoleId;
        }

        /// <summary>
        /// Checks the existing record.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool checkExistingRecordSectionRole(SectionRole model)
        {
            try
            {
                var query = from p in _sectionRoleRepository.Table
                            where p.RoleId == model.RoleId &&
                           p.SectionId == model.SectionId &&
                           p.SectionRoleId != model.SectionRoleId
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
        /// gets all section roles
        /// </summary>
        /// <returns></returns>
        public List<SectionRole> GetAllSectionRoles()
        {
            try
            {
                var query = from p in _sectionRoleRepository.Table
                            select p;
                return query.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #endregion
    }
}
