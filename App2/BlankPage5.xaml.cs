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
    public sealed partial class BlankPage5 : Page
    {
        public BlankPage5()
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

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Frame.Navigate(typeof(BlankPage2));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Buttons.Children.IndexOf((Button)sender)<=3)
            App.FileName = Buttons.Children.IndexOf((Button)sender).ToString();
            else App.FileName = (Buttons.Children.IndexOf((Button)sender)-1).ToString();
            Frame.Navigate(typeof(BlankPage7));


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();


        }
    }
}
