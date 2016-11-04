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
    public sealed partial class BlankPage3 : Page
    {
        public BlankPage3()
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
            foreach(Lawyer l in App.CurrentCity.Lawyers)
            {
                Button b = new Button();
                b.Width = Window.Current.Bounds.Width;
                b.Height = 70;
                b.Content = l.Name;
               
                b.Tapped += (sender, eargs) =>
                {
                    foreach (Lawyer lawyer in App.CurrentCity.Lawyers)
                    {
                        if (lawyer.Name == ((Button)sender).Content.ToString())
                        {
                            App.CurrentLawyer = lawyer;
                            break;
                        }
                    }
                    Frame.Navigate(typeof(BlankPage4));

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
