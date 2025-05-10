using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenTabletDriver.External.Avalonia.Models;
using OpenTabletDriver.External.Common.Serializables;
using OpenTabletDriver.External.Common.Serializables.Properties;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

public partial class PluginSettingStoreEditorViewModel : ViewModelBase
{
    private IEnumerable<PropertySettingPair> _propertySettingPairs = [];
    
    [ObservableProperty]
    private SerializablePluginSettingsStore? _store;

    [ObservableProperty]
    private IEnumerable<SerializableProperty>? _properties;

    public PluginSettingStoreEditorViewModel()
    {
        PropertyChanged += PropertiesChanged;
    }

    public IEnumerable<PropertySettingPair> PropertySettingPairs
    {
        get => _propertySettingPairs;
        private set => SetProperty(ref _propertySettingPairs, value);
    }

    public void PropertiesChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Rebuild PropertySettingPairs when either Store or Properties change
        if ((e.PropertyName == nameof(Store) || e.PropertyName == nameof(Properties)) && 
            Store is not null && Properties is not null)
            PropertySettingPairs = Properties.Select(p => new PropertySettingPair(p, Store[p]));
    }
}