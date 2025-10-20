using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Monki.Admin.ViewModels;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using System.Collections.ObjectModel;

namespace Monki.Admin.ModelView
{
	public partial class MainViewModel : ObservableObject
	{
		private readonly IDeckService _deckService;
		private readonly IUserService _userService;
		private readonly IServiceProvider _serviceProvider;

		[ObservableProperty]
		private MonkiDeck? selectedDeck;
		[ObservableProperty]
		private ObservableCollection<MonkiDeck> decks = new();
		[ObservableProperty]
		private ObservableCollection<MonkiUser> users = new();
		[ObservableProperty]
		private string deckSearchText = string.Empty;

		public MainViewModel(IDeckService deckService, IUserService userService, IServiceProvider serviceProvider)
		{
			_deckService = deckService;
			_userService = userService;
			_serviceProvider = serviceProvider;
			users = new ObservableCollection<MonkiUser>(_userService.GetAll().ToList());
			decks = new ObservableCollection<MonkiDeck>(_deckService.GetAll().ToList());

		}

		partial void OnDeckSearchTextChanging(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				Decks = new ObservableCollection<MonkiDeck>(_deckService.GetAll().ToList());
				return;
			}
			Decks = new ObservableCollection<MonkiDeck>(_deckService.GetAll().Where(x => x.Name.ToLower().Contains(value.ToLower())).ToList());
		}

		[RelayCommand]
		private void OpenCards()
		{
			var window = _serviceProvider.GetRequiredService<CardsWindow>();
			var vm = _serviceProvider.GetRequiredService<CardsViewModel>();
			vm.Initialize(SelectedDeck);

			window.DataContext = vm;
			window.ShowDialog();
		}

		[RelayCommand]
		private void DecksNext()
		{
			var window = _serviceProvider.GetRequiredService<CardsWindow>();
			var vm = _serviceProvider.GetRequiredService<CardsViewModel>();
			vm.Initialize(SelectedDeck);

			window.DataContext = vm;
			window.ShowDialog();
		}
		[RelayCommand]
		private void DecksPrevious()
		{
			var window = _serviceProvider.GetRequiredService<CardsWindow>();
			var vm = _serviceProvider.GetRequiredService<CardsViewModel>();
			vm.Initialize(SelectedDeck);

			window.DataContext = vm;
			window.ShowDialog();
		}
	}
}
