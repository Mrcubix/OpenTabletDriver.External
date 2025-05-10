using Avalonia.Data.Converters;
using Newtonsoft.Json.Linq;
using OpenTabletDriver.External.Common.Serializables.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OpenTabletDriver.External.Avalonia.Converters;

public class JTokenMultiConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count != 2)
            return null;
            //throw new NotSupportedException("Only 2 parameters are supported");

        if (values[0] is not JToken token)
            return null;
            //throw new NotSupportedException("First parameter must be a JToken");

        if (values[1] is not SerializableProperty property)
            return null;
            //throw new NotSupportedException("Second parameter must be a SerializableProperty");

        return property.Type switch
        {
            JTokenType.Integer => token.Type == JTokenType.Null ? 0 : token.ToObject<int>(),
            JTokenType.Float => token.Type == JTokenType.Null ? 0 : token.ToObject<double>(),
            JTokenType.String => token.Type == JTokenType.Null ? string.Empty : token.ToObject<string>(),
            JTokenType.Boolean => token.Type != JTokenType.Null && token.ToObject<bool>(),
            _ => null
        };
    }
}
