using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeatherLocationInfo.Views;
using Xamarin.Essentials;

namespace WeatherLocationInfo.Views
{
    public partial class ItemsPage : ContentPage
    {
        RestServiceForecast _restServiceForecast;

        public ItemsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            CloudImage.IsVisible = false;
            WindImage.IsVisible = false;
            HumidityImage.IsVisible = false;
            PressureImage.IsVisible = false;
            RainImage.IsVisible = false;
            IconSunrise.IsVisible = false;
            IconSunset.IsVisible = false;
            frameForecast.IsVisible = false;
            frameSunriseSunset.IsVisible = false;

            _restServiceForecast = new RestServiceForecast();

            _ = ButtonClickedGPS();
            _ = ButtonClickedGPS();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

            string GenerateRequestUri(string endpoint, Entry _cityEntry)
            {
                string requestUri = endpoint;
                requestUri += $"?q={_cityEntry.Text}";
                requestUri += "&units=metric"; // or units=metric
                requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
                return requestUri;
            }

            string GenerateRequestUriForecast(string endpoint, Entry _cityEntry)
            {
                string requestUri = endpoint;
                requestUri += $"?q={_cityEntry.Text}";
                requestUri += "&units=metric"; // or units=metric
                requestUri += $"&APPID={ConstantsForecast.OpenWeatherMapAPIKey2}";
                return requestUri;
            }

            async Task CurrentWeather()
            {
                CloudImage.IsVisible = true;
                WindImage.IsVisible = true;
                HumidityImage.IsVisible = true;
                PressureImage.IsVisible = true;
                RainImage.IsVisible = true;
                IconSunrise.IsVisible = true;
                IconSunset.IsVisible = true;
                frameForecast.IsVisible = true;
                frameSunriseSunset.IsVisible = true;


            WeatherBindingData weatherBindingData = await _restServiceForecast.GetWeatherDataForecast(GenerateRequestUri(Constants.OpenWeatherMapEndpoint, _cityEntry), GenerateRequestUriForecast(ConstantsForecast.OpenWeatherMapEndpoint2, _cityEntry), GenerateRequestUriForecast(ConstantsForecast.OpenWeatherMapEndpoint2, _cityEntry));
                BindingContext = weatherBindingData;

                string description = weatherBindingData.WeatherDataForecastHourly.List[0].WeatherForecast[0].DescriptionForecast;

                if (description == "clear sky")
                {
                    BackgroundImage.Source = "clearsky.jpg";
                    BackgroundImage.Opacity = 1;
                }
                else if (description == "few clouds")
                {
                    BackgroundImage.Source = "fewclouds.jpg";
                    BackgroundImage.Opacity = 0.3;
                }
                else if (description == "scattered clouds")
                {
                    BackgroundImage.Source = "scatteredclouds.jpg";
                    BackgroundImage.Opacity = 0.3;
                }
                else if (description == "broken clouds")
                {
                    BackgroundImage.Source = "brokenclouds.jpg";
                    BackgroundImage.Opacity = 0.3;
                }
                else if (description == "light rain")
                {
                    BackgroundImage.Source = "lightrain.jpg";
                    BackgroundImage.Opacity = 0.3;
                }
                else if (description == "rain")
                {
                    BackgroundImage.Source = "rain.jpg";
                    BackgroundImage.Opacity = 0.3;
                }
                else if (description == "thunderstorm")
                {
                    BackgroundImage.Source = "thunderstorm.jpg";
                    BackgroundImage.Opacity = 0.3;
                }
                else if (description == "snow")
                {
                    BackgroundImage.Source = "snow.jpg";
                    BackgroundImage.Opacity = 0.3;
                }
                else if (description == "mist")
                {
                    BackgroundImage.Source = "mist.jpg";
                    BackgroundImage.Opacity = 0.3;
                }
                else if (description == "overcast clouds")
                {
                    BackgroundImage.Source = "overcastclouds.jpg";
                    BackgroundImage.Opacity = 0.3;
                }
                else if (description == "moderate rain")
                {
                    BackgroundImage.Source = "moderaterain.jpg";
                    BackgroundImage.Opacity = 0.3;
                }
                else if (description == "light snow")
                {
                    BackgroundImage.Source = "lightsnow.jpeg";
                    BackgroundImage.Opacity = 0.3;
                }

                string icon = weatherBindingData.WeatherDataForecastHourly.List[0].WeatherForecast[0].IconForecast.ToString();

                if (icon == "01d")
                {
                    IconCondition.Source = "w01d.png";
                }
                else if (icon == "01n")
                {
                    IconCondition.Source = "w01n.png";
                }
                else if (icon == "02d")
                {
                    IconCondition.Source = "w02d.png";
                }
                else if (icon == "02n")
                {
                    IconCondition.Source = "w02n.png";
                }
                else if (icon == "03d")
                {
                    IconCondition.Source = "w03d.png";
                }
                else if (icon == "03n")
                {
                    IconCondition.Source = "w03n.png";
                }
                else if (icon == "04d")
                {
                    IconCondition.Source = "w04d.png";
                }
                else if (icon == "04n")
                {
                    IconCondition.Source = "w04n.png";
                }
                else if (icon == "09d")
                {
                    IconCondition.Source = "w09d.png";
                }
                else if (icon == "09n")
                {
                    IconCondition.Source = "w09n.png";
                }
                else if (icon == "10d")
                {
                    IconCondition.Source = "w10d.png";
                }
                else if (icon == "10n")
                {
                    IconCondition.Source = "w10n.png";
                }
                else if (icon == "11d")
                {
                    IconCondition.Source = "w11d.png";
                }
                else if (icon == "11n")
                {
                    IconCondition.Source = "w11n.png";
                }
                else if (icon == "13d")
                {
                    IconCondition.Source = "w13d.png";
                }
                else if (icon == "13n")
                {
                    IconCondition.Source = "w13n.png";
                }
                else if (icon == "50d")
                {
                    IconCondition.Source = "w50d.png";
                }
                else if (icon == "50n")
                {
                    IconCondition.Source = "w50n.png";
                }

                double RainChance = weatherBindingData.WeatherDataForecastHourly.List[0].ChanceOfRain * 100;
                ChanceOfRain.Text = RainChance.ToString() + "%";

                var tempArray = weatherBindingData.WeatherDataForecastHourly.List;

                var TemperaturesList = new List<TemperatureMinMaxAvg>();

                DateTime CurrentDate = DateTime.Now;

                var currentDate2 = CurrentDate.Date;

                for (var i = 0; i < 5; i++)
                {
                    double TempMinValue = 100;

                    double TempMaxValue = -100;

                    var currDay = currentDate2.AddDays(i);

                    var DateTimeNow = currDay.ToString("yyyy-MM-dd");

                    var ForecastTempDay = currDay.ToString("yyyy-MM-dd");

                    double sumAvg = 0;

                    int count = 0;

                    foreach (var item in tempArray)
                    {
                        DateTime DatesTemp1 = DateTime.ParseExact(item.DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

                        var TempDayNow = DatesTemp1.Date;

                        double avg = item.MainForecasts.TempForecast;

                        var TempDay = DatesTemp1.ToString("yyyy-MM-dd");

                        if (TempDay == DateTimeNow)
                        {
                            TempMinValue = Math.Min(TempMinValue, item.MainForecasts.TempMinForecast);

                            TempMaxValue = Math.Max(TempMaxValue, item.MainForecasts.TempMaxForecast);

                            sumAvg += avg;
                            count++;

                            ForecastTempDay = DateTimeNow;
                        }
                    }

                    //TemperaturesList[0].MaxTemperature.ToString();

                    TemperaturesList.Add(new TemperatureMinMaxAvg()
                    {
                        MinTemperature = Math.Round(TempMinValue),
                        MaxTemperature = Math.Round(TempMaxValue),
                        AvgTemperature = Math.Round(sumAvg / count),
                        DateTime5DaysForecast = ForecastTempDay
                    });

                    //set DateTime on ScrollView 
                    DateTime DateDays1 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[0].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);
                    string Info1 = DateDays1.ToString("HH:mm ");
                    Date1HourlyForecast.Text = Info1;

                    DateTime DateDays2 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[1].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);
                    string Info2 = DateDays2.ToString("HH:mm ");
                    Date2HourlyForecast.Text = Info2;

                    DateTime DateDays3 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[2].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);
                    string Info3 = DateDays3.ToString("HH:mm ");
                    Date3HourlyForecast.Text = Info3;

                    DateTime DateDays4 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[3].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);
                    string Info4 = DateDays4.ToString("HH:mm ");
                    Date4HourlyForecast.Text = Info4;

                    DateTime DateDays5 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[4].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);
                    string Info5 = DateDays5.ToString("HH:mm ");
                    Date5HourlyForecast.Text = Info5;

                    DateTime DateDays6 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[5].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);
                    string Info6 = DateDays6.ToString("HH:mm ");
                    Date6HourlyForecast.Text = Info6;

                    DateTime DateDays7 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[6].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);
                    string Info7 = DateDays7.ToString("HH:mm ");
                    Date7HourlyForecast.Text = Info7;

                    DateTime DateDays8 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[7].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);
                    string Info8 = DateDays8.ToString("HH:mm ");
                    Date8HourlyForecast.Text = Info8;

                    DateTime DateDays9 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[8].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);
                    string Info9 = DateDays9.ToString("HH:mm ");
                    Date9HourlyForecast.Text = Info9;

                    //set Icons on scrollview 
                    string iconStringHourly1 = weatherBindingData.WeatherDataForecastHourly.List[0].WeatherForecast[0].IconForecast.ToString();

                    if (iconStringHourly1 == "01d")
                    {
                        iconHourly1.Source = "w01d.png";
                    }
                    else if (iconStringHourly1 == "01n")
                    {
                        iconHourly1.Source = "w01n.png";
                    }
                    else if (iconStringHourly1 == "02d")
                    {
                        iconHourly1.Source = "w02d.png";
                    }
                    else if (iconStringHourly1 == "02n")
                    {
                        iconHourly1.Source = "w02n.png";
                    }
                    else if (iconStringHourly1 == "03d")
                    {
                        iconHourly1.Source = "w03d.png";
                    }
                    else if (iconStringHourly1 == "03n")
                    {
                        iconHourly1.Source = "w03n.png";
                    }
                    else if (iconStringHourly1 == "04d")
                    {
                        iconHourly1.Source = "w04d.png";
                    }
                    else if (iconStringHourly1 == "04n")
                    {
                        iconHourly1.Source = "w04n.png";
                    }
                    else if (iconStringHourly1 == "09d")
                    {
                        iconHourly1.Source = "w09d.png";
                    }
                    else if (iconStringHourly1 == "09n")
                    {
                        iconHourly1.Source = "w09n.png";
                    }
                    else if (iconStringHourly1 == "10d")
                    {
                        iconHourly1.Source = "w10d.png";
                    }
                    else if (iconStringHourly1 == "10n")
                    {
                        iconHourly1.Source = "w10n.png";
                    }
                    else if (iconStringHourly1 == "11d")
                    {
                        iconHourly1.Source = "w11d.png";
                    }
                    else if (iconStringHourly1 == "11n")
                    {
                        iconHourly1.Source = "w11n.png";
                    }
                    else if (iconStringHourly1 == "13d")
                    {
                        iconHourly1.Source = "w13d.png";
                    }
                    else if (iconStringHourly1 == "13n")
                    {
                        iconHourly1.Source = "w13n.png";
                    }
                    else if (iconStringHourly1 == "50d")
                    {
                        iconHourly1.Source = "w50d.png";
                    }
                    else if (iconStringHourly1 == "50n")
                    {
                        iconHourly1.Source = "w50n.png";
                    }

                    string iconStringHourly2 = weatherBindingData.WeatherDataForecastHourly.List[1].WeatherForecast[0].IconForecast.ToString();

                    if (iconStringHourly2 == "01d")
                    {
                        iconHourly2.Source = "w01d.png";
                    }
                    else if (iconStringHourly2 == "01n")
                    {
                        iconHourly2.Source = "w01n.png";
                    }
                    else if (iconStringHourly2 == "02d")
                    {
                        iconHourly2.Source = "w02d.png";
                    }
                    else if (iconStringHourly2 == "02n")
                    {
                        iconHourly2.Source = "w02n.png";
                    }
                    else if (iconStringHourly2 == "03d")
                    {
                        iconHourly2.Source = "w03d.png";
                    }
                    else if (iconStringHourly2 == "03n")
                    {
                        iconHourly2.Source = "w03n.png";
                    }
                    else if (iconStringHourly2 == "04d")
                    {
                        iconHourly2.Source = "w04d.png";
                    }
                    else if (iconStringHourly2 == "04n")
                    {
                        iconHourly2.Source = "w04n.png";
                    }
                    else if (iconStringHourly2 == "09d")
                    {
                        iconHourly2.Source = "w09d.png";
                    }
                    else if (iconStringHourly2 == "09n")
                    {
                        iconHourly2.Source = "w09n.png";
                    }
                    else if (iconStringHourly2 == "10d")
                    {
                        iconHourly2.Source = "w10d.png";
                    }
                    else if (iconStringHourly2 == "10n")
                    {
                        iconHourly2.Source = "w10n.png";
                    }
                    else if (iconStringHourly2 == "11d")
                    {
                        iconHourly2.Source = "w11d.png";
                    }
                    else if (iconStringHourly2 == "11n")
                    {
                        iconHourly2.Source = "w11n.png";
                    }
                    else if (iconStringHourly2 == "13d")
                    {
                        iconHourly2.Source = "w13d.png";
                    }
                    else if (iconStringHourly2 == "13n")
                    {
                        iconHourly2.Source = "w13n.png";
                    }
                    else if (iconStringHourly2 == "50d")
                    {
                        iconHourly2.Source = "w50d.png";
                    }
                    else if (iconStringHourly2 == "50n")
                    {
                        iconHourly2.Source = "w50n.png";
                    }

                    string iconStringHourly3 = weatherBindingData.WeatherDataForecastHourly.List[2].WeatherForecast[0].IconForecast.ToString();

                    if (iconStringHourly3 == "01d")
                    {
                        iconHourly3.Source = "w01d.png";
                    }
                    else if (iconStringHourly3 == "01n")
                    {
                        iconHourly3.Source = "w01n.png";
                    }
                    else if (iconStringHourly3 == "02d")
                    {
                        iconHourly3.Source = "w02d.png";
                    }
                    else if (iconStringHourly3 == "02n")
                    {
                        iconHourly3.Source = "w02n.png";
                    }
                    else if (iconStringHourly3 == "03d")
                    {
                        iconHourly3.Source = "w03d.png";
                    }
                    else if (iconStringHourly3 == "03n")
                    {
                        iconHourly3.Source = "w03n.png";
                    }
                    else if (iconStringHourly3 == "04d")
                    {
                        iconHourly3.Source = "w04d.png";
                    }
                    else if (iconStringHourly3 == "04n")
                    {
                        iconHourly3.Source = "w04n.png";
                    }
                    else if (iconStringHourly3 == "09d")
                    {
                        iconHourly3.Source = "w09d.png";
                    }
                    else if (iconStringHourly3 == "09n")
                    {
                        iconHourly3.Source = "w09n.png";
                    }
                    else if (iconStringHourly3 == "10d")
                    {
                        iconHourly3.Source = "w10d.png";
                    }
                    else if (iconStringHourly3 == "10n")
                    {
                        iconHourly3.Source = "w10n.png";
                    }
                    else if (iconStringHourly3 == "11d")
                    {
                        iconHourly3.Source = "w11d.png";
                    }
                    else if (iconStringHourly3 == "11n")
                    {
                        iconHourly3.Source = "w11n.png";
                    }
                    else if (iconStringHourly3 == "13d")
                    {
                        iconHourly3.Source = "w13d.png";
                    }
                    else if (iconStringHourly3 == "13n")
                    {
                        iconHourly3.Source = "w13n.png";
                    }
                    else if (iconStringHourly3 == "50d")
                    {
                        iconHourly3.Source = "w50d.png";
                    }
                    else if (iconStringHourly3 == "50n")
                    {
                        iconHourly3.Source = "w50n.png";
                    }

                    string iconStringHourly4 = weatherBindingData.WeatherDataForecastHourly.List[3].WeatherForecast[0].IconForecast.ToString();

                    if (iconStringHourly4 == "01d")
                    {
                        iconHourly4.Source = "w01d.png";
                    }
                    else if (iconStringHourly4 == "01n")
                    {
                        iconHourly4.Source = "w01n.png";
                    }
                    else if (iconStringHourly4 == "02d")
                    {
                        iconHourly4.Source = "w02d.png";
                    }
                    else if (iconStringHourly4 == "02n")
                    {
                        iconHourly4.Source = "w02n.png";
                    }
                    else if (iconStringHourly4 == "03d")
                    {
                        iconHourly4.Source = "w03d.png";
                    }
                    else if (iconStringHourly4 == "03n")
                    {
                        iconHourly4.Source = "w03n.png";
                    }
                    else if (iconStringHourly4 == "04d")
                    {
                        iconHourly4.Source = "w04d.png";
                    }
                    else if (iconStringHourly4 == "04n")
                    {
                        iconHourly4.Source = "w04n.png";
                    }
                    else if (iconStringHourly4 == "09d")
                    {
                        iconHourly4.Source = "w09d.png";
                    }
                    else if (iconStringHourly4 == "09n")
                    {
                        iconHourly4.Source = "w09n.png";
                    }
                    else if (iconStringHourly4 == "10d")
                    {
                        iconHourly4.Source = "w10d.png";
                    }
                    else if (iconStringHourly4 == "10n")
                    {
                        iconHourly4.Source = "w10n.png";
                    }
                    else if (iconStringHourly4 == "11d")
                    {
                        iconHourly4.Source = "w11d.png";
                    }
                    else if (iconStringHourly4 == "11n")
                    {
                        iconHourly4.Source = "w11n.png";
                    }
                    else if (iconStringHourly4 == "13d")
                    {
                        iconHourly4.Source = "w13d.png";
                    }
                    else if (iconStringHourly4 == "13n")
                    {
                        iconHourly4.Source = "w13n.png";
                    }
                    else if (iconStringHourly4 == "50d")
                    {
                        iconHourly4.Source = "w50d.png";
                    }
                    else if (iconStringHourly4 == "50n")
                    {
                        iconHourly4.Source = "w50n.png";
                    }

                    string iconStringHourly5 = weatherBindingData.WeatherDataForecastHourly.List[4].WeatherForecast[0].IconForecast.ToString();

                    if (iconStringHourly5 == "01d")
                    {
                        iconHourly5.Source = "w01d.png";
                    }
                    else if (iconStringHourly5 == "01n")
                    {
                        iconHourly5.Source = "w01n.png";
                    }
                    else if (iconStringHourly5 == "02d")
                    {
                        iconHourly5.Source = "w02d.png";
                    }
                    else if (iconStringHourly5 == "02n")
                    {
                        iconHourly5.Source = "w02n.png";
                    }
                    else if (iconStringHourly5 == "03d")
                    {
                        iconHourly5.Source = "w03d.png";
                    }
                    else if (iconStringHourly5 == "03n")
                    {
                        iconHourly5.Source = "w03n.png";
                    }
                    else if (iconStringHourly5 == "04d")
                    {
                        iconHourly5.Source = "w04d.png";
                    }
                    else if (iconStringHourly5 == "04n")
                    {
                        iconHourly5.Source = "w04n.png";
                    }
                    else if (iconStringHourly5 == "09d")
                    {
                        iconHourly5.Source = "w09d.png";
                    }
                    else if (iconStringHourly5 == "09n")
                    {
                        iconHourly5.Source = "w09n.png";
                    }
                    else if (iconStringHourly5 == "10d")
                    {
                        iconHourly5.Source = "w10d.png";
                    }
                    else if (iconStringHourly5 == "10n")
                    {
                        iconHourly5.Source = "w10n.png";
                    }
                    else if (iconStringHourly5 == "11d")
                    {
                        iconHourly5.Source = "w11d.png";
                    }
                    else if (iconStringHourly5 == "11n")
                    {
                        iconHourly5.Source = "w11n.png";
                    }
                    else if (iconStringHourly5 == "13d")
                    {
                        iconHourly5.Source = "w13d.png";
                    }
                    else if (iconStringHourly5 == "13n")
                    {
                        iconHourly5.Source = "w13n.png";
                    }
                    else if (iconStringHourly5 == "50d")
                    {
                        iconHourly5.Source = "w50d.png";
                    }
                    else if (iconStringHourly5 == "50n")
                    {
                        iconHourly5.Source = "w50n.png";
                    }

                    string iconStringHourly6 = weatherBindingData.WeatherDataForecastHourly.List[5].WeatherForecast[0].IconForecast.ToString();

                    if (iconStringHourly6 == "01d")
                    {
                        iconHourly6.Source = "w01d.png";
                    }
                    else if (iconStringHourly6 == "01n")
                    {
                        iconHourly6.Source = "w01n.png";
                    }
                    else if (iconStringHourly6 == "02d")
                    {
                        iconHourly6.Source = "w02d.png";
                    }
                    else if (iconStringHourly6 == "02n")
                    {
                        iconHourly6.Source = "w02n.png";
                    }
                    else if (iconStringHourly6 == "03d")
                    {
                        iconHourly6.Source = "w03d.png";
                    }
                    else if (iconStringHourly6 == "03n")
                    {
                        iconHourly6.Source = "w03n.png";
                    }
                    else if (iconStringHourly6 == "04d")
                    {
                        iconHourly6.Source = "w04d.png";
                    }
                    else if (iconStringHourly6 == "04n")
                    {
                        iconHourly6.Source = "w04n.png";
                    }
                    else if (iconStringHourly6 == "09d")
                    {
                        iconHourly6.Source = "w09d.png";
                    }
                    else if (iconStringHourly6 == "09n")
                    {
                        iconHourly6.Source = "w09n.png";
                    }
                    else if (iconStringHourly6 == "10d")
                    {
                        iconHourly6.Source = "w10d.png";
                    }
                    else if (iconStringHourly6 == "10n")
                    {
                        iconHourly6.Source = "w10n.png";
                    }
                    else if (iconStringHourly6 == "11d")
                    {
                        iconHourly6.Source = "w11d.png";
                    }
                    else if (iconStringHourly6 == "11n")
                    {
                        iconHourly6.Source = "w11n.png";
                    }
                    else if (iconStringHourly6 == "13d")
                    {
                        iconHourly6.Source = "w13d.png";
                    }
                    else if (iconStringHourly6 == "13n")
                    {
                        iconHourly6.Source = "w13n.png";
                    }
                    else if (iconStringHourly6 == "50d")
                    {
                        iconHourly6.Source = "w50d.png";
                    }
                    else if (iconStringHourly6 == "50n")
                    {
                        iconHourly6.Source = "w50n.png";
                    }

                    string iconStringHourly7 = weatherBindingData.WeatherDataForecastHourly.List[6].WeatherForecast[0].IconForecast.ToString();

                    if (iconStringHourly7 == "01d")
                    {
                        iconHourly7.Source = "w01d.png";
                    }
                    else if (iconStringHourly7 == "01n")
                    {
                        iconHourly7.Source = "w01n.png";
                    }
                    else if (iconStringHourly7 == "02d")
                    {
                        iconHourly7.Source = "w02d.png";
                    }
                    else if (iconStringHourly7 == "02n")
                    {
                        iconHourly7.Source = "w02n.png";
                    }
                    else if (iconStringHourly7 == "03d")
                    {
                        iconHourly7.Source = "w03d.png";
                    }
                    else if (iconStringHourly7 == "03n")
                    {
                        iconHourly7.Source = "w03n.png";
                    }
                    else if (iconStringHourly7 == "04d")
                    {
                        iconHourly7.Source = "w04d.png";
                    }
                    else if (iconStringHourly7 == "04n")
                    {
                        iconHourly7.Source = "w04n.png";
                    }
                    else if (iconStringHourly7 == "09d")
                    {
                        iconHourly7.Source = "w09d.png";
                    }
                    else if (iconStringHourly7 == "09n")
                    {
                        iconHourly7.Source = "w09n.png";
                    }
                    else if (iconStringHourly7 == "10d")
                    {
                        iconHourly7.Source = "w10d.png";
                    }
                    else if (iconStringHourly7 == "10n")
                    {
                        iconHourly7.Source = "w10n.png";
                    }
                    else if (iconStringHourly7 == "11d")
                    {
                        iconHourly7.Source = "w11d.png";
                    }
                    else if (iconStringHourly7 == "11n")
                    {
                        iconHourly7.Source = "w11n.png";
                    }
                    else if (iconStringHourly7 == "13d")
                    {
                        iconHourly7.Source = "w13d.png";
                    }
                    else if (iconStringHourly7 == "13n")
                    {
                        iconHourly7.Source = "w13n.png";
                    }
                    else if (iconStringHourly7 == "50d")
                    {
                        iconHourly7.Source = "w50d.png";
                    }
                    else if (iconStringHourly7 == "50n")
                    {
                        iconHourly7.Source = "w50n.png";
                    }

                    string iconStringHourly8 = weatherBindingData.WeatherDataForecastHourly.List[7].WeatherForecast[0].IconForecast.ToString();

                    if (iconStringHourly8 == "01d")
                    {
                        iconHourly8.Source = "w01d.png";
                    }
                    else if (iconStringHourly8 == "01n")
                    {
                        iconHourly8.Source = "w01n.png";
                    }
                    else if (iconStringHourly8 == "02d")
                    {
                        iconHourly8.Source = "w02d.png";
                    }
                    else if (iconStringHourly8 == "02n")
                    {
                        iconHourly8.Source = "w02n.png";
                    }
                    else if (iconStringHourly8 == "03d")
                    {
                        iconHourly8.Source = "w03d.png";
                    }
                    else if (iconStringHourly8 == "03n")
                    {
                        iconHourly8.Source = "w03n.png";
                    }
                    else if (iconStringHourly8 == "04d")
                    {
                        iconHourly8.Source = "w04d.png";
                    }
                    else if (iconStringHourly8 == "04n")
                    {
                        iconHourly8.Source = "w04n.png";
                    }
                    else if (iconStringHourly8 == "09d")
                    {
                        iconHourly8.Source = "w09d.png";
                    }
                    else if (iconStringHourly8 == "09n")
                    {
                        iconHourly8.Source = "w09n.png";
                    }
                    else if (iconStringHourly8 == "10d")
                    {
                        iconHourly8.Source = "w10d.png";
                    }
                    else if (iconStringHourly8 == "10n")
                    {
                        iconHourly8.Source = "w10n.png";
                    }
                    else if (iconStringHourly8 == "11d")
                    {
                        iconHourly8.Source = "w11d.png";
                    }
                    else if (iconStringHourly8 == "11n")
                    {
                        iconHourly8.Source = "w11n.png";
                    }
                    else if (iconStringHourly8 == "13d")
                    {
                        iconHourly8.Source = "w13d.png";
                    }
                    else if (iconStringHourly8 == "13n")
                    {
                        iconHourly8.Source = "w13n.png";
                    }
                    else if (iconStringHourly8 == "50d")
                    {
                        iconHourly8.Source = "w50d.png";
                    }
                    else if (iconStringHourly8 == "50n")
                    {
                        iconHourly8.Source = "w50n.png";
                    }

                    string iconStringHourly9 = weatherBindingData.WeatherDataForecastHourly.List[8].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly9 = "http://openweathermap.org/img/wn/" + iconStringHourly9 + "@4x.png";
                    iconHourly9.Source = urlHourly9;

                    if (iconStringHourly9 == "01d")
                    {
                        iconHourly9.Source = "w01d.png";
                    }
                    else if (iconStringHourly9 == "01n")
                    {
                        iconHourly9.Source = "w01n.png";
                    }
                    else if (iconStringHourly9 == "02d")
                    {
                        iconHourly9.Source = "w02d.png";
                    }
                    else if (iconStringHourly9 == "02n")
                    {
                        iconHourly9.Source = "w02n.png";
                    }
                    else if (iconStringHourly9 == "03d")
                    {
                        iconHourly9.Source = "w03d.png";
                    }
                    else if (iconStringHourly9 == "03n")
                    {
                        iconHourly9.Source = "w03n.png";
                    }
                    else if (iconStringHourly9 == "04d")
                    {
                        iconHourly9.Source = "w04d.png";
                    }
                    else if (iconStringHourly9 == "04n")
                    {
                        iconHourly9.Source = "w04n.png";
                    }
                    else if (iconStringHourly9 == "09d")
                    {
                        iconHourly9.Source = "w09d.png";
                    }
                    else if (iconStringHourly9 == "09n")
                    {
                        iconHourly9.Source = "w09n.png";
                    }
                    else if (iconStringHourly9 == "10d")
                    {
                        iconHourly9.Source = "w10d.png";
                    }
                    else if (iconStringHourly9 == "10n")
                    {
                        iconHourly9.Source = "w10n.png";
                    }
                    else if (iconStringHourly9 == "11d")
                    {
                        iconHourly9.Source = "w11d.png";
                    }
                    else if (iconStringHourly9 == "11n")
                    {
                        iconHourly9.Source = "w11n.png";
                    }
                    else if (iconStringHourly9 == "13d")
                    {
                        iconHourly9.Source = "w13d.png";
                    }
                    else if (iconStringHourly9 == "13n")
                    {
                        iconHourly9.Source = "w13n.png";
                    }
                    else if (iconStringHourly9 == "50d")
                    {
                        iconHourly9.Source = "w50d.png";
                    }
                    else if (iconStringHourly9 == "50n")
                    {
                        iconHourly9.Source = "w50n.png";
                    }

                    //set Temperature values on scrollView
                    string TempValueForecast1 = Math.Round(weatherBindingData.WeatherDataForecastHourly.List[0].MainForecasts.TempForecast, 0).ToString() + "°";
                    TempForecast1.Text = TempValueForecast1;

                    string TempValueForecast2 = Math.Round(weatherBindingData.WeatherDataForecastHourly.List[1].MainForecasts.TempForecast, 0).ToString() + "°";
                    TempForecast2.Text = TempValueForecast2;

                    string TempValueForecast3 = Math.Round(weatherBindingData.WeatherDataForecastHourly.List[2].MainForecasts.TempForecast, 0).ToString() + "°";
                    TempForecast3.Text = TempValueForecast3;

                    string TempValueForecast4 = Math.Round(weatherBindingData.WeatherDataForecastHourly.List[3].MainForecasts.TempForecast, 0).ToString() + "°";
                    TempForecast4.Text = TempValueForecast4;

                    string TempValueForecast5 = Math.Round(weatherBindingData.WeatherDataForecastHourly.List[4].MainForecasts.TempForecast, 0).ToString() + "°";
                    TempForecast5.Text = TempValueForecast5;

                    string TempValueForecast6 = Math.Round(weatherBindingData.WeatherDataForecastHourly.List[5].MainForecasts.TempForecast, 0).ToString() + "°";
                    TempForecast6.Text = TempValueForecast6;

                    string TempValueForecast7 = Math.Round(weatherBindingData.WeatherDataForecastHourly.List[6].MainForecasts.TempForecast, 0).ToString() + "°";
                    TempForecast7.Text = TempValueForecast7;

                    string TempValueForecast8 = Math.Round(weatherBindingData.WeatherDataForecastHourly.List[7].MainForecasts.TempForecast, 0).ToString() + "°";
                    TempForecast8.Text = TempValueForecast8;

                    string TempValueForecast9 = Math.Round(weatherBindingData.WeatherDataForecastHourly.List[8].MainForecasts.TempForecast, 0).ToString() + "°";
                    TempForecast9.Text = TempValueForecast9;
                }

                HighLowTemperature.Text = "L:" + Math.Round(TemperaturesList[0].MinTemperature) + "°C" + " H:" + Math.Round(TemperaturesList[0].MaxTemperature) + "°C";
            }

            void OnGetWeatherButtonClicked(System.Object sender, System.EventArgs e)
            {
                _ = CurrentWeather();
            }

        string GenerateRequestUriGPS(string endpoint, Xamarin.Essentials.Location location)
        {
            string requestUri = endpoint;
            requestUri += $"?lat={location.Latitude}";
            requestUri += $"&lon={location.Longitude}";
            requestUri += "&units=metric"; // or units=metric
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }

        private static string GenerateRequestUriAir(string endpoint, Location location1)
        {
            string requestUri = endpoint;
            requestUri += $"?lat={location1.Latitude}";
            requestUri += $"&lon={location1.Longitude}";
            requestUri += $"&APPID={ConstantsAir.OpenWeatherMapAPIKey3}";
            return requestUri;
        }

        async Task ButtonClickedGPS()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Denied && status == PermissionStatus.Disabled)
            {
                _ = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            else
            {
                try
                {
                    var location = await Geolocation.GetLastKnownLocationAsync();

                    if (location != null)
                    {
                        WeatherBindingData weatherBindingData = await _restServiceForecast.GetWeatherDataForecast(GenerateRequestUriGPS(Constants.OpenWeatherMapEndpoint, location), GenerateRequestUriForecast(ConstantsForecast.OpenWeatherMapEndpoint2, _cityEntry), GenerateRequestUriAir(ConstantsAir.OpenWeatherMapEndpoint3, location));
                        
                        var CityName = weatherBindingData.WeatherDataCurrent.Title.ToString();
                        _cityEntry.Text = CityName;

                        await CurrentWeather();
                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    // Handle not supported on device exception
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    // Handle not enabled on device exception
                }
                catch (PermissionException pEx)
                {
                    // Handle permission exception
                }
                catch (Exception ex)
                {
                    // Unable to get location
                }
            }
        }

        async void OnGetGPS(System.Object sender, System.EventArgs e)
        {
            await ButtonClickedGPS();
            await CurrentWeather();
        }

         void OnGetFavorites(System.Object sender, System.EventArgs e)
        {
            Preferences.Set("cityName", _cityEntry.Text);

            string city = _cityEntry.Text;

            var tabbedPage = this.Parent.Parent as TabbedPage;

            var page = (Favorites)tabbedPage.Children[2];

            page.AddCity(_cityEntry.Text);

            tabbedPage.CurrentPage = page;
        }
    }
}