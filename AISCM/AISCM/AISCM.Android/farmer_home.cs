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
using Android.Speech.Tts;
namespace AISCM.Droid
{
    [Activity(Label = "farmer_home")]
    public class farmer_home : Activity,TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.farmer_home);


             speaker = new TextToSpeech(this, this, "com.google.android.tts");
        }
        public void speak(string msg)
        {
            
            speaker.Speak(msg, QueueMode.Flush, null, null);
        }

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            
        }
    }
}