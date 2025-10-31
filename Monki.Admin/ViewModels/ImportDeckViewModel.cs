using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using Monki.APKG;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace Monki.Admin.ViewModels
{
	public partial class ImportDeckViewModel : ObservableObject
	{
		private readonly IDeckService _deckService;
		private MonkiUser _monkiUser;
		[ObservableProperty]
		private ObservableCollection<MonkiCard> cards = new();
		[ObservableProperty]
		private bool isBusy = false;
		[ObservableProperty]
		private string deckName = string.Empty;
		[ObservableProperty]
		private string deckDescription = string.Empty;
		[ObservableProperty]
		private string cardCount = string.Empty;
		[ObservableProperty]
		private bool isPrivate = true;

		public ImportDeckViewModel(IDeckService deckService)
		{
			_deckService = deckService;
		}

		public void Initialize(MonkiUser monkiUser)
		{
			_monkiUser = monkiUser;
		}

		[RelayCommand]
		public async void SaveDeck()
		{
			if (Cards.Count == 0)
				return;
			IsBusy = true;
			MonkiDeck newDeck = new MonkiDeck
			{
				Name = DeckName,
				Description = DeckDescription,
				IsPrivate = IsPrivate,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				UserId = _monkiUser.Id,
				Cards = Cards.ToList()
			};
			await _deckService.AddAsync(newDeck);
			IsBusy = false;
			MessageBox.Show("Deck imported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		[RelayCommand]
		public async void OpenFile()
		{
			var dialog = new OpenFileDialog
			{
				Title = "Select a file",
				Filter = "Apkg File (*.apkg)|*.apkg|All Files (*.*)|*.*"
			};

			if (dialog.ShowDialog() == true)
			{
				IsBusy = true;
				Cards.Clear();
				string filePath = dialog.FileName;
				var apkgImporter = new MankiImport();


				var apkgList = await Task.Run(async () => await apkgImporter.ReadApkgAsync(filePath));

				DeckName = System.IO.Path.GetFileNameWithoutExtension(filePath);
				CardCount = apkgList.Count.ToString();

				Cards = new System.Collections.ObjectModel.ObservableCollection<MonkiCard>();

				foreach (var apkgCard in apkgList)
				{
					MonkiCard newCard = new MonkiCard
					{
						SideA = apkgCard.SideA,
						SideB = apkgCard.SideB,
						ExampleA = apkgCard.ExampleA ?? string.Empty,
						ExampleB = apkgCard.ExampleB ?? string.Empty,
						CreatedAt = DateTime.UtcNow,
						UpdatedAt = DateTime.UtcNow
					};
					Cards.Add(newCard);
				}

			}

			IsBusy = false;
		}
	}
}
