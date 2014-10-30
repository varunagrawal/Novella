using CoverFlowControl;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Novella
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BooksList : Page
    {
        ObservableCollection<Book> books;

		private int CurrentBookIndex
		{
			get 
			{
				if(ApplicationData.Current.RoamingSettings.Values.ContainsKey("currentbookindex"))
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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            books = await BookModel.GetBooksList();

			CoverFlow.SpaceBetweenItems = 60.0;
			CoverFlow.ItemsSource = books;

			CoverFlow.LayoutUpdated += CoverFlow_LayoutUpdated;

        }

		void CoverFlow_LayoutUpdated(object sender, object e)
		{
			CoverFlow.SelectedIndex = CurrentBookIndex;
		}

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
 	        base.OnNavigatedFrom(e);
        }


        private void Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("SelectionChanged");
        }

        private void CoverFlow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selected = CoverFlow.SelectedItem;

			CurrentBookIndex = CoverFlow.SelectedIndex;

            this.Frame.Navigate(typeof(MainPage), selected);
        }

        /*
        private void CoverFlow_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selected = CoverFlow.SelectedItem;
            this.Frame.Navigate(typeof(MainPage), selected);
        }
        */
        
    }
}
