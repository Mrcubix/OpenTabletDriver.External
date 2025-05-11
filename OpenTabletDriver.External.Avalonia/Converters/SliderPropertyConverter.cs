using System;
using System.Globalization;
using Avalonia.Data.Converters;
using OpenTabletDriver.External.Common.Enums;
using OpenTabletDriver.External.Common.Serializables.Properties;

namespace OpenTabletDriver.External.Avalonia.Converters;

public class SliderPropertyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SerializableSliderProperty sliderProperty)
            return Array.Empty<object?>();

        if (parameter is not AttributeModifierType modifierType)
            return null;

        return modifierType switch
        {
            AttributeModifierType.Minimum => sliderProperty.Minimum,
            AttributeModifierType.Maximum => sliderProperty.Maximum,
            _ => null
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}