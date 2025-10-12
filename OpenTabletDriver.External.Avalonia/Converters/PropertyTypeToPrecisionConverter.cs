using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Newtonsoft.Json.Linq;

namespace OpenTabletDriver.External.Avalonia.Converters;

public class PropertyTypeToPrecisionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not JTokenType type)
            return 0;

        return type switch
        {
            JTokenType.Integer => 0,
            JTokenType.Float => 5,
            _ => 0
        };
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}