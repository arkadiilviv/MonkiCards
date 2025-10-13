using Monki.DAL.Models;

namespace Monki.DAL.Interfaces
{
	public interface ICardService : IBaseService<MonkiCard>
	{
		public IEnumerable<MonkiCard> GetCardsByDeckId(int deckId);
	}
}
