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
		public const string Path = @"C:\Users\Эмиль\Desktop\document.png.hocr";

		public IDocumentService documentService;
		public IHocrParser hocrParser;

		public DocumentController(IDocumentService documentService, IHocrParser hocrParser)
		{
			this.documentService = documentService;
			this.hocrParser = hocrParser;
		}

		[HttpGet]
		public HocrObject GetCurrentDocument(string session)
		{
			return documentService.GetCurrentDocument(session);
		}

		[HttpGet]
		public HocrObject GetDocumentByID(Guid documentID)
		{
			return documentService.GetDocumentByID(documentID);
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
				this.documentService.AddDocument(hocrObject, session);
			}
			catch (Exception ex)
			{
				return this.BadRequest(ex.ToString());
			}
			return this.Ok();
		}
	}
}