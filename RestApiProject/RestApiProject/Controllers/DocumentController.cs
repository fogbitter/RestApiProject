using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiProject.Interfaces;
using RestApiProject.Models;
using RestApiProject.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RestApiProject.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DocumentController : Controller
	{
		public IDocumentService documentService;
		public IHocrParser hocrParser;

		public DocumentController(IDocumentService documentService, IHocrParser hocrParser)
		{
			this.documentService = documentService;
			this.hocrParser = hocrParser;
		}

		[HttpGet("session={session}")]
		public ActionResult<HocrObject> GetCurrentDocument(string session)
		{
			HocrObject obj = documentService.GetCurrentDocument(session);
			return this.Ok(obj)
		}

		[HttpGet("documentID={documentID}")]
		public ActionResult<HocrObject> GetDocumentByID(Guid documentID)
		{
			HocrObject obj = documentService.GetDocumentByID(documentID);
			return this.Ok(obj)
		}

		[HttpPatch]
		public ActionResult SetCurrentDocument(string session, Guid documentID)
		{
			documentService.SetCurrent(session, documentID);
			return this.Ok();
		}

		[HttpPost]
		public ActionResult UploadDocument([FromQuery] string session, IFormFile file)
		{
			try
			{
				HocrObject hocrObject;
				using (StreamReader sr = new StreamReader(file.OpenReadStream()))
				{
					hocrObject = this.hocrParser.Parse(sr);
				}
				this.documentService.AddDocument(hocrObject, session);
			}
			catch (Exception ex)
			{
				return this.BadRequest(ex.ToString());
			}
			return this.Ok();
		}

		[HttpDelete("session={session}")]
		public ActionResult DeleteCurrent(string session)
		{
			this.documentService.DeleteCurrentDocument(session);
			return this.Ok();
		}

		[HttpDelete("documentID={documentID}")]
		public ActionResult<bool> DeleteByID(Guid id)
		{
			this.documentService.DeleteDocumentByID(id);
			return this.Ok();
		}
	}
}