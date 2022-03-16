using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiProject.Models
{
	public class HocrPar : HocrElement
	{
		public List<HocrLine> Lines = new List<HocrLine>();
	}
}
