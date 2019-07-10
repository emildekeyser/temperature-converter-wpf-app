using System;

namespace Model
{
    public interface ITemperatureScale
    {
        string Name { get; }

        double ConvertToKelvin(double temperature);
        double ConvertFromKelvin(double temperature);
    }

    public class KelvinTemperatureScale : ITemperatureScale
    {
        public string Name { get { return "Kelvin"; } }
        public double ConvertFromKelvin(double temperature) { return Math.Round(temperature, 3); }
        public double ConvertToKelvin(double temperature) { return Math.Round(temperature, 3); }
    }

    public class CelsiusTemperatureScale : ITemperatureScale
    {
        public string Name { get { return "Celsius"; } }

        public double ConvertFromKelvin(double temperature)
        {
            double kelvin = temperature;
            double celsius = kelvin - 273.15;
            return Math.Round(celsius, 3);
        }

        public double ConvertToKelvin(double temperature)
        {
            double celsius = temperature;
            double kelvin = celsius + 273.15;
            return Math.Round(kelvin, 3);
        }
    }

    public class FarenheitTemperatureScale : ITemperatureScale
    {
        public string Name { get { return "Farenheit"; } }
        public double ConvertFromKelvin(double temperature)
        {
            var kelvin = temperature;
            var farenheit = kelvin * 9 / 5 - 459.67;
            return Math.Round(farenheit, 3);
        }

        public double ConvertToKelvin(double temperature)
        {
            var farenheit = temperature;
            var kelvin = (farenheit + 459.67) * 5 / 9;
            return Math.Round(kelvin, 3);
        }
    }
}
