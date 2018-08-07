using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AISCM
{
    public class ListBidsModel : ContentPage, INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string BidID { get; set; }
        public string CropRate { get; set; }
        public string CropQuantity { get; set; }
        public string Parameters { get; set; }

        public ICommand BidParameterCommand { get; private set; }
        public double CropIDParameterResult { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ListBidsModel()
        {
            BidParameterCommand = new Command<string>(GetBids);
        }

        async void GetBids(string value)
        {
            System.Diagnostics.Debug.WriteLine("CropsModel:{0} - {1}", value, Global_portable.email);
            //DependencyService.Get<call_web_service>().set_bids(Global_portable.email,value);
            //ToastNotification.Init();
            DisplayAlert("Alert", "New Bid Placed Successfully!!!", "OK");

            //CropIDParameterResult = Math.Sqrt(num);
            //OnPropertyChanged("SquareRootWithParameterResult");
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