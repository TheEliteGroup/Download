using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AISCM
{
    public partial class FarmDetails : ContentPage
    {
        public FarmDetails()
        {
            InitializeComponent();
        }
        void OnSliderValChanged(object sender, ValueChangedEventArgs e)
        {
            lbldisp.Text = String.Format("Slider value is {0:F1}", e.NewValue);
        }

        void Entry_Completed(object sender, EventArgs e)
        {
            var text = ((Entry)sender).Text; //cast sender to access the properties of the Entry
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                phSel.Text = (string)picker.ItemsSource[selectedIndex];
            }
        }
    }
}
