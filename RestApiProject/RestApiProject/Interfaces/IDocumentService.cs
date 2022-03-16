using RestApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiProject.Interfaces
{
	public interface IDocumentService
	{
		Guid AddDocument(HocrObject document, string session);
		bool SetCurrent(string session, Guid id);
		HocrObject GetCurrentDocument(string session);
		HocrObject GetDocumentByID(Guid id);
		void DeleteCurrentDocument(string session);
		void DeleteDocumentByID(Guid id);
	}
}
