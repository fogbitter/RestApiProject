using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiProject.Controllers;
using RestApiProject.Interfaces;
using RestApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiProject.Services
{
    public class DocumentService : IDocumentService
    {
        private Dictionary<string, SavedDocumentList> container = new Dictionary<string, SavedDocumentList>();
        private string lockObject = "lockObject";

        public HocrObject GetCurrentDocument(string session)
        {
			SavedDocumentList docList = this.GetDocumentList(session);
			return docList.GetCurrent().Document;
		}

		public HocrObject GetDocumentByID(string session, Guid id)
		{
			SavedDocumentList docList = this.GetDocumentList(session);
			return docList.GetById(id).Document;
		}
		public HocrObject GetDocumentByID(Guid id)
		{
			throw new NotImplementedException();
		}

		public SavedDocumentList GetDocumentList(string session)
		{
			return container.ContainsKey(session) ? container[session] : null;
		}

		public Guid AddDocument(HocrObject document, string session)
        {
			Guid id = Guid.NewGuid();
            lock (lockObject)
            {
				if (!container.ContainsKey(session))
				{
					container[session] = new SavedDocumentList();
				}
				container[session].AddCurrentDocument(new SavedDocument(id, document, true));
            }
			return id;
        }

		public void DeleteCurrentDocument(string session)
		{
			lock (lockObject)
			{
				SavedDocumentList docList = this.GetDocumentList(session);
				docList.DeleteCurrent();
			}
		}

		public void DeleteDocumentByID(string session, Guid id)
		{
			lock (lockObject)
			{
				SavedDocumentList docList = this.GetDocumentList(session);
				docList.DeleteByID(id);
			}
		}

		public bool SetCurrent(string session, Guid id)
		{
			if (!container.ContainsKey(session))
			{
				return false;
			}
			lock (lockObject)
			{
				return container[session].SetCurrent(id);
			}
		}
	}

	public class SavedDocument
	{
		public Guid ID { get; set; }
		public HocrObject Document { get; set; }
		public bool Current { get; set; }
		public SavedDocument(Guid id, HocrObject document, bool current)
		{
			ID = id;
			Document = document;
			Current = current;
		}
	}

	public class SavedDocumentList : List<SavedDocument>
	{
		public void AddCurrentDocument(SavedDocument doc)
		{
			this.RemoveCurrent();
			this.Add(doc);
		}

		public void RemoveCurrent()
		{
			this.GetCurrent().Current = false;
		}

		public bool SetCurrent(Guid id)
		{
			SavedDocument doc = this.GetById(id);
			if (doc == null)
			{
				return false;
			}
			doc.Current = true;
			return true;
		}

		public void DeleteCurrent()
		{
			SavedDocument doc = this.GetCurrent();
			this.Remove(doc);
		}
		public void DeleteByID(Guid id)
		{
			SavedDocument doc = this.GetById(id);
			this.Remove(doc);
		}

		public SavedDocument GetCurrent()
		{
			return this.FirstOrDefault(doc => doc.Current);
		}

		public SavedDocument GetById(Guid id)
		{
			return this.FirstOrDefault(doc => doc.ID == id);
		}
	}
}
