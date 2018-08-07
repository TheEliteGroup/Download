using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AISCM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class showDetailMarketsFarmer : ContentPage
    {
        public ObservableCollection<SetBidsFarmerModel> crops { get; set; }
        // private Dictionary<string, string> PickerItems = new Dictionary<string, string>() { { "AF", "Afghanistan" }, { "AL", "Albania" } };


        private Dictionary<string, string> MarketDetails = new Dictionary<string, string>() { };

        public List<KeyValuePair<string, string>> CropItemList = new List<KeyValuePair<string, string>>();
        public showDetailMarketsFarmer(string cropID, string marketID)
        {
            InitializeComponent();
            InitializeComponent();
            String[] cropList = new String[100];
            System.Diagnostics.Debug.WriteLine("Market D======{0}==={1}==={2}==", Global_portable.email, cropID, marketID);
            cropList = DependencyService.Get<call_web_service>().getMarketDetailsFarmer(Global_portable.email, marketID, cropID);

            for (int i = 0; i < cropList.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine("Market Details======{0}", cropList[i]);
                MarketDetails.Add("1", cropList[i]);
            }
            lstView.ItemsSource = MarketDetails.ToList();
        }
    }
}