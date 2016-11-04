using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace App2
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPage7 : Page
    {
        public BlankPage7()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var uri = new System.Uri("ms-appx:///"+App.FileName+".txt", UriKind.Absolute);
            var sfile = Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
            sfile.Completed += (w, s) =>
            {
                StorageFile file = sfile.GetResults();
                var reader = file.OpenReadAsync();
                reader.Completed += async (a, b) =>
                {
                    var re = reader.GetResults();
                    StreamReader r = new StreamReader(re.AsStreamForRead());
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        while (r.Peek() >= 0)
                        {
                            Button bu = new Button();
                            bu.Width = Window.Current.Bounds.Width;
                            bu.Height = 70;
                            string str = r.ReadLine();

                            bu.Tag = str.Remove(2);
                            bu.Content = str.Remove(0, 2);
                            bu.Tapped += (sender, eargs) =>
                            {
                                App.CurrentWebPage = bu.Tag.ToString();
                                Frame.Navigate(typeof(BlankPage6));


                            };
                            Buttons.Children.Add(bu);

                        }
                    });





                };
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
