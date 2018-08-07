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
    public partial class FarmerDetailMarketView : ContentPage
    {
        private Dictionary<string, string> BidDetails = new Dictionary<string, string>() { };
        public FarmerDetailMarketView(string id)
        {
            InitializeComponent();
            String[] bidDetails = new String[100];
            bidDetails = DependencyService.Get<call_web_service>().getBidDetailsMarket(id);
            string farmerName = "";
            string address = "";
            string contact = "";
            int currloc = 0;
            int nextloc = 0;
            nextloc = bidDetails[1].IndexOf(",", currloc);
            farmerName = bidDetails[1].Substring(0, nextloc);
            currloc = nextloc + 1;
            nextloc = bidDetails[1].IndexOf(",", currloc);
            address = bidDetails[1].Substring(currloc, (nextloc - currloc));
            currloc = nextloc + 1;
            contact = bidDetails[1].Substring(currloc);
            
            System.Diagnostics.Debug.WriteLine("================FarmerDetails:{0}", bidDetails[1]);  // Same as sop.
            System.Diagnostics.Debug.WriteLine("=============={0}", id);
            BidDetails.Add("1", string.Format("Name : {0}\nAddress : {1}\nContact : {2}",farmerName,address,contact));
            lstView.ItemsSource = BidDetails.ToList();
        }
    }
}