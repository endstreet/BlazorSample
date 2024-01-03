using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using TrevaliOperationalReport.Domain.ErrorLogs;

namespace TrevaliOperationalReport.Data
{
    public class TrevaliOperationalReportObjectContext : DbContext, IDbContext, IDisposable
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="TrevaliOperationalReportObjectContext"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        public TrevaliOperationalReportObjectContext(string nameOrConnectionString) :
            base(nameOrConnectionString)
        {
            //this.Configuration.LazyLoadingEnabled = false;
            var initializer = new CreateDatabaseIfNotExists<TrevaliOperationalReportObjectContext>();
            Database.SetInitializer(initializer);
        }

        #endregion

        #region Utilities

        public virtual IDbSet<Domain.General.Users> Users { get; set; }
        public virtual IDbSet<Domain.General.Distribution> Distribution { get; set; }
        public virtual IDbSet<Domain.General.Menus> Menus { get; set; }
        public virtual IDbSet<Domain.General.MenuAccessRights> MenuAccessRights { get; set; }
        public virtual IDbSet<Domain.General.RoleMenuAccessRights> RoleMenuAccessRights { get; set; }
        public virtual IDbSet<Domain.General.Role> Role { get; set; }
        public virtual IDbSet<Domain.General.UserRole> UserRoles { get; set; }
        public virtual IDbSet<Domain.General.AccessRights> AccessRights { get; set; }
        public virtual IDbSet<Domain.General.UserResetPassword> UserResetPassword { get; set; }
        public virtual IDbSet<Domain.General.Settings> Settings { get; set; }
        public virtual IDbSet<Domain.General.Section> Section { get; set; }
        public virtual IDbSet<Domain.General.Unit> Unit { get; set; }
        public virtual IDbSet<Domain.General.Site> Site { get; set; }
        public virtual IDbSet<Domain.General.Reports> Reports { get; set; }
        public virtual IDbSet<Domain.General.Metrics> Metrics { get; set; }
        public virtual IDbSet<Domain.General.SiteMetrics> SiteMetrics { get; set; }
        public virtual IDbSet<Domain.General.Shift> Shift { get; set; }
        public virtual IDbSet<Domain.General.EquipmentTypes> EquipmentTypes { get; set; }
        public virtual IDbSet<Domain.General.Equipment> Equipment { get; set; }
        public virtual IDbSet<Domain.Report.MonthlyBudgetPlanData> MonthlyBudgetPlanData { get; set; }
        public virtual IDbSet<Domain.Report.WeeklyOperationalData> WeeklyOperationalData { get; set; }
        public virtual IDbSet<Domain.Report.SafetyIncidentType> SafetyIncidentType { get; set; }
        public virtual IDbSet<Domain.Report.Safety> Safety { get; set; }
        public virtual IDbSet<Domain.Report.SafetyIncident> SafetyIncident { get; set; }
        public virtual IDbSet<Domain.General.SectionRole> SectionRoles { get; set; }
        public virtual IDbSet<Domain.Report.DailyShiftOperationalData> DailyShiftOperationalData { get; set; }
        public virtual IDbSet<Domain.Report.DailyOperationalData> DailyOperationalData { get; set; }
        public virtual IDbSet<Domain.Report.DailyUploadOperationalData> DailyUploadOperationalData { get; set; }
        public virtual IDbSet<Domain.Report.UploadSheetData> UploadSheetData { get; set; }
        public virtual IDbSet<Domain.General.PBIReports> PBIReports { get; set; }
        public virtual IDbSet<Domain.General.PBIUserReports> PBIUserReports { get; set; }
        public virtual IDbSet<Domain.General.PBIReportRoles> ReportRoles { get; set; }
        public virtual DbSet<Domain.General.UserSiteRights> UserSiteRights { get; set; }
        public virtual DbSet<Domain.General.ProjectType> ProjectType { get; set; }
        public virtual DbSet<Domain.Projects.Documents> Documents { get; set; }
        public virtual DbSet<Domain.Projects.Project> Project { get; set; }
        public virtual DbSet<Domain.Projects.ProjectResource> ProjectResource { get; set; }
        public virtual DbSet<Domain.Projects.ProjectTask> ProjectTask { get; set; }
        public virtual DbSet<Domain.Projects.TaskFeedback> TaskFeedback { get; set; }
        public virtual DbSet<Domain.Projects.TaskResource> TaskResource { get; set; }

        public virtual DbSet<Domain.General.SiteParameters> SiteParameters { get; set; }

        public virtual DbSet<Domain.Report.WeeklyOperationalDataApprove> WeeklyOperationalDataApprove { get; set; }
        public virtual IDbSet<Domain.Report.MonthlyForecast> MonthlyForecast { get; set; }
        public virtual IDbSet<Domain.Report.MonthlyForecastData> MonthlyForecastData { get; set; }
        public virtual DbSet<Domain.Report.MonthlyBudgetPlanDataApprove> MonthlyBudgetPlanDataApprove { get; set; }
        public virtual IDbSet<ErrorLog> ErrorLog { get; set; }

        public virtual IDbSet<Domain.Report.MonthlyPlanData> MonthlyPlanData { get; set; }


        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Report.WeeklyOperationalData>().Property(o => o.ActualValue).HasPrecision(12, 2);
            modelBuilder.Entity<Domain.Report.WeeklyOperationalData>().Property(o => o.MTDValue).HasPrecision(12, 2);
            modelBuilder.Entity<Domain.Report.DailyOperationalData>().Property(o => o.ActualValue).HasPrecision(12, 2);
            modelBuilder.Entity<Domain.Report.MonthlyBudgetPlanData>().Property(o => o.Budget).HasPrecision(12, 2);
            modelBuilder.Entity<Domain.Report.MonthlyBudgetPlanData>().Property(o => o.Forecast).HasPrecision(12, 2);
            modelBuilder.Entity<Domain.Report.MonthlyBudgetPlanData>().Property(o => o.Actual).HasPrecision(12, 2);
            modelBuilder.Entity<Domain.Report.DailyShiftOperationalData>().Property(o => o.ActualValue).HasPrecision(12, 2);
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the database script.
        /// </summary>
        /// <returns></returns>
        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// Sets this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>
        /// Entities
        /// </returns>
        /// <exception cref="System.Exception">Not support parameter type</exception>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }
            return Database.SqlQuery<TEntity>(commandText, parameters).ToList();
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return Database.SqlQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// Excecutes the SQL command.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int ExcecuteSqlCommand(string sql, params object[] parameters)
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    sql += i == 0 ? " " : ", ";

                    sql += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        sql += " output";
                    }
                }
            }

            return Database.ExecuteSqlCommand(sql, parameters);
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public Database Db
        {
            get
            {
                return Database;
            }
        }

        /// <summary>
        /// Disposes the context. The underlying <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" /> is also disposed if it was created
        /// is by this context or ownership was passed to this context when this context was created.
        /// The connection to the database (<see cref="T:System.Data.Common.DbConnection" /> object) is also disposed if it was created
        /// is by this context or ownership was passed to this context when this context was created.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Dispose context
        /// </summary>
        public void DisposeContext()
        {
            base.Dispose();
        }

        #endregion
    }
}
