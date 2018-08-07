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

namespace AISCM.Droid
{
    public static class Global
    {
        public static string email {get; set; }
        public static string user_id { get; set; }
        public static int login { get; set; }
        public static net.azurewebsites.agc20171.AISCM a = new net.azurewebsites.agc20171.AISCM();
    }
}