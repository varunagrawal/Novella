using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.Popups;

namespace Novella
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
	public sealed partial class BooksList : Page
	{
		ObservableCollection<Book> books = null;

		private int CurrentBookIndex
		{
			get
			{
				if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("currentbookindex"))
					return (int)ApplicationData.Current.RoamingSettings.Values["currentbookindex"];
				else return 0;
			}
			set
			{
				ApplicationData.Current.RoamingSettings.Values["currentbookindex"] = value;
			}
		}

		public BooksList()
		{
			this.InitializeComponent();
		}


		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			LoadBooks();
		}


		private async void LoadBooks()
		{
			try
			{
				books = await BookModel.GetBooksList();

				if (App.IsTrial)
				{
					List<string> trialBooks = new List<string> { "Hamlet", "Julius Caesar", "Othello", "Romeo and Juliet" };
					books = new ObservableCollection<Book>(books.Where(x => trialBooks.Contains(x.Name)).ToList());
				}
			}
			catch (Exception)
			{
				books = null;
			}

			if (books == null)
			{
				MessageDialog md = new MessageDialog("Oops. Error getting the books. Please restart the app.");
				var x = await md.ShowAsync();


				App.Current.Exit();
			}
			else
			{
				Books.LayoutUpdated += Books_LayoutUpdated;

				Books.ItemsSource = books;
				//Books.UpdateLayout();
				//Books.ScrollIntoView(books[CurrentBookIndex]);
			}
		}


		void Books_LayoutUpdated(object sender, object e)
		{
			Books.SelectedIndex = CurrentBookIndex;
			Books.ScrollIntoView(Books.Items[CurrentBookIndex]);
			Books.LayoutUpdated -= Books_LayoutUpdated;
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
		}

		private void Books_Tapped(object sender, TappedRoutedEventArgs e)
		{
			var selected = Books.SelectedItem;
			CurrentBookIndex = Books.SelectedIndex;

			this.Frame.Navigate(typeof(Novella.MainPage), selected);
		}

		private void Books_ItemClick(object sender, ItemClickEventArgs e)
		{
			Book selected = e.ClickedItem as Book;
			CurrentBookIndex = books.IndexOf(selected);

			this.Frame.Navigate(typeof(Novella.MainPage), selected);
		}

		private void Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//ListView lv = (ListView)sender;
			//Book selected = lv.SelectedItem as Book;
			//CurrentBookIndex = lv.SelectedIndex;
			var selected = Books.SelectedItem;
			CurrentBookIndex = Books.SelectedIndex;

			this.Frame.Navigate(typeof(Novella.MainPage), selected);
		}

		private void About_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(About));
		}

		private void Help_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Help));
		}

	}
}
