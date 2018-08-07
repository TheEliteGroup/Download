using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AISCM
{
    public class AcceptedBidsFarmerModel : ContentPage, INotifyPropertyChanged
    {
        public string CropName { get; set; }
        public string BidID { get; set; }
        public string Rate { get; set; }
        public string Quantity { get; set; }
        public string Parameters { get; set; }

        public ICommand BidParameterCommand { get; private set; }
        public double CropIDParameterResult { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public AcceptedBidsFarmerModel()
        {
            BidParameterCommand = new Command<string>(GetBids);
        }

        async void GetBids(string value)
        {
            System.Diagnostics.Debug.WriteLine("CropsModel:{0} - {1}", value, Global_portable.email);
            
           
            //DisplayAlert("Alert", "New Bid Placed Successfully!!!", "OK");
            //await Navigation.PushAsync(new AcceptedBidsFarmerView());


        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}