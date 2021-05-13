using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using EntryMicrocharts = Microcharts.ChartEntry;
using Microcharts;
using SkiaSharp;
using Xamarin.Essentials;
using System.Collections.Generic;
using Entry = Xamarin.Forms.Entry;

namespace WeatherLocationInfo
{
    public partial class MainPage : ContentPage
    {
     

        RestServiceForecast _restServiceForecast;

        public MainPage()
        {
            InitializeComponent();

            _restServiceForecast = new RestServiceForecast();

            _ = DisplayHourlyForecastStartUp();

            _ = ButtonClickedGPS();

            co2lbl.Text = "CO2";
            nolbl.Text = "NO";
            no2lbl.Text = "NO2";
            o3lbl.Text = "O3";
            so2lbl.Text = "SO2";
            pm2_5lbl.Text = "PM2_5";
            pm10lbl.Text = "PM10";
            nh3lbl.Text = "NH3";

            frameFinePowders.IsVisible = false;
        }



        private static string GenerateRequestUriAir(string endpoint, Location location1)
        {
            string requestUri = endpoint;
            requestUri += $"?lat={location1.Latitude}";
            requestUri += $"&lon={location1.Longitude}";
            requestUri += $"&APPID={ConstantsAir.OpenWeatherMapAPIKey3}";
            return requestUri;
        }

        async Task DisplayHourlyForecast()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_cityEntry.Text))
                {
                    WeatherBindingData weatherBindingData = await _restServiceForecast.GetWeatherDataForecast(GenerateRequestUri(Constants.OpenWeatherMapEndpoint, _cityEntry), GenerateRequestUriForecast(ConstantsForecast.OpenWeatherMapEndpoint2, _cityEntry), GenerateRequestUriForecast(ConstantsForecast.OpenWeatherMapEndpoint2, _cityEntry));
                    BindingContext = weatherBindingData;

                    string description = weatherBindingData.WeatherDataForecastHourly.List[0].WeatherForecast[0].DescriptionForecast;

                    if (description == "clear sky")
                    {
                        BackgroundImage.Source = "Images/ClearSky.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "few clouds")
                    {
                        BackgroundImage.Source = "Images/FewClouds.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "scattered clouds")
                    {
                        BackgroundImage.Source = "Images/Scattering.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "broken clouds")
                    {
                        BackgroundImage.Source = "Images/BrokenClouds.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "light rain")
                    {
                        BackgroundImage.Source = "Images/LightRain.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "rain")
                    {
                        BackgroundImage.Source = "Images/Rain.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "thunderstorm")
                    {
                        BackgroundImage.Source = "Images/Thunderstorm.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "snow")
                    {
                        BackgroundImage.Source = "Images/Snow.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "mist")
                    {
                        BackgroundImage.Source = "Images/Mist.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "overcast clouds")
                    {
                        BackgroundImage.Source = "Images/OverCastClouds.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "moderate rain")
                    {
                        BackgroundImage.Source = "Images/ModerateRain.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "light snow")
                    {
                        BackgroundImage.Source = "Images/LightSnow.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }

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
                    }

                    //set Temperature on first day from forecast
                    AvgTempForecastDay1.Text = TemperaturesList[0].AvgTemperature.ToString() + "°C";
                    MinTempForecastDay1.Text = TemperaturesList[0].MinTemperature.ToString() + "°C";
                    MaxTempForecastDay1.Text = TemperaturesList[0].MaxTemperature.ToString() + "°C";

                    //set Temperature on second day from forecast
                    AvgTempForecastDay2.Text = TemperaturesList[1].AvgTemperature.ToString() + "°C";
                    MinTempForecastDay2.Text = TemperaturesList[1].MinTemperature.ToString() + "°C";
                    MaxTempForecastDay2.Text = TemperaturesList[1].MaxTemperature.ToString() + "°C";

                    //set Temperature on third day from forecast
                    AvgTempForecastDay3.Text = TemperaturesList[2].AvgTemperature.ToString() + "°C";
                    MinTempForecastDay3.Text = TemperaturesList[2].MinTemperature.ToString() + "°C";
                    MaxTempForecastDay3.Text = TemperaturesList[2].MaxTemperature.ToString() + "°C";

                    //set Temperature on forth day from forecast
                    AvgTempForecastDay4.Text = TemperaturesList[3].AvgTemperature.ToString() + "°C";
                    MinTempForecastDay4.Text = TemperaturesList[3].MinTemperature.ToString() + "°C";
                    MaxTempForecastDay4.Text = TemperaturesList[3].MaxTemperature.ToString() + "°C";

                    //set Temperature on fiveth day from forecast
                    AvgTempForecastDay5.Text = TemperaturesList[4].AvgTemperature.ToString() + "°C";
                    MinTempForecastDay5.Text = TemperaturesList[4].MinTemperature.ToString() + "°C";
                    MaxTempForecastDay5.Text = TemperaturesList[4].MaxTemperature.ToString() + "°C";

                    //Set Icon and strings with binding weather data from API
                    string icon = weatherBindingData.WeatherDataForecastHourly.List[0].WeatherForecast[0].IconForecast.ToString();
                    string url = "https://openweathermap.org/img/wn/" + icon + "@4x.png";
                    IconCurrentTemp.Source = url;
                    HighLowTemperature.Text = "L: " + Math.Round(TemperaturesList[0].MinTemperature) + "°" + " H: " + Math.Round(TemperaturesList[0].MaxTemperature) + "°";
                    string urlFeelsLike = "https://img.icons8.com/color/96/000000/cool--v1.png";
                 
                    FeelsLike.Text = "Feels Like: " + Math.Round(weatherBindingData.WeatherDataCurrent.Main.FeelsLike) + "°C";
                    string urlSunrise = "https://img.icons8.com/plasticine/100/000000/sunrise.png";
                    IconSunrise.Source = urlSunrise;
                    string urlSunset = "https://img.icons8.com/plasticine/100/000000/sunset.png";
                    IconSunset.Source = urlSunset;

                    //Set 6 Icons for Clouds, Wind Speed, Humidity, Pressure, Chance of Rain and Rain Values
                    string urlClouds = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconClouds.Source = urlClouds;
                    string urlWindSpeed = "https://img.icons8.com/color/96/000000/wind.png";
                    IconWindSpeed.Source = urlWindSpeed;
                    string urlHumidity = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconHumidity.Source = urlHumidity;
                    string urlPressure = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconPressure.Source = urlPressure;
                    string urlChanceOfRain = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconChanceOfRain.Source = urlChanceOfRain;
                    string urlRainValue = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconRainValue.Source = urlRainValue;
                    string urlSnowValue = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconSnowValue.Source = urlSnowValue;

                    double chanceOfRain = weatherBindingData.WeatherDataForecastHourly.List[0].ChanceOfRain * 100;
                    ChanceOfRainLbl.Text = chanceOfRain.ToString() + "%";

                    

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
                    string urlHourly1 = "https://openweathermap.org/img/wn/" + iconStringHourly1 + "@4x.png";
                    iconHourly1.Source = urlHourly1;

                    string iconStringHourly2 = weatherBindingData.WeatherDataForecastHourly.List[1].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly2 = "https://openweathermap.org/img/wn/" + iconStringHourly2 + "@4x.png";
                    iconHourly2.Source = urlHourly2;

                    string iconStringHourly3 = weatherBindingData.WeatherDataForecastHourly.List[2].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly3 = "https://openweathermap.org/img/wn/" + iconStringHourly3 + "@4x.png";
                    iconHourly3.Source = urlHourly3;

                    string iconStringHourly4 = weatherBindingData.WeatherDataForecastHourly.List[3].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly4 = "https://openweathermap.org/img/wn/" + iconStringHourly4 + "@4x.png";
                    iconHourly4.Source = urlHourly4;

                    string iconStringHourly5 = weatherBindingData.WeatherDataForecastHourly.List[4].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly5 = "https://openweathermap.org/img/wn/" + iconStringHourly5 + "@4x.png";
                    iconHourly5.Source = urlHourly5;

                    string iconStringHourly6 = weatherBindingData.WeatherDataForecastHourly.List[5].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly6 = "https://openweathermap.org/img/wn/" + iconStringHourly6 + "@4x.png";
                    iconHourly6.Source = urlHourly6;

                    string iconStringHourly7 = weatherBindingData.WeatherDataForecastHourly.List[6].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly7 = "https://openweathermap.org/img/wn/" + iconStringHourly7 + "@4x.png";
                    iconHourly7.Source = urlHourly7;

                    string iconStringHourly8 = weatherBindingData.WeatherDataForecastHourly.List[7].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly8 = "https://openweathermap.org/img/wn/" + iconStringHourly8 + "@4x.png";
                    iconHourly8.Source = urlHourly8;

                    string iconStringHourly9 = weatherBindingData.WeatherDataForecastHourly.List[8].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly9 = "https://openweathermap.org/img/wn/" + iconStringHourly9 + "@4x.png";
                    iconHourly9.Source = urlHourly9;

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

                    //First Day Forecast

                    string descriptionForecast1 = weatherBindingData.WeatherDataForecastHourly.List[0].WeatherForecast[0].DescriptionForecast;
                    ImageDescriptionForecast1.Opacity = 0.5;
                    ImageDescriptionForecast1.VerticalOptions = LayoutOptions.FillAndExpand;
                    ImageDescriptionForecast1.HorizontalOptions = LayoutOptions.FillAndExpand;
                   

                    if (descriptionForecast1 == "clear sky")
                    {
                        ImageDescriptionForecast1.Source = "Images/ClearSky.jpg";
                      
                    }
                    else if (descriptionForecast1 == "few clouds")
                    {
                        ImageDescriptionForecast1.Source = "Images/FewClouds.jpg";
                     
                    }
                    else if (descriptionForecast1 == "scattered clouds")
                    {
                        ImageDescriptionForecast1.Source = "Images/Scattering.jpg";
                        
                    }
                    else if (descriptionForecast1 == "broken clouds")
                    {
                        ImageDescriptionForecast1.Source = "Images/BrokenClouds.jpg";
                    
                    }
                    else if (descriptionForecast1 == "light rain")
                    {
                        ImageDescriptionForecast1.Source = "Images/LightRain.jpg";
                        
                    }
                    else if (descriptionForecast1 == "rain")
                    {
                        ImageDescriptionForecast1.Source = "Images/Rain.jpg";
                       
                    }
                    else if (descriptionForecast1 == "thunderstorm")
                    {
                        ImageDescriptionForecast1.Source = "Images/Thunderstorm.jpg";
                    
                    }
                    else if (descriptionForecast1 == "snow")
                    {
                        ImageDescriptionForecast1.Source = "Images/Snow.jpg";
                      
                    }
                    else if (descriptionForecast1 == "mist")
                    {
                        ImageDescriptionForecast1.Source = "Images/Mist.jpg";
                    
                    }
                    else if (descriptionForecast1 == "overcast clouds")
                    {
                        ImageDescriptionForecast1.Source = "Images/OverCastClouds.jpg";
                       
                    }
                    else if (descriptionForecast1 == "moderate rain")
                    {
                        ImageDescriptionForecast1.Source = "Images/ModerateRain.jpg";
                       
                    }
                    else if (descriptionForecast1 == "light snow")
                    {
                        ImageDescriptionForecast1.Source = "Images/LightSnow.jpg";
                
                    }

                    DateTime DateDays1Forecast = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[0].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

                    string Info1Forecast = DateDays1Forecast.DayOfWeek.ToString();
                    DateTimeForecast1.Text = Info1Forecast;

                    DateTime DateDays2Forecast = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[0].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                       System.Globalization.CultureInfo.InvariantCulture);

                    string Info2Forecast = DateDays2Forecast.ToString("dd-MM-yy");
                    DateTimeForecast2.Text = Info2Forecast;

                    string IconForecast1 = weatherBindingData.WeatherDataForecastHourly.List[0].WeatherForecast[0].IconForecast.ToString();
                    string urlIconForecast1 = "https://openweathermap.org/img/wn/" + IconForecast1 + "@4x.png";
                    IconForecast11.Source = urlIconForecast1;

                    string urlForecast2 = "https://img.icons8.com/carbon-copy/100/000000/temperature-sensitive.png";
                    IconForecast12.Source = urlForecast2;

                    string urlForecast3 = "https://img.icons8.com/color/96/000000/wind.png";
                    IconForecast13.Source = urlForecast3;

                    string urlForecast4 = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconForecast14.Source = urlForecast4;

                    string urlForecast5 = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconForecast15.Source = urlForecast5;

                    double chanceOfRainForecast = weatherBindingData.WeatherDataForecastHourly.List[0].ChanceOfRain * 100;
                    ChanceOfRainForecast1.Text = chanceOfRainForecast.ToString() + "%";

                    string urlFeelsLike1 = "https://img.icons8.com/color/96/000000/cool--v1.png";
                    IconForecast5Days1.Source = urlFeelsLike1;

                    string urlFeelsLike2 = "https://img.icons8.com/ultraviolet/80/000000/temperature--v1.png";
                    IconForecast5Days2.Source = urlFeelsLike2;

                    string urlFeelsLike3 = "https://img.icons8.com/color/96/000000/thermometer.png";
                    IconForecast5Days3.Source = urlFeelsLike3;

                    string urlFeelsLike4 = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconForecast5Days4.Source = urlFeelsLike4;

                    string urlFeelsLike5 = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconForecast5Days5.Source = urlFeelsLike5;

                    string urlFeelsLike26 = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconForecast5Days26.Source = urlFeelsLike26;

                    string urlFeelsLike27 = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconForecast5Days27.Source = urlFeelsLike27;

                    string urlFeelsLike28 = "https://img.icons8.com/color/96/000000/ground.png";
                    IconForecast5Days28.Source = urlFeelsLike28;

                    string urlFeelsLike29 = "https://img.icons8.com/color/96/000000/low-tide.png";
                    IconForecast5Days29.Source = urlFeelsLike29;

                    string urlFeelsLike30 = "https://img.icons8.com/color/48/000000/windsock--v1.png";
                    IconForecast5Days30.Source = urlFeelsLike30;

                    //Second Day Forecast
                    string descriptionForecast2 = weatherBindingData.WeatherDataForecastHourly.List[8].WeatherForecast[0].DescriptionForecast;

                    ImageDescriptionForecast2.Opacity = 0.5;
                    ImageDescriptionForecast2.VerticalOptions = LayoutOptions.FillAndExpand;
                    ImageDescriptionForecast2.HorizontalOptions = LayoutOptions.FillAndExpand;
               

                    if (descriptionForecast2 == "clear sky")
                    {
                        ImageDescriptionForecast2.Source = "Images/ClearSky.jpg";
                      
                    }
                    else if (descriptionForecast2 == "few clouds")
                    {
                        ImageDescriptionForecast2.Source = "Images/FewClouds.jpg";
                      
                    }
                    else if (descriptionForecast2 == "scattered clouds")
                    {
                        ImageDescriptionForecast2.Source = "Images/Scattering.jpg";
                        
                    }
                    else if (descriptionForecast2 == "broken clouds")
                    {
                        ImageDescriptionForecast2.Source = "Images/BrokenClouds.jpg";
                       
                    }
                    else if (descriptionForecast2 == "light rain")
                    {
                        ImageDescriptionForecast2.Source = "Images/LightRain.jpg";
                       
                    }
                    else if (descriptionForecast2 == "rain")
                    {
                        ImageDescriptionForecast2.Source = "Images/Rain.jpg";
                       
                    }
                    else if (descriptionForecast2 == "thunderstorm")
                    {
                        ImageDescriptionForecast2.Source = "Images/Thunderstorm.jpg";
                       
                    }
                    else if (descriptionForecast2 == "snow")
                    {
                        ImageDescriptionForecast2.Source = "Images/Snow.jpg";
                    
                    }
                    else if (descriptionForecast2 == "mist")
                    {
                        ImageDescriptionForecast2.Source = "Images/Mist.jpg";
                      
                    }
                    else if (descriptionForecast2 == "overcast clouds")
                    {
                        ImageDescriptionForecast2.Source = "Images/OverCastClouds.jpg";
                       
                    }
                    else if (descriptionForecast2 == "moderate rain")
                    {
                        ImageDescriptionForecast2.Source = "Images/ModerateRain.jpg";
                       
                    }
                    else if (descriptionForecast2 == "light snow")
                    {
                        ImageDescriptionForecast2.Source = "Images/LightSnow.jpg";
                        
                    }

                    DateTime DateDays1Forecast3 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[8].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

                    string Info1Forecast3 = DateDays1Forecast3.DayOfWeek.ToString();
                    DateTimeForecast3.Text = Info1Forecast3;

                    DateTime DateDays2Forecast4 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[8].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                       System.Globalization.CultureInfo.InvariantCulture);

                    string Info2Forecast4 = DateDays2Forecast4.ToString("dd-MM-yy");
                    DateTimeForecast4.Text = Info2Forecast4;

                    string IconForecast6 = weatherBindingData.WeatherDataForecastHourly.List[8].WeatherForecast[0].IconForecast.ToString();
                    string urlIconForecast6 = "https://openweathermap.org/img/wn/" + IconForecast6 + "@4x.png";
                    IconForecast1111.Source = urlIconForecast6;

                    string urlForecast7 = "https://img.icons8.com/carbon-copy/100/000000/temperature-sensitive.png";
                    IconForecast1211.Source = urlForecast7;

                    string urlForecast8 = "https://img.icons8.com/color/96/000000/wind.png";
                    IconForecast1311.Source = urlForecast8;

                    string urlForecast9 = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconForecast1411.Source = urlForecast9;

                    string urlForecast10 = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconForecast1511.Source = urlForecast10;

                    double chanceOfRainForecast2 = weatherBindingData.WeatherDataForecastHourly.List[8].ChanceOfRain * 100;
                    ChanceOfRainForecast111.Text = chanceOfRainForecast2.ToString() + "%";

                    string urlFeelsLike6 = "https://img.icons8.com/color/96/000000/cool--v1.png";
                    IconForecast5Days6.Source = urlFeelsLike6;

                    string urlFeelsLike7 = "https://img.icons8.com/ultraviolet/80/000000/temperature--v1.png";
                    IconForecast5Days7.Source = urlFeelsLike7;

                    string urlFeelsLike8 = "https://img.icons8.com/color/96/000000/thermometer.png";
                    IconForecast5Days8.Source = urlFeelsLike8;

                    string urlFeelsLike9 = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconForecast5Days9.Source = urlFeelsLike9;

                    string urlFeelsLike10 = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconForecast5Days10.Source = urlFeelsLike10;

                    string urlFeelsLike31 = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconForecast5Days31.Source = urlFeelsLike31;

                    string urlFeelsLike32 = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconForecast5Days32.Source = urlFeelsLike32;

                    string urlFeelsLike33 = "https://img.icons8.com/color/96/000000/ground.png";
                    IconForecast5Days33.Source = urlFeelsLike33;

                    string urlFeelsLike34 = "https://img.icons8.com/color/96/000000/low-tide.png";
                    IconForecast5Days34.Source = urlFeelsLike34;

                    string urlFeelsLike35 = "https://img.icons8.com/color/48/000000/windsock--v1.png";
                    IconForecast5Days35.Source = urlFeelsLike35;

                    //Third Day Forecast
                    string descriptionForecast3 = weatherBindingData.WeatherDataForecastHourly.List[16].WeatherForecast[0].DescriptionForecast;

                    ImageDescriptionForecast3.Opacity = 0.5;
                    ImageDescriptionForecast3.VerticalOptions = LayoutOptions.FillAndExpand;
                    ImageDescriptionForecast3.HorizontalOptions = LayoutOptions.FillAndExpand;
                

                    if (descriptionForecast3 == "clear sky")
                    {
                        ImageDescriptionForecast3.Source = "Images/ClearSky.jpg";
                        
                    }
                    else if (descriptionForecast3 == "few clouds")
                    {
                        ImageDescriptionForecast3.Source = "Images/FewClouds.jpg";
                      
                    }
                    else if (descriptionForecast3 == "scattered clouds")
                    {
                        ImageDescriptionForecast3.Source = "Images/Scattering.jpg";
                       
                    }
                    else if (descriptionForecast3 == "broken clouds")
                    {
                        ImageDescriptionForecast3.Source = "Images/BrokenClouds.jpg";
                        
                    }
                    else if (descriptionForecast3 == "light rain")
                    {
                        ImageDescriptionForecast3.Source = "Images/LightRain.jpg";
                       
                    }
                    else if (descriptionForecast3 == "rain")
                    {
                        ImageDescriptionForecast3.Source = "Images/Rain.jpg";
                        
                    }
                    else if (descriptionForecast3 == "thunderstorm")
                    {
                        ImageDescriptionForecast3.Source = "Images/Thunderstorm.jpg";
                       
                    }
                    else if (descriptionForecast3 == "snow")
                    {
                        ImageDescriptionForecast3.Source = "Images/Snow.jpg";
                    
                    }
                    else if (descriptionForecast3 == "mist")
                    {
                        ImageDescriptionForecast3.Source = "Images/Mist.jpg";
                       
                    }
                    else if (descriptionForecast3 == "overcast clouds")
                    {
                        ImageDescriptionForecast3.Source = "Images/OverCastClouds.jpg";
                      
                    }
                    else if (descriptionForecast3 == "moderate rain")
                    {
                        ImageDescriptionForecast3.Source = "Images/ModerateRain.jpg";
                        
                    }
                    else if (descriptionForecast3 == "light snow")
                    {
                        ImageDescriptionForecast3.Source = "Images/LightSnow.jpg";
                       
                    }

                    DateTime DateDays1Forecast5 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[16].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

                    string Info1Forecast5 = DateDays1Forecast5.DayOfWeek.ToString();
                    DateTimeForecast5.Text = Info1Forecast5;

                    DateTime DateDays2Forecast6 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[16].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                       System.Globalization.CultureInfo.InvariantCulture);

                    string Info2Forecast6 = DateDays2Forecast6.ToString("dd-MM-yy");
                    DateTimeForecast6.Text = Info2Forecast6;

                    string IconForecast112 = weatherBindingData.WeatherDataForecastHourly.List[16].WeatherForecast[0].IconForecast.ToString();
                    string urlIconForecast11 = "https://openweathermap.org/img/wn/" + IconForecast112 + "@4x.png";
                    IconForecast1122.Source = urlIconForecast11;

                    string urlForecast12 = "https://img.icons8.com/carbon-copy/100/000000/temperature-sensitive.png";
                    IconForecast1222.Source = urlForecast12;

                    string urlForecast13 = "https://img.icons8.com/color/96/000000/wind.png";
                    IconForecast1322.Source = urlForecast13;

                    string urlForecast14 = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconForecast1422.Source = urlForecast14;

                    string urlForecast15 = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconForecast1522.Source = urlForecast15;

                    double chanceOfRainForecast3 = weatherBindingData.WeatherDataForecastHourly.List[16].ChanceOfRain * 100;
                    ChanceOfRainForecast122.Text = chanceOfRainForecast3.ToString() + "%";

                    string urlFeelsLike11 = "https://img.icons8.com/color/96/000000/cool--v1.png";
                    IconForecast5Days11.Source = urlFeelsLike11;

                    string urlFeelsLike12 = "https://img.icons8.com/ultraviolet/80/000000/temperature--v1.png";
                    IconForecast5Days12.Source = urlFeelsLike12;

                    string urlFeelsLike13 = "https://img.icons8.com/color/96/000000/thermometer.png";
                    IconForecast5Days13.Source = urlFeelsLike13;

                    string urlFeelsLike14 = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconForecast5Days14.Source = urlFeelsLike14;

                    string urlFeelsLike15 = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconForecast5Days15.Source = urlFeelsLike15;

                    string urlFeelsLike36 = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconForecast5Days36.Source = urlFeelsLike36;

                    string urlFeelsLike37 = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconForecast5Days37.Source = urlFeelsLike37;

                    string urlFeelsLike38 = "https://img.icons8.com/color/96/000000/ground.png";
                    IconForecast5Days38.Source = urlFeelsLike38;

                    string urlFeelsLike39 = "https://img.icons8.com/color/96/000000/low-tide.png";
                    IconForecast5Days39.Source = urlFeelsLike39;

                    string urlFeelsLike40 = "https://img.icons8.com/color/48/000000/windsock--v1.png";
                    IconForecast5Days40.Source = urlFeelsLike40;

                    //Fourth Day Forecast
                    string descriptionForecast4 = weatherBindingData.WeatherDataForecastHourly.List[24].WeatherForecast[0].DescriptionForecast;
                    ImageDescriptionForecast4.Opacity = 0.5;
                    ImageDescriptionForecast4.VerticalOptions = LayoutOptions.FillAndExpand;
                    ImageDescriptionForecast4.HorizontalOptions = LayoutOptions.FillAndExpand;
                 

                    if (descriptionForecast4 == "clear sky")
                    {
                        ImageDescriptionForecast4.Source = "Images/ClearSky.jpg";
                        
                    }
                    else if (descriptionForecast4 == "few clouds")
                    {
                        ImageDescriptionForecast4.Source = "Images/FewClouds.jpg";
                       
                    }
                    else if (descriptionForecast4 == "scattered clouds")
                    {
                        ImageDescriptionForecast4.Source = "Images/Scattering.jpg";
                       
                    }
                    else if (descriptionForecast4 == "broken clouds")
                    {
                        ImageDescriptionForecast4.Source = "Images/BrokenClouds.jpg";
                       
                    }
                    else if (descriptionForecast4 == "light rain")
                    {
                        ImageDescriptionForecast4.Source = "Images/LightRain.jpg";
                     
                    }
                    else if (descriptionForecast4 == "rain")
                    {
                        ImageDescriptionForecast4.Source = "Images/Rain.jpg";
                     
                    }
                    else if (descriptionForecast4 == "thunderstorm")
                    {
                        ImageDescriptionForecast4.Source = "Images/Thunderstorm.jpg";
                        
                    }
                    else if (descriptionForecast4 == "snow")
                    {
                        ImageDescriptionForecast4.Source = "Images/Snow.jpg";
                     
                    }
                    else if (descriptionForecast4 == "mist")
                    {
                        ImageDescriptionForecast4.Source = "Images/Mist.jpg";
                        
                    }
                    else if (descriptionForecast4 == "overcast clouds")
                    {
                        ImageDescriptionForecast4.Source = "Images/OverCastClouds.jpg";
                       
                    }
                    else if (descriptionForecast4 == "moderate rain")
                    {
                        ImageDescriptionForecast4.Source = "Images/ModerateRain.jpg";
                        
                    }
                    else if (descriptionForecast4 == "light snow")
                    {
                        ImageDescriptionForecast4.Source = "Images/LightSnow.jpg";
                        
                    }

                    DateTime DateDays1Forecast7 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[24].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

                    string Info1Forecast7 = DateDays1Forecast7.DayOfWeek.ToString();
                    DateTimeForecast7.Text = Info1Forecast7;

                    DateTime DateDays2Forecast8 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[24].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                       System.Globalization.CultureInfo.InvariantCulture);

                    string Info2Forecast8 = DateDays2Forecast8.ToString("dd-MM-yy");
                    DateTimeForecast8.Text = Info2Forecast8;

                    string IconForecast113 = weatherBindingData.WeatherDataForecastHourly.List[24].WeatherForecast[0].IconForecast.ToString();
                    string urlIconForecast16 = "https://openweathermap.org/img/wn/" + IconForecast113 + "@4x.png";
                    IconForecast1133.Source = urlIconForecast16;

                    string urlForecast16 = "https://img.icons8.com/carbon-copy/100/000000/temperature-sensitive.png";
                    IconForecast1233.Source = urlForecast16;

                    string urlForecast17 = "https://img.icons8.com/color/96/000000/wind.png";
                    IconForecast1333.Source = urlForecast17;

                    string urlForecast18 = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconForecast1433.Source = urlForecast18;

                    string urlForecast19 = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconForecast1533.Source = urlForecast19;

                    double chanceOfRainForecast4 = weatherBindingData.WeatherDataForecastHourly.List[24].ChanceOfRain * 100;
                    ChanceOfRainForecast133.Text = chanceOfRainForecast4.ToString() + "%";

                    string urlFeelsLike16 = "https://img.icons8.com/color/96/000000/cool--v1.png";
                    IconForecast5Days16.Source = urlFeelsLike16;

                    string urlFeelsLike17 = "https://img.icons8.com/ultraviolet/80/000000/temperature--v1.png";
                    IconForecast5Days17.Source = urlFeelsLike17;

                    string urlFeelsLike18 = "https://img.icons8.com/color/96/000000/thermometer.png";
                    IconForecast5Days18.Source = urlFeelsLike18;

                    string urlFeelsLike19 = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconForecast5Days19.Source = urlFeelsLike19;

                    string urlFeelsLike20 = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconForecast5Days20.Source = urlFeelsLike20;

                    string urlFeelsLike41 = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconForecast5Days41.Source = urlFeelsLike41;

                    string urlFeelsLike42 = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconForecast5Days42.Source = urlFeelsLike42;

                    string urlFeelsLike43 = "https://img.icons8.com/color/96/000000/ground.png";
                    IconForecast5Days43.Source = urlFeelsLike43;

                    string urlFeelsLike44 = "https://img.icons8.com/color/96/000000/low-tide.png";
                    IconForecast5Days44.Source = urlFeelsLike44;

                    string urlFeelsLike45 = "https://img.icons8.com/color/48/000000/windsock--v1.png";
                    IconForecast5Days45.Source = urlFeelsLike45;

                    //Five Day Forecast
                    string descriptionForecast5 = weatherBindingData.WeatherDataForecastHourly.List[32].WeatherForecast[0].DescriptionForecast;
                    ImageDescriptionForecast5.Opacity = 0.5;
                    ImageDescriptionForecast5.VerticalOptions = LayoutOptions.FillAndExpand;
                    ImageDescriptionForecast5.HorizontalOptions = LayoutOptions.FillAndExpand;
              



                    if (descriptionForecast5 == "clear sky")
                    {
                        ImageDescriptionForecast5.Source = "Images/ClearSky.jpg";
                        
                    }
                    else if (descriptionForecast5 == "few clouds")
                    {
                        ImageDescriptionForecast5.Source = "Images/FewClouds.jpg";
                       
                    }
                    else if (descriptionForecast5 == "scattered clouds")
                    {
                        ImageDescriptionForecast5.Source = "Images/Scattering.jpg";
                       
                    }
                    else if (descriptionForecast5 == "broken clouds")
                    {
                        ImageDescriptionForecast5.Source = "Images/BrokenClouds.jpg";
                      
                    }
                    else if (descriptionForecast5 == "light rain")
                    {
                        ImageDescriptionForecast5.Source = "Images/LightRain.jpg";
                       
                    }
                    else if (descriptionForecast5 == "rain")
                    {
                        ImageDescriptionForecast5.Source = "Images/Rain.jpg";
                       
                    }
                    else if (descriptionForecast5 == "thunderstorm")
                    {
                        ImageDescriptionForecast5.Source = "Images/Thunderstorm.jpg";
                     
                    }
                    else if (descriptionForecast5 == "snow")
                    {
                        ImageDescriptionForecast5.Source = "Images/Snow.jpg";
                        
                    }
                    else if (descriptionForecast5 == "mist")
                    {
                        ImageDescriptionForecast5.Source = "Images/Mist.jpg";
                      
                    }
                    else if (descriptionForecast5 == "overcast clouds")
                    {
                        ImageDescriptionForecast5.Source = "Images/OverCastClouds.jpg";
                       
                    }
                    else if (descriptionForecast5 == "moderate rain")
                    {
                        ImageDescriptionForecast5.Source = "Images/ModerateRain.jpg";
                        
                    }
                    else if (descriptionForecast5 == "light snow")
                    {
                        ImageDescriptionForecast5.Source = "Images/LightSnow.jpg";
                      
                    }

                    DateTime DateDays1Forecast9 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[32].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

                    string Info1Forecast9 = DateDays1Forecast9.DayOfWeek.ToString();
                    DateTimeForecast9.Text = Info1Forecast9;

                    DateTime DateDays2Forecast10 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[32].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                       System.Globalization.CultureInfo.InvariantCulture);

                    string Info2Forecast10 = DateDays2Forecast10.ToString("dd-MM-yy");
                    DateTimeForecast10.Text = Info2Forecast10;

                    string IconForecast114 = weatherBindingData.WeatherDataForecastHourly.List[32].WeatherForecast[0].IconForecast.ToString();
                    string urlIconForecast20 = "https://openweathermap.org/img/wn/" + IconForecast114 + "@4x.png";
                    IconForecast1144.Source = urlIconForecast20;

                    string urlForecast20 = "https://img.icons8.com/carbon-copy/100/000000/temperature-sensitive.png";
                    IconForecast1244.Source = urlForecast20;

                    string urlForecast21 = "https://img.icons8.com/color/96/000000/wind.png";
                    IconForecast1344.Source = urlForecast21;

                    string urlForecast22 = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconForecast1444.Source = urlForecast22;

                    string urlForecast23 = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconForecast1544.Source = urlForecast23;

                    double chanceOfRainForecast5 = weatherBindingData.WeatherDataForecastHourly.List[24].ChanceOfRain * 100;
                    ChanceOfRainForecast144.Text = chanceOfRainForecast5.ToString() + "%";

                    string urlFeelsLike21 = "https://img.icons8.com/color/96/000000/cool--v1.png";
                    IconForecast5Days21.Source = urlFeelsLike21;

                    string urlFeelsLike22 = "https://img.icons8.com/ultraviolet/80/000000/temperature--v1.png";
                    IconForecast5Days22.Source = urlFeelsLike22;

                    string urlFeelsLike23 = "https://img.icons8.com/color/96/000000/thermometer.png";
                    IconForecast5Days23.Source = urlFeelsLike23;

                    string urlFeelsLike24 = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconForecast5Days24.Source = urlFeelsLike24;

                    string urlFeelsLike25 = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconForecast5Days25.Source = urlFeelsLike25;

                    string urlFeelsLike46 = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconForecast5Days46.Source = urlFeelsLike46;

                    string urlFeelsLike47 = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconForecast5Days47.Source = urlFeelsLike47;

                    string urlFeelsLike48 = "https://img.icons8.com/color/96/000000/ground.png";
                    IconForecast5Days48.Source = urlFeelsLike48;

                    string urlFeelsLike49 = "https://img.icons8.com/color/96/000000/low-tide.png";
                    IconForecast5Days49.Source = urlFeelsLike49;

                    string urlFeelsLike50 = "https://img.icons8.com/color/48/000000/windsock--v1.png";
                    IconForecast5Days50.Source = urlFeelsLike50;

                    frameFinePowders.IsVisible = true;

                    List<EntryMicrocharts> entries = new List<EntryMicrocharts>
        {
            new EntryMicrocharts ((float)TemperaturesList[0].AvgTemperature)
            {
                Color=SKColor.Parse("#fcf003"),
                Label = Info1Forecast,
                ValueLabel = Math.Round((float)TemperaturesList[0].AvgTemperature,0).ToString() + "°C",
                ValueLabelColor = SKColor.Parse("#fcf003")
            },
            new EntryMicrocharts((float)TemperaturesList[1].AvgTemperature)
            {
                Color = SKColor.Parse("#fcf003"),
                Label = Info1Forecast3,
                ValueLabel = Math.Round((float)TemperaturesList[1].AvgTemperature,0).ToString() + "°C",
                ValueLabelColor = SKColor.Parse("#fcf003")
            },
            new EntryMicrocharts((float)TemperaturesList[2].AvgTemperature)
            {
                Color =  SKColor.Parse("#fcf003"),
                Label = Info1Forecast5,
                ValueLabel = Math.Round((float)TemperaturesList[2].AvgTemperature,0).ToString() + "°C",
                ValueLabelColor = SKColor.Parse("#fcf003")
            },
            new EntryMicrocharts ((float)TemperaturesList[3].AvgTemperature)
            {
                Color = SKColor.Parse("#fcf003"),
                Label = Info1Forecast7,
                ValueLabel = Math.Round((float)TemperaturesList[3].AvgTemperature,0).ToString() + "°C",
                ValueLabelColor = SKColor.Parse("#fcf003")
            },
            new EntryMicrocharts((float)TemperaturesList[4].AvgTemperature)
            {
                Color =  SKColor.Parse("#fcf003"),
                Label = Info1Forecast9,
                ValueLabel = Math.Round((float)TemperaturesList[4].AvgTemperature,0).ToString() + "°C",
                ValueLabelColor = SKColor.Parse("#fcf003")
            },
            };
                    Chart1.Chart = new BarChart()
                    {
                        Entries = entries,
                        LabelTextSize = 45f,
                        LabelOrientation = Orientation.Horizontal,
                        ValueLabelOrientation = Orientation.Horizontal,
                        BackgroundColor = SKColors.Transparent,
                        LabelColor = SKColor.Parse("#fcf003")
                    };
                }

            }
            catch (Exception)
            {
                await DisplayAlert("Wrong Location !", "In order for this message to appear, you probably entered the wrong place.", "OK");
            }
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


        async Task DisplayHourlyForecastStartUp()
        {
            try
            {
                co2lbl.IsVisible = false;
                nolbl.IsVisible = false;
                no2lbl.IsVisible = false;
                o3lbl.IsVisible = false;
                so2lbl.IsVisible = false;
                pm2_5lbl.IsVisible = false;
                pm10lbl.IsVisible = false;
                nh3lbl.IsVisible = false;

                co2.IsVisible = false;
                no.IsVisible = false;
                no2.IsVisible = false;
                o3.IsVisible = false;
                so2.IsVisible = false;
                pm2_5.IsVisible = false;
                pm10.IsVisible = false;
                nh3.IsVisible = false;

                if (!string.IsNullOrWhiteSpace(_cityEntry.Text))
                {
                    //var location1 = await Geolocation.GetLastKnownLocationAsync();

                    WeatherBindingData weatherBindingData = await _restServiceForecast.GetWeatherDataForecast(GenerateRequestUri(Constants.OpenWeatherMapEndpoint, _cityEntry), GenerateRequestUriForecast(ConstantsForecast.OpenWeatherMapEndpoint2, _cityEntry), GenerateRequestUriForecast(ConstantsForecast.OpenWeatherMapEndpoint2, _cityEntry));
                    BindingContext = weatherBindingData;

                    string description = weatherBindingData.WeatherDataForecastHourly.List[0].WeatherForecast[0].DescriptionForecast;

                    if (description == "clear sky")
                    {
                        BackgroundImage.Source = "Images/ClearSky.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "few clouds")
                    {
                        BackgroundImage.Source = "Images/FewClouds.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "scattered clouds")
                    {
                        BackgroundImage.Source = "Images/Scattering.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "broken clouds")
                    {
                        BackgroundImage.Source = "Images/BrokenClouds.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "light rain")
                    {
                        BackgroundImage.Source = "Images/LightRain.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "rain")
                    {
                        BackgroundImage.Source = "Images/Rain.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "thunderstorm")
                    {
                        BackgroundImage.Source = "Images/Thunderstorm.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "snow")
                    {
                        BackgroundImage.Source = "Images/Snow.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "mist")
                    {
                        BackgroundImage.Source = "Images/Mist.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "overcast clouds")
                    {
                        BackgroundImage.Source = "Images/OverCastClouds.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "moderate rain")
                    {
                        BackgroundImage.Source = "Images/ModerateRain.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }
                    else if (description == "light snow")
                    {
                        BackgroundImage.Source = "Images/LightSnow.jpg";
                        BackgroundImage.Opacity = 1.0;
                    }

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
                    }

                    //set Temperature on first day from forecast
                    AvgTempForecastDay1.Text = TemperaturesList[0].AvgTemperature.ToString() + "°C";
                    MinTempForecastDay1.Text = TemperaturesList[0].MinTemperature.ToString() + "°C";
                    MaxTempForecastDay1.Text = TemperaturesList[0].MaxTemperature.ToString() + "°C";

                    //set Temperature on second day from forecast
                    AvgTempForecastDay2.Text = TemperaturesList[1].AvgTemperature.ToString() + "°C";
                    MinTempForecastDay2.Text = TemperaturesList[1].MinTemperature.ToString() + "°C";
                    MaxTempForecastDay2.Text = TemperaturesList[1].MaxTemperature.ToString() + "°C";

                    //set Temperature on third day from forecast
                    AvgTempForecastDay3.Text = TemperaturesList[2].AvgTemperature.ToString() + "°C";
                    MinTempForecastDay3.Text = TemperaturesList[2].MinTemperature.ToString() + "°C";
                    MaxTempForecastDay3.Text = TemperaturesList[2].MaxTemperature.ToString() + "°C";

                    //set Temperature on forth day from forecast
                    AvgTempForecastDay4.Text = TemperaturesList[3].AvgTemperature.ToString() + "°C";
                    MinTempForecastDay4.Text = TemperaturesList[3].MinTemperature.ToString() + "°C";
                    MaxTempForecastDay4.Text = TemperaturesList[3].MaxTemperature.ToString() + "°C";

                    //set Temperature on fiveth day from forecast
                    AvgTempForecastDay5.Text = TemperaturesList[4].AvgTemperature.ToString() + "°C";
                    MinTempForecastDay5.Text = TemperaturesList[4].MinTemperature.ToString() + "°C";
                    MaxTempForecastDay5.Text = TemperaturesList[4].MaxTemperature.ToString() + "°C";

                    //Set Icon and strings with binding weather data from API
                    string icon = weatherBindingData.WeatherDataForecastHourly.List[0].WeatherForecast[0].IconForecast.ToString();
                    string url = "https://openweathermap.org/img/wn/" + icon + "@4x.png";
                    IconCurrentTemp.Source = url;
                    HighLowTemperature.Text = "L: " + Math.Round(TemperaturesList[0].MinTemperature) + "°" + " H: " + Math.Round(TemperaturesList[0].MaxTemperature) + "°";
                    FeelsLike.Text = "Feels Like: " + Math.Round(weatherBindingData.WeatherDataCurrent.Main.FeelsLike) + "°C";
                    string urlSunrise = "https://img.icons8.com/plasticine/100/000000/sunrise.png";
                    IconSunrise.Source = urlSunrise;
                    string urlSunset = "https://img.icons8.com/plasticine/100/000000/sunset.png";
                    IconSunset.Source = urlSunset;

                    //Set 6 Icons for Clouds, Wind Speed, Humidity, Pressure, Chance of Rain and Rain Values
                    string urlClouds = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconClouds.Source = urlClouds;
                    string urlWindSpeed = "https://img.icons8.com/color/96/000000/wind.png";
                    IconWindSpeed.Source = urlWindSpeed;
                    string urlHumidity = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconHumidity.Source = urlHumidity;
                    string urlPressure = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconPressure.Source = urlPressure;
                    string urlChanceOfRain = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconChanceOfRain.Source = urlChanceOfRain;
                    string urlRainValue = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconRainValue.Source = urlRainValue;
                    string urlSnowValue = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconSnowValue.Source = urlSnowValue;

                    double chanceOfRain = weatherBindingData.WeatherDataForecastHourly.List[0].ChanceOfRain * 100;
                    ChanceOfRainLbl.Text = chanceOfRain.ToString() + "%";

                    
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
                    string urlHourly1 = "https://openweathermap.org/img/wn/" + iconStringHourly1 + "@4x.png";
                    iconHourly1.Source = urlHourly1;

                    string iconStringHourly2 = weatherBindingData.WeatherDataForecastHourly.List[1].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly2 = "https://openweathermap.org/img/wn/" + iconStringHourly2 + "@4x.png";
                    iconHourly2.Source = urlHourly2;

                    string iconStringHourly3 = weatherBindingData.WeatherDataForecastHourly.List[2].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly3 = "https://openweathermap.org/img/wn/" + iconStringHourly3 + "@4x.png";
                    iconHourly3.Source = urlHourly3;

                    string iconStringHourly4 = weatherBindingData.WeatherDataForecastHourly.List[3].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly4 = "https://openweathermap.org/img/wn/" + iconStringHourly4 + "@4x.png";
                    iconHourly4.Source = urlHourly4;

                    string iconStringHourly5 = weatherBindingData.WeatherDataForecastHourly.List[4].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly5 = "https://openweathermap.org/img/wn/" + iconStringHourly5 + "@4x.png";
                    iconHourly5.Source = urlHourly5;

                    string iconStringHourly6 = weatherBindingData.WeatherDataForecastHourly.List[5].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly6 = "https://openweathermap.org/img/wn/" + iconStringHourly6 + "@4x.png";
                    iconHourly6.Source = urlHourly6;

                    string iconStringHourly7 = weatherBindingData.WeatherDataForecastHourly.List[6].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly7 = "https://openweathermap.org/img/wn/" + iconStringHourly7 + "@4x.png";
                    iconHourly7.Source = urlHourly7;

                    string iconStringHourly8 = weatherBindingData.WeatherDataForecastHourly.List[7].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly8 = "https://openweathermap.org/img/wn/" + iconStringHourly8 + "@4x.png";
                    iconHourly8.Source = urlHourly8;

                    string iconStringHourly9 = weatherBindingData.WeatherDataForecastHourly.List[8].WeatherForecast[0].IconForecast.ToString();
                    string urlHourly9 = "https://openweathermap.org/img/wn/" + iconStringHourly9 + "@4x.png";
                    iconHourly9.Source = urlHourly9;

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

                    //First Day Forecast
                    string descriptionForecast1 = weatherBindingData.WeatherDataForecastHourly.List[0].WeatherForecast[0].DescriptionForecast;

                    ImageDescriptionForecast1.Opacity = 0.5;
                    ImageDescriptionForecast1.VerticalOptions = LayoutOptions.FillAndExpand;
                    ImageDescriptionForecast1.HorizontalOptions = LayoutOptions.FillAndExpand;
        

                    if (descriptionForecast1 == "clear sky")
                    {
                        ImageDescriptionForecast1.Source = "Images/ClearSky.jpg";
                        
                    }
                    else if (descriptionForecast1 == "few clouds")
                    {
                        ImageDescriptionForecast1.Source = "Images/FewClouds.jpg";
                       
                    }
                    else if (descriptionForecast1 == "scattered clouds")
                    {
                        ImageDescriptionForecast1.Source = "Images/Scattering.jpg";
                        
                    }
                    else if (descriptionForecast1 == "broken clouds")
                    {
                        ImageDescriptionForecast1.Source = "Images/BrokenClouds.jpg";
                       
                    }
                    else if (descriptionForecast1 == "light rain")
                    {
                        ImageDescriptionForecast1.Source = "Images/LightRain.jpg";
                       
                    }
                    else if (descriptionForecast1 == "rain")
                    {
                        ImageDescriptionForecast1.Source = "Images/Rain.jpg";
                       
                    }
                    else if (descriptionForecast1 == "thunderstorm")
                    {
                        ImageDescriptionForecast1.Source = "Images/Thunderstorm.jpg";
                        
                    }
                    else if (descriptionForecast1 == "snow")
                    {
                        ImageDescriptionForecast1.Source = "Images/Snow.jpg";
                      
                    }
                    else if (descriptionForecast1 == "mist")
                    {
                        ImageDescriptionForecast1.Source = "Images/Mist.jpg";
                       
                    }
                    else if (descriptionForecast1 == "overcast clouds")
                    {
                        ImageDescriptionForecast1.Source = "Images/OverCastClouds.jpg";
                      
                    }
                    else if (descriptionForecast1 == "moderate rain")
                    {
                        ImageDescriptionForecast1.Source = "Images/ModerateRain.jpg";
                        
                    }
                    else if (descriptionForecast1 == "light snow")
                    {
                        ImageDescriptionForecast1.Source = "Images/LightSnow.jpg";
                      
                    }

                    DateTime DateDays1Forecast = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[0].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

                    string Info1Forecast = DateDays1Forecast.DayOfWeek.ToString();
                    DateTimeForecast1.Text = Info1Forecast;

                    DateTime DateDays2Forecast = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[0].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                       System.Globalization.CultureInfo.InvariantCulture);

                    string Info2Forecast = DateDays2Forecast.ToString("dd-MM-yy");
                    DateTimeForecast2.Text = Info2Forecast;

                    string IconForecast1 = weatherBindingData.WeatherDataForecastHourly.List[0].WeatherForecast[0].IconForecast.ToString();
                    string urlIconForecast1 = "https://openweathermap.org/img/wn/" + IconForecast1 + "@4x.png";
                    IconForecast11.Source = urlIconForecast1;

                    string urlForecast2 = "https://img.icons8.com/carbon-copy/100/000000/temperature-sensitive.png";
                    IconForecast12.Source = urlForecast2;

                    string urlForecast3 = "https://img.icons8.com/color/96/000000/wind.png";
                    IconForecast13.Source = urlForecast3;

                    string urlForecast4 = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconForecast14.Source = urlForecast4;

                    string urlForecast5 = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconForecast15.Source = urlForecast5;

                    double chanceOfRainForecast = weatherBindingData.WeatherDataForecastHourly.List[0].ChanceOfRain * 100;
                    ChanceOfRainForecast1.Text = chanceOfRainForecast.ToString() + "%";

                    string urlFeelsLike1 = "https://img.icons8.com/color/96/000000/cool--v1.png";
                    IconForecast5Days1.Source = urlFeelsLike1;

                    string urlFeelsLike2 = "https://img.icons8.com/ultraviolet/80/000000/temperature--v1.png";
                    IconForecast5Days2.Source = urlFeelsLike2;

                    string urlFeelsLike3 = "https://img.icons8.com/color/96/000000/thermometer.png";
                    IconForecast5Days3.Source = urlFeelsLike3;

                    string urlFeelsLike4 = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconForecast5Days4.Source = urlFeelsLike4;

                    string urlFeelsLike5 = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconForecast5Days5.Source = urlFeelsLike5;

                    string urlFeelsLike26 = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconForecast5Days26.Source = urlFeelsLike26;

                    string urlFeelsLike27 = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconForecast5Days27.Source = urlFeelsLike27;

                    string urlFeelsLike28 = "https://img.icons8.com/color/96/000000/ground.png";
                    IconForecast5Days28.Source = urlFeelsLike28;

                    string urlFeelsLike29 = "https://img.icons8.com/color/96/000000/low-tide.png";
                    IconForecast5Days29.Source = urlFeelsLike29;

                    string urlFeelsLike30 = "https://img.icons8.com/color/48/000000/windsock--v1.png";
                    IconForecast5Days30.Source = urlFeelsLike30;

                    //Second Day Forecast

                    string descriptionForecast2 = weatherBindingData.WeatherDataForecastHourly.List[8].WeatherForecast[0].DescriptionForecast;
                    ImageDescriptionForecast2.Opacity = 0.5;
                    ImageDescriptionForecast2.VerticalOptions = LayoutOptions.FillAndExpand;
                    ImageDescriptionForecast2.HorizontalOptions = LayoutOptions.FillAndExpand;
                   


                    if (descriptionForecast2 == "clear sky")
                    {
                        ImageDescriptionForecast2.Source = "Images/ClearSky.jpg";
                      
                    }
                    else if (descriptionForecast2 == "few clouds")
                    {
                        ImageDescriptionForecast2.Source = "Images/FewClouds.jpg";
                        
                    }
                    else if (descriptionForecast2 == "scattered clouds")
                    {
                        ImageDescriptionForecast2.Source = "Images/Scattering.jpg";
                        
                    }
                    else if (descriptionForecast2 == "broken clouds")
                    {
                        ImageDescriptionForecast2.Source = "Images/BrokenClouds.jpg";
                       
                    }
                    else if (descriptionForecast2 == "light rain")
                    {
                        ImageDescriptionForecast2.Source = "Images/LightRain.jpg";
                     
                    }
                    else if (descriptionForecast2 == "rain")
                    {
                        ImageDescriptionForecast2.Source = "Images/Rain.jpg";
                        
                    }
                    else if (descriptionForecast2 == "thunderstorm")
                    {
                        ImageDescriptionForecast2.Source = "Images/Thunderstorm.jpg";
                       
                    }
                    else if (descriptionForecast2 == "snow")
                    {
                        ImageDescriptionForecast2.Source = "Images/Snow.jpg";
                       
                    }
                    else if (descriptionForecast2 == "mist")
                    {
                        ImageDescriptionForecast2.Source = "Images/Mist.jpg";
                       
                    }
                    else if (descriptionForecast2 == "overcast clouds")
                    {
                        ImageDescriptionForecast2.Source = "Images/OverCastClouds.jpg";
                       
                    }
                    else if (descriptionForecast2 == "moderate rain")
                    {
                        ImageDescriptionForecast2.Source = "Images/ModerateRain.jpg";
                        
                    }
                    else if (descriptionForecast2 == "light snow")
                    {
                        ImageDescriptionForecast2.Source = "Images/LightSnow.jpg";
                        
                    }

                    DateTime DateDays1Forecast3 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[8].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

                    string Info1Forecast3 = DateDays1Forecast3.DayOfWeek.ToString();
                    DateTimeForecast3.Text = Info1Forecast3;

                    DateTime DateDays2Forecast4 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[8].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                       System.Globalization.CultureInfo.InvariantCulture);

                    string Info2Forecast4 = DateDays2Forecast4.ToString("dd-MM-yy");
                    DateTimeForecast4.Text = Info2Forecast4;

                    string IconForecast6 = weatherBindingData.WeatherDataForecastHourly.List[8].WeatherForecast[0].IconForecast.ToString();
                    string urlIconForecast6 = "https://openweathermap.org/img/wn/" + IconForecast6 + "@4x.png";
                    IconForecast1111.Source = urlIconForecast6;

                    string urlForecast7 = "https://img.icons8.com/carbon-copy/100/000000/temperature-sensitive.png";
                    IconForecast1211.Source = urlForecast7;

                    string urlForecast8 = "https://img.icons8.com/color/96/000000/wind.png";
                    IconForecast1311.Source = urlForecast8;

                    string urlForecast9 = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconForecast1411.Source = urlForecast9;

                    string urlForecast10 = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconForecast1511.Source = urlForecast10;

                    double chanceOfRainForecast2 = weatherBindingData.WeatherDataForecastHourly.List[8].ChanceOfRain * 100;
                    ChanceOfRainForecast111.Text = chanceOfRainForecast2.ToString() + "%";

                    string urlFeelsLike6 = "https://img.icons8.com/color/96/000000/cool--v1.png";
                    IconForecast5Days6.Source = urlFeelsLike6;

                    string urlFeelsLike7 = "https://img.icons8.com/ultraviolet/80/000000/temperature--v1.png";
                    IconForecast5Days7.Source = urlFeelsLike7;

                    string urlFeelsLike8 = "https://img.icons8.com/color/96/000000/thermometer.png";
                    IconForecast5Days8.Source = urlFeelsLike8;

                    string urlFeelsLike9 = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconForecast5Days9.Source = urlFeelsLike9;

                    string urlFeelsLike10 = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconForecast5Days10.Source = urlFeelsLike10;

                    string urlFeelsLike31 = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconForecast5Days31.Source = urlFeelsLike31;

                    string urlFeelsLike32 = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconForecast5Days32.Source = urlFeelsLike32;

                    string urlFeelsLike33 = "https://img.icons8.com/color/96/000000/ground.png";
                    IconForecast5Days33.Source = urlFeelsLike33;

                    string urlFeelsLike34 = "https://img.icons8.com/color/96/000000/low-tide.png";
                    IconForecast5Days34.Source = urlFeelsLike34;

                    string urlFeelsLike35 = "https://img.icons8.com/color/48/000000/windsock--v1.png";
                    IconForecast5Days35.Source = urlFeelsLike35;


                    //Third Day Forecast

                    string descriptionForecast3 = weatherBindingData.WeatherDataForecastHourly.List[16].WeatherForecast[0].DescriptionForecast;
                    ImageDescriptionForecast3.Opacity = 0.5;
                    ImageDescriptionForecast3.VerticalOptions = LayoutOptions.FillAndExpand;
                    ImageDescriptionForecast3.HorizontalOptions = LayoutOptions.FillAndExpand;
              


                    if (descriptionForecast3 == "clear sky")
                    {
                        ImageDescriptionForecast3.Source = "Images/ClearSky.jpg";
                       
                    }
                    else if (descriptionForecast3 == "few clouds")
                    {
                        ImageDescriptionForecast3.Source = "Images/FewClouds.jpg";
                        
                    }
                    else if (descriptionForecast3 == "scattered clouds")
                    {
                        ImageDescriptionForecast3.Source = "Images/Scattering.jpg";
                      
                    }
                    else if (descriptionForecast3 == "broken clouds")
                    {
                        ImageDescriptionForecast3.Source = "Images/BrokenClouds.jpg";
                     
                    }
                    else if (descriptionForecast3 == "light rain")
                    {
                        ImageDescriptionForecast3.Source = "Images/LightRain.jpg";
                        
                    }
                    else if (descriptionForecast3 == "rain")
                    {
                        ImageDescriptionForecast3.Source = "Images/Rain.jpg";
                       
                    }
                    else if (descriptionForecast3 == "thunderstorm")
                    {
                        ImageDescriptionForecast3.Source = "Images/Thunderstorm.jpg";
                      
                    }
                    else if (descriptionForecast3 == "snow")
                    {
                        ImageDescriptionForecast3.Source = "Images/Snow.jpg";
                       
                    }
                    else if (descriptionForecast3 == "mist")
                    {
                        ImageDescriptionForecast3.Source = "Images/Mist.jpg";
                       
                    }
                    else if (descriptionForecast3 == "overcast clouds")
                    {
                        ImageDescriptionForecast3.Source = "Images/OverCastClouds.jpg";
                       
                    }
                    else if (descriptionForecast3 == "moderate rain")
                    {
                        ImageDescriptionForecast3.Source = "Images/ModerateRain.jpg";
                       
                    }
                    else if (descriptionForecast3 == "light snow")
                    {
                        ImageDescriptionForecast3.Source = "Images/LightSnow.jpg";
                      
                    }

                    DateTime DateDays1Forecast5 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[16].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

                    string Info1Forecast5 = DateDays1Forecast5.DayOfWeek.ToString();
                    DateTimeForecast5.Text = Info1Forecast5;

                    DateTime DateDays2Forecast6 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[16].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                       System.Globalization.CultureInfo.InvariantCulture);

                    string Info2Forecast6 = DateDays2Forecast6.ToString("dd-MM-yy");
                    DateTimeForecast6.Text = Info2Forecast6;

                    string IconForecast112 = weatherBindingData.WeatherDataForecastHourly.List[16].WeatherForecast[0].IconForecast.ToString();
                    string urlIconForecast11 = "https://openweathermap.org/img/wn/" + IconForecast112 + "@4x.png";
                    IconForecast1122.Source = urlIconForecast11;

                    string urlForecast12 = "https://img.icons8.com/carbon-copy/100/000000/temperature-sensitive.png";
                    IconForecast1222.Source = urlForecast12;

                    string urlForecast13 = "https://img.icons8.com/color/96/000000/wind.png";
                    IconForecast1322.Source = urlForecast13;

                    string urlForecast14 = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconForecast1422.Source = urlForecast14;

                    string urlForecast15 = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconForecast1522.Source = urlForecast15;

                    double chanceOfRainForecast3 = weatherBindingData.WeatherDataForecastHourly.List[16].ChanceOfRain * 100;
                    ChanceOfRainForecast122.Text = chanceOfRainForecast3.ToString() + "%";

                    string urlFeelsLike11 = "https://img.icons8.com/color/96/000000/cool--v1.png";
                    IconForecast5Days11.Source = urlFeelsLike11;

                    string urlFeelsLike12 = "https://img.icons8.com/ultraviolet/80/000000/temperature--v1.png";
                    IconForecast5Days12.Source = urlFeelsLike12;

                    string urlFeelsLike13 = "https://img.icons8.com/color/96/000000/thermometer.png";
                    IconForecast5Days13.Source = urlFeelsLike13;

                    string urlFeelsLike14 = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconForecast5Days14.Source = urlFeelsLike14;

                    string urlFeelsLike15 = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconForecast5Days15.Source = urlFeelsLike15;

                    string urlFeelsLike36 = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconForecast5Days36.Source = urlFeelsLike36;

                    string urlFeelsLike37 = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconForecast5Days37.Source = urlFeelsLike37;

                    string urlFeelsLike38 = "https://img.icons8.com/color/96/000000/ground.png";
                    IconForecast5Days38.Source = urlFeelsLike38;

                    string urlFeelsLike39 = "https://img.icons8.com/color/96/000000/low-tide.png";
                    IconForecast5Days39.Source = urlFeelsLike39;

                    string urlFeelsLike40 = "https://img.icons8.com/color/48/000000/windsock--v1.png";
                    IconForecast5Days40.Source = urlFeelsLike40;

                    //Fourth Day Forecast
                    string descriptionForecast4 = weatherBindingData.WeatherDataForecastHourly.List[24].WeatherForecast[0].DescriptionForecast;
                    ImageDescriptionForecast4.Opacity = 0.5;
                    ImageDescriptionForecast4.VerticalOptions = LayoutOptions.FillAndExpand;
                    ImageDescriptionForecast4.HorizontalOptions = LayoutOptions.FillAndExpand;
                   

                    if (descriptionForecast4 == "clear sky")
                    {
                        ImageDescriptionForecast4.Source = "Images/ClearSky.jpg";
                       
                    }
                    else if (descriptionForecast4 == "few clouds")
                    {
                        ImageDescriptionForecast4.Source = "Images/FewClouds.jpg";
                       
                    }
                    else if (descriptionForecast4 == "scattered clouds")
                    {
                        ImageDescriptionForecast4.Source = "Images/Scattering.jpg";
                      
                    }
                    else if (descriptionForecast4 == "broken clouds")
                    {
                        ImageDescriptionForecast4.Source = "Images/BrokenClouds.jpg";
                        
                    }
                    else if (descriptionForecast4 == "light rain")
                    {
                        ImageDescriptionForecast4.Source = "Images/LightRain.jpg";
                        
                    }
                    else if (descriptionForecast4 == "rain")
                    {
                        ImageDescriptionForecast4.Source = "Images/Rain.jpg";
                       
                    }
                    else if (descriptionForecast4 == "thunderstorm")
                    {
                        ImageDescriptionForecast4.Source = "Images/Thunderstorm.jpg";
                       
                    }
                    else if (descriptionForecast4 == "snow")
                    {
                        ImageDescriptionForecast4.Source = "Images/Snow.jpg";
                       
                    }
                    else if (descriptionForecast4 == "mist")
                    {
                        ImageDescriptionForecast4.Source = "Images/Mist.jpg";
                      
                    }
                    else if (descriptionForecast4 == "overcast clouds")
                    {
                        ImageDescriptionForecast4.Source = "Images/OverCastClouds.jpg";
                       
                    }
                    else if (descriptionForecast4 == "moderate rain")
                    {
                        ImageDescriptionForecast4.Source = "Images/ModerateRain.jpg";
                       
                    }
                    else if (descriptionForecast4 == "light snow")
                    {
                        ImageDescriptionForecast4.Source = "Images/LightSnow.jpg";
                        
                    }

                    DateTime DateDays1Forecast7 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[24].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

                    string Info1Forecast7 = DateDays1Forecast7.DayOfWeek.ToString();
                    DateTimeForecast7.Text = Info1Forecast7;

                    DateTime DateDays2Forecast8 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[24].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                       System.Globalization.CultureInfo.InvariantCulture);

                    string Info2Forecast8 = DateDays2Forecast8.ToString("dd-MM-yy");
                    DateTimeForecast8.Text = Info2Forecast8;

                    string IconForecast113 = weatherBindingData.WeatherDataForecastHourly.List[24].WeatherForecast[0].IconForecast.ToString();
                    string urlIconForecast16 = "https://openweathermap.org/img/wn/" + IconForecast113 + "@4x.png";
                    IconForecast1133.Source = urlIconForecast16;

                    string urlForecast16 = "https://img.icons8.com/carbon-copy/100/000000/temperature-sensitive.png";
                    IconForecast1233.Source = urlForecast16;

                    string urlForecast17 = "https://img.icons8.com/color/96/000000/wind.png";
                    IconForecast1333.Source = urlForecast17;

                    string urlForecast18 = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconForecast1433.Source = urlForecast18;

                    string urlForecast19 = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconForecast1533.Source = urlForecast19;

                    double chanceOfRainForecast4 = weatherBindingData.WeatherDataForecastHourly.List[24].ChanceOfRain * 100;
                    ChanceOfRainForecast133.Text = chanceOfRainForecast4.ToString() + "%";

                    string urlFeelsLike16 = "https://img.icons8.com/color/96/000000/cool--v1.png";
                    IconForecast5Days16.Source = urlFeelsLike16;

                    string urlFeelsLike17 = "https://img.icons8.com/ultraviolet/80/000000/temperature--v1.png";
                    IconForecast5Days17.Source = urlFeelsLike17;

                    string urlFeelsLike18 = "https://img.icons8.com/color/96/000000/thermometer.png";
                    IconForecast5Days18.Source = urlFeelsLike18;

                    string urlFeelsLike19 = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconForecast5Days19.Source = urlFeelsLike19;

                    string urlFeelsLike20 = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconForecast5Days20.Source = urlFeelsLike20;

                    string urlFeelsLike41 = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconForecast5Days41.Source = urlFeelsLike41;

                    string urlFeelsLike42 = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconForecast5Days42.Source = urlFeelsLike42;

                    string urlFeelsLike43 = "https://img.icons8.com/color/96/000000/ground.png";
                    IconForecast5Days43.Source = urlFeelsLike43;

                    string urlFeelsLike44 = "https://img.icons8.com/color/96/000000/low-tide.png";
                    IconForecast5Days44.Source = urlFeelsLike44;

                    string urlFeelsLike45 = "https://img.icons8.com/color/48/000000/windsock--v1.png";
                    IconForecast5Days45.Source = urlFeelsLike45;

                    //Five Day Forecast
                    string descriptionForecast5 = weatherBindingData.WeatherDataForecastHourly.List[32].WeatherForecast[0].DescriptionForecast;
                    ImageDescriptionForecast5.Opacity = 0.5;
                    ImageDescriptionForecast5.VerticalOptions = LayoutOptions.FillAndExpand;
                    ImageDescriptionForecast5.HorizontalOptions = LayoutOptions.FillAndExpand;
           


                    if (descriptionForecast5 == "clear sky")
                    {
                        ImageDescriptionForecast5.Source = "Images/ClearSky.jpg";
                    
                    }
                    else if (descriptionForecast5 == "few clouds")
                    {
                        ImageDescriptionForecast5.Source = "Images/FewClouds.jpg";
                       
                    }
                    else if (descriptionForecast5 == "scattered clouds")
                    {
                        ImageDescriptionForecast5.Source = "Images/Scattering.jpg";
                       
                    }
                    else if (descriptionForecast5 == "broken clouds")
                    {
                        ImageDescriptionForecast5.Source = "Images/BrokenClouds.jpg";
                       
                    }
                    else if (descriptionForecast5 == "light rain")
                    {
                        ImageDescriptionForecast5.Source = "Images/LightRain.jpg";
                      
                    }
                    else if (descriptionForecast5 == "rain")
                    {
                        ImageDescriptionForecast5.Source = "Images/Rain.jpg";
                      
                    }
                    else if (descriptionForecast5 == "thunderstorm")
                    {
                        ImageDescriptionForecast5.Source = "Images/Thunderstorm.jpg";
                       
                    }
                    else if (descriptionForecast5 == "snow")
                    {
                        ImageDescriptionForecast5.Source = "Images/Snow.jpg";
                       
                    }
                    else if (descriptionForecast5 == "mist")
                    {
                        ImageDescriptionForecast5.Source = "Images/Mist.jpg";
                       
                    }
                    else if (descriptionForecast5 == "overcast clouds")
                    {
                        ImageDescriptionForecast5.Source = "Images/OverCastClouds.jpg";
                       
                    }
                    else if (descriptionForecast5 == "moderate rain")
                    {
                        ImageDescriptionForecast5.Source = "Images/ModerateRain.jpg";
                       
                    }
                    else if (descriptionForecast5 == "light snow")
                    {
                        ImageDescriptionForecast5.Source = "Images/LightSnow.jpg";
                       
                    }

                    DateTime DateDays1Forecast9 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[32].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

                    string Info1Forecast9 = DateDays1Forecast9.DayOfWeek.ToString();
                    DateTimeForecast9.Text = Info1Forecast9;

                    DateTime DateDays2Forecast10 = DateTime.ParseExact(weatherBindingData.WeatherDataForecastHourly.List[32].DateTimeFull, "yyyy-MM-dd HH:mm:ss",
                                                       System.Globalization.CultureInfo.InvariantCulture);

                    string Info2Forecast10 = DateDays2Forecast10.ToString("dd-MM-yy");
                    DateTimeForecast10.Text = Info2Forecast10;

                    string IconForecast114 = weatherBindingData.WeatherDataForecastHourly.List[32].WeatherForecast[0].IconForecast.ToString();
                    string urlIconForecast20 = "https://openweathermap.org/img/wn/" + IconForecast114 + "@4x.png";
                    IconForecast1144.Source = urlIconForecast20;

                    string urlForecast20 = "https://img.icons8.com/carbon-copy/100/000000/temperature-sensitive.png";
                    IconForecast1244.Source = urlForecast20;

                    string urlForecast21 = "https://img.icons8.com/color/96/000000/wind.png";
                    IconForecast1344.Source = urlForecast21;

                    string urlForecast22 = "https://img.icons8.com/color/96/000000/skydrive.png";
                    IconForecast1444.Source = urlForecast22;

                    string urlForecast23 = "https://img.icons8.com/color/96/000000/rain--v1.png";
                    IconForecast1544.Source = urlForecast23;

                    double chanceOfRainForecast5 = weatherBindingData.WeatherDataForecastHourly.List[24].ChanceOfRain * 100;
                    ChanceOfRainForecast144.Text = chanceOfRainForecast5.ToString() + "%";

                    string urlFeelsLike21 = "https://img.icons8.com/color/96/000000/cool--v1.png";
                    IconForecast5Days21.Source = urlFeelsLike21;

                    string urlFeelsLike22 = "https://img.icons8.com/ultraviolet/80/000000/temperature--v1.png";
                    IconForecast5Days22.Source = urlFeelsLike22;

                    string urlFeelsLike23 = "https://img.icons8.com/color/96/000000/thermometer.png";
                    IconForecast5Days23.Source = urlFeelsLike23;

                    string urlFeelsLike24 = "https://img.icons8.com/color/96/000000/rainwater-catchment.png";
                    IconForecast5Days24.Source = urlFeelsLike24;

                    string urlFeelsLike25 = "https://img.icons8.com/color/96/000000/snowflake.png";
                    IconForecast5Days25.Source = urlFeelsLike25;

                    string urlFeelsLike46 = "https://img.icons8.com/color/96/000000/humidity.png";
                    IconForecast5Days46.Source = urlFeelsLike46;

                    string urlFeelsLike47 = "https://img.icons8.com/color/96/000000/pressure.png";
                    IconForecast5Days47.Source = urlFeelsLike47;

                    string urlFeelsLike48 = "https://img.icons8.com/color/96/000000/ground.png";
                    IconForecast5Days48.Source = urlFeelsLike48;

                    string urlFeelsLike49 = "https://img.icons8.com/color/96/000000/low-tide.png";
                    IconForecast5Days49.Source = urlFeelsLike49;

                    string urlFeelsLike50 = "https://img.icons8.com/color/48/000000/windsock--v1.png";
                    IconForecast5Days50.Source = urlFeelsLike50;

                    frameFinePowders.IsVisible = false;

                    List<EntryMicrocharts> entries = new List<EntryMicrocharts>
        {
            new EntryMicrocharts ((float)TemperaturesList[0].AvgTemperature)
            {
                Color=SKColor.Parse("#fcf003"),
                Label = Info1Forecast,
                ValueLabel = Math.Round((float)TemperaturesList[0].AvgTemperature,0).ToString() + "°C",
                ValueLabelColor = SKColor.Parse("#fcf003")
            },
            new EntryMicrocharts((float)TemperaturesList[1].AvgTemperature)
            {
                Color = SKColor.Parse("#fcf003"),
                Label = Info1Forecast3,
                ValueLabel = Math.Round((float)TemperaturesList[1].AvgTemperature,0).ToString() + "°C",
                ValueLabelColor = SKColor.Parse("#fcf003")
            },
            new EntryMicrocharts((float)TemperaturesList[2].AvgTemperature)
            {
                Color =  SKColor.Parse("#fcf003"),
                Label = Info1Forecast5,
                ValueLabel = Math.Round((float)TemperaturesList[2].AvgTemperature,0).ToString() + "°C",
                ValueLabelColor = SKColor.Parse("#fcf003")
            },
            new EntryMicrocharts ((float)TemperaturesList[3].AvgTemperature)
            {
                Color = SKColor.Parse("#fcf003"),
                Label = Info1Forecast7,
                ValueLabel = Math.Round((float)TemperaturesList[3].AvgTemperature,0).ToString() + "°C",
                ValueLabelColor = SKColor.Parse("#fcf003")
            },
            new EntryMicrocharts((float)TemperaturesList[4].AvgTemperature)
            {
                Color =  SKColor.Parse("#fcf003"),
                Label = Info1Forecast9,
                ValueLabel = Math.Round((float)TemperaturesList[4].AvgTemperature,0).ToString() + "°C",
                ValueLabelColor = SKColor.Parse("#fcf003")
            },
            };
                    Chart1.Chart = new BarChart()
                    {
                        Entries = entries,
                        LabelTextSize = 45f,
                        LabelOrientation = Orientation.Horizontal,
                        ValueLabelOrientation = Orientation.Horizontal,
                        BackgroundColor = SKColors.Transparent,
                        LabelColor = SKColor.Parse("#fcf003")
                    };
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Wrong Location !", "In order for this message to appear, you probably entered the wrong place.", "OK");
            }
        }


        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            await DisplayHourlyForecastStartUp();
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
                    var location = await Geolocation.GetLocationAsync();

                    if (location != null)
                    {
                        WeatherBindingData weatherBindingData = await _restServiceForecast.GetWeatherDataForecast(GenerateRequestUriGPS(Constants.OpenWeatherMapEndpoint, location), GenerateRequestUriForecast(ConstantsForecast.OpenWeatherMapEndpoint2, _cityEntry), GenerateRequestUriAir(ConstantsAir.OpenWeatherMapEndpoint3, location));
                        //WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestUriGPS(Constants.OpenWeatherMapEndpoint, location));
                        var CityName = weatherBindingData.WeatherDataCurrent.Title.ToString();
                        //var CityName = weatherData.Title.ToString();
                        _cityEntry.Text = CityName;

                        co2.Text = weatherBindingData.WeatherDataAirForecast.List2[0].ComponentsAirForecasts.coAirForecast.ToString();
                        no.Text = weatherBindingData.WeatherDataAirForecast.List2[0].ComponentsAirForecasts.noAirForecast.ToString();
                        no2.Text = weatherBindingData.WeatherDataAirForecast.List2[0].ComponentsAirForecasts.no2AirForecast.ToString();
                        o3.Text = weatherBindingData.WeatherDataAirForecast.List2[0].ComponentsAirForecasts.o3AirForecast.ToString();
                        so2.Text = weatherBindingData.WeatherDataAirForecast.List2[0].ComponentsAirForecasts.so2AirForecast.ToString();
                        pm2_5.Text = weatherBindingData.WeatherDataAirForecast.List2[0].ComponentsAirForecasts.pm2_5AirForecast.ToString();
                        pm10.Text = weatherBindingData.WeatherDataAirForecast.List2[0].ComponentsAirForecasts.pm10AirForecast.ToString();
                        nh3.Text = weatherBindingData.WeatherDataAirForecast.List2[0].ComponentsAirForecasts.nh3AirForecast.ToString();

                        co2lbl.IsVisible = true;
                        nolbl.IsVisible = true;
                        no2lbl.IsVisible = true;
                        o3lbl.IsVisible = true;
                        so2lbl.IsVisible = true;
                        pm2_5lbl.IsVisible = true;
                        pm10lbl.IsVisible = true;
                        nh3lbl.IsVisible = true;

                        co2.IsVisible = true;
                        no.IsVisible = true;
                        no2.IsVisible = true;
                        o3.IsVisible = true;
                        so2.IsVisible = true;
                        pm2_5.IsVisible = true;
                        pm10.IsVisible = true;
                        nh3.IsVisible = true;

                        await DisplayHourlyForecast();
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

            string GenerateRequestUriGPS(string endpoint, Xamarin.Essentials.Location location)
            {
                string requestUri = endpoint;
                requestUri += $"?lat={location.Latitude}";
                requestUri += $"&lon={location.Longitude}";
                requestUri += "&units=metric"; // or units=metric
                requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
                return requestUri;
            }
        }

        async void OnGetGPS(System.Object sender, System.EventArgs e)
            {
                await ButtonClickedGPS();

                await DisplayHourlyForecast();
            }
        }
    }
