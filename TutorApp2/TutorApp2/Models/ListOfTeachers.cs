using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xamarin.Forms;
using Image = Xamarin.Forms.Image;

namespace TutorApp2.Models
{
    public class ListOfTeachers
    {
        public string name { get; set; }
        public string gakunen { get; set; }
        public string kamoku { get; set; }
        public string moyori { get; set; }
        public Image image  {get;set;}
    }
}
