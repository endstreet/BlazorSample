using System;
using System.Collections.Generic;
using System.Linq;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service.Projects
{
    public class DocumentService : IDocumentService
    {
        #region Fields

        private readonly IRepository<Documents> _documentRepository;

        #endregion

        #region Ctor

        public DocumentService(IRepository<Documents> documentRepository)
        {
            _documentRepository = documentRepository;
        }

        #endregion

        #region Method

        /// <summary>
        /// Searches the documents.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>
        /// IList&lt;Documents&gt;.
        /// </returns>
        public IList<DownloadDocument> SearchDocuments(int projectId)
        {
            var query = from p in _documentRepository.Table
                        where (p.ProjectId == projectId && p.TaskId == null)
                        orderby p.DocumentId descending
                        select new DownloadDocument { DocumentId = p.DocumentId, ProjectId = p.ProjectId, DocumentName = p.DocumentName };
            return query.ToList();
        }

        /// <summary>
        /// Searches the task documents.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        public IList<DownloadDocument> SearchTaskDocuments(int taskId)
        {
            var query = from p in _documentRepository.Table
                        where (p.TaskId == taskId)
                        orderby p.DocumentId descending
                        select new DownloadDocument { DocumentId = p.DocumentId, ProjectId = p.ProjectId, DocumentName = p.DocumentName };
            return query.ToList();
        }

        /// <summary>
        /// Inserts the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <exception cref="System.ArgumentNullException">document</exception>
        public void InsertDocument(Documents document)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            _documentRepository.Insert(document);
        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">document</exception>
        public bool DeleteDocument(Documents document)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            try
            {
                using (TrevaliOperationalReportObjectContext context = new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName))
                {
                    if (document.TaskId > 0)
                    {
                        var data = context.Documents.Where(p => p.TaskId == document.TaskId).ToList();
                        foreach (Documents obj in data)
                        {
                            context.Documents.Remove(obj);
                        }
                        context.SaveChanges();
                    }
                    else
                    {
                        var data = context.Documents.Where(p => p.ProjectId == document.ProjectId).ToList();
                        foreach (Documents obj in data)
                        {
                            context.Documents.Remove(obj);
                        }
                        context.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes the project document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">document</exception>
        public bool DeleteProjectDocument(Documents document)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            try
            {
                _documentRepository.Delete(document);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the document by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Documents.
        /// </returns>
        public Documents GetDocumentById(int id)
        {
            return _documentRepository.GetById(id);
        }

        #endregion
    }
}