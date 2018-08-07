using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AISCM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarketInputView : ContentPage
    {
        public ObservableCollection<SetBidsFarmerModel> crops { get; set; }
        // private Dictionary<string, string> PickerItems = new Dictionary<string, string>() { { "AF", "Afghanistan" }, { "AL", "Albania" } };


        private Dictionary<string, string> CropItems = new Dictionary<string, string>() { };

        public List<KeyValuePair<string, string>> CropItemList = new List<KeyValuePair<string, string>>();
        public MarketInputView()
        {
            InitializeComponent();
            String[] cropList = new String[100];
            cropList = DependencyService.Get<call_web_service>().get_crops(Global_portable.email);
            crops = new ObservableCollection<SetBidsFarmerModel>();
            for (int i = 0; i < cropList.Length; i++)
            {
                string cropID = "";
                string cName = "";

                int currloc = 0;
                int nextloc = 0;
                System.Diagnostics.Debug.WriteLine("CropList========{0}", cropList[i]);
                nextloc = cropList[i].IndexOf(":", currloc);
                cropID = cropList[i].Substring(0, nextloc);
                currloc = nextloc + 1;

                cName = cropList[i].Substring(currloc);
                System.Diagnostics.Debug.WriteLine("Contruct===={0}===={1}", cropID.ToString(), cName);
                CropItems.Add(cropID.ToString(), cName);
                // PickerItems.Add(cropID.ToString(), cName);
                //crops.Add(new SetBidsFarmerModel { cropName = cName,  });

            }
            System.Diagnostics.Debug.WriteLine(CropItems.Keys);
            // System.Diagnostics.Debug.WriteLine(PickerItems.Keys);

            cropPicker.ItemsSource = CropItems.ToList();
        }


        void OnCropChoosen(object sender, EventArgs e)
        {

            Picker pickervalues = (Picker)sender;
            var data = pickervalues.Items[pickervalues.SelectedIndex];
            var id = CropItems.FirstOrDefault(x => x.Value == data).Key;
            System.Diagnostics.Debug.WriteLine(id);
            System.Diagnostics.Debug.WriteLine(data);
        }

        private void addProdMarket(object sender, EventArgs e)
        {
            var data = cropPicker.Items[cropPicker.SelectedIndex];
            var id = CropItems.FirstOrDefault(x => x.Value == data).Key;
            var quant = quantity.Text;
            System.Diagnostics.Debug.WriteLine("Market Input======={0}====={1}====={2}=====", id, data, quant);
            Navigation.PushAsync(new showMarketsFarmer(id));
            DependencyService.Get<call_web_service>().addApproxProd(Global_portable.email, id, quant);
        }

    }
}