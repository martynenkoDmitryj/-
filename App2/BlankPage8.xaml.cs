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
using Windows.UI.Popups;
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
    public sealed partial class BlankPage8 : Page
    {
        List<IPunishment> pun4Quest = new List<IPunishment>();
        List<IPunishment> pun4QuestNow = new List<IPunishment>();

        //List<IPunishment> punishment = new List<IPunishment>();
        //int currentQuestion = 0;
        int questionsCount = 0;
        string questionText = "";
        public BlankPage8()
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
            LoadQuestion();
            int jailCount = 0;
            int driveoutCount = 0;
            int fineCount = 0;

            foreach (IPunishment p in App.Punishment)
            {
                if (p is Jail)
                    jailCount += p.GetInt();
                else if (p is Driveout)
                    driveoutCount += p.GetInt();
                else if (p is Fine)
                    fineCount += p.GetInt();


            }
            string punisgmentText = (jailCount != 0 ? (jailCount / 12).ToString() + " лет лишения свободы," : "").ToString() + (driveoutCount != 0 ? driveoutCount.ToString() + " месяцев лишение права управления ТС, " : "").ToString() + (fineCount != 0 ? fineCount.ToString() + " руб. штрафа" : "").ToString();
            PunishmentSize.Text = punisgmentText;

            if(App.WasPressed)
            {

                NextButton.IsEnabled = true;
                YesButton.IsEnabled = false;
                NoButton.IsEnabled = false;
            }
            else
            {
                NextButton.IsEnabled = false;
                YesButton.IsEnabled = true;
                NoButton.IsEnabled = true;
            }
        }

        public void LoadQuestion()
        {

            NextButton.IsEnabled = false;
            YesButton.IsEnabled = true;
            NoButton.IsEnabled = true;

            var uri = new System.Uri("ms-appx:///game.txt", UriKind.Absolute);
            var sfile = Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
            sfile.Completed += (w, s) =>
            {
                StorageFile file = sfile.GetResults();
                var reader = file.OpenReadAsync();
                reader.Completed += async (a, b) =>
                {
                    var re = reader.GetResults();
                    StreamReader r = new StreamReader(re.AsStreamForRead());

                    string text = r.ReadToEnd();
                    MatchCollection matches = Regex.Matches(text, "Question\": (.*?)\",");
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        string q = matches[App.CurrentQuestion].Value.Remove(0, 12);
                        Question.Text = q.Remove(q.Length - 2);
                        questionText = Question.Text;
                        GetPunishments();

                    });

                };
            };
        }

        public void GetPunishments()
        {
            pun4Quest.Clear();
            var uri = new System.Uri("ms-appx:///game.txt", UriKind.Absolute);
            var sfile = Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
            sfile.Completed += (w, s) =>
            {
                StorageFile file = sfile.GetResults();
                var reader = file.OpenReadAsync();
                reader.Completed += async (a, b) =>
                {
                    var re = reader.GetResults();
                    StreamReader r = new StreamReader(re.AsStreamForRead());

                    string text = r.ReadToEnd();
                    MatchCollection matches = Regex.Matches(text, "Question\": (.*?)\",");
                    questionsCount = matches.Count;
                    for (int i = 0; i != matches.Count; i++)
                    {
                        string str = matches[i].Value.Remove(0, 12);
                        if (str.Remove(str.Length - 2, 2) == questionText)
                        {

                            if (i + 1 < matches.Count)
                                text = text.Remove(text.IndexOf(matches[i + 1].Value));
                            break;

                        }
                    }
                    text = text.Remove(text.Length - 12);
                    text = text.Remove(0, text.Length - (text.Length - text.IndexOf(questionText))).ToString();

                    MatchCollection matches1 = Regex.Matches(text, "Penalty\": (.*?),");
                    for (int i = 0; i != matches1.Count; i++)
                    {
                        string type = Regex.Matches(text, "Type\": (.*?)\",")[i].Value.Remove(0, 8);
                        string isGood = Regex.Matches(text, "IsGood\": (.*?)\",")[i].Value.Remove(0, 10);

                        string size = matches1[i].Value.Remove(0, 10);

                        switch (type.Remove(type.Length - 2))
                        {
                            case "many":
                                pun4Quest.Add(new Fine(size.Remove(size.Length - 1), isGood.Remove(isGood.Length-2) == "yes" ? true : false));
                                break;
                            case "driveout":
                                pun4Quest.Add(new Driveout(size.Remove(size.Length - 1), isGood.Remove(isGood.Length - 2) == "yes" ? true : false));
                                break;
                            case "jail":
                                pun4Quest.Add(new Jail(size.Remove(size.Length - 1), isGood.Remove(isGood.Length - 2) == "yes" ? true : false));
                                break;
                        }

                    }
                };
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            App.WasPressed = false;
            if (App.CurrentQuestion == 1 | App.CurrentQuestion == 3 | App.CurrentQuestion == 5 | App.CurrentQuestion == 7 | App.CurrentQuestion == 8 | App.CurrentQuestion == 10)
                AboutThem.Visibility = Visibility.Collapsed;
            else
                AboutThem.Visibility = Visibility.Visible;


          

            if (App.CurrentQuestion != questionsCount-1)
            {
                App.CurrentQuestion++;

                LoadQuestion();
            }
            else
            {

                int jailCount = 0;
                int driveoutCount = 0;
                int fineCount = 0;

                foreach (IPunishment p in App.Punishment)
                {
                    if (p is Jail)
                        jailCount += p.GetInt();
                    else if (p is Driveout)
                        driveoutCount += p.GetInt();
                    else if (p is Fine)
                        fineCount += p.GetInt();


                }
                string punisgmentText = (jailCount != 0 ? (jailCount / 12).ToString() + " лет лишения свободы," : "").ToString() + (driveoutCount != 0 ? driveoutCount.ToString() + " месяцев лишение права управления ТС, " : "").ToString() + (fineCount != 0 ? fineCount.ToString() + " руб. штрафа" : "").ToString();
                if (jailCount == 0 & driveoutCount == 0 & fineCount == 0)
                    punisgmentText = "нет";

                Windows.UI.Popups.MessageDialog md = new Windows.UI.Popups.MessageDialog("Наказание: " + punisgmentText, "Конец");
                md.Commands.Add(new UICommand("закрыть", (command) =>
                {
                    Frame.GoBack();
                    App.CurrentQuestion = 0;
                    App.Punishment.Clear();
                }));
                md.ShowAsync();

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)//yes
        {
            App.WasPressed = true;
            pun4QuestNow.Clear();
            Windows.UI.Popups.MessageDialog md = new Windows.UI.Popups.MessageDialog("Вам повезло с адвокатом?");
            md.Commands.Add(new UICommand("да", (command) =>
            {
                foreach (IPunishment pun in pun4Quest)
                {
                    if (pun.IsGood)
                    {
                        App.Punishment.Add(pun);
                        pun4QuestNow.Add(pun);
                    }
                }
                CountUserPunishment(pun4QuestNow);
                int jailCount = 0;
                int driveoutCount = 0;
                int fineCount = 0;

                foreach (IPunishment p in App.Punishment)
                {
                    if (p is Jail)
                        jailCount += p.GetInt();
                    else if (p is Driveout)
                        driveoutCount += p.GetInt();
                    else if (p is Fine)
                        fineCount += p.GetInt();


                }
                string punisgmentText = (jailCount != 0 ? (jailCount / 12).ToString() + " лет лишения свободы," : "").ToString() + (driveoutCount != 0 ? driveoutCount.ToString() + " месяцев лишение права управления ТС, " : "").ToString() + (fineCount != 0 ? fineCount.ToString() + " руб. штрафа" : "").ToString();
                if (jailCount == 0 & driveoutCount == 0 & fineCount == 0)
                    punisgmentText = "нет";
                PunishmentSize.Text = punisgmentText;


            }));
            md.Commands.Add(new UICommand("нет", (command) =>
            {
                foreach (IPunishment pun in pun4Quest)
                {
                    if (!pun.IsGood)
                    {
                        App.Punishment.Add(pun);
                        pun4QuestNow.Add(pun);

                    }
                }
                CountUserPunishment(pun4QuestNow);
                int jailCount = 0;
                int driveoutCount = 0;
                int fineCount = 0;

                foreach (IPunishment p in App.Punishment)
                {
                    if (p is Jail)
                        jailCount += p.GetInt();
                    else if (p is Driveout)
                        driveoutCount += p.GetInt();
                    else if (p is Fine)
                        fineCount += p.GetInt();


                }
                string punisgmentText = (jailCount != 0 ? (jailCount / 12).ToString() + " лет лишения свободы," : "").ToString() + (driveoutCount != 0 ? driveoutCount.ToString() + " месяцев лишение права управления ТС, " : "").ToString() + (fineCount != 0 ? fineCount.ToString() + " руб. штрафа" : "").ToString();
                if (jailCount == 0 & driveoutCount == 0 & fineCount == 0)
                    punisgmentText = "нет";
                PunishmentSize.Text = punisgmentText;



            }));
            md.ShowAsync();
          
            NextButton.IsEnabled = true;
            YesButton.IsEnabled = false;
            NoButton.IsEnabled = false;


           


        }

        private void Button_Click_3(object sender, RoutedEventArgs e)//no
        {
            App.WasPressed = true;
            NextButton.IsEnabled = true;
            YesButton.IsEnabled = false;
            NoButton.IsEnabled = false;
        }

        public void CountUserPunishment(List<IPunishment> pun)
        {
            int jailCount = 0;
            int driveoutCount = 0;
            int fineCount = 0;

            foreach(IPunishment p in pun)
            {
                if (p is Jail)
                    jailCount += p.GetInt();
                else if(p is Driveout)
                    driveoutCount += p.GetInt();
                else if(p is Fine)
                    fineCount += p.GetInt();

               
            }
            string punisgmentText = (jailCount != 0 ? (jailCount / 12).ToString() + " лет лишения свободы," : "").ToString() + (driveoutCount != 0 ? driveoutCount.ToString() + " месяцев лишение права управления ТС, " : "").ToString() + (fineCount != 0 ? fineCount.ToString() + " руб. штрафа" : "").ToString();
            if (jailCount == 0 & driveoutCount == 0 & fineCount == 0)
                punisgmentText = "нет";
            Windows.UI.Popups.MessageDialog md = new Windows.UI.Popups.MessageDialog(punisgmentText);

            md.ShowAsync();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            App.CurrentWebPage = "g" + (App.CurrentQuestion + 1).ToString();
            Frame.Navigate(typeof(BlankPage6));
        }
    }
}

