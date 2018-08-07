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
    [Activity(Label = "admin_home")]
    public class admin_home : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.admin_home);
            Button addfamer = FindViewById<Button>(Resource.Id.button1);
            addfamer.Click += delegate
              {
                  StartActivity(typeof(add_farmer));
              };
        }
    }
}