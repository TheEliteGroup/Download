#region Copyright Syncfusion Inc. 2001-2016.
// Copyright Syncfusion Inc. 2001-2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AISCM
{
    public class ViewModel
    {
		public ObservableCollection < ChartDataPoint > FarmTemperature  
		{
			get;
			set;
		}
        public ObservableCollection<ChartDataPoint> FarmMoisture
        {
            get;
            set;
        }
        public ObservableCollection<ChartDataPoint> ReqTemp { get; set; }
        public ViewModel  ()         
		{            
			FarmTemperature  =  new ObservableCollection < ChartDataPoint >   ();
            FarmMoisture = new ObservableCollection<ChartDataPoint>();
            string[] msg = new string[100];
            msg = DependencyService.Get<call_web_service>().get_farm_status(Global_portable.email);

            ReqTemp = new ObservableCollection<ChartDataPoint>();
            
            for (int i = 0; i<msg.Length; i++)
            {
                string t = "";
                string timestamp = "";
                string m = "";
                int currloc = 0;
                int nextloc = 0;
                nextloc = msg[i].IndexOf(",", currloc);
                t = msg[i].Substring(0, nextloc);
                currloc = nextloc + 1;
                nextloc = msg[i].IndexOf(",", currloc);
                m = msg[i].Substring(currloc, (nextloc - currloc));
                currloc = nextloc + 1;
                timestamp = msg[i].Substring(currloc);
                double temp = Convert.ToDouble(t);
                FarmTemperature.Add(new ChartDataPoint(timestamp, temp));
                FarmMoisture.Add(new ChartDataPoint(timestamp, Convert.ToDouble(m)));
               
                //DisplayAlert("data", t + " " + m + " " + timestamp, "ok");                                                                 
            } 
        }    

    }
}
