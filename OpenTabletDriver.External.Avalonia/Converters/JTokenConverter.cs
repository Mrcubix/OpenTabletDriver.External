using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Newtonsoft.Json.Linq;

namespace OpenTabletDriver.External.Avalonia.Converters;

public class JTokenConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not JToken token)
            return null;

        return token.Type switch
        {
            JTokenType.Boolean => token.Type != JTokenType.Null && token.ToObject<bool>(),
            JTokenType.Float => token.Type == JTokenType.Null ? 0 : token.ToObject<double>(),
            JTokenType.Integer => token.Type == JTokenType.Null ? 0 : token.ToObject<double>(),
            JTokenType.String => token.Type == JTokenType.Null ? string.Empty : token.ToObject<string>(),
            JTokenType.Array => token.Type == JTokenType.Null ? [] : token.ToObject<object[]>(),
            _ => null
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value == null ? JValue.CreateNull() : JToken.FromObject(value);
    }
}