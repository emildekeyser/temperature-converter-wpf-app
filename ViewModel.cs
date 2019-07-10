namespace view
{
    class ConverterViewModel : INotifyPropertyChanged
    {
        private double _temperatureInKelvin;

        public double TemperatureInKelvin
        {
            get => _temperatureInKelvin;
            set
            {
                _temperatureInKelvin = value;

                var eventArgs = new PropertyChangedEventArgs(nameof(this.TemperatureInKelvin));
                // ?. notation instead of if null check
                this.PropertyChanged?.Invoke(this, eventArgs);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}