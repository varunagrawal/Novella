using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Store;
using Windows.UI.Popups;
using System.Threading.Tasks;

namespace Novella
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
		private static LicenseInformation licenseInformation;

		private static bool isTrial = true;
		public static bool IsTrial
		{
			get
			{
				return isTrial;
			}
			set
			{
				isTrial = value;
			}
		}

		private bool InitializeLicense()
		{
			licenseInformation = CurrentApp.LicenseInformation;

			//licenseInformation.LicenseChanged += licenseInformation_LicenseChanged;

			return licenseInformation.IsTrial;
		}

		/// <summary>
		/// Check to see the status of the License Change
		/// </summary>
		private async void licenseInformation_LicenseChanged()
		{
			await CheckLicenseInformation();	
		}

		public async static Task CheckLicenseInformation()
		{
			if (licenseInformation.IsActive)
			{
				if (licenseInformation.IsTrial)
				{
					App.IsTrial = true;

					String message = string.Format("Buy the full version to get access to all of Novella!");
					MessageDialog md = new MessageDialog(message);
					md.Commands.Add(new UICommand { Label = "Buy", Id = 0 });
					md.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
					var result = await md.ShowAsync();

					if (result == null)
						App.Current.Exit();

					if ((int)result.Id == 0)
					{
						await Windows.System.Launcher.LaunchUriAsync(new Uri("http://www.windowsphone.com/s?appid=4db3076d-8458-4072-b252-e248d56ade0c"));
					}
					else if ((int)result.Id == 1)
					{
						//Exit();
					}

				}
				else
				{
					App.IsTrial = false;
					String message = "Thank you for buying the full version of Novella! Hope you enjoy it.";
					MessageDialog md = new MessageDialog(message);
					await md.ShowAsync();
				}
			}
			else
			{
				String message = "You don't have an active license! Get the complete features.";
				MessageDialog md = new MessageDialog(message);
				md.Commands.Add(new UICommand { Label = "Buy", Id = 0 });
				md.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
				var result = await md.ShowAsync();

				if ((int)result.Id == 0)
				{
					await Windows.System.Launcher.LaunchUriAsync(new Uri("http://www.windowsphone.com/s?appid=4db3076d-8458-4072-b252-e248d56ade0c"));
				}
				else if ((int)result.Id == 1)
				{
					//Exit();
				}
			}
		}

#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

			this.UnhandledException += App_UnhandledException;
		
            #if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            #endif
        }

		void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(e.Message);
		}

        #if WINDOWS_PHONE_APP
        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null && rootFrame.CanGoBack && !e.Handled)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }
#endif

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

			App.IsTrial = InitializeLicense();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(BooksList), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

			// TODO: Save application state and stop any background activity
            deferral.Complete();
        }

    }
}