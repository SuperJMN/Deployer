using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

namespace Deployer.Gui
{
    public abstract class AsyncConverter<T> : IValueConverter
    {
        public abstract Task<T> AsyncConvert(object value, Type targetType, object parameter, CultureInfo culture);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var res = new GenericAsyncViewModel();
            res.Start(AsyncConvert(value, targetType, parameter, culture));
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private class GenericAsyncViewModel : INotifyPropertyChanged
        {
            private T _result;
            public event PropertyChangedEventHandler PropertyChanged;

            public T Result
            {
                get => _result;
                set
                {
                    _result = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }

            public async void Start(Task<T> task)
            {
                Result = await task;
            }
        }
    }
}