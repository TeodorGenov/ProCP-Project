using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    class WeatherConditions
    {
        public double Humidity
        { get; set; }
        public double WindSpeed
        { get; set; }
        public WindDirection WindDirection
        { get; set; }
        public double Visibility
        { get; set; }
        public double TemperatureC
        { get; set; }
        public RainType RainType
        { get; set; }
        public double RainIntensity
        { get; set; }

        public WeatherConditions(double windSpeed, double visibility, double temperatureC, double rainIntensity)
        {
            this.WindSpeed = windSpeed;
            this.TemperatureC = temperatureC;
            this.RainIntensity = rainIntensity;
            this.Visibility = visibility;
        }

        public void ChangeWeather()
        {
            if (TemperatureC > 0 && RainIntensity > 20)
            {
                RainType = RainType.RAIN;
            }
            else if (TemperatureC <= 0 && RainIntensity <= 40)
            {
                RainType = RainType.SNOWFALL;
            }
            else if (TemperatureC <= 0 && RainIntensity > 40)
            {
                RainType = RainType.HALE;
            }
            else
            {
                RainType = RainType.CLEAR;
            }
        }
    }
}
