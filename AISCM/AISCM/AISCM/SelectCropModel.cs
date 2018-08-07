using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AISCM
{
    public class SelectCropModel : ContentPage,INotifyPropertyChanged
    {
            public string Name { get; set;}
            public string CropID { get; set; }
            public string Parameters { get; set;}

        public ICommand CropIDParameterCommand { get; private set; }
        public double CropIDParameterResult { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public SelectCropModel()
        {
            CropIDParameterCommand = new Command<string>(AddNewCropAsync);

        }

        async void AddNewCropAsync(string value)
        {
            System.Diagnostics.Debug.WriteLine("CropsP:{0} - {1}", value, Global_portable.email);
            //DependencyService.Get<call_web_service>().add_new_crop(Global_portable.email, value);
            //ToastNotification.Init();
            DisplayAlert("Alert", "Your Crop Is Ready To Be Sown", "OK");
            Navigation.PushAsync(new GetCropView());

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