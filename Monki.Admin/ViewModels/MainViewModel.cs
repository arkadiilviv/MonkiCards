using CommunityToolkit.Mvvm.ComponentModel;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using System.Collections.ObjectModel;

namespace Monki.Admin.ModelView
{
	public partial class MainViewModel : ObservableObject
	{
		private readonly IDeckService _deckService;
		private readonly IUserService _userService;
		[ObservableProperty]
		private ObservableCollection<MonkiDeck> decks = new();
		[ObservableProperty]
		private ObservableCollection<MonkiUser> users = new();
		public MainViewModel(IDeckService deckService, IUserService userService)
		{
			_deckService = deckService;
			_userService = userService;
			users = new ObservableCollection<MonkiUser>(_userService.GetAll().ToList());
			decks = new ObservableCollection<MonkiDeck>(_deckService.GetAll().ToList());

		}
	}
}
