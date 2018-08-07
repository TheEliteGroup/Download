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
    public partial class AcceptedBidsFarmerView : ContentPage
    {
        public ObservableCollection<AcceptedBidsFarmerModel> getAcceptedBids { get; set; }

        private Dictionary<string, string> CropItems = new Dictionary<string, string>() { };

        public List<KeyValuePair<string, string>> CropItemList = new List<KeyValuePair<string, string>>();

        public AcceptedBidsFarmerView()
        {
            InitializeComponent();
            int cnt = 0;
            BindingContext = new AcceptedBidsFarmerModel();
            String[] acceptedBidList = new String[100];
            System.Diagnostics.Debug.WriteLine("=================CropsP:{0}", Global_portable.email);
            acceptedBidList = DependencyService.Get<call_web_service>().getBidFarmer(Global_portable.email);
            
            getAcceptedBids = new ObservableCollection<AcceptedBidsFarmerModel>();
            for (int i = 0; i < acceptedBidList.Length; i++)
            {
                cnt++;
                string bidID = "";
                string cropName = "";
                string rate = "";
                string quantity = "";
                string status = "";

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
                nextloc = acceptedBidList[i].IndexOf(",", currloc);
                rate = acceptedBidList[i].Substring(currloc, (nextloc - currloc));
                currloc = nextloc + 1;
                status = acceptedBidList[i].Substring(currloc);
                System.Diagnostics.Debug.WriteLine("======{0} - {1} - {2} - {3}=========", bidID, cropName, quantity, rate);

                //getAcceptedBids.Add(new AcceptedBidsModel { CropName = string.Format("Bid ID : {0} \t CropName : {1} \t Quantity : {2} \t Rate : {3} ", bidID, cropName, quantity, rate) });
                //getAcceptedBids.Add(new AcceptedBidsFarmerModel { CropName = string.Format("Bid ID : {0} \n CropName : {1} \n Quantity : {2}", bidID, cropName, quantity), BidID = bidID });
                CropItems.Add(bidID, string.Format("{0}\nCrop : {1}\nQuantity(Qtl) : {2}\nRate(Rs/Q) : {3}\nStatus : {4}", bidID, cropName, quantity, rate, status));

            }
            lstView.ItemsSource = CropItems.ToList();
            //lstView.ItemsSource = getAcceptedBids;
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

            Navigation.PushAsync(new BidDetailFarmerView(bidID));
        }
    }
}