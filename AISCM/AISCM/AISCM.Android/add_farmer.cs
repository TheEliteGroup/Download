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
    [Activity(Label = "add_farmer")]
    public class add_farmer : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.add_farmer);
            EditText email = FindViewById<EditText>(Resource.Id.editText1);
            EditText rid = FindViewById<EditText>(Resource.Id.editText2);
            net.azurewebsites.agc20171.AISCM agc = new net.azurewebsites.agc20171.AISCM();
            agc.adduser(email.Text,"2", rid.Text);
        }
    }
}