using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Monki.Admin.ViewModels;
using Monki.Admin.Windows;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Windows;

namespace Monki.Admin.ModelView
{
	public partial class MainViewModel : ObservableObject
	{
		private readonly IDeckService _deckService;
		private readonly IUserService _userService;
		private readonly IServiceProvider _serviceProvider;

		[ObservableProperty]
		private DeckListViewModel decksListVm;
		[ObservableProperty]
		private MonkiUser? selectedUser;
		[ObservableProperty]
		private ObservableCollection<MonkiUser> users = new();
		[ObservableProperty]
		private string deckSearchText = string.Empty;
		[ObservableProperty]
		private string userSearchText = string.Empty;

		public MainViewModel(IDeckService deckService, IUserService userService, IServiceProvider serviceProvider)
		{
			try
			{
				_deckService = deckService;
				_userService = userService;
				_serviceProvider = serviceProvider;
				users = new ObservableCollection<MonkiUser>(_userService.GetAll().ToList());
				DecksListVm = new DeckListViewModel(
					_deckService.GetAll()
						.Select(x => new DeckViewModel(x)),
					_serviceProvider);
			}
			catch (Exception)
			{
				MessageBox.Show("Could not connect to the database. Please check your connection settings and try again.", "Database Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
				System.Windows.Application.Current.Shutdown();
			}
		}

		partial void OnDeckSearchTextChanged(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				DecksListVm.Decks = new ObservableCollection<DeckViewModel>(
					_deckService.GetAll()
						.Select(x => new DeckViewModel(x))
				);
				return;
			}
			DecksListVm.Decks = new ObservableCollection<DeckViewModel>(
				_deckService.GetAll()
					.Where(x => x.Name.ToLower().Contains(value.ToLower()))
					.Select(x => new DeckViewModel(x))
				);
		}

		partial void OnUserSearchTextChanged(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				Users = new ObservableCollection<MonkiUser>(_userService.GetAll().ToList());
				return;
			}
			Users = new ObservableCollection<MonkiUser>(_userService.GetAll().Where(x => x.UserName.ToLower().Contains(value.ToLower())).ToList());
		}

		[RelayCommand]
		public void OpenUser()
		{
			if (SelectedUser == null)
				return;
			var window = _serviceProvider.GetRequiredService<UserDetailsWindow>();
			var vm = _serviceProvider.GetRequiredService<UserDetailsViewModel>();
			vm.InitModel(SelectedUser);
			window.DataContext = vm;
			window.Show();
		}
	}
}
