using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TutorApp.Models
{
    class MasterMenuItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Color BackgroundColor { get; set; }
        public Type Targettype { get; set; }

        public MasterMenuItem(string Title, string IconSource, Color color, Type target)
        {
            this.Title = Title;
            this.IconSource = IconSource;
            this.BackgroundColor = color;
            this.Targettype = target;
        }
    }
}
