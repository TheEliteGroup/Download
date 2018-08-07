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
    public partial class homepage : ContentPage
    {
        public homepage()
        {
            InitializeComponent();
            
        }
        private async void OnMonitorStatusClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new monitorstatus());
        }
        private async void OnWaterStatusClicked(object sender,EventArgs e)
        {
            await Navigation.PushAsync(new display_water_related_data());
        }
    }
}