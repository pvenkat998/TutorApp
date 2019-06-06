using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForumMessageTabPage : TabbedPage
    {
        public ForumMessageTabPage()
        {
            Children.Add(new HomeDetail2());
            Children.Add(new Forum());
            InitializeComponent();

        }
    }
}