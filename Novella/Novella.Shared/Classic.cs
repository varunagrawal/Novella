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
        public static void LoadBooks()
        { 
            
        }

        public static async Task<ObservableCollection<Dialogue>> Load(string file)
        {
            ObservableCollection<Dialogue> dialogues = await DialogueModel.GetDialoguesFromFile(file);

            dialogues = AssignColorsToSpeakers(dialogues);

            return dialogues;
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
            //foreach (Dialogue d in dialogues)
            //{
            //    if (!Characters.Contains(d.Name))
            //    {
            //        Characters.Add(d.Name);
            //    }
            //}

            Color color = new Color();
            color.R = 0;
            color.G = 60;
            color.B = 0;

            foreach (string speaker in Characters)
            {
                foreach (var t in dialogues.Where(x => x.Name == speaker))
                    t.BgColor = "#" + color.ToString().Substring(3);

                color.G += 20;
            }

            return dialogues;
        }

    }
}
