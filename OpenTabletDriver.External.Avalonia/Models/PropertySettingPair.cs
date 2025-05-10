using CommunityToolkit.Mvvm.ComponentModel;
using OpenTabletDriver.External.Common.Serializables;
using OpenTabletDriver.External.Common.Serializables.Properties;

namespace OpenTabletDriver.External.Avalonia.Models;

public partial class PropertySettingPair(SerializableProperty property, SerializablePluginSettings setting) : ObservableObject
{
    [ObservableProperty]
    private SerializableProperty _property = property;
    
    [ObservableProperty]
    private SerializablePluginSettings _setting = setting;
}