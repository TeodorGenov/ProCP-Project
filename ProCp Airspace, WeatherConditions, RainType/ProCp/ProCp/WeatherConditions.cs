using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCp
{
    public class WeatherConditions
    {


        public double Humidity
        { get; set; }

        public double WindSpeed
        { get; set; }

        public double WindDIrection
        { get; set; }

        public double Visibility
        { get; set; }

        public double TemperatureC
        { get; set; }

        public RainType RainType
        { get; set; }

        public double RainIntensity
        { get; set; }


        public WeatherConditions(double humidity, double windSpeed, double windDirection, double visibility, double temperatureC, RainType rainType, double rainIntensity)
        {
            Humidity = humidity;
            WindSpeed = windSpeed;
            WindDIrection = windDirection;
            Visibility = visibility;
            TemperatureC = temperatureC;
            RainType = rainType;
            RainIntensity = rainIntensity;
        }

    }
} 
