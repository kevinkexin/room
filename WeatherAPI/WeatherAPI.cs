using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UNERoomPlus
{
    public class WeatherAPI
    {
        private string APIKey { get; set; }

        public WeatherAPI(string APIKey)
        {
            this.APIKey = APIKey;
        }

        public WeatherDate GetCurrenWeatherData(int CityTag, string Language = "en")
        {
            WeatherDate wdToday = new WeatherDate();
            try
            {
                WeatherNet.ClientSettings.ApiKey = APIKey;

                WeatherNet.Model.SingleResult<WeatherNet.Model.CurrentWeatherResult> weather = WeatherNet.Clients.CurrentWeather.GetByCityId(CityTag, Language, "metric");

                if (weather.Success)
                {
                    wdToday.CityName = weather.Item.City;
                    wdToday.Code = weather.Item.Icon;
                    wdToday.MaxTemp = Convert.ToInt32(weather.Item.Temp);
                    wdToday.MinTemp = Convert.ToInt32(weather.Item.Temp);
                    wdToday.Condition = weather.Item.Description;
                    wdToday.Tag = CityTag.ToString();
                    wdToday.WeatherDay = DateTime.Now.Date;
                    wdToday.DayIndex = 0; //Today
                    wdToday.Icon = weather.Item.Icon;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError(ex);
            }

            return wdToday;
        }

        public List<WeatherDate> GetForcastWeatherData(int CityTag, string Language = "en")
        {
            List<WeatherDate> ltWeatherDate = new List<WeatherDate>();

            try
            {
                WeatherNet.ClientSettings.ApiKey = APIKey;

                WeatherNet.Model.Result<WeatherNet.Model.FiveDaysForecastResult> weather = WeatherNet.Clients.FiveDaysForecast.GetByCityId(CityTag, Language, "metric");

                //Get the Individual Date
                int DayIndex = 1;
                if (weather.Success)
                {
                    for (int i = 0; i < weather.Items.Count; i++)
                    {
                        var v = from p in ltWeatherDate
                                where p.WeatherDay.DayOfYear == weather.Items[i].Date.DayOfYear
                                && p.WeatherDay.DayOfYear != DateTime.Now.DayOfYear //Remove the Current Day
                                select p;

                        if (!v.Any())
                        {
                            WeatherDate wDate = new WeatherDate();
                            wDate.WeatherDay = weather.Items[i].Date;
                            wDate.CityName = weather.Items[i].Icon;
                            wDate.Code = weather.Items[i].Icon;
                            wDate.MaxTemp = Convert.ToInt32(weather.Items[i].TempMax);
                            wDate.MinTemp = Convert.ToInt32(weather.Items[i].TempMin);
                            wDate.Tag = CityTag.ToString();
                            wDate.Condition = weather.Items[i].Description;
                            wDate.WeatherDay = weather.Items[i].Date;
                            wDate.DayIndex = DayIndex; //
                            wDate.Icon = weather.Items[i].Icon;

                            ltWeatherDate.Add(wDate);
                            DayIndex++;
                        }
                        else
                        {
                            if (v.ToList()[0].MaxTemp < Convert.ToInt32(weather.Items[i].TempMax))
                                v.ToList()[0].MaxTemp = Convert.ToInt32(weather.Items[i].TempMax);

                            if (v.ToList()[0].MinTemp > Convert.ToInt32(weather.Items[i].TempMin))
                                v.ToList()[0].MinTemp = Convert.ToInt32(weather.Items[i].TempMin);
                        }//end of else
                    }//end of for
                } //end of check success
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError(ex);
            }

            return ltWeatherDate;
        }
    }
}
