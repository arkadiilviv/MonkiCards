using Monki.DAL.Models;

namespace Monki.API.Models
{
	public class DeckDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public bool IsPrivate { get; set; } = true;
	}

	public class DeckDTOResponse : DeckDTO
	{
		public DeckDTOResponse(MonkiDeck deck) {
			Id = deck.Id;
			CreatedAt = deck.CreatedAt;
			UpdatedAt = deck.UpdatedAt;
			UserId = deck.UserId;
			Cards = deck.Cards;
			Name = deck.Name;
			Description = deck.Description;
			IsPrivate = deck.IsPrivate;
		}
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string UserId { get; set; } = string.Empty;
		public IEnumerable<MonkiCard> Cards { get; set; } = [];
	}
}
