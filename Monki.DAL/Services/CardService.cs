using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monki.DAL.Services
{
	public class CardService : ICardService
	{
		private readonly MonkiContext _context;
		public CardService(MonkiContext context)
		{
			_context = context;
		}
		public async Task<MonkiCard> AddAsync(MonkiCard item)
		{
			_context.Cards.Add(item);
			var res = await _context.SaveChangesAsync();
			if (res > 0)
				return item;
			throw new Exception("Failed to add card.");
		}

		public async Task Delete(MonkiCard item)
		{
			_context.Cards.Remove(item);
			var res = await _context.SaveChangesAsync();
			if (res == 0)
				throw new Exception("Failed to delete card.");

			return;
		}

		public IEnumerable<MonkiCard> GetAll()
		{
			return _context.Cards;
		}

		public MonkiCard GetById(int id)
		{
			return _context.Cards.FirstOrDefault(x => x.Id == id);	
		}

		public IEnumerable<MonkiCard> GetCardsByDeckId(int deckId)
		{
			return _context.Cards.Where(x => x.DeckId == deckId);
		}

		public async Task UpdateModelAsync(MonkiCard item)
		{
			var existing = _context.Cards.FirstOrDefault(x => x.Id == item.Id);
			if (existing == null)
				throw new Exception("Card not found.");

			existing.SideA = item.SideA;
			existing.SideB = item.SideB;
			existing.ExampleA = item.ExampleA;
			existing.ExampleB = item.ExampleB;
			existing.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();
		}
	}
}
