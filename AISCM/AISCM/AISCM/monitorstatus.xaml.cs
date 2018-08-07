using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using Syncfusion.SfChart.XForms;

namespace AISCM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class monitorstatus : ContentPage
    {
                public monitorstatus()
        {
            InitializeComponent();
            NumericalAxis n = new NumericalAxis() { Maximum = 100, Minimum = 0 };
            TempChart.SecondaryAxis = n;
            NumericalStripLine stripLine1 = new NumericalStripLine()
            {

                Start = 20,
                Width = 10,
                Text = "Required Temperature",
                FillColor = Color.FromHex("#20B622")

            };

            n.StripLines.Add(stripLine1);
            
        }
    }
}