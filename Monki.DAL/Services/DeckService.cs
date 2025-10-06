using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monki.DAL.Services
{
	public class DeckService : IDeckService
	{
		private readonly MonkiContext _context;
		public DeckService(MonkiContext context)
		{
			_context = context;
		}
		public async Task<MonkiDeck> AddAsync(MonkiDeck item)
		{
			_context.Decks.Add(item);
			var res = await _context.SaveChangesAsync();
			if (res > 0)
				return item;
			throw new Exception("Failed to add deck.");
		}

		public async Task Delete(MonkiDeck item)
		{
			_context.Decks.Remove(item);
			var res = await _context.SaveChangesAsync();
			if (res == 0)
				throw new Exception("Failed to delete deck.");

			return;
		}

		public IEnumerable<MonkiDeck> GetAll()
		{
			return _context.Decks;
		}

		public Task<MonkiDeck> GetById(int id)
		{
			throw new NotImplementedException();
		}

		public void UpdateModel(MonkiDeck item)
		{
			throw new NotImplementedException();
		}

		public MonkiDeck UploadApkgDeck()
		{
			throw new NotImplementedException();
		}
	}
}
