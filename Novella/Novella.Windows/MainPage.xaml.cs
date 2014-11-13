using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Novella.Utility;
using System.Collections.ObjectModel;
using Windows.UI.Input;
using Windows.UI.Popups;
using Windows.ApplicationModel.DataTransfer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Novella
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Book b;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            b = e.Parameter as Book;
            ObservableCollection<Dialogue> dialogues = await Classic.Load(b.FileName);
			
			if(dialogues == null)
			{
				MessageDialog md = new MessageDialog("Error loading book.");
				await md.ShowAsync();

				return;
			}

            Dialogues.DataContext = dialogues;

            Dialogues.UpdateLayout();

            string bookmark = Classic.GetBookmark(b.FileName);
            if (!string.IsNullOrEmpty(bookmark))
            {
                Dialogue d = dialogues.Where(x => x.Line == bookmark).SingleOrDefault();

                if (d != null)
                    Dialogues.ScrollIntoView(d);
            }
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //StateManager.SaveScrollViewerOffset(Dialogues);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void Bookmark_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem element = sender as MenuFlyoutItem;
            if (element == null) return;

            Dialogue d = element.DataContext as Dialogue;

            Classic.AddBookmark(b.FileName, d);
        }

        private void Dialogue_Holding(object sender, HoldingRoutedEventArgs args)
        {
            // this event is fired multiple times. We do not want to show the menu twice
            if (args.HoldingState != HoldingState.Started) return;

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            // If the menu was attached properly, we just need to call this handy method
            FlyoutBase.ShowAttachedFlyout(element);
        }

        private void StackPanel_RightTapped(object sender, RightTappedRoutedEventArgs args)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            // If the menu was attached properly, we just need to call this handy method
            FlyoutBase.ShowAttachedFlyout(element);
        }

		private void Share_Click(object sender, RoutedEventArgs e)
		{
			MenuFlyoutItem element = sender as MenuFlyoutItem;
			if (element == null) return;
			Dialogue d = element.DataContext as Dialogue;

			DataTransferManager dtm = DataTransferManager.GetForCurrentView();
			dtm.DataRequested += (s, requestArgs) =>
			{
				DataRequest dr = requestArgs.Request;

				dr.Data.Properties.Title = "Novella";
				dr.Data.Properties.Description = b.Name;
				dr.Data.SetText(d.Line);
			};

			DataTransferManager.ShowShareUI();
		}
        
    }
}
