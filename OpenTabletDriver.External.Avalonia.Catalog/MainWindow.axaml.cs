using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using OpenTabletDriver.External.Avalonia.Catalog.ViewModels;
using OpenTabletDriver.External.Avalonia.Dialogs;
using OpenTabletDriver.External.Avalonia.ViewModels;
using OpenTabletDriver.External.Avalonia.Views;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.Catalog;

#nullable enable

public partial class MainWindow : AppMainWindow
{
    private static readonly BindingEditorDialogViewModel _bindingEditorDialogViewModel = new();
    private static readonly AdvancedBindingEditorDialogViewModel _advancedBindingEditorDialogViewModel = new();
    private static readonly ObservableCollection<SerializablePlugin> _plugins = new();
    private static bool _isEditorDialogOpen = false;

    public MainWindow()
    {
        InitializeComponent();

        _plugins.Add(new SerializablePlugin());
        _plugins.Add(new SerializablePlugin()
        {
            PluginName = "Plugin X",
            FullName = "yes",
            Identifier = 1,
            ValidProperties = new string[] { "a", "b" }
        });
    }

    public override void ShowBindingEditorDialog(object? sender, BindingDisplayViewModel e)
    {
        _ = Dispatcher.UIThread.InvokeAsync(() => ShowBindingEditorDialogCore(e));
    }

    public override void ShowAdvancedBindingEditorDialog(object? sender, BindingDisplayViewModel e)
    {
        _ = Dispatcher.UIThread.InvokeAsync(() => ShowAdvancedBindingEditorDialogCore(e));
    }

    private async Task ShowBindingEditorDialogCore(BindingDisplayViewModel e)
    {
        if (DataContext is MainViewModel vm && !_isEditorDialogOpen)
        {
            _isEditorDialogOpen = true;

            // Now we setup the dialog

            var dialog = new BindingEditorDialog()
            {
                Plugins = _plugins,
                DataContext = _bindingEditorDialogViewModel
            };

            #if DEBUG

            dialog.AttachDevTools();

            #endif

            // Now we show the dialog

            var res = await dialog.ShowDialog<SerializablePluginSettings>(this);

            _isEditorDialogOpen = false;

            // We handle the result

            // The dialog was closed or the cancel button was pressed
            if (res == null)
                return;

            // The user selected "Clear"
            if (res.Identifier == -1 || res.Value == "None")
            {
                e.PluginProperty = null;
                e.Content = "";
            }
            else
            {
                e.PluginProperty = res;
                e.Content = res.Value;
            }
        }
    }

    private async Task ShowAdvancedBindingEditorDialogCore(BindingDisplayViewModel e)
    {
        if (DataContext is MainViewModel vm && !_isEditorDialogOpen)
        {
            _isEditorDialogOpen = true;

            // Now we set the view model's properties

            var types = _plugins.Select(p => p.PluginName ?? p.FullName ?? "Unknown").ToList();

            var currentPlugin = _plugins.FirstOrDefault(p => p.Identifier == e.PluginProperty?.Identifier);
            var selectedType = currentPlugin?.PluginName ?? currentPlugin?.FullName ?? "Unknown";

            var validProperties = currentPlugin?.ValidProperties ?? new string[0];
            var selectedProperty = e.PluginProperty?.Value ?? "";

            // Now we set the view model's properties

            _advancedBindingEditorDialogViewModel.Types = new ObservableCollection<string>(types);
            _advancedBindingEditorDialogViewModel.SelectedType = selectedType;
            _advancedBindingEditorDialogViewModel.ValidProperties = new ObservableCollection<string>(validProperties);
            _advancedBindingEditorDialogViewModel.SelectedProperty = selectedProperty;

            // Now we setup the dialog

            var dialog = new AdvancedBindingEditorDialog()
            {
                DataContext = _advancedBindingEditorDialogViewModel,
                Plugins = _plugins
            };

            // Now we show the dialog

            var res = await dialog.ShowDialog<SerializablePluginSettings>(this);

            _isEditorDialogOpen = false;

            // We handle the result

            // The dialog was closed or the cancel button was pressed
            if (res == null)
                return;

            // The user selected "Clear"
            if (res.Identifier == -1 || res.Value == "None")
            {
                e.PluginProperty = null;
                e.Content = "";
            }
            else
            {
                e.PluginProperty = res;
                e.Content = res.Value;
            }
        }
    }
}