using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AISCM
{
    public class SetBidsFarmerModel : ContentPage, INotifyPropertyChanged
    {
        public string cropID { get; set; }
        public string cropName { get; set; }
        private KeyValuePair<string, string> _selectedItem;

        public KeyValuePair<string, string> SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }
        public ICommand CropIDParameterCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public SetBidsFarmerModel()
        {

            CropIDParameterCommand = new Command<string>(AddNewCropAsync);
        }

        async void AddNewCropAsync(string obj)
        {
            throw new NotImplementedException();
        }
    }
}