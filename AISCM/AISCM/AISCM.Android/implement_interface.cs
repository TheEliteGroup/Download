using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using AISCM;

[assembly: Dependency(typeof(AISCM.Droid.implement_interface))]
namespace AISCM.Droid
{
    public class implement_interface : call_web_service
    {
        public implement_interface() { }
        net.azurewebsites.agc20171.AISCM a = new net.azurewebsites.agc20171.AISCM();

        public void startLoginActivity()
        {
            Global_portable.email = null;
            Global_portable.user_id = 0;
            Forms.Context.StartActivity(typeof(Login));
        }

       
        public string hello_world()
        {
            string msg = "";
            msg = a.HelloWorld();
            return msg;
        }
        public string[] get_farm_status(string email)
        {
            string[] msg = new string[100];
            msg = a.get_current_farm_status(email);
            return msg;
        }
        public string get_water_status(string email)
        {
            string msg = a.get_water_status(Global_portable.email);
            return msg;
        }
        public string[] predict_crops(string email)
        {
            string[] crops = new string[100];
            crops = a.predict_crops(Global_portable.email);
            return crops;
        }
        public void add_new_crop(string email, string cropid, string approxProd)
        {
            a.add_new_crop_farmer(email, cropid, approxProd);
        }

        public string[] get_crops(string email)
        {
            string[] crops = new string[100];
            crops = a.get_current_crops(Global_portable.email);
            return crops;
        }

        public string[] get_bids(string email)
        {
            string[] crops = new string[100];
            crops = a.get_current_bids(email);
            return crops;
        }

        public void set_bids(string email, string id)
        {
            a.accept_bid(email, id);

        }

        public string[] get_accepted_bids(string email)
        {
            string[] bids = new string[100];
            bids = a.get_bids(email);
            return bids;
        }

        public void setBidFarmer(string email, string cropID, string prod, string rate)
        {
            a.set_new_bid(email, cropID, prod, rate);
        }

        public string[] getBidFarmer(string email)
        {
            string[] bids = new string[100];
            bids = a.get_bids_farmer(email);
            return bids;
        }

        public string[] getBidDetails(string bidid)
        {
            string[] bids = new string[100];
            bids = a.get_bid_details(bidid);
            return bids;
        }

        public string[] getBidDetailsMarket(string bidid)
        {
            string[] bids = new string[100];
            bids = a.get_bid_details_mucp(bidid);
            return bids;
        }
        public string[] getMarketsFarmer(string cropid)
        {
            string[] markets = new string[100];
            markets = a.get_markets(cropid);
            return markets;
        }
        public string[] getMarketDetailsFarmer(string email, string mid, string cid)
        {
            string[] marketDetails = new string[100];
            marketDetails = a.get_market_details(email, mid, cid);
            return marketDetails;
        }

        public void addApproxProd(string email, string cropid, string approxProd)
        {
            a.add_appx_production(email, cropid, approxProd);
        }
    }
}