using System;
using System.Collections.Generic;
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

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace App2
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPage2 : Page
    {
        public BlankPage2()
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
            foreach(City c in App.Cities)
            {
                Button b = new Button();
                b.Width = Window.Current.Bounds.Width;
                b.Height = 70;
                b.Content = c.Name;
                b.Tapped += (sender, eargs) =>
                {
                    foreach(City city in App.Cities)
                    {
                        if(city.Name == ((Button)sender).Content.ToString())
                        {
                            App.CurrentCity = city;
                            break;
                        }
                    }
                  Frame.Navigate(typeof(BlankPage3));
                    

                };
                Buttons.Children.Add(b);

            }
           

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();

        }

       
    }
}
