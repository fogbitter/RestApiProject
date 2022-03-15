using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiProject.Services
{
    public interface IDocumentService
    {
        void SetDocument(object document, string session);
        object GetDocument(string session);
    }

    public class DocumentService : IDocumentService
    {
        private Dictionary<string, object> container = new Dictionary<string,object>();
        private string lockObject = "lockObject";

        public object GetDocument(string session)
        {
            throw new NotImplementedException();
        }

        public void SetDocument(object document, string session)
        {
            lock (lockObject)
            {
                container[session] = document;
            }
        }
    }
}
