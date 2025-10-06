using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monki.DAL.Models
{
	public class MonkiDeck
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public bool IsPrivate { get; set; } = true;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		public string UserId { get; set; } = string.Empty;
		[JsonIgnore]
		public MonkiUser? User { get; set; }
		public List<MonkiCard> Cards { get; set; } = new List<MonkiCard>();
	}
}
