using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiProject.Models
{
	public class HocrPage : HocrElement
	{
		public List<HocrBlock> Blocks = new List<HocrBlock>();
	}
}
