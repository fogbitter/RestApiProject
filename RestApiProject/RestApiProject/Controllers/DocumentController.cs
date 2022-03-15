using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public DocumentController(IDocumentService documentService)
        {
            this.documentService = documentService; 
        }

        [HttpGet]
        public string Index()
        {
            return "Hello world";
        }

        [HttpPost]
        [Consumes("text/plain")]
        public async Task<ActionResult> UploadDocument([FromQuery] string session,[FromBody] string rawDocument)
        {
            try
            {
                DefaultHocrParser parse = new DefaultHocrParser();
                HocrObject hocrObject = parse.Parse(rawDocument);
                this.documentService.SetDocument(hocrObject, session);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.ToString());
            }
            return this.Ok();
        }

        public class UploadDocumentRequest
        {
            public string Session { get; set; }
            public string RawDocument { get; set; }
        }
    }


    public interface IHocrParser
    {
        HocrObject Parse(string hocr);
    }

    public class DefaultHocrParser : IHocrParser
    {
        public HocrObject Parse(string hocr)
        {
            var sr = new StringReader(hocr);
            var reader = XmlReader.Create(sr, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse});
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(reader);

            // получим корневой элемент
            XmlElement? xRoot = xDoc.DocumentElement; 
            HocrObject hocrObject = null;
            foreach (XmlElement xnode in xRoot)
            {
                if (xnode.Name == "body")
                {
                    return this.InnerParse(xnode);
                }
            }
            return null;
        }

        public HocrObject InnerParse(XmlElement element)
        {
            HocrObject hocrObject = new HocrObject();

            foreach (XmlNode pageNode in element.ChildNodes)
            {
                HocrPage page = new HocrPage();
                hocrObject.Pages.Add(page);
                this.PageParse(pageNode, page);
            }
            return hocrObject;
        }
        
        private void PageParse(XmlNode node, HocrPage page)
        {
            this.ElementParse(node, page);
            foreach (XmlNode blockNode in node.ChildNodes)
            {
                HocrBlock block = new HocrBlock();
                page.Blocks.Add(block);
                this.BlockParse(blockNode, block);
            }
        }

        private void BlockParse(XmlNode node, HocrBlock block)
        {
            this.ElementParse(node, block);
            foreach (XmlNode parNode in node.ChildNodes)
            {
                HocrPar par = new HocrPar();
                block.Par = par;
                this.ParParse(parNode, par);
            }
        }
        private void ParParse(XmlNode node, HocrPar par)
        {
            this.ElementParse(node, par);
            foreach (XmlNode lineNode in node.ChildNodes)
            {
                HocrLine line = new HocrLine();
                par.Lines.Add(line);
                this.LineParse(lineNode, line);
            }
        }
        private void LineParse(XmlNode node, HocrLine line)
        {
            this.ElementParse(node, line);
            foreach (XmlNode wordNode in node.ChildNodes)
            {
                HocrWord word = new HocrWord();
                line.Words.Add(word);
                this.WordParse(wordNode, word);
            }
        }
        private void WordParse(XmlNode node, HocrWord word)
        {
            this.ElementParse(node, word);
        }

        private void ElementParse(XmlNode node, HocrElement element)
        {
            XmlNode? attribute = node.Attributes.GetNamedItem("class");
            element.ClassName = attribute?.Value;
            attribute = node.Attributes.GetNamedItem("id");
            element.Id = attribute?.Value;
            attribute = node.Attributes.GetNamedItem("title");
            element.Title = attribute?.Value;
        }
    }

    public class HocrObject
    {
        public List<HocrPage> Pages = new List<HocrPage>();
    }

    public class HocrPage : HocrElement
    {
        public List<HocrBlock> Blocks = new List<HocrBlock>();
    }

    public class HocrBlock : HocrElement
    {
        public HocrPar Par { get; set; }
    }

    public class HocrPar : HocrElement
    {
        public List<HocrLine> Lines = new List<HocrLine>();
    }

    public class HocrLine : HocrElement
    {
        public List<HocrWord> Words = new List<HocrWord>();
    }
    public class HocrWord: HocrElement
    {

    }

    public class HocrElement
    {
        public string Id { get; set; }
        public string ClassName { get; set; }
        public string Title { get; set; }

    }
}
