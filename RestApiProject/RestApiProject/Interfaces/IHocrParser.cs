using RestApiProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiProject.Interfaces
{
	public interface IHocrParser
	{
		HocrObject Parse(StreamReader sr);
	}
}
