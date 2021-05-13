using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherLocationInfo.Views
{
    public partial class Favorites : ContentPage
    {
        ObservableCollection<string> CityData { get; set; }

        public Favorites()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            CityData = new ObservableCollection<string>();
            this.BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public void AddCity(string city)
        {
            CityData.Add(city);
        }

    }
}
