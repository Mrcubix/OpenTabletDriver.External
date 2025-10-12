using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using OpenTabletDriver.External.Common.Enums;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.Converters;

public class ModifierFromEnumConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not IEnumerable<SerializableAttributeModifier> modifiers)
            return null;

        if (parameter is not AttributeModifierType modifierType)
            return null;

        return modifiers.FirstOrDefault(m => m.Type == modifierType)?.Value;
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}