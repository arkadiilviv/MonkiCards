using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monki.Admin.ModelView;
using Monki.Admin.ViewModels;
using Monki.Admin.Windows;
using Monki.DAL;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using Monki.DAL.Services;
using SQLitePCL;
using System.Configuration;
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

			serviceCollection.AddTransient<DeckDetailsViewModel>();
			serviceCollection.AddTransient<UserDetailsViewModel>();
			serviceCollection.AddTransient<ImportDeckViewModel>();

			serviceCollection.AddTransient<DeckDetailsWindow>();
			serviceCollection.AddTransient<UserDetailsWindow>();
			serviceCollection.AddTransient<ImportDeckWindow>();


			_serviceCollection = serviceCollection.BuildServiceProvider();

			Batteries.Init();
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
