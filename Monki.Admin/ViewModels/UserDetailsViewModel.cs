using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Monki.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monki.Admin.ViewModels
{
	public partial class UserDetailsViewModel : ObservableObject
	{
		private readonly IServiceProvider _serviceProvider;
		private MonkiUser _monkiUser;
		[ObservableProperty]
		private string id;
		[ObservableProperty]
		private string userName;
		[ObservableProperty]
		private string createdAt;
		[ObservableProperty]
		private string updatedAt;
		[ObservableProperty]
		private string email;
		[ObservableProperty]
		private bool isActive;
		[ObservableProperty]
		private bool isDeleted;
		[ObservableProperty]
		private DeckListViewModel decksListVm;

		public UserDetailsViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public void InitModel(MonkiUser monkiUser)
		{
			_monkiUser = monkiUser;
			Id = monkiUser.Id;
			UserName = monkiUser.UserName!;
			CreatedAt = monkiUser.CreatedAt.ToString("g");
			UpdatedAt = monkiUser.UpdatedAt.ToString("g");
			Email = monkiUser.Email!;
			IsActive = monkiUser.IsActive;
			IsDeleted = monkiUser.IsDeleted;
			DecksListVm = new DeckListViewModel(
				monkiUser.Decks
					.Select(x => new DeckViewModel(x))
				, _serviceProvider);
		}
	}
}
