using RestApiProject.Interfaces;
using RestApiProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace RestApiProject.Services
{
	public class DefaultHocrParser : IHocrParser
	{
		public HocrObject Parse(StreamReader sr)
		{
			var reader = XmlReader.Create(sr, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse });
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(reader);

			XmlElement? xRoot = xDoc.DocumentElement;
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
}
