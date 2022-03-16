using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiProject.Models
{
	public class HocrLine : HocrElement
	{
		public List<HocrWord> Words = new List<HocrWord>();
	}
}
