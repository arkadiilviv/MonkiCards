using CommunityToolkit.Mvvm.ComponentModel;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monki.Admin.ViewModels
{
	public partial class CardsViewModel : ObservableObject
	{
		private readonly ICardService _cardService;

		[ObservableProperty]
		private MonkiDeck? deck;
		[ObservableProperty]
		private ObservableCollection<MonkiCard> cards;
		[ObservableProperty]
		private MonkiCard? selectedCard;

		public CardsViewModel(ICardService cardService)
		{
			_cardService = cardService;
		}
		public void Initialize(MonkiDeck deck)
		{
			Deck = deck;
			Cards = new ObservableCollection<MonkiCard>(_cardService.GetAll().Where(x => x.DeckId == deck.Id));
		}
	}
}
