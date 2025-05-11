using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

public partial class AdvancedBindingEditorDialogViewModel : BindingEditorDialogViewModel
{
    [ObservableProperty]
    private ObservableCollection<SerializablePlugin> _bindingTypes = [];

    [ObservableProperty]
    private SerializablePlugin? _selectedBindingType = null;

    [ObservableProperty]
    private PluginSettingStoreEditorViewModel? _settingStore = new();

    public AdvancedBindingEditorDialogViewModel() {}

    public AdvancedBindingEditorDialogViewModel(ObservableCollection<SerializablePlugin> types, PluginSettingStoreEditorViewModel settingStore)
    {
        BindingTypes = types;
        SettingStore = settingStore;
    }

    public event EventHandler ApplyRequested = null!;

    public void Apply()
    {
        ApplyRequested?.Invoke(this, EventArgs.Empty);
    }
}