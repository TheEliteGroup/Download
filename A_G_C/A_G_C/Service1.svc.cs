using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
namespace A_G_C
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        SqlConnection con = new SqlConnection("Data Source=aditya.database.windows.net;Initial Catalog=demo;Integrated Security=False;User ID=aditya.sd15;Password=********;Connect Timeout=15;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        public int login(String email)
        {
            int user_type=0;
            
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select user_type from users where email='" + email + "'",con);
                int num_of_rows=cmd.ExecuteNonQuery();
                if(num_of_rows>0)
                {
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    foreach(DataRow r in dt.Rows)
                    {
                        user_type = Convert.ToInt32(r["user_type"].ToString());
                    }
                }
                else
                {
                    user_type = 0;
                }
            }
            finally
            {
                con.Close();
            }
            return user_type;
        }
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
