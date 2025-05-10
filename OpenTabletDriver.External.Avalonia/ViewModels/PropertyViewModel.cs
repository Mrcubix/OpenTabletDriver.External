using CommunityToolkit.Mvvm.ComponentModel;
using OpenTabletDriver.External.Common.Serializables;
using OpenTabletDriver.External.Common.Serializables.Properties;

namespace OpenTabletDriver.External.Avalonia.ViewModels.Generics
{
    public partial class PropertyViewModel<T> : ViewModelBase 
        where T : SerializableProperty
    {
        [ObservableProperty]
        private SerializablePluginSettings? _pluginSetting;

        public T Property { get; } = default!;
    }
}