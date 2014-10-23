using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

namespace Novella
{
    /// <summary>
    /// Class to represent each dialogue present in the book
    /// </summary>
    public class Dialogue
    {
        public string Name { get; set; }
        public string Line { get; set; }
        public Constants.LineType LineType { get; set; }
        public Uri Picture { get; set; }
        public string BgColor { get; set; }
        public TextAlignment Alignment { get; set; }
    }
    
}
