using NUnit.Framework;
using RestApiProject.Models;
using RestApiProject.Services;
using System.IO;

namespace RestApiProjectTest
{
	public class Tests
	{
		public const string TestFilePath = "../../../testHocr.txt";

		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void DefaultHocrParserTest()
		{
			StreamReader sr = new StreamReader(TestFilePath);
			DefaultHocrParser parser = new DefaultHocrParser();
			HocrObject hocrObject = parser.Parse(sr);

			if (hocrObject == null)
			{
				Assert.Fail();
			}

			if (hocrObject.Pages == null || hocrObject.Pages.Count != 1)
			{
				Assert.Fail();
			}

			HocrPage page = hocrObject.Pages[0];
			if (page.Blocks == null || page.Blocks.Count != 1)
			{
				Assert.Fail();
			}
			HocrBlock block = page.Blocks[0];
			if (block.Par == null)
			{
				Assert.Fail();
			}

			HocrPar par = block.Par;
			if (par.Lines == null || par.Lines.Count != 1)
			{
				Assert.Fail();
			}

			HocrLine line = par.Lines[0];
			if (line.Words == null || line.Words.Count != 1)
			{
				Assert.Fail();
			}
			HocrWord word = line.Words[0];
			if (word == null)
			{
				Assert.Fail();
			}

			Assert.Pass();
		}
	}
}