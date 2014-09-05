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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Novella
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BooksList : Page
    {
        ObservableCollection<Book> books;

        public BooksList()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            books = await BookModel.GetBooksList();

            //Books.ItemsSource = books;
            CoverFlow.ItemsSource = books;

        }

        private void Books_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Debug.WriteLine("Tapped");
            //var selected = (Book)e.ClickedItem;

            //this.Frame.Navigate(typeof(MainPage), selected);
        }

        private void Books_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Debug.WriteLine("ItemClick" + ((Book)e.ClickedItem).Name);
            var selected = (Book)e.ClickedItem;

            this.Frame.Navigate(typeof(MainPage), selected);
        }

        private void Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("SelectionChanged");
        }

        private void CoverFlow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selected = CoverFlow.SelectedItem;
            this.Frame.Navigate(typeof(MainPage), selected);
        }

        private void CoverFlow_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selected = CoverFlow.SelectedItem;
            this.Frame.Navigate(typeof(MainPage), selected);
        }

        

    }
}
