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

		public MonkiDeck GetById(int id)
		{
			return _context.Decks.FirstOrDefault(x => x.Id == id);
		}

		public async Task UpdateModelAsync(MonkiDeck item)
		{
			var existing = _context.Decks.FirstOrDefault(x => x.Id == item.Id);
			if (existing == null)
				throw new Exception("Deck not found.");

			existing.Name = item.Name;
			existing.Description = item.Description;
			existing.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();
		}

		public MonkiDeck UploadApkgDeck()
		{
			throw new NotImplementedException();
		}
	}
}
