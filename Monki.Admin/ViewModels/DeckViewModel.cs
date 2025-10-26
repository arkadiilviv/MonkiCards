using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Monki.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monki.Admin.ViewModels
{
	public partial class DeckViewModel : ObservableObject
	{
		public MonkiDeck _monkiDeck;
		[ObservableProperty]
		private int id = 0;
		[ObservableProperty]
		private string name = string.Empty;
		[ObservableProperty]
		private string description = string.Empty;
		[ObservableProperty]
		private string userName = string.Empty;

		public DeckViewModel(MonkiDeck monkiDeck)
		{
			_monkiDeck = monkiDeck;
			Id = monkiDeck.Id;
			Name = monkiDeck.Name;
			Description = monkiDeck.Description;
			UserName = monkiDeck.User?.UserName ?? "Unknown";
		}
	}
}
