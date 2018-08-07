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
    public partial class BidDetailFarmerView : ContentPage
    {
        private Dictionary<string, string> BidDetails = new Dictionary<string, string>() { };
        public BidDetailFarmerView(string id)
        {
            InitializeComponent();
            String[] bidDetails = new String[100];
            bidDetails = DependencyService.Get<call_web_service>().getBidDetails(id);
            System.Diagnostics.Debug.WriteLine("=================CropsDetails:{0}", bidDetails[0]);
            System.Diagnostics.Debug.WriteLine("=============={0}", id);
            BidDetails.Add("1", bidDetails[0]);
            lstView.ItemsSource = BidDetails.ToList();
        }

    }
}