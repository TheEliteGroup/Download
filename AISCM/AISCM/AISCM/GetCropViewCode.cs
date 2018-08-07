using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AISCM
{
    public class GetCropViewCode : ContentPage
    {
        public ObservableCollection<GetCropModel> veggies { get; set; }
        public GetCropViewCode()
        {
            veggies = new ObservableCollection<GetCropModel>();
            ListView lstView = new ListView();
            lstView.RowHeight = 60;
            this.Title = "ListView Code Sample";
            lstView.ItemTemplate = new DataTemplate(typeof(CustomVeggieCell));
            veggies.Add(new GetCropModel { CropName = "Tomato"});
            veggies.Add(new GetCropModel { CropName = "Romaine Lettuce"});
            veggies.Add(new GetCropModel { CropName = "ABC"});
            lstView.ItemsSource = veggies;
            Content = lstView;
        }

        public class CustomVeggieCell : ViewCell
        {
            public CustomVeggieCell()
            {
                //instantiate each of our views
                var image = new Image();
                var nameLabel = new Label();
                var typeLabel = new Label();
                var verticaLayout = new StackLayout();
                var horizontalLayout = new StackLayout() { BackgroundColor = Color.Olive };

                //set bindings
                nameLabel.SetBinding(Label.TextProperty, new Binding("Name"));
                typeLabel.SetBinding(Label.TextProperty, new Binding("Type"));
                image.SetBinding(Image.SourceProperty, new Binding("Image"));

                //Set properties for desired design
                horizontalLayout.Orientation = StackOrientation.Horizontal;
                horizontalLayout.HorizontalOptions = LayoutOptions.Fill;
                image.HorizontalOptions = LayoutOptions.End;
                nameLabel.FontSize = 24;

                //add views to the view hierarchy
                verticaLayout.Children.Add(nameLabel);
                verticaLayout.Children.Add(typeLabel);
                horizontalLayout.Children.Add(verticaLayout);
                horizontalLayout.Children.Add(image);

                // add to parent view
                View = horizontalLayout;
            }
        }
    }
}