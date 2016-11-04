using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=391641

namespace App2
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var uri = new System.Uri("ms-appx:///TextFile1.txt", UriKind.Absolute);
            var sfile = Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
            sfile.Completed += (w, s) =>
            {
                StorageFile file = sfile.GetResults();
                var reader = file.OpenReadAsync();
                reader.Completed += async(a, b) =>
                {
                    var re = reader.GetResults();
                    StreamReader r = new StreamReader(re.AsStreamForRead());
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        MatchCollection matches = Regex.Matches(r.ReadToEnd(), "\"(.*?)\"");

                        string message = matches[new Random().Next(0, matches.Count-1)].Value;
                        Windows.UI.Popups.MessageDialog md = new Windows.UI.Popups.MessageDialog(message);
                         md.ShowAsync();
                    });





                };
            };
             var uri1 = new System.Uri("ms-appx:///lawyers.txt", UriKind.Absolute);
            var sfile1 = Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri1);

            sfile1.Completed += (c, d) =>
            {
                StorageFile file = sfile1.GetResults();
                var reader = file.OpenReadAsync();
                reader.Completed +=  (a, b) =>
                {
                    var re = reader.GetResults();
                    StreamReader r = new StreamReader(re.AsStreamForRead());
                    string text = r.ReadToEnd();
                    MatchCollection matches1 = Regex.Matches(text, "CityName\": (.*?)\",");
                    for (int i = 0; i != matches1.Count; i++)
                    {
                        string str = matches1[i].Value.Remove(0, 12);
                        str = str.Remove(str.Length - 2, 2);
                        App.Cities.Add(new City(str));
                    }
                };
            };


        }

        
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage5));

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage8));

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage2));

        }
    }
    
    
}
