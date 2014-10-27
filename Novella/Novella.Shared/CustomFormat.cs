using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Novella
{
    public class CustomFormat
    {
        public static async Task<ObservableCollection<Dialogue>> GetDialogues(string filename)
        {
            ObservableCollection<Dialogue> dialogues = new ObservableCollection<Dialogue>();

            try
            {
                string previousName = "";
                TextAlignment previousAlignment = TextAlignment.Right;

                // Get the file.
                StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFolder books = await folder.GetFolderAsync("Books");

                var file = await books.GetFileAsync(filename);

                IList<string> lines = await Windows.Storage.FileIO.ReadLinesAsync(file);

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
            catch (NullReferenceException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ObservableCollection<Dialogue>();
            }
        }
    }
}
