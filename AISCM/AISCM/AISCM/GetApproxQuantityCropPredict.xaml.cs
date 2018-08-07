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
    public partial class GetApproxQuantityCropPredict : ContentPage
    {
        string cropID;
        public GetApproxQuantityCropPredict(string id)
        {
            InitializeComponent();
            cropID = id;
            

        }

        private void addCrop(object sender, EventArgs e)
        {
            var approxProd = Quant.Text;

            System.Diagnostics.Debug.WriteLine("CropsP:{0} - {1} - {2}", cropID, Global_portable.email, approxProd);
            DependencyService.Get<call_web_service>().add_new_crop(Global_portable.email, cropID, approxProd);
            //ToastNotification.Init();
            DisplayAlert("Alert", "Your Crop Is Ready To Be Sown", "OK");
            App.Current.MainPage = new MasterDetailPage1();
        }
    }
}