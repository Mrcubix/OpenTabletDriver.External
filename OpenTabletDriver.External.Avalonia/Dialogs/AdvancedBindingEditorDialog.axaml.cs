using System;
using System.Linq;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using OpenTabletDriver.External.Avalonia.ViewModels;
using OpenTabletDriver.External.Common.Serializables;
using OpenTabletDriver.External.Avalonia.Extensions;

namespace OpenTabletDriver.External.Avalonia.Dialogs;

#nullable enable

public partial class AdvancedBindingEditorDialog : Window
{
    private SerializablePlugin? _previousPlugin = null;
    private SerializablePluginSettingsStore? _previousStore = null;
    protected ObservableCollection<SerializablePlugin> _plugins = null!;

    public AdvancedBindingEditorDialog()
    {
        InitializeComponent();
    }

    public ObservableCollection<SerializablePlugin> Plugins
    {
        get => _plugins;
        set => _plugins = value;
    }

    protected override void OnDataContextBeginUpdate()
    {
        base.OnDataContextBeginUpdate();

        if (DataContext is AdvancedBindingEditorDialogViewModel vm)
        {
            _previousPlugin = null;
            _previousStore = null;

            vm.ClearRequested -= OnClearRequested;
            vm.ApplyRequested -= OnApplyRequested;

            TypesComboBox.SelectionChanged -= OnTypesComboBoxSelectionChanged;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is AdvancedBindingEditorDialogViewModel vm)
        {
            _previousPlugin ??= vm.SelectedBindingType;
            _previousStore ??= vm.SettingStore?.Store;

            vm.ClearRequested += OnClearRequested;
            vm.ApplyRequested += OnApplyRequested;

            TypesComboBox.SelectionChanged += OnTypesComboBoxSelectionChanged;
        }
    }

    // TODO : Get rid of this mess & use a future DialogResult Property once avalonia have fixed their shit
    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (!e.IsProgrammatic)
        {
            e.Cancel = true;
            Close(_previousStore);
        }

        base.OnClosing(e);
    }

    private void OnClearRequested(object? sender, EventArgs e)
    {
        Close(null);
    }

    private void OnApplyRequested(object? sender, EventArgs e)
    {
        if (DataContext is AdvancedBindingEditorDialogViewModel vm)
            Close(vm.SettingStore?.Store);
    }

    private void OnTypesComboBoxSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataContext is AdvancedBindingEditorDialogViewModel vm)
        {
            if (Plugins == null)
                return;

            var selectedType = (SerializablePlugin?)(TypesComboBox.SelectedItem);
            var plugin = Plugins.FirstOrDefault(p => p == selectedType);

            if (plugin != null && plugin != _previousPlugin)
            {
                if (vm.SettingStore == null)
                {
                    // Create a new store with the plugin's properties
                    vm.SettingStore = new PluginSettingStoreEditorViewModel()
                    {
                        Properties = plugin.Properties,
                        Store = new SerializablePluginSettingsStore(plugin)
                    };
                }
                else
                {
                    // Update the existing store
                    vm.SettingStore.Properties ??= [];

                    // Replacing the collection would result in binding issues
                    vm.SettingStore.Properties?.Clear(); 
                    vm.SettingStore.Properties?.AddRange(plugin.Properties);
                    vm.SettingStore.Store = new SerializablePluginSettingsStore(plugin);
                }
            }

            _previousPlugin = plugin;
        }
    }
}
