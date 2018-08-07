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
    public partial class CropMarketFarmerView : ContentPage
    {
        public CropMarketFarmerView()
        {
            InitializeComponent();
           // Global_portable.email = "aditya.sd12@gmail.com";
        }
        private async void OnSetNewBidsClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SetNewBidsFarmerView());

        }


        async void OnAcceptedBidsClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new AcceptedBidsFarmerView());
        }
        async void OnMarketsClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new MarketInputView());
        }

    }
}