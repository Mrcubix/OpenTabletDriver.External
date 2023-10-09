using System;
using System.Linq;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using OpenTabletDriver.External.Avalonia.ViewModels;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.Dialogs;

public partial class AdvancedBindingEditorDialog : Window
{
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

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is AdvancedBindingEditorDialogViewModel vm)
        {
            vm.CloseRequested += (s, e) => Close(new SerializablePluginSettings()
            {
                Identifier = -1,
                Value = "None"
            });

            vm.ApplyRequested += (s, e) => Close(new SerializablePluginSettings()
            {
                Identifier = Plugins.FirstOrDefault(p => p.PluginName == vm.SelectedType)?.Identifier ?? -1,
                Value = vm.SelectedProperty
            });

            TypesComboBox.SelectionChanged += (s, e) =>
            {
                if (Plugins == null)
                    return;

                var plugin = Plugins.FirstOrDefault(p => p.PluginName == (string?)(TypesComboBox.SelectedItem));

                if (plugin != null)
                {
                    PropertiesComboBox.ItemsSource = plugin.ValidProperties;
                }
            };
        }
    }
}
