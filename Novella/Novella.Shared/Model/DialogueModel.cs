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
			try
			{
				Dialogues = await GetDialogues(filename);
				return Dialogues;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
        }

		public static async Task<ObservableCollection<Dialogue>> GetDialogues(string filename)
		{
			ObservableCollection<Dialogue> dialogues = new ObservableCollection<Dialogue>();

			try
			{
				string previousName = "";
				TextAlignment previousAlignment = TextAlignment.Right;

				// Get the file.
				//StorageFolder books = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Books");
				//StorageFile file = await books.GetFileAsync(filename);
				
				StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Books/" + filename));
				
				string text = await Windows.Storage.FileIO.ReadTextAsync(file);

				char[] sep = new char[] { '\r', '\n' };
				List<string> lines = new List<string>(text.Split(sep, StringSplitOptions.RemoveEmptyEntries));

				foreach (string line in lines)
				{
					var d = new Dialogue();
					var types = line.Split('#');

					d.LineType = (Constants.LineType)Enum.Parse(typeof(Constants.LineType), types[0]);

					switch (d.LineType)
					{
						case Constants.LineType.Banner:
							d.Alignment = TextAlignment.Center;
							break;
						case Constants.LineType.Notice:
							d.Alignment = TextAlignment.Center;
							break;
						case Constants.LineType.Dialogue:
							d.Alignment = TextAlignment.Left;
							break;
					}

					var speaker = types[1].Split('=');

					d.Name = speaker[0];
					if (string.IsNullOrEmpty(d.Name))
						d.Name = "Commentator";

					if (speaker.Length > 1)
						d.Line = speaker[1];

					if (d.LineType == Constants.LineType.Dialogue)
					{
						if (d.Name == previousName)
						{
							d.Alignment = previousAlignment;
						}
						else
						{
							if (previousAlignment == TextAlignment.Left)
								d.Alignment = TextAlignment.Right;
							else d.Alignment = TextAlignment.Left;
						}

						previousName = d.Name;
						previousAlignment = d.Alignment;
					}

					dialogues.Add(d);
				}

				return dialogues;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Customformat error:" + ex.Message);
				throw ex;
			}
		}

    }
}
