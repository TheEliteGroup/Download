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
    public partial class showMarketsFarmer : ContentPage
    {
        string cropID;
        public ObservableCollection<SetBidsFarmerModel> crops { get; set; }
        // private Dictionary<string, string> PickerItems = new Dictionary<string, string>() { { "AF", "Afghanistan" }, { "AL", "Albania" } };


        private Dictionary<string, string> CropItems = new Dictionary<string, string>() { };

        public List<KeyValuePair<string, string>> CropItemList = new List<KeyValuePair<string, string>>();
        public showMarketsFarmer(string id)
        {
            InitializeComponent();
            String[] cropList = new String[100];
            cropID = id;
            cropList = DependencyService.Get<call_web_service>().getMarketsFarmer(cropID);
            System.Diagnostics.Debug.WriteLine("MarketList length======{0}", cropList.Length);
            for (int i = 0; i < cropList.Length; i++)
            {
                string marketID = "";
                string marketName = "";

                int currloc = 0;
                int nextloc = 0;
                nextloc = cropList[i].IndexOf(",", currloc);
                marketID = cropList[i].Substring(0, nextloc);
                currloc = nextloc + 1;

                marketName = cropList[i].Substring(currloc);
                System.Diagnostics.Debug.WriteLine("MarketList======{0}", cropList[i]);
                CropItems.Add(marketID, string.Format("Market : {0}", marketName));
            }
            lstView.ItemsSource = CropItems.ToList();
        }
        void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem.ToString();
            int currloc = 0;
            int nextloc = 0;
            nextloc = item.IndexOf(",", currloc);
            string mID = item.Substring(1, nextloc - 1);
            currloc = nextloc + 1;
            nextloc = item.IndexOf("]", currloc);
            string mName = item.Substring(currloc + 1, (nextloc - currloc));

            System.Diagnostics.Debug.WriteLine("Selected==================={0}==={1}", mID, mName);

            Navigation.PushAsync(new showDetailMarketsFarmer(cropID, mID));
        }
    }
}