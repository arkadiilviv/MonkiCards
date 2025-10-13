using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monki.Admin.ModelView;
using Monki.DAL;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using Monki.DAL.Services;
using System;
using System.Configuration;
using System.Data;
using System.Printing;
using System.Windows;

namespace Monki.Admin
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static IServiceProvider _serviceCollection { get; private set; }

		public App()
		{
			string dbVal = ConfigurationManager.AppSettings["monkidb"];

			var serviceCollection = new ServiceCollection();
			serviceCollection.AddDbContextPool<MonkiContext>(opt =>
				opt.UseNpgsql(
					dbVal,
					npgsqlOptions => npgsqlOptions.MigrationsAssembly("Monki.DAL")
				)
			);
			serviceCollection.AddIdentity<MonkiUser, IdentityRole>()
				.AddEntityFrameworkStores<MonkiContext>()
				.AddDefaultTokenProviders();

			serviceCollection.AddScoped<UserManager<MonkiUser>>();
			serviceCollection.AddLogging();

			serviceCollection.AddScoped<IUserService, UserService>();
			serviceCollection.AddScoped<IDeckService, DeckService>();
			serviceCollection.AddScoped<ICardService, CardService>();


			serviceCollection.AddSingleton<MainViewModel>();


			_serviceCollection = serviceCollection.BuildServiceProvider();
		}
		protected override void OnStartup(StartupEventArgs e)
		{
			var mainWindow = new MainWindow
			{
				DataContext = _serviceCollection.GetRequiredService<MainViewModel>()
			};

			mainWindow.Show();
			base.OnStartup(e);
		}

		private void OnExit(object sender, ExitEventArgs e)
		{
			// Dispose of services if needed
			if (_serviceCollection is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
	}

}
