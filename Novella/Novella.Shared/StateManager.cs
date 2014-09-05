using System.Windows;
using Windows.Storage;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Novella.Utility
{
    public static class StateManager
    {
        #region Methods

        public static void SaveScrollViewerOffset(DependencyObject dependencyObject)
        {
            try
            {
                ScrollViewer scrollViewer = GetScrollViewer(dependencyObject);

                if (scrollViewer != null)
                {
                    string key = GetUniqueKey(dependencyObject);

                    ApplicationData.Current.RoamingSettings.Values[key] = scrollViewer.VerticalOffset;
                }
            }
            catch
            {
            }
        }

        public static void RestoreScrollViewerOffset(DependencyObject dependencyObject)
        {
            try
            {
                ScrollViewer scrollViewer = GetScrollViewer(dependencyObject);

                if (scrollViewer != null)
                {
                    string key = GetUniqueKey(dependencyObject);

                    if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(key))
                    {
                        scrollViewer.ChangeView(0, (double)ApplicationData.Current.RoamingSettings.Values[key], 1);

                        ApplicationData.Current.RoamingSettings.Values.Remove(key);
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region Private Methods

        private static ScrollViewer GetScrollViewer(DependencyObject dependencyObject)
        {
            ScrollViewer scrollViewer = null;

            if (dependencyObject is ScrollViewer)
            {
                scrollViewer = dependencyObject as ScrollViewer;
            }
            else
            {
                FrameworkElement frameworkElement = VisualTreeHelper.GetChild(dependencyObject, 0) as FrameworkElement;

                if (frameworkElement != null)
                {
                    scrollViewer = frameworkElement.FindName("ScrollViewer") as ScrollViewer;
                }
            }

            return scrollViewer;
        }

        private static Page GetPage(FrameworkElement frameworkElement)
        {
            Page phoneApplicationPage = null;

            while (frameworkElement != null)
            {
                if (frameworkElement.Parent is Page)
                {
                    phoneApplicationPage = frameworkElement.Parent as Page;
                    break;
                }

                frameworkElement = frameworkElement.Parent as FrameworkElement;
            }

            return phoneApplicationPage;
        }

        private static string GetUniqueKey(DependencyObject dependencyObject)
        {
            string key = "ScrollOffset";

            FrameworkElement frameworkElement = dependencyObject as FrameworkElement;

            if (frameworkElement != null)
            {
                Page page = GetPage(frameworkElement);

                key = page != null ? page.GetType().Name + frameworkElement.Name + "ScrollOffset" : frameworkElement.Name + "ScrollOffset";
            }

            return key;
        }

        #endregion
    }
}