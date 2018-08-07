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
    [Activity(Label = "register_manu_company")]
    public class register_manu_company : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string region = "";
            SetContentView(Resource.Layout.register_manu_company);
            Button add = FindViewById<Button>(Resource.Id.button1);
            EditText name = FindViewById<EditText>(Resource.Id.editText1);
            EditText address = FindViewById<EditText>(Resource.Id.editText2);
            EditText cnum = FindViewById<EditText>(Resource.Id.editText3);
            RadioGroup rgdid = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            RadioButton did = FindViewById<RadioButton>(rgdid.CheckedRadioButtonId);
            region = did.Text;
            net.azurewebsites.agc20171.AISCM a = new net.azurewebsites.agc20171.AISCM();
            Button add_details = FindViewById<Button>(Resource.Id.button1);
            add_details.Click += delegate
              {
                  a.add_manufacturing_company_details(Global_portable.email, address.Text, cnum.Text, name.Text, region);
                  StartActivity(typeof(Login));
              };
            
        }
    }
}