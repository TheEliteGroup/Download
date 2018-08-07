using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISCM
{
    public interface call_web_service
    {
        void startLoginActivity();
        string hello_world();
        string[] get_farm_status(string email);
        string get_water_status(string email);
        string[] predict_crops(string email);
        void add_new_crop(string email, string cropid, string prod);
        string[] get_crops(string email);
        string[] get_bids(string email);
        string[] get_accepted_bids(string email);
        void set_bids(string email, string bid);
        void setBidFarmer(string email, string cropID, string prod, string rate);
        string[] getBidFarmer(string email);
        void addApproxProd(string email, string cropid, string approxProd);
        string[] getBidDetails(string bidid);
        string[] getBidDetailsMarket(string bid);
        string[] getMarketsFarmer(string cropid);
        string[] getMarketDetailsFarmer(string email, string marketid, string cropid);
    }
}
