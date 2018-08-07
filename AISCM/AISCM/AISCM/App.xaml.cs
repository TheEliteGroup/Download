using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AISCM
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if(Global_portable.user_id==2)
            MainPage = new MasterDetailPage1();
            else
            MainPage = new NavigationPage(new CropMarketView());
            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
