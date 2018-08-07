using AISCM.Resources;
using System;
using System.Collections.Generic;
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
    public partial class display_water_related_data : ContentPage
    {
        public display_water_related_data()
        {
            InitializeComponent();
            update_data();
        }
        public void update_data()
        {
            string ResourceId = "AISCM.Resources.AppResource";
            string data = DependencyService.Get<call_web_service>().get_water_status(Global_portable.email);
            if (data.Length > 0)
            {
                int index = data.IndexOf(",", 0);
                string water_pump = data.Substring(0, index);
                string water_tank = data.Substring(index + 1, ((data.Length - 1) - index));
                water_pump_status.Text += "Water pump is switched " + water_pump;
                water_tank_level.Text += "Water tank level is (in %): " + water_tank;
            }
            else
            {
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("hi-IN");
                ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
                string text_converted = resourceManager.GetString("CropMarketing", CultureInfo.DefaultThreadCurrentCulture);
                water_pump_status.Text = text_converted;
            }
        }
    }
}