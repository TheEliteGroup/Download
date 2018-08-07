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
    public partial class AcceptedBidsView : ContentPage
    {
        public ObservableCollection<AcceptedBidsModel> getAcceptedBids { get; set; }
        private Dictionary<string, string> CropItems = new Dictionary<string, string>() { };

        public List<KeyValuePair<string, string>> CropItemList = new List<KeyValuePair<string, string>>();
        public AcceptedBidsView()
        {
            InitializeComponent();
            String[] acceptedBidList = new String[100];
            acceptedBidList = DependencyService.Get<call_web_service>().get_accepted_bids(Global_portable.email);
            //System.Diagnostics.Debug.WriteLine("=================CropsP:{0}", acceptedBidList[0]);
            getAcceptedBids = new ObservableCollection<AcceptedBidsModel>();
            for (int i = 0; i < acceptedBidList.Length; i++)
            {
                string bidID = "";
                string cropName = "";
                string quantity = "";
                string rate = "";

                int currloc = 0;
                int nextloc = 0;
                nextloc = acceptedBidList[i].IndexOf(",", currloc);
                bidID = acceptedBidList[i].Substring(0, nextloc);
                currloc = nextloc + 1;
                nextloc = acceptedBidList[i].IndexOf(",", currloc);
                System.Diagnostics.Debug.WriteLine("======{0} - {1} - {2}=========", currloc, nextloc, bidID);
                cropName = acceptedBidList[i].Substring(currloc, (nextloc - currloc));
                currloc = nextloc + 1;
                nextloc = acceptedBidList[i].IndexOf(",", currloc);
                quantity = acceptedBidList[i].Substring(currloc, (nextloc - currloc));
                currloc = nextloc + 1;
                
                rate = acceptedBidList[i].Substring(currloc);
                System.Diagnostics.Debug.WriteLine("======{0} - {1} - {2}=========", bidID, cropName, quantity);

                //getAcceptedBids.Add(new AcceptedBidsModel { CropName = string.Format("Bid ID : {0} \t CropName : {1} \t Quantity : {2}", bidID, cropName, quantity) });
                CropItems.Add(bidID, string.Format("{0} \nCropName : {1} \nQuantity(Qtl) : {2}\nRate(Rs/Q) : {3}", bidID, cropName, quantity, rate));
            }

            //lstView.ItemsSource = getAcceptedBids;
            lstView.ItemsSource = CropItems.ToList();
        }
        void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem.ToString();
            int currloc = 0;
            int nextloc = 0;
            nextloc = item.IndexOf(",", currloc);
            string bidID = item.Substring(1, nextloc - 1);
            currloc = nextloc + 1;
            nextloc = item.IndexOf("]", currloc);
            string cropName = item.Substring(currloc + 1, (nextloc - currloc));

            System.Diagnostics.Debug.WriteLine("Selected==================={0}==={1}", bidID, cropName);

            Navigation.PushAsync(new FarmerDetailMarketView(bidID));





        }

    }
}