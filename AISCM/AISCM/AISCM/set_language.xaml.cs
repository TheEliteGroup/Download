using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AISCM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class set_language : ContentPage
    {
        public set_language()
        {
            InitializeComponent();
        }
        private void change_language_english(object sender, EventArgs e)
        {
            Global_portable.default_language = "en-IN";
            App.Current.MainPage = new MasterDetailPage1();
        }
        private void change_language_hindi(object sender,EventArgs e)
        {
            Global_portable.default_language = "hi-IN";
            App.Current.MainPage = new MasterDetailPage1();
        }
    }
}