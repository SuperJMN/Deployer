using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using CSharpFunctionalExtensions;
using Zafiro.Reflection;

namespace Deployer.Gui
{
    public class MaybeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetMaybeValue(value);
        }

        private object? GetMaybeValue(object value)
        {
            var type = value.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Maybe<>))
            {
                if (value.Get<bool>("HasValue"))
                {
                    return value.Get<object>("Value");
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Maybe.From((string)value);
        }
    }
}