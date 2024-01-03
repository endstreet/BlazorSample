using System.Collections.Generic;
using TrevaliOperationalReport.Domain.Projects;

namespace TrevaliOperationalReport.Service.Projects
{
    public interface IDocumentService
    {
        /// <summary>
        /// Searches the documents.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>
        /// IList&lt;Documents&gt;.
        /// </returns>
        IList<DownloadDocument> SearchDocuments(int projectId);

        /// <summary>
        /// Searches the task documents.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        IList<DownloadDocument> SearchTaskDocuments(int taskId);
        /// <summary>
        /// Inserts the document.
        /// </summary>
        /// <param name="document">The document.</param>
        void InsertDocument(Documents document);

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        bool DeleteDocument(Documents document);

        /// <summary>
        /// Deletes the project document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        bool DeleteProjectDocument(Documents document);

        /// <summary>
        /// Gets the document by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Documents.
        /// </returns>
        Documents GetDocumentById(int id);

    }
}