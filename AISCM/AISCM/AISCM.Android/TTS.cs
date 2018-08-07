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
    public class TTS : Java.Lang.Object, TextToSpeech.IOnInitListener
    {
        public void OnInit([GeneratedEnum] OperationResult status)
        {
            
        }
    }
}