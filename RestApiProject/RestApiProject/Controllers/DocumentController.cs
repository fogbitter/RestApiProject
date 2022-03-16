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
using System.Threading;
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
		public async Task<ActionResult<HocrObject>> GetCurrentDocument(string session)
		{
			HocrObject obj = await documentService.GetCurrentDocument(session);
			return this.Ok(obj);
		}

		[HttpGet("documentID={documentID}")]
		public async Task<ActionResult<HocrObject>> GetDocumentByID(Guid documentID)
		{
			HocrObject obj = await documentService.GetDocumentByID(documentID);
			return this.Ok(obj);
		}

		[HttpPatch]
		public async Task<ActionResult> SetCurrentDocument(string session, Guid documentID)
		{
			await documentService.SetCurrent(session, documentID);
			return this.Ok();
		}

		[HttpPost]
		public async Task<ActionResult> UploadDocument([FromQuery] string session, IFormFile file)
		{
			try
			{
				HocrObject hocrObject;
				using (StreamReader sr = new StreamReader(file.OpenReadStream()))
				{
					hocrObject = this.hocrParser.Parse(sr);
				}
				await this.documentService.AddDocument(hocrObject, session);
			}
			catch (Exception ex)
			{
				return this.BadRequest(ex.ToString());
			}
			return this.Ok();
		}

		[HttpDelete("session={session}")]
		public async Task<ActionResult> DeleteCurrent(string session)
		{
			await this .documentService.DeleteCurrentDocument(session);
			return this.Ok();
		}

		[HttpDelete("documentID={documentID}")]
		public async Task<ActionResult> DeleteByID(Guid id)
		{
			await this.documentService.DeleteDocumentByID(id);
			return this.Ok();
		}
	}
}