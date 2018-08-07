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
    public partial class CropMarketView : ContentPage
    {
        public CropMarketView()
        {
            InitializeComponent();
            //Global_portable.email = "desai.mangala@gmail.com";
        }

        private async void OnAcceptBidsClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ListBidsView());

        }


        async void OnViewAcceptedClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new AcceptedBidsView());
        }
    }
}