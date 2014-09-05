using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

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
           
            return dialogues;
        }

        public static ObservableCollection<Dialogue> LoadXml(string file)
        {
            ObservableCollection<Dialogue> dialogues = DialogueModel.GetDialoguesFromXml(file);

            return dialogues;
        }

    }
}
