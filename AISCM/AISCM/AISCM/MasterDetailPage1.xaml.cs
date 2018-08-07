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
    public partial class MasterDetailPage1 : MasterDetailPage
    {
        public MasterDetailPage1()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterDetailPage1MenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;
            if(item.Title == "Home")
            {
                Detail = new NavigationPage(new index());
                IsPresented = false;
            }
            else if (item.Title == "Water Tank Status")
            {
                Detail = new NavigationPage(new display_water_related_data());
                IsPresented = false;
            }
            else if (item.Title == "Update PH")
            {
                Detail = new NavigationPage(new FarmDetails());
                IsPresented = false;
            }
            else if (item.Title == "Change Language")
            {
                Detail = new NavigationPage(new set_language());
                IsPresented = false;
            }
            else
            {
                Global_portable.email = "";
                Global_portable.user_id = 0;
                DependencyService.Get<call_web_service>().startLoginActivity();
                IsPresented = false;
            }

            MasterPage.ListView.SelectedItem = null;
        }
    }
}