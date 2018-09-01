using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using BogdanM.LocationServices;
using System.IO;
using GoogleMaps.LocationServices;

namespace A_G_C
{
    /// <summary>
    /// Summary description for DB nameCM
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DB nameCM : System.Web.Services.WebService
    {
        string status = "";
        public const string API_KEY = "Enter your API Key";
        public string MESSAGE = "Hello, Xamarin!";
        [WebMethod]
        public string[] get_markets(string cropid)
        {
            string[] msg;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand get_markets = new SqlCommand("select m.market_id,m.market_name from markets m,crop_rate cr where cr.CropId=@cropid and cr.market_id=m.market_id", con);
                get_markets.Parameters.AddWithValue("@cropid", cropid);

                SqlDataAdapter gm_adp = new SqlDataAdapter(get_markets);
                DataTable gm_dt = new DataTable();
                gm_adp.Fill(gm_dt);
                msg = new string[gm_dt.Rows.Count];
                int i = 0;
                foreach(DataRow r in gm_dt.Rows)
                {
                 
                    msg[i++] = r["market_id"].ToString() + "," + r["market_name"].ToString();
                }

            }
            finally
            {
                con.Close();
            }
            

            return msg;
        }
        [WebMethod]
        public void sell_to_market(string email,string cropid)
        {
            
        }
        [WebMethod]
        public string[] get_market_details(string email,string marketid,string cropid)
        {
            string[] msg;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand get_markets = new SqlCommand("select m.market_name,m.market_address,cr.rate,f.farm_lat,f.farm_long,m.market_lat,m.market_long,ucd.approximate_production from farm_data f,markets m,crop_rate cr,user_crop_data ucd where f.user_email=@email and ucd.user_email=@email and ucd.CropId=@cropid and ucd.crop_sold=0 and m.market_id=@mid and cr.CropId=@cropid and cr.market_id=m.market_id", con);
                get_markets.Parameters.AddWithValue("@mid", marketid);
                get_markets.Parameters.AddWithValue("@cropid", cropid);
                get_markets.Parameters.AddWithValue("@email", email);
                SqlDataAdapter gm_adp = new SqlDataAdapter(get_markets);
                DataTable gm_dt = new DataTable();
                gm_adp.Fill(gm_dt);
                int i = 0;
                msg = new string[gm_dt.Rows.Count];
                foreach(DataRow r in gm_dt.Rows)
                {
                    double distance = 0.0;
                    
                    double farmer_lat =Convert.ToDouble(r["farm_lat"]);
                    double farmer_lng = Convert.ToDouble(r["farm_long"]);

                    double dLat1InRad = farmer_lat * (Math.PI / 180.0);
                    double dLong1InRad = farmer_lng * (Math.PI / 180.0);

                    double market_lat = Convert.ToDouble(r["market_lat"]);
                    double market_lng = Convert.ToDouble(r["market_long"]);
                    double dDistance = Double.MinValue;
                    double dLat2InRad = market_lat * (Math.PI / 180.0);
                    double dLong2InRad = market_lng * (Math.PI / 180.0);

                    double dLongitude = dLong2InRad - dLong1InRad;
                    double dLatitude = dLat2InRad - dLat1InRad;

                    // Intermediate result a.
                    double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                               Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                               Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

                    // Intermediate result c (great circle distance in Radians).
                    double c = 2.0 * Math.Asin(Math.Sqrt(a));

                    // Distance.
                    // const Double kEarthRadiusMiles = 3956.0;
                    const Double kEarthRadiusKms = 6376.5;
                    dDistance = kEarthRadiusKms * c;
                    int appx_prod =Convert.ToInt32( r["approximate_production"].ToString());
                    int transport_cost = 0;
                    int rate = Convert.ToInt16(r["rate"].ToString());
                    int total_selling_cost = rate * appx_prod;
                    int total_ben = 0;
                    int count = 1;
                    if(appx_prod<=6)
                    {
                        transport_cost =Convert.ToInt32( dDistance * 20);
                    }
                    else if(appx_prod>6&&appx_prod<=10)
                    {
                        transport_cost = Convert.ToInt32(dDistance * 32);

                    }
                    else if(appx_prod>10&&appx_prod<=15)
                    {
                        transport_cost = Convert.ToInt32(dDistance * 35);
                    }
                    else if(appx_prod>15&&appx_prod<=35)
                    {
                        transport_cost = Convert.ToInt32(dDistance * 48);
                    }
                    else if(appx_prod>35 && appx_prod<=70)
                    {
                        transport_cost = Convert.ToInt32(dDistance * 57);
                    }
                    else if(appx_prod>70 && appx_prod<=160)
                    {
                        transport_cost = Convert.ToInt32(dDistance * 51);
                    }
                    else if(appx_prod>160 && appx_prod<240)
                    {
                        transport_cost = Convert.ToInt32(dDistance * 65);
                    }
                    else if(appx_prod>240 && appx_prod<=270)
                    {
                        transport_cost = Convert.ToInt32(dDistance * 70);
                    }
                    else if(appx_prod>270 && appx_prod<=330)
                    {
                        transport_cost = Convert.ToInt32(dDistance * 80);
                    }
                    else
                    {
                        count = appx_prod / 330;
                        transport_cost = Convert.ToInt32(dDistance * 80 * count);
                    }
                    total_ben = total_selling_cost - transport_cost;
                    msg[i++] = "Market Name : "+r["market_name"].ToString() + "\n\nAddress : " + r["market_address"].ToString() + "\n\nRate(per qtl) : " + r["rate"].ToString() + "\n\nDistance(in Km) : " + dDistance.ToString()+"\n\nQuantity(In Qtl) : " +appx_prod+"\n\nTotal selling Cost(Rs) : "+total_selling_cost+"\n\nTransportation Cost(Rs) : "+transport_cost+"\n\nTotal benefits after selling crop to this market : "+total_ben;
                }
            }
            finally
            {
                con.Close();
            }
            return msg;
        }
       [WebMethod]
       public string getlatlng(string address)
        {
            string msg = "";
            var locationService = new GoogleLocationService();
            var point = locationService.GetLatLongFromAddress(address);
            var latitude = point.Latitude;
            var longitude = point.Longitude;
            msg += latitude.ToString()+","+longitude.ToString();
            
            return msg;
        }
        [WebMethod]
        public int signin(string email)
        {
            int user_type = 0;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            int profile_complete = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from user_login where email='" + email + "'", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    profile_complete =Convert.ToInt32( r["profile_complete"]);
                    if(profile_complete==0)
                    {
                        user_type = -1;
                    }
                    else
                    {
                        user_type = Convert.ToInt32(r["user_type"]);
                    }
                    
                }
            }
            finally
            {
                con.Close();
            }
            return user_type;
        }
        [WebMethod]
        public void update_gcm_token(string email,string token)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand updategcm = new SqlCommand("update user_gcm_token set gcm_token=@gcm_token where user_email=@user_email", con);
                updategcm.Parameters.AddWithValue("@gcm_token", token);
                updategcm.Parameters.AddWithValue("@user_email", email);
                updategcm.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }

        }
        [WebMethod]
        public void add_token(string email,string token)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand addgcm = new SqlCommand("insert into user_gcm_token (user_email,gcm_token) values(@user_email,@gcm_token) ", con);
                addgcm.Parameters.AddWithValue("@gcm_token", token);
                addgcm.Parameters.AddWithValue("@user_email", email);
                addgcm.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }

        }
        [WebMethod]
        public int signup(string email,string name,string gender,string ut)
        {
            int user_type = 0;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into user_login(email,user_name,gender,user_type) values(@email,@user_name,@gender,@user_type)", con);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@user_name", name);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@user_type", ut);
                int c=cmd.ExecuteNonQuery();
                if(c>0)
                {
                    SqlCommand check = new SqlCommand("select * from user_login where user_email=@email", con);
                    check.Parameters.AddWithValue("@email", email);
                    SqlDataAdapter cadp = new SqlDataAdapter(check);
                    DataTable cdt = new DataTable();
                    foreach(DataRow r in cdt.Rows)
                    {
                        user_type =Convert.ToInt32(r["user_type"].ToString());
                    }
                    add_token(email, "");
                }
            }
            finally
            {
                con.Close();
            }
            return user_type;
        }
        [WebMethod]
        public void add_farmer_details(string email,string address,string cnum,string ph,string region,string wth,string raspberry_id)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                string did="";
                SqlCommand getdid = new SqlCommand("select * from District where District_name=@dname", con);
                SqlDataAdapter dadp = new SqlDataAdapter(getdid);
                DataTable ddt = new DataTable();
                dadp.Fill(ddt);
                foreach(DataRow rdid in ddt.Rows)
                {
                    did = rdid["DistrictId"].ToString();
                }
                string latlng = getlatlng(address);
                double lat =Convert.ToDouble( latlng.Substring(0, latlng.IndexOf(",")));
                double lng = Convert.ToDouble(latlng.Substring(latlng.IndexOf(",")));
                SqlCommand add_details = new SqlCommand("insert into farm_data(user_email,DistrictId,SoilPh,water_tank_height,farm_address,contact_number,farm_lat,farm_long) values(@user_email,@DistrictId,@SoilPh,@water_tank_height,@farm_address,@contact_number,@farm_lat,@farm_long)", con);
                add_details.Parameters.AddWithValue("@farm_lat", lat);
                add_details.Parameters.AddWithValue("@farm_long", lng);
                add_details.Parameters.AddWithValue("@user_email", email);
                add_details.Parameters.AddWithValue("@DistrictId", did);
                add_details.Parameters.AddWithValue("@SoilPh", ph);
                add_details.Parameters.AddWithValue("@water_tank_height", wth);
                add_details.Parameters.AddWithValue("@farm_address", address);
                add_details.Parameters.AddWithValue("@contact_number", cnum);
                add_details.ExecuteNonQuery();
                SqlCommand get_raspi_id = new SqlCommand("select * from raspberry_map where raspberry_unique_name=@run", con);
                get_raspi_id.Parameters.AddWithValue("@run", raspberry_id);
                SqlDataAdapter get_rid_adp = new SqlDataAdapter(get_raspi_id);
                DataTable get_rid_dt = new DataTable();
                get_rid_adp.Fill(get_rid_dt);
                foreach(DataRow get_rid_row in get_rid_dt.Rows)
                {
                    string rid = get_rid_row["raspberry_id"].ToString();
                    SqlCommand update_rid = new SqlCommand("update user_login set raspberry_id=@rasp_id where email=@email", con);
                    update_rid.Parameters.AddWithValue("@rasp_id", rid);
                    update_rid.Parameters.AddWithValue("@email", email);
                    update_rid.ExecuteNonQuery();
                }
            }
            finally
            {
                con.Close();
            }
        }
        [WebMethod]
        public void add_manufacturing_company_details(string email, string address, string cnum, string name, string region)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                string did="";
                SqlCommand getdid = new SqlCommand("select * from District where DistrictName=@dname", con);
                getdid.Parameters.AddWithValue("@dname", region);
                SqlDataAdapter dadp = new SqlDataAdapter(getdid);
                DataTable ddt = new DataTable();
                dadp.Fill(ddt);
                foreach (DataRow rdid in ddt.Rows)
                {
                    did = rdid["DistrictId"].ToString();
                }
                
                SqlCommand add_details = new SqlCommand("insert into manufacturing_company(user_email,Company_name,DistrictId,Company_address,Contact_details) values(@user_email,@Company_name,@DistrictId,@Company_address,@Contact_details)", con);
                add_details.Parameters.AddWithValue("@user_email", email);
                add_details.Parameters.AddWithValue("@DistrictId", did);
                add_details.Parameters.AddWithValue("@Company_name", name);
                add_details.Parameters.AddWithValue("@Company_address", address);
                add_details.Parameters.AddWithValue("@Contact_details", cnum);
                add_details.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
        }
        [WebMethod]
        public string update_farm_ph(string email,string ph)
        {
            string msg = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand updateph = new SqlCommand("update farm_data set SoilPh=@ph where user_email=@email", con);
                updateph.Parameters.AddWithValue("@ph", ph);
                updateph.Parameters.AddWithValue("@email", email);

            }
            finally
            {

            }
            return msg;
        }
        [WebMethod]
        public string adduser(string email,string user_type,string raspberry_id)
        {
            string msg = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into user_login (email,raspberry_id,user_type) values(@email,@raspberry_id,@user_type)", con);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@raspberry_id", raspberry_id);
                cmd.Parameters.AddWithValue("@user_type", user_type);
                int c=cmd.ExecuteNonQuery();
                if(c>0)
                {
                    msg = "User added";
                }
                else
                {
                    msg = "try again";
                }
            }
            finally
            {
                con.Close();
            }
            return msg;
        }
        [WebMethod]
        public string getSoilId(string p,string disid)
        {
            string soilId = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            double soilphlow = 0.0;double soilphhigh = 0.0;double ph = Convert.ToDouble(p);
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select SoilId,SoilPhLow,SoilPhHigh from SoilQualities ",con);
                SqlDataAdapter res_soilid = new SqlDataAdapter(cmd);
                DataTable dt_soilId = new DataTable();
                res_soilid.Fill(dt_soilId);
                foreach (DataRow r in dt_soilId.Rows)
                {

                  if(soilphlow>(Convert.ToDouble( r["SoilPhLow"])-ph) && soilphhigh < (Convert.ToDouble(r["SoilPhHigh"]) - ph))
                    {
                        soilId = r["SoilId"].ToString();
                        soilphlow = Convert.ToDouble(r["SoilPhLow"]) - ph;
                        soilphhigh = Convert.ToDouble(r["SoilPhHigh"]) - ph;
                    }
                }
                SqlCommand check = new SqlCommand("select * from District where DistrictId='"+disid+"'", con);
                SqlDataAdapter checkada = new SqlDataAdapter(check);
                DataTable checkdt = new DataTable();
                checkada.Fill(checkdt);
                foreach (DataRow r in checkdt.Rows)
                {
                    if(soilId==r["SoilId"].ToString())
                    {

                    }
                    else
                    {
                        soilId = r["SoilId"].ToString();
                    }
                }
                
            }
            finally
            {
                con.Close();
            }
            return soilId;
        }
        [WebMethod]
        public string[] predict_crops(string email)
        {
            string[] crops;
            string soil_ph = "";
            string districtid = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            string raspberry_id = "";
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            string soilid = "";
            int i = 0;
            DateTime dt = DateTime.Now;
            int year = dt.Year;
            int month = dt.Month;
            string season = "";
            if (month >= 6 && month <= 10)
                season = "Kharif";
            if (month >= 11 || month <= 3)
                season = "Rabi";
            if (month >= 4 && month <= 6)
                season = "Summer";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd_getph = new SqlCommand("select * from farm_data where user_email='" + email + "'", con);
                SqlDataAdapter getph_adp = new SqlDataAdapter(cmd_getph);
                DataTable getph_dt = new DataTable();
                getph_adp.Fill(getph_dt);
                foreach(DataRow r in getph_dt.Rows)
                {
                    soil_ph = r["SoilPh"].ToString();
                    districtid = r["DistrictId"].ToString();
                }
                soilid = getSoilId(soil_ph, districtid);
                SqlCommand cmd_getcrops = new SqlCommand("select c.Cropid,c.Cropname from CropsBasics c,cropmap cm,CropReqQualities cr,prev_year_need pcn where cm.DistrictId=@districtid and cr.SuitableSoilId=@soilid and c.CropId=cm.CropId and cm.CropId=cr.CropId and pcn.CropId=c.CropId and prev_need>(select SUM(approximate_production) from user_crop_data ucd where ucd.CropId=cr.CropId and ucd.crop_sold=0)",con);
                cmd_getcrops.Parameters.AddWithValue("@districtid", districtid);
                cmd_getcrops.Parameters.AddWithValue("@soilid", soilid);
                SqlDataAdapter getcrops_adp = new SqlDataAdapter(cmd_getcrops);
                DataTable getcrops_dt = new DataTable();
                getcrops_adp.Fill(getcrops_dt);
                i = 0;
                crops = new string[getcrops_dt.Rows.Count+1];
                SqlCommand getdname = new SqlCommand("select * from District where DistrictId=@did", con);
                getdname.Parameters.AddWithValue("@did", districtid);
                SqlDataAdapter dnameadp = new SqlDataAdapter(getdname);
                DataTable getdnamedt = new DataTable();
                dnameadp.Fill(getdnamedt);
                string dname = "";
                foreach(DataRow getdnamerow in getdnamedt.Rows)
                {
                    dname = getdnamerow["DistrictName"].ToString();
                }
                crops[i++] = "Crop prediction is done using Following parameters: \nSoilPh=" + soil_ph + " \n District Name=" + dname+" \nFor season:"+season;
                foreach(DataRow r in getcrops_dt.Rows)
                {
                    crops[i++] = r["Cropid"].ToString() + "," + r["Cropname"].ToString();
                }
            }
            finally
            {
                con.Close();
            }
            return crops;
        }
        [WebMethod]
        public string get_water_status(string email)
        {
            string msg = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            string raspberry_id = "";
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            DateTime dtime = DateTime.Now;
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            string date = dtime.Date.ToString().Substring(0, 10);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from user_login where email='" + email + "'", con);
                SqlDataAdapter res = new SqlDataAdapter(cmd);
                DataTable dt1 = new DataTable();
                res.Fill(dt1);
                foreach (DataRow r in dt1.Rows)
                {

                    raspberry_id = r["raspberry_id"].ToString();
                }
                SqlCommand status = new SqlCommand("select top(1) * from user_farm_data where update_date=@update_date and raspberry_id=@raspberry_id order by update_time desc",con);
                status.Parameters.AddWithValue("@raspberry_id", raspberry_id);
                status.Parameters.AddWithValue("@update_date", date);
                SqlDataAdapter status_reader = new SqlDataAdapter(status);
                DataTable status_dt = new DataTable();
                status_reader.Fill(status_dt);
                foreach(DataRow r in status_dt.Rows)
                {
                    msg += r["water_pump_status"].ToString();
                    msg += ",";
                    msg += r["water_tank_level"].ToString();
                }
            }
            finally
            {
                con.Close();   
            }
                return msg;
        }
        [WebMethod]
        public string[] get_current_farm_status(string email)
        {
            string[] status ;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            string raspberry_id = "";
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            DateTime dtime = DateTime.Now;
            string date = dtime.Date.ToString().Substring(0, 10);
            string[] temp;
            string[] mois ;
            string[] timestamp;
            int i = 0;
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from user_login where email='" + email + "'", con);
                SqlDataAdapter res = new SqlDataAdapter(cmd);
                DataTable dt1 = new DataTable();
                res.Fill(dt1);
                foreach (DataRow r in dt1.Rows)
                {

                    raspberry_id = r["raspberry_id"].ToString();
                }
                
                    SqlCommand farm_data = new SqlCommand("select distinct * from user_farm_data where raspberry_id='" + raspberry_id + "' and update_date='"+date+"' order by update_time ASC", con);
                    SqlDataAdapter sda = new SqlDataAdapter(farm_data);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                status = new string[dt.Rows.Count];
                temp = new string[dt.Rows.Count];
                mois = new string[dt.Rows.Count];
                timestamp = new string[dt.Rows.Count];
                    foreach (DataRow r in dt.Rows)
                    {

                       temp[i] =r["farm_soil_temp"].ToString();
                       mois[i] = r["farm_soil_mois"].ToString();
                    timestamp[i++] = r["update_time"].ToString();
                    }
                for(int count = 0; count < i; count++)
                {
                   
                        status[count] += temp[count];
                        status[count] += ",";
                        status[count] += mois[count];
                        status[count] += ",";
                        status[count] += timestamp[count];
                    
                }

            }
            finally
            {
                con.Close();
            }
            return status;
        }
        [WebMethod]
        public string[] get_current_crops(string email)
        {
            string[] crops;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand get_crops = new SqlCommand("select * from CropsBasics as cb,user_crop_data ucd where ucd.user_email=@user_email and ucd.crop_sold=0 and cb.Cropid=ucd.CropId",con);
                get_crops.Parameters.AddWithValue("@user_email", email);
                
                SqlDataAdapter get_crops_adp = new SqlDataAdapter(get_crops);
                DataTable get_crops_dt = new DataTable();
                get_crops_adp.Fill(get_crops_dt);
                crops = new string[get_crops_dt.Rows.Count];
                int i = 0;
                foreach(DataRow r in get_crops_dt.Rows)
                {
                    crops[i++] = r["Cropid"].ToString() + ":" + r["Cropname"];
                }
            }
            finally
            {
                con.Close();
            }
            return crops;
        }
        [WebMethod]
        public void add_appx_production(string email,string cropid,string appx_prod)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand update_appx_prod = new SqlCommand("update user_crop_data set approximate_production=@ap where user_email=@email and CropId=@cropid and crop_sold=0", con);
                update_appx_prod.Parameters.AddWithValue("@ap", appx_prod);
                update_appx_prod.Parameters.AddWithValue("@email", email);
                update_appx_prod.Parameters.AddWithValue("@cropid", cropid);
                update_appx_prod.ExecuteNonQuery();
            }
            finally
            {
                con.Close();

            }
        }
        [WebMethod]
        public string add_new_crop_farmer(string email,string cropid,string app_prod)
        {
            string msg = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            DateTime dt = DateTime.Now;
            int year = dt.Year;
            int month = dt.Month;
            string season = "";
            if (month >= 6 && month <= 10)
                season = "Kharif" + year;
            if (month >= 11 || month <= 3)
                season = "Rabi" + year;
            if (month >= 4 && month <= 6)
                season = "Summer" + year;
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand check = new SqlCommand("select * from user_crop_data where user_email=@email and season=@season and crop_sold=0 and CropId=@cropid", con);
                check.Parameters.AddWithValue("@email", email);
                check.Parameters.AddWithValue("@cropid", cropid);
                check.Parameters.AddWithValue("@season", season);
                SqlDataAdapter checkadp = new SqlDataAdapter(check);
                DataTable check_dt = new DataTable();
                checkadp.Fill(check_dt);
                if(check_dt.Rows.Count>0)
                {
                    msg = "Crop is already added..";
                }
                else
                {
                    SqlCommand add_crop = new SqlCommand("insert into user_crop_data (user_email,CropId,season,crop_sold,approximate_production) values(@user_email,@CropId,@season,@crop_sold,@approximate_production)", con);
                    add_crop.Parameters.AddWithValue("@approximate_production", app_prod);
                    add_crop.Parameters.AddWithValue("@user_email", email);
                    add_crop.Parameters.AddWithValue("@CropId", cropid);
                    add_crop.Parameters.AddWithValue("@season", season);
                    add_crop.Parameters.AddWithValue("@crop_sold", 0);
                    int c=add_crop.ExecuteNonQuery();
                    if (c > 0)
                        msg = "Crop added.";
                    else
                        msg = "Try again";
                }
                return msg;
            }
            finally
            {
                con.Close();
            }
        }

        //For Manufacutring company  to view bids available for them
        [WebMethod]
        public string[] get_current_bids(string email)
        {
            string[] bids;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            int districtid = 0;
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                SqlCommand getdid = new SqlCommand("select DistrictId from manufacturing_company where user_email=@user_email", con);
                getdid.Parameters.AddWithValue("@user_email", email);
                SqlDataAdapter didadp = new SqlDataAdapter(getdid);
                DataTable diddt = new DataTable();
                didadp.Fill(diddt);
                foreach(DataRow r in diddt.Rows)
                {
                    districtid = Convert.ToInt32(r["DistrictId"].ToString());
                }
                SqlCommand getbids = new SqlCommand("select cb.Cropname,cb.Cropid,scb.approximate_production,scb.bid_id from set_crop_bid as scb,CropsBasics as cb where scb.DistrictId=@did and scb.bid_locked=0 and scb.CropId=cb.CropId", con);
                getbids.Parameters.AddWithValue("@did", districtid);
                SqlDataAdapter getbidadp = new SqlDataAdapter(getbids);
                DataTable dt = new DataTable();
                getbidadp.Fill(dt);
                bids = new string[dt.Rows.Count];
                int i = 0;
                foreach(DataRow r in dt.Rows)
                {
                    bids[i++] = r["bid_id"].ToString()+","+r["Cropid"].ToString() + "," + r["Cropname"].ToString() + "," + r["approximate_production"].ToString();
                }
            }
            finally
            {
                con.Close();
            }
            return bids;
        }

        //for manufacturing companies...... to view accepted bids by them
        [WebMethod]
        public string[] get_bids(string email)
        {
            string[] bids;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            string districtid = "";
            DateTime date = DateTime.Now;
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand getbids = new SqlCommand("select cb.Cropname,sb.bid_id,sb.approximate_production,sb.rate_per_qtl from CropsBasics as cb, set_crop_bid sb where sb.manu_company_id=@did and sb.bid_locked=1 and cb.CropId=sb.CropId", con);
                getbids.Parameters.AddWithValue("@did", email);
                SqlDataAdapter getbidsadp = new SqlDataAdapter(getbids);
                DataTable getbidsdt = new DataTable();
                getbidsadp.Fill(getbidsdt);
                int i = 0;
                bids = new string[getbidsdt.Rows.Count];
                foreach (DataRow r in getbidsdt.Rows)
                {
                    bids[i++] = r["bid_id"].ToString() + "," + r["Cropname"].ToString() + "," + r["approximate_production"].ToString() + "," + r["rate_per_qtl"].ToString();

                }
            }
            finally
            {
                con.Close();
            }
            return bids;
        }
        [WebMethod]
        public string accept_bid(string email,string bidid)
        {
            string msg = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand accept_bid = new SqlCommand("update set_crop_bid set bid_locked=1,manu_company_id=@mcid where bid_id=@bid_id", con);
                accept_bid.Parameters.AddWithValue("@mcid", email);
                accept_bid.Parameters.AddWithValue("@bid_id", bidid);
                int c = accept_bid.ExecuteNonQuery();
                string u_email = bidid.Substring((bidid.LastIndexOf(":") + 1), ((bidid.Length)-(bidid.LastIndexOf(":") + 1)));
                string crop = bidid.Substring(9, 4);
                msg += u_email + crop;
                SqlCommand update_crop = new SqlCommand("update user_crop_data set crop_sold=1 where CropId=@cid and user_email=@email", con);
                update_crop.Parameters.AddWithValue("@cid", crop);
                update_crop.Parameters.AddWithValue("@email", u_email);
                int d=update_crop.ExecuteNonQuery();
                if(c>0&&d>0)

                {
                    msg += "Bid Accepted.";
                    //send notification to farmer
                }
                else
                {
                    msg += "Try again";
                }
            }
            finally
            {
                con.Close();
            }
            return msg;
        }

        [WebMethod]
        public string[] get_bid_details_mucp(string bidid)
        {
            string[] biddetails = new string[2];
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            string districtid = "";
            DateTime date = DateTime.Now;
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from set_crop_bid where bid_id=@bidid", con);
                cmd.Parameters.AddWithValue("@bidid", bidid);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    if (Convert.ToInt32(r["bid_locked"].ToString()) == 1)
                    {
                        SqlCommand get_muc_details = new SqlCommand("select * from farm_data fd,user_login ul where fd.user_email=@email and ul.email=fd.user_email", con);
                        get_muc_details.Parameters.AddWithValue("@email", r["user_email"].ToString());
                        SqlDataAdapter gmd_adp = new SqlDataAdapter(get_muc_details);
                        DataTable gmd_dt = new DataTable();
                        gmd_adp.Fill(gmd_dt);
                        int j = 0;

                        biddetails[j++] = "Bid is accepted.Farmer Details are:";
                        foreach (DataRow r1 in gmd_dt.Rows)
                        {
                            biddetails[j] = r1["user_name"].ToString() + "," + r1["farm_address"].ToString() + "," + r1["contact_number"].ToString();
                        }
                    }
                    else
                    {
                        int j = 0;
                        string cropname = "";
                        biddetails[j++] = "Bid is not accepted yet.";
                        foreach (DataRow r1 in dt.Rows)
                        {
                            SqlCommand getcnmae = new SqlCommand("select * from CropsBasics where CropId=@cid", con);
                            getcnmae.Parameters.AddWithValue("@cid", r1["CropId"].ToString());
                            SqlDataAdapter cnameadp = new SqlDataAdapter(getcnmae);
                            DataTable cnamedt = new DataTable();
                            cnameadp.Fill(cnamedt);
                            foreach (DataRow namer in cnamedt.Rows)
                            {
                                biddetails[j++] = r1["bid_id"].ToString() + "," + r1["Cropname"].ToString() + "," + r1["approximate_production"].ToString() + "," + r1["rate_per_qtl"].ToString();
                            }
                        }
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return biddetails;
        }

        //for farmer...
        [WebMethod]
        public string[] get_bid_details(string bidid)
        {
            string[] biddetails=new string[2];
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            string districtid = "";
            DateTime date = DateTime.Now;
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from set_crop_bid where bid_id=@bidid", con);
                cmd.Parameters.AddWithValue("@bidid", bidid);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach(DataRow r in dt.Rows)
                {
                    if(Convert.ToInt32( r["bid_locked"].ToString())==1)
                    {
                        SqlCommand get_muc_details = new SqlCommand("select * from manufacturing_company where user_email=@email", con);
                        get_muc_details.Parameters.AddWithValue("@email", r["manu_company_id"].ToString());
                        SqlDataAdapter gmd_adp = new SqlDataAdapter(get_muc_details);
                        DataTable gmd_dt = new DataTable();
                        gmd_adp.Fill(gmd_dt);
                        int j = 0;
                        
                        biddetails[j] = "Bid is accepted by Company.\n"; 
                        foreach(DataRow r1 in gmd_dt.Rows)
                        {
                            biddetails[j] += "Company Name:"+r1["Company_name"].ToString() + "\nCompany Address:" + r1["Company_address"].ToString() + "\nContact Number:" + r1["Contact_details"].ToString();
                        }
                    }
                    else
                    {
                        int j = 0;
                        string cropname = "";
                        biddetails[j++] = "Bid is not accepted yet.\n";
                        foreach(DataRow r1 in dt.Rows)
                        {
                            SqlCommand getcnmae = new SqlCommand("select * from CropsBasics where CropId=@cid", con);
                            getcnmae.Parameters.AddWithValue("@cid", r1["CropId"].ToString());
                            SqlDataAdapter cnameadp = new SqlDataAdapter(getcnmae);
                            DataTable cnamedt = new DataTable();
                            cnameadp.Fill(cnamedt);
                            foreach(DataRow namer in cnamedt.Rows)
                            {
                                biddetails[j] = "Bid id:"+r["bid_id"].ToString() + "\nCrop Name:" + namer["Cropname"].ToString() + "\nProduction(In qtl):" + r["approximate_production"].ToString() + "\nCrop rate(per qtl):" + r["rate_per_qtl"].ToString();
                            }
                        }
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return biddetails;
        }
        [WebMethod]
        public string[] get_bids_farmer(string email)
        {
            string[] bids;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            string districtid = "";
            DateTime date = DateTime.Now;
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand getbids = new SqlCommand("select cb.Cropname,sb.bid_id,sb.approximate_production,sb.rate_per_qtl,sb.bid_locked from CropsBasics as cb, set_crop_bid sb where sb.user_email=@email and cb.CropId=sb.CropId", con);
                getbids.Parameters.AddWithValue("@email", email);
                SqlDataAdapter getbidsadp = new SqlDataAdapter(getbids);
                DataTable getbidsdt = new DataTable();
                getbidsadp.Fill(getbidsdt);
                int i = 0;
                string status = "";
                bids = new string[getbidsdt.Rows.Count];
                foreach (DataRow r in getbidsdt.Rows)
                {
                    if (Convert.ToInt32(r["bid_locked"].ToString()) == 0)
                        status = "Open";
                    else
                        status = "Accepted";
                    bids[i++] = r["bid_id"].ToString() + "," + r["Cropname"].ToString() + "," + r["approximate_production"].ToString() + "," + r["rate_per_qtl"].ToString()+","+status;

                }
            }
            finally
            {
                con.Close();
            }
            return bids;
        }
        [WebMethod]
        public string set_new_bid(string email,string cropid,string appx_prod,string rate_per_qtl)
        {
            string msg = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            string districtid="";
            DateTime date = DateTime.Now;
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand get_districtId = new SqlCommand("select * from farm_data where user_email=@user_email", con);
                get_districtId.Parameters.AddWithValue("@user_email", email);
                SqlDataAdapter gd_adp = new SqlDataAdapter(get_districtId);
                DataTable gd_dt = new DataTable();
                gd_adp.Fill(gd_dt);
                foreach(DataRow r in gd_dt.Rows)
                {
                    districtid =r["DistrictId"].ToString();

                }
                string bidid = "bid" +":"+ date.Year+":" + cropid+":" + email;
                SqlCommand check = new SqlCommand("select * from set_crop_bid where bid_id=@bidid", con);
                check.Parameters.AddWithValue("@bidid", bidid);
                SqlDataAdapter checkadp = new SqlDataAdapter(check);
                DataTable checkdt = new DataTable();
                checkadp.Fill(checkdt);
                if(checkdt.Rows.Count>0)
                { }
                else
                {
                    SqlCommand add_bid = new SqlCommand("insert into set_crop_bid (user_email,CropId,approximate_production,rate_per_qtl,bid_locked,bid_id,DistrictId) values(@user_email,@CropId,@approximate_production,@rate_per_qtl,@bid_locked,@bid_id,@DistrictId)", con);
                    add_bid.Parameters.AddWithValue("@user_email", email);
                    add_bid.Parameters.AddWithValue("@CropId", cropid);
                    add_bid.Parameters.AddWithValue("@approximate_production", appx_prod);
                    add_bid.Parameters.AddWithValue("@rate_per_qtl", rate_per_qtl);
                    add_bid.Parameters.AddWithValue("@bid_locked", 0);
                    add_bid.Parameters.AddWithValue("@bid_id", bidid);
                    add_bid.Parameters.AddWithValue("@DistrictId", districtid);
                    int c = add_bid.ExecuteNonQuery();
                    if (c > 0)
                    {
                        msg = "Bid added";
                        SqlCommand get_gcm = new SqlCommand("select user_email from manufacturin_company where DistrictId=@did ", con);
                        get_gcm.Parameters.AddWithValue("@did", districtid);
                        SqlDataAdapter gcm_adp = new SqlDataAdapter(get_gcm);
                        DataTable gcm_dt = new DataTable();
                        foreach (DataRow r in gcm_dt.Rows)
                        {
                            send_notificaton_generalized(r["user_email"].ToString(), "New Bid is added from your area.");
                        }

                    }

                    else
                        msg = "Try again!!";
                }
                
            }
            finally
            {
                con.Close();
            }

            return msg;
        }
        [WebMethod]
        public string update_farm_status(string temp,string mois,string raspberry_id,string water_tank_level,string water_pump_status,string send_noti)
        {
            string msg = "Off";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            DateTime dt = DateTime.Now;
            string time = dt.TimeOfDay.ToString().Substring(0, 5);
            string date = dt.Date.ToString().Substring(0, 10);
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into user_farm_data (raspberry_id,farm_soil_temp,farm_soil_mois,water_pump_status,update_date,update_time,water_tank_level) values(@raspberry_id,@farm_soil_temp,@farm_soil_mois,@water_pump_status,@update_date,@update_time,@water_tank_level)", con);
                cmd.Parameters.AddWithValue("@raspberry_id", raspberry_id);
                cmd.Parameters.AddWithValue("@farm_soil_temp", temp);
                cmd.Parameters.AddWithValue("@farm_soil_mois", mois);
                cmd.Parameters.AddWithValue("@water_pump_status", water_pump_status);
                cmd.Parameters.AddWithValue("@water_tank_level", water_tank_level);
                cmd.Parameters.AddWithValue("@update_date", date);
                cmd.Parameters.AddWithValue("@update_time", time);
                int c=cmd.ExecuteNonQuery();
                if (c > 0)
                {
                    msg = "DATA updated";
                    if(Convert.ToInt32(send_noti)==1)
                    {
                        send_notification(raspberry_id, water_pump_status);
                    }
                    else
                    {
                        
                    }
                    status = water_pump_status;
                }
                else
                    msg = "FAILED";

                
            }
            finally
            {
                con.Close();
            }
            return msg;
        }
        //Genralized notification function
        [WebMethod]
        public void send_notificaton_generalized(string email,string msg)
        {
            string gcm_token = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand get_gt = new SqlCommand("select * from user_gcm_token where user_email=@email",con);
                get_gt.Parameters.AddWithValue("@email", email);
                SqlDataAdapter gt_adp = new SqlDataAdapter(get_gt);
                DataTable dt = new DataTable();
                gt_adp.Fill(dt);
                foreach(DataRow r in dt.Rows)
                {
                    gcm_token = r["gcm_token"].ToString();
                }
                var jGcmData = new JObject();
                var jData = new JObject();
                jData.Add("message", msg);
                jGcmData.Add("to", gcm_token);
                jGcmData.Add("data", jData);
                var url = new Uri("https://gcm-http.googleapis.com/gcm/send");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.TryAddWithoutValidation(
                        "Authorization", "key=" + API_KEY);

                    Task.WaitAll(client.PostAsync(url,
                        new StringContent(jGcmData.ToString(), Encoding.Default, "application/json"))
                            .ContinueWith(response =>
                            {
                                Console.WriteLine(response);
                                Console.WriteLine("Message sent: check the client device notification tray.");
                            }));
                }
                }
            finally
            {
                con.Close();
            }
        }

        //For water pump..
        [WebMethod]
        public void send_notification(string id,string st)
        {
            string email = "";
            string gcm_token = "";
            MESSAGE = "Water pump is switched " + st;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Data Source from Connection String";
            builder.UserID = "User ID of the database";
            builder.Password = "Password of the database";
            builder.InitialCatalog = "DB name";
            SqlConnection con = new SqlConnection(builder.ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select email from user_login where raspberry_id='" + id + "'", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach(DataRow r in dt.Rows)
                {
                    email = r["email"].ToString();
                }
                SqlCommand cmd2 = new SqlCommand("select gcm_token from user_gcm_token where user_email='" + email + "'", con);
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd2);
                DataTable dt1 = new DataTable();
                sda1.Fill(dt1);
                foreach(DataRow r in dt1.Rows)
                {
                    gcm_token = r["gcm_token"].ToString();
                }
                var jGcmData = new JObject();
                var jData = new JObject();
                jData.Add("message", MESSAGE);
                jGcmData.Add("to", gcm_token);
                jGcmData.Add("data", jData);
                var url = new Uri("https://gcm-http.googleapis.com/gcm/send");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.TryAddWithoutValidation(
                        "Authorization", "key=" + API_KEY);

                    Task.WaitAll(client.PostAsync(url,
                        new StringContent(jGcmData.ToString(), Encoding.Default, "application/json"))
                            .ContinueWith(response =>
                            {
                                Console.WriteLine(response);
                                Console.WriteLine("Message sent: check the client device notification tray.");
                            }));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to send GCM message:");
                Console.Error.WriteLine(e.StackTrace);
            }
            finally
            {
                con.Close();
            }
            


        }
        [WebMethod]
        public string getdatetime()
        {
            DateTime dt = DateTime.Now;
            string msg = "";
            int month = dt.Month;
            string season = "";
            int year = dt.Year;
            
            msg = dt.Date.ToString().Substring(8, 10) + " " + dt.TimeOfDay.ToString().Substring(0, 5) + dt.Month + dt.Year+""+season;
            return msg;
        }
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }


}
