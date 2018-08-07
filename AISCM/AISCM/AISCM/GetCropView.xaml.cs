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
    public partial class GetCropView : ContentPage
    {
        public ObservableCollection<GetCropModel> getCrops { get; set; }
        public GetCropView()
        {
            InitializeComponent();
            String[] cropList = new String[100];
            cropList = DependencyService.Get<call_web_service>().get_crops(Global_portable.email);
            getCrops = new ObservableCollection<GetCropModel>();
            for (int i = 0; i < cropList.Length; i++)
            {
                string cropID = "";
                string cropName = "";
                string ResourceId = "AISCM.Resources.AppResource";
                string translated_cropname = "";
                int currloc = 0;
                int nextloc = 0;
                nextloc = cropList[i].IndexOf(":", currloc);
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
                getCrops.Add(new GetCropModel { CropName = translated_cropname });

            }


            lstView.ItemsSource = getCrops;
        }
    }
}