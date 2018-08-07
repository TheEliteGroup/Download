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
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        const string ResourceId = "AISCM.Resources.AppResource";
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            CultureInfo c;
            ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
            if (Global_portable.default_language == null)
            {
                if (Text == null)
                    return null;

                c = CultureInfo.CurrentCulture;
                
            }
            else
            {
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(Global_portable.default_language);
                c = CultureInfo.DefaultThreadCurrentCulture; 
            }
            return resourceManager.GetString(Text, c);
        }
    }
}
