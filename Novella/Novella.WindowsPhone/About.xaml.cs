using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
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
    public sealed partial class About : Page
    {
        public About()
        {
            this.InitializeComponent();
        }

		private async void Contact_Click(object sender, RoutedEventArgs e)
		{
			bool success = await Launcher.LaunchUriAsync(new Uri("mailto:varunagrawal@outlook.com", UriKind.RelativeOrAbsolute));

			if (!success)
			{
				await new MessageDialog("Failed to launch app.").ShowAsync();
			}
		}

		private async void Rate_Click(object sender, RoutedEventArgs e)
		{
			await Launcher.LaunchUriAsync(new Uri("http://www.windowsphone.com/s?appid=4db3076d-8458-4072-b252-e248d56ade0c", UriKind.RelativeOrAbsolute));
		}
    }
}
