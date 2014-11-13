using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using System.Linq;
using Windows.UI;

namespace Novella
{
    public class Classic
    {
        public static async Task<ObservableCollection<Dialogue>> Load(string file)
        {
			try
			{
				ObservableCollection<Dialogue> dialogues = await DialogueModel.GetDialoguesFromFile(file);

				dialogues = AssignColorsToSpeakers(dialogues);

				return dialogues;
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Load error: " + ex.Message);
				return null;
			}
        }

        public static ObservableCollection<Dialogue> LoadXml(string file)
        {
            ObservableCollection<Dialogue> dialogues = DialogueModel.GetDialoguesFromXml(file);

            return dialogues;
        }

        private static ObservableCollection<Dialogue> AssignColorsToSpeakers(ObservableCollection<Dialogue> dialogues)
        {
            List<string> Characters = new List<string>();

            Characters = dialogues.Where(x => x.LineType == Constants.LineType.Dialogue).Select(x => x.Name).Distinct().ToList();

            Color color = new Color();
            color.R = 0;
            color.G = 40;
            color.B = 0;

            foreach (string speaker in Characters)
            {
                foreach (var t in dialogues.Where(x => x.Name == speaker))
                    t.BgColor = "#" + color.ToString().Substring(3);

                color.G += 10;
            }

            return dialogues;
        }

        public static void AddBookmark(string book, Dialogue d)
        {
			var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

			roamingSettings.Values[book] = d.Line;

        }

        public static string GetBookmark(string book)
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey(book))
                return (string)roamingSettings.Values[book];
            else return null;
        }
    }
}
