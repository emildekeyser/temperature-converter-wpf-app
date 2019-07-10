using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Cells;
using Model;

namespace ViewModel
{
    public class ConverterViewModel
    {
        public ConverterViewModel()
        {
            this.TemperatureInKelvin = new Cell<double>();
            this.Kelvin = new TemperatureScaleViewModel(this, new KelvinTemperatureScale());
            this.Celsius = new TemperatureScaleViewModel(this, new CelsiusTemperatureScale());
            this.Farenheit = new TemperatureScaleViewModel(this, new FarenheitTemperatureScale());
        }

        public Cell<double> TemperatureInKelvin { get; set; }
        public TemperatureScaleViewModel Kelvin { get; }
        public TemperatureScaleViewModel Celsius { get; }
        public TemperatureScaleViewModel Farenheit { get; }

        public IEnumerable<TemperatureScaleViewModel> Scales
        {
            get
            {
                yield return Celsius;
                yield return Farenheit;
                yield return Kelvin;
            }
        }
    }

    public class TemperatureScaleViewModel
    {
        private readonly ConverterViewModel parent;
        private readonly ITemperatureScale temperatureScale;

        public TemperatureScaleViewModel(ConverterViewModel parent, ITemperatureScale temperatureScale)
        {
            this.parent = parent;
            this.temperatureScale = temperatureScale;
            this.Temperature = this.parent.TemperatureInKelvin.Derive(
                kelvin => temperatureScale.ConvertFromKelvin(kelvin),
                from => temperatureScale.ConvertToKelvin(from));

            var minimum = temperatureScale.ConvertFromKelvin(0);
            var maximum = temperatureScale.ConvertFromKelvin(1000);
            this.Increment = new AddCommand(this.Temperature, 1, minimum, maximum);
            this.Decrement = new AddCommand(this.Temperature, -1, minimum, maximum);
        }

        public string Name => temperatureScale.Name;
        public Cell<double> Temperature {get; set;}
        public ICommand Increment { get; }
        public ICommand Decrement { get; }
    }

    internal class AddCommand : ICommand
    {
        private readonly Cell<double> cell;

        public AddCommand(Cell<double> cell, double delta, double minimum, double maximum)
        {
            this.cell = cell;
            this.Delta = delta;
            this.Minimum = minimum;
            this.Maximum = maximum;

            this.cell.PropertyChanged += (sender, args) => CanExecuteChanged(this, new EventArgs());
        }

        public double Delta { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            var wouldBe = Math.Round(this.cell.Value + this.Delta);
            return !(wouldBe < this.Minimum || wouldBe > this.Maximum);
        }

        public void Execute(object parameter)
        {
            this.cell.Value = Math.Round(this.cell.Value + this.Delta);
        }
    }
}