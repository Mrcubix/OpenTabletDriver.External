using System;
using System.Globalization;
using Avalonia.Data.Converters;
using OpenTabletDriver.External.Common.Serializables.Properties;

namespace OpenTabletDriver.External.Avalonia.Converters;

public class ValidatedPropertyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SerializableValidatedProperty validatedProperty)
            return Array.Empty<object?>();

        return validatedProperty.Values;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}