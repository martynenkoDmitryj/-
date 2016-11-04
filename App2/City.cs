using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace App2
{
    public class City
    {
        public string Name { get; set; }
        public List<Lawyer> Lawyers { get; set; }
        StreamReader reader;
        public City()
        {
            Lawyers = new List<Lawyer>();
        }

        public City(string name)
        {
            Name = name;


            var uri = new System.Uri("ms-appx:///lawyers.txt", UriKind.Absolute);
            var sfile = Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
            sfile.Completed += (w, s) =>
            {
                StorageFile file = sfile.GetResults();
                var reader1 = file.OpenReadAsync();
                reader1.Completed += (a, b) =>
                {
                    var re = reader1.GetResults();
                    reader = new StreamReader(re.AsStreamForRead());
                    Lawyers = GetLawyersByCity();
                };
            };


        }

        public void LoadLawyers(string cityName)
        {
            Name = cityName;

            var uri = new System.Uri("ms-appx:///lawyers.txt", UriKind.Absolute);
            var sfile = Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
            sfile.Completed += (w, s) =>
            {
                StorageFile file = sfile.GetResults();
                var reader1 = file.OpenReadAsync();
                reader1.Completed += (a, b) =>
                {
                    var re = reader1.GetResults();
                    reader = new StreamReader(re.AsStreamForRead());
                    Lawyers = GetLawyersByCity();
                };
            };
            Lawyers = GetLawyersByCity();
        }

        private List<Lawyer> GetLawyersByCity()
        {
            List<Lawyer> List = new List<Lawyer>();
            string text = reader.ReadToEnd();
            MatchCollection matches = Regex.Matches(text, "CityName\": (.*?)\",");

            for (int i = 0; i != matches.Count; i++)
            {
                string str = matches[i].Value.Remove(0, 12);
                if (str.Remove(str.Length - 2, 2) == Name)
                {

                    if (i + 1 < matches.Count)
                        text = text.Remove(text.IndexOf(matches[i + 1].Value));
                    break;

                }
            }
            text = text.Remove(text.Length - 12);
            text = text.Remove(0, text.Length - (text.Length - text.IndexOf(Name))).ToString();
            MatchCollection matches1 = Regex.Matches(text, "Phone\": (.*?)\",");
            for (int i = 0; i != matches1.Count; i++)
            {
                string email = Regex.Matches(text, "Email\": (.*?)\",")[i].Value.Remove(0, 9);
                string phone = matches1[i].Value.Remove(0, 9);
                string nam = Regex.Matches(text, "Name\": (.*?)\",")[i].Value.Remove(0, 8);
                List.Add(
                    new Lawyer(nam.Remove(nam.Length - 2), phone.Remove(phone.Length - 2), email.Remove(email.Length - 2)));
            }
            return List;
        }

    }
}
