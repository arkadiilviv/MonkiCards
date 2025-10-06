using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monki.DAL.Models
{
	public class MonkiCard
	{
		public int Id { get; set; }
		public string SideA { get; set; } = string.Empty;
		public string SideB { get; set; } = string.Empty;
		public string ExampleA { get; set; } = string.Empty;
		public string ExampleB { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		public int DeckId { get; set; }
		[JsonIgnore]
		public MonkiDeck? Deck { get; set; }
	}
}
