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
    [Activity(Label = "register_farmer")]
    public class register_farmer : Activity,SeekBar.IOnSeekBarChangeListener
    {
        TextView phval;
        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            phval.Text= string.Format("pH is:{0}", seekBar.Progress);

        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.register_farmer);
            phval = FindViewById<TextView>(Resource.Id.textView2);
            Button add = FindViewById<Button>(Resource.Id.button1);
            add.Click += delegate
            {
                EditText address = FindViewById<EditText>(Resource.Id.editText1);
                EditText cnum = FindViewById<EditText>(Resource.Id.editText2);
                EditText raspi_name = FindViewById<EditText>(Resource.Id.editText11);
                SeekBar ph = FindViewById<SeekBar>(Resource.Id.seekBar1);
                ph.SetOnSeekBarChangeListener(this);
                RadioGroup dg = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
                RadioButton rb = FindViewById<RadioButton>(dg.CheckedRadioButtonId);
                string district = rb.Text;
                EditText height = FindViewById<EditText>(Resource.Id.editText1);
                net.azurewebsites.agc20171.AISCM a = new net.azurewebsites.agc20171.AISCM();
                a.add_farmer_details(Global_portable.email, address.Text, cnum.Text, phval.Text, district, height.Text,raspi_name.Text);
                StartActivity(typeof(Login));
            };
        }
    }
}