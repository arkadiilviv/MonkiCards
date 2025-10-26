using Monki.Admin.ViewModels;
using Monki.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monki.Admin.Controls
{
    /// <summary>
    /// Interaction logic for DeckListControl.xaml
    /// </summary>
    public partial class DeckListControl : UserControl
    {
        public DeckListControl()
        {
            InitializeComponent();
        }

		public static readonly DependencyProperty DecksListVmProperty =
			DependencyProperty.Register(
				nameof(DecksListVm),
				typeof(DeckListViewModel),
				typeof(DeckListControl),
				new PropertyMetadata(null));

		public DeckListViewModel DecksListVm
		{
			get => (DeckListViewModel)GetValue(DecksListVmProperty);
			set => SetValue(DecksListVmProperty, value);
		}
	}
}
