

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using Newtonsoft.Json.Linq;
using OpenTabletDriver.External.Avalonia.Models;

namespace OpenTabletDriver.External.Avalonia.DataTemplates;

public class PropertySettingPairDataTemplate : IDataTemplate
{
    // TODO : Move to resx file
    public const string UNSUPPORTED_PARAM = "Provided parameter is not a PropertySettingPair";
    public const string UNSUPPORTED_PROPERTY_TYPE = "Unsupported Property Type";

    [Content]
    public Dictionary<JTokenType, IDataTemplate> PropertyTemplates { get; } = [];

    public Control? Build(object? param)
    {
        if (param is not PropertySettingPair pair)
            return new TextBlock { Text = UNSUPPORTED_PARAM };

        if (PropertyTemplates.TryGetValue(pair.Property.Type, out var template))
            return template.Build(null);

        return new TextBlock { Text = UNSUPPORTED_PROPERTY_TYPE };
    }

    public bool Match(object? data)
    {
        return data is PropertySettingPair;
    }
}