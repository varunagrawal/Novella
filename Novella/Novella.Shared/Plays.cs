using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using System.Xml;
using System.Xml.Linq;

namespace Novella
{
    public class Plays
    {
        public static ObservableCollection<Dialogue> GetPlayFromXML(string filename)
        {
            ObservableCollection<Dialogue> dialogues = new ObservableCollection<Dialogue>();

            try
            {
                XDocument xml = XDocument.Load(@"Books/" + filename);

                //XmlReader reader = XmlReader.Create(@"Books/" + filename);
                XmlSerializer ser = new XmlSerializer(typeof(Play));

                XmlReader reader = xml.CreateReader();
                Play play = (Play)ser.Deserialize(reader);

                return dialogues;

            }
            catch (Exception)
            {
                return null;
            }

        }
    }

    [XmlRoot(ElementName = "PLAY")]
    public class Play
    {
        [XmlElement("ACT")]
        public List<Act> Acts { get; set; }
    }

    public class Act
    {
        [XmlElement("TITLE")]
        public string Title { get; set; }

        [XmlElement("SCENE")]
        public List<Scene> Scenes { get; set; }
    }

    public class Scene
    {
        [XmlElement("TITLE")]
        public string Title { get; set; }

        [XmlElement("STAGEDIR")]
        public List<string> StageDirs { get; set; }
        
        [XmlElement("SPEECH")]
        public List<Speech> SceneLines { get; set; }

    }

    public class Speech
    {
        [XmlElement("SPEAKER")]
        public string Speaker { get; set; }

        [XmlElement("LINE")]
        public List<string> Line { get; set; }
    }
}
