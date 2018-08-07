using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using AISCM.AISCM_Ref;
namespace AISCM
{
    public partial class MainPage : ContentPage
    {
        public string email = "";
        public string user_type = "";
        public MainPage()
        {
            InitializeComponent();
            NavigationPage np = new NavigationPage(new MasterDetailPage1());
            
            
        }
    }
}
