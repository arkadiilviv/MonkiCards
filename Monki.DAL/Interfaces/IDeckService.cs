using Monki.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monki.DAL.Interfaces
{
	public interface IDeckService : IBaseService<MonkiDeck>
	{
		public MonkiDeck UploadApkgDeck();
	}
}
