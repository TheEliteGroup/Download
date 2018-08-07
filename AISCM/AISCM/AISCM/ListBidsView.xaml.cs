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
    public partial class ListBidsView : ContentPage
    {
        public ObservableCollection<ListBidsModel> bidds { get; set; }
        private Dictionary<string, string> CropItems = new Dictionary<string, string>() { };

        public List<KeyValuePair<string, string>> CropItemList = new List<KeyValuePair<string, string>>();
        public ListBidsView()
        {
            InitializeComponent();
            String[] bidList = new String[100];
            bidList = DependencyService.Get<call_web_service>().get_bids(Global_portable.email);

            bidds = new ObservableCollection<ListBidsModel>();
            System.Diagnostics.Debug.WriteLine("CropsP:{0}", bidList[0]);

            if (bidList != null)
            {
                for (int i = 0; i < bidList.Length; i++)
                {
                    string bidID = "";
                    string cropID = "";
                    string cropName = "";
                    string cropQuantity = "";
                    string cropRate = "";

                    int currloc = 0;
                    int nextloc = 0;
                    nextloc = bidList[i].IndexOf(",", currloc);
                    //System.Diagnostics.Debug.WriteLine("==========={0} - {1}==========", currloc, nextloc);
                    bidID = bidList[i].Substring(0, nextloc);
                    currloc = nextloc + 1;
                    nextloc = bidList[i].IndexOf(",", currloc);
                    cropID = bidList[i].Substring(currloc, (nextloc - currloc));
                    currloc = nextloc + 1;
                    nextloc = bidList[i].IndexOf(",", currloc);
                    //System.Diagnostics.Debug.WriteLine("==========={0} - {1}==========", currloc, nextloc);
                    // currloc = nextloc + 1;
                    // nextloc = bidList[i].IndexOf(",", currloc);
                    // System.Diagnostics.Debug.WriteLine("==========={0} - {1}==========", currloc, bidID);
                    cropName = bidList[i].Substring(currloc, (nextloc - currloc));
                    currloc = nextloc + 1;
                    nextloc = bidList[i].IndexOf(",", currloc);
                    cropQuantity = bidList[i].Substring(currloc);


                    System.Diagnostics.Debug.WriteLine("===========bID - {0} - Crop - {1}==========", bidID, cropName);



                    System.Diagnostics.Debug.WriteLine("Crops:{0} - {1} - {2} - {3}", bidID, cropName, cropQuantity, cropRate);
                    //bidds.Add(new ListBidsModel { Name = cropName, BidID = bidID, CropRate = cropRate, CropQuantity = cropQuantity });
                    CropItems.Add(bidID, string.Format("{0}\nCrop : {1}\nQuantiy(Qtl) : {2}",bidID, cropName, cropQuantity));
                }

                //lstView.ItemsSource = bidds;
                lstView.ItemsSource = CropItems.ToList();
            }
            else
            {
                bidds.Add(new ListBidsModel { Name = "No Bids Yet", });
            }
        }

        void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem.ToString();
            System.Diagnostics.Debug.WriteLine("Selected Crop==================={0}",item);
            int currloc = 0;
            int nextloc = 0;
            nextloc = item.IndexOf(",", currloc);
            string bidID = item.Substring(1, nextloc - 1);
            currloc = nextloc + 1;
            nextloc = item.IndexOf("]", currloc);
            string cropName = item.Substring(currloc + 1, (nextloc - currloc));

            DependencyService.Get<call_web_service>().set_bids(Global_portable.email, bidID);
            DisplayAlert("Alert", "New Bid Accepted Successfully!!!", "OK");
            System.Diagnostics.Debug.WriteLine("Selected Crop==================={0}==={1}===", bidID, Global_portable.email);

            Navigation.PushAsync(new CropMarketView());



        }
    }
}