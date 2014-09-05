using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using System.IO;

namespace Novella
{
    class DialogueModel
    {
        public static ObservableCollection<Dialogue> Dialogues { get; set; }

        public DialogueModel()
        {
            Dialogues = new ObservableCollection<Dialogue>();

            // Sample data
            //Dialogues.Add(new Dialogue { Name = "ACT I", Line = "", Alignment = TextAlignment.Center });
            //Dialogues.Add(new Dialogue { Name = "Varun", Line = "Hey", Alignment = TextAlignment.Left });
            //Dialogues.Add(new Dialogue { Name = "Pratiksha", Line = "Yo bro!", Alignment = TextAlignment.Right });
        }

        public static ObservableCollection<Dialogue> GetDialoguesFromXml(string filename)
        {
            Dialogues = Plays.GetPlayFromXML(filename);

            if (Dialogues != null)
            {
                return Dialogues;
            }
            else
            {
                throw new NullReferenceException();
            }

        }

        public static async Task<ObservableCollection<Dialogue>> GetDialoguesFromFile(string filename)
        {
            Dialogues = await CustomFormat.GetDialogues(filename);

            if (Dialogues != null)
            {
                return Dialogues;
            }
            else
            {
                throw new NullReferenceException();
            }

        }
    }
}
