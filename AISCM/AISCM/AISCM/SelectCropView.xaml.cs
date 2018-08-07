using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AISCM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectCropView : ContentPage
    {
        public ObservableCollection<SelectCropModel> veggies { get; set; }

        private Dictionary<string, string> CropItems = new Dictionary<string, string>() { };

        public List<KeyValuePair<string, string>> CropItemList = new List<KeyValuePair<string, string>>();
        public SelectCropView()
        {
            InitializeComponent();
            String[] cropList = new String[100];
            cropList = DependencyService.Get<call_web_service>().predict_crops(Global_portable.email);
            string ResourceId = "AISCM.Resources.AppResource";
            string translated_cropname = "";

            veggies = new ObservableCollection<SelectCropModel>();
            System.Diagnostics.Debug.WriteLine("CropsP:{0}", cropList[0]);
            veggies.Add(new SelectCropModel { Parameters = cropList[0] });

            for (int i = 1; i < cropList.Length; i++)
            {
                string cropID = "";
                string cropName = "";

                int currloc = 0;
                int nextloc = 0;
                nextloc = cropList[i].IndexOf(",", currloc);
                cropID = cropList[i].Substring(0, nextloc);
                currloc = nextloc + 1;

                cropName = cropList[i].Substring(currloc);
                if (Global_portable.default_language != null)
                {
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(Global_portable.default_language);
                    ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
                    string text_converted = resourceManager.GetString(cropName, CultureInfo.DefaultThreadCurrentCulture);
                    translated_cropname = text_converted;
                }
                else
                {
                    ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
                    string text_converted = resourceManager.GetString(cropName, CultureInfo.CurrentCulture);
                    translated_cropname = text_converted;
                }

                System.Diagnostics.Debug.WriteLine("Crops:{0} - {1}", cropID, cropName);
                //veggies.Add(new SelectCropModel { Name = translated_cropname, CropID = cropID });
                CropItems.Add(cropID, string.Format("{0}",cropName));


            }
            //  veggies.Add(new SelectCropModel { Name = crops[1], Type = "Fruit" });

            lstView.ItemsSource = veggies;
            lstView.ItemsSource = CropItems.ToList();
        }

        void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem.ToString();
            //System.Diagnostics.Debug.WriteLine("Selected Crop==================={0}==={1}", bidID, cropName);

            int currloc = 0;
            int nextloc = 0;
            nextloc = item.IndexOf(",", currloc);
            string cropID = item.Substring(1, nextloc - 1);
            currloc = nextloc + 1;
            nextloc = item.IndexOf("]", currloc);
            string cropName = item.Substring(currloc + 1, (nextloc - currloc));

            System.Diagnostics.Debug.WriteLine("Selected Crop==================={0}==={1}===={2}", item,cropID, cropName);

            Navigation.PushAsync(new GetApproxQuantityCropPredict(cropID));

        }

    }
}