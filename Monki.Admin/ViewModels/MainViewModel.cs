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

		public MainViewModel(IDeckService deckService, IUserService userService, IServiceProvider serviceProvider)
		{
			_deckService = deckService;
			_userService = userService;
			_serviceProvider = serviceProvider;
			users = new ObservableCollection<MonkiUser>(_userService.GetAll().ToList());
			decks = new ObservableCollection<MonkiDeck>(_deckService.GetAll().ToList());

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
	}
}
