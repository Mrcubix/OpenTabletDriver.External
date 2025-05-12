using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using OpenTabletDriver.External.Avalonia.Catalog.ViewModels;
using OpenTabletDriver.External.Avalonia.Dialogs;
using OpenTabletDriver.External.Avalonia.Extensions;
using OpenTabletDriver.External.Avalonia.ViewModels;
using OpenTabletDriver.External.Avalonia.Views;
using OpenTabletDriver.External.Common.Enums;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.Catalog;

#nullable enable

public partial class MainWindow : AppMainWindow
{
    private static readonly ObservableCollection<SerializablePlugin> _plugins = new();
    private static bool _isEditorDialogOpen = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            _plugins.Clear();
            _plugins.AddRange(vm.Plugins);
        }

        base.OnDataContextChanged(e);
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

            var bindingEditorDialogViewModel = new BindingEditorDialogViewModel
            {
                Store = e.Store
            };

            // Now we setup the dialog
            var dialog = new BindingEditorDialog()
            {
                Plugins = _plugins,
                DataContext = bindingEditorDialogViewModel
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

            var advancedBindingEditorDialogViewModel = new AdvancedBindingEditorDialogViewModel([.. bindingPlugins], settingsStoreEditor)
            {
                SelectedBindingType = selectedPlugin,
            };

            // Now we setup the dialog
            var dialog = new AdvancedBindingEditorDialog()
            {
                DataContext = advancedBindingEditorDialogViewModel,
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

        // If we don't dispose it & GC it, Avalonia will randomly convert 
        // Selected values from ValidatedProperties to null for some reason
        // TODO : Get rid of this when a better solution is found
        if (dialog.DataContext is IDisposable disposable)
            disposable.Dispose();
    }
}