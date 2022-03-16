using RestApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiProject.Interfaces
{
	public interface IDocumentService
	{
		Task<Guid> AddDocument(HocrObject document, string session);
		Task<bool> SetCurrent(string session, Guid id);
		Task<HocrObject> GetCurrentDocument(string session);
		Task<HocrObject> GetDocumentByID(Guid id);
		Task DeleteCurrentDocument(string session);
		Task DeleteDocumentByID(Guid id);
	}
}
