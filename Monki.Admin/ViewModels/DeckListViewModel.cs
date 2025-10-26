using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Monki.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monki.Admin.ViewModels
{
	public partial class DeckListViewModel : ObservableObject
	{
		private readonly IServiceProvider _serviceProvider;
		[ObservableProperty]
		private ObservableCollection<DeckViewModel> decks;
		[ObservableProperty]
		private DeckViewModel? selectedDeck;

		public DeckListViewModel(IEnumerable<DeckViewModel> decks, IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			Decks = new ObservableCollection<DeckViewModel>(decks);
			selectedDeck = decks.FirstOrDefault();
		}

		[RelayCommand]
		private void OpenDeck()
		{
			var window = _serviceProvider.GetRequiredService<DeckDetailsWindow>();
			var vm = _serviceProvider.GetRequiredService<DeckDetailsViewModel>();
			vm.Initialize(selectedDeck._monkiDeck);

			window.DataContext = vm;
			window.Show();
		}
	}
}
