using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Newtonsoft.Json.Linq;
using OpenTabletDriver.External.Avalonia.Catalog.ViewModels;
using OpenTabletDriver.External.Avalonia.Dialogs;
using OpenTabletDriver.External.Avalonia.ViewModels;
using OpenTabletDriver.External.Avalonia.Views;
using OpenTabletDriver.External.Common.Enums;
using OpenTabletDriver.External.Common.Serializables;
using OpenTabletDriver.External.Common.Serializables.Properties;

namespace OpenTabletDriver.External.Avalonia.Catalog;

#nullable enable

public partial class MainWindow : AppMainWindow
{
    private static readonly BindingEditorDialogViewModel _bindingEditorDialogViewModel = new();
    private static readonly AdvancedBindingEditorDialogViewModel _advancedBindingEditorDialogViewModel = new();
    private static readonly ObservableCollection<SerializablePlugin> _plugins = new();
    private static readonly string[] _bindingValues = new[] { "a", "b", "c" };
    private static readonly string[] _mouseButtonValues = new[] { "Left", "Middle", "Right", "Backward", "Forward" };
    private static readonly IEnumerable<SerializableAttributeModifier> _exampleModifiers = new[]
    {
        new SerializableAttributeModifier(AttributeModifierType.Tooltip, "Tooltip Here"),
        new SerializableAttributeModifier(AttributeModifierType.Unit, "ms"),
    };
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
            Type = PluginType.Binding,
            Properties = new()
            {
                new SerializableValidatedProperty("Validated String", JTokenType.Array, _bindingValues, _exampleModifiers),
                new SerializableProperty("Example Double", JTokenType.Float, _exampleModifiers),
                new SerializableProperty("Example String", JTokenType.String, _exampleModifiers)
            }
        });
        _plugins.Add(new SerializablePlugin()
        {
            PluginName = "Mouse Button Binding",
            FullName = "OpenTabletDriver.Desktop.Binding.MouseBinding",
            Identifier = 2,
            Type = PluginType.Binding,
            Properties = new()
            {
                new SerializableValidatedProperty("Button", JTokenType.Array, _mouseButtonValues, _exampleModifiers)
            }
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

            // Now we set the view model's properties

            var bindingPlugins = _plugins.Where(p => p.Type == PluginType.Binding).ToList();
            var selectedPlugin = bindingPlugins.FirstOrDefault(p => p.Identifier == e.Store?.Identifier);

            _bindingEditorDialogViewModel.Store = e.Store;

            // Now we setup the dialog

            var dialog = new BindingEditorDialog()
            {
                Plugins = _plugins,
                DataContext = _bindingEditorDialogViewModel
            };

#if DEBUG
            dialog.AttachDevTools();
#endif

            // Now we show & handle the dialog
            await HandleBindingEditorDialog(dialog, e);
        }
    }

    private async Task ShowAdvancedBindingEditorDialogCore(BindingDisplayViewModel e)
    {
        if (DataContext is MainViewModel vm && !_isEditorDialogOpen)
        {
            _isEditorDialogOpen = true;

            // Now we set the view model's properties

            var bindingPlugins = _plugins.Where(p => p.Type == PluginType.Binding).ToList();
            var selectedPlugin = bindingPlugins.FirstOrDefault(p => p.Identifier == e.Store?.Identifier);
            
            var settingsStoreEditor = new PluginSettingStoreEditorViewModel()
            {
                Properties = selectedPlugin?.Properties ?? [],
                Store = e.Store
            };

            // Now we set the view model's properties

            _advancedBindingEditorDialogViewModel.BindingTypes = [.. bindingPlugins];
            _advancedBindingEditorDialogViewModel.SelectedBindingType = selectedPlugin;
            _advancedBindingEditorDialogViewModel.SettingStore = settingsStoreEditor;

            // Now we setup the dialog
            var dialog = new AdvancedBindingEditorDialog()
            {
                DataContext = _advancedBindingEditorDialogViewModel,
                Plugins = _plugins
            };

#if DEBUG
            dialog.AttachDevTools();
#endif

            // Now we show & handle the dialog
            await HandleBindingEditorDialog(dialog, e);
        }
    }

    private async Task HandleBindingEditorDialog(Window dialog, BindingDisplayViewModel e)
    {
        var res = await dialog.ShowDialog<SerializablePluginSettingsStore>(this);

        _isEditorDialogOpen = false;

        // If the result is the same as before or null, we don't need to do anything
        if (res == e.Store)
            return;

        // We handle the result
        e.Store = res;
        e.Content = res?.GetHumanReadableString();
    }
}