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
using Android.Database.Sqlite;

namespace AISCM.Droid
{
    [Activity(Label = "monitorstatus")]
    public class monitorstatus : Activity
    {
        String res = "";
        string temprature = "";
        string moisture = "";
        int index = 0;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.monitorstatus);
            net.azurewebsites.agc20171.AISCM aiscm = new net.azurewebsites.agc20171.AISCM();
            MainActivity g = new MainActivity();

            TextView temp = FindViewById<TextView>(Resource.Id.textView3);
            TextView mois = FindViewById<TextView>(Resource.Id.textView4);
            
        }
    }
}