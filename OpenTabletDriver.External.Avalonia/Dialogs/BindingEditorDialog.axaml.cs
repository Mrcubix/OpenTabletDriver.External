using System;
using System.Linq;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Input;
using OpenTabletDriver.External.Avalonia.ViewModels;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.Dialogs;

#nullable enable

public partial class BindingEditorDialog : Window
{
    private SerializablePluginSettingsStore? _previousStore = null;
    protected ObservableCollection<SerializablePlugin> _plugins = null!;

    public BindingEditorDialog()
    {
        InitializeComponent();
    }

    public ObservableCollection<SerializablePlugin> Plugins
    {
        get => _plugins;
        set
        {
            KeyBindingPlugin = value.FirstOrDefault(p => p.FullName == "OpenTabletDriver.Desktop.Binding.KeyBinding");
            MouseBindingPlugin = value.FirstOrDefault(p => p.FullName == "OpenTabletDriver.Desktop.Binding.MouseBinding");

            _plugins = value;
        }
    }

    public SerializablePlugin? KeyBindingPlugin { get; private set; }
    public SerializablePlugin? MouseBindingPlugin { get; private set; }

    protected override void OnOpened(EventArgs e)
    {
        if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
        {
            Activate();
            Inputs.Focus();
        }

        base.OnOpened(e);
    }

    protected override void OnDataContextBeginUpdate()
    {
        base.OnDataContextBeginUpdate();

        if (DataContext is BindingEditorDialogViewModel vm)
        {
            _previousStore = null;
            vm.ClearRequested -= OnClearRequested;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is BindingEditorDialogViewModel vm)
        {
            _previousStore = vm.Store;
            vm.ClearRequested += OnClearRequested;
        }
    }

    private string ParseMouseClick(PointerPressedEventArgs e)
    {
        var properties = e.GetCurrentPoint(this).Properties;

        if (properties.IsLeftButtonPressed)
            return "Left";
        else if (properties.IsRightButtonPressed)
            return "Right";
        else if (properties.IsMiddleButtonPressed)
            return "Middle";
        else if (properties.IsXButton1Pressed)
            return "Backward";
        else if (properties.IsXButton2Pressed)
            return "Forward";
        else
            return "None";
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (DataContext is BindingEditorDialogViewModel vm)
        {
            if (e.Key == Key.Escape)
            {
                Close(_previousStore);
            }
            else
            {
                Close(new SerializablePluginSettingsStore()
                {
                    PluginName = "Key Binding",
                    FullName = "OpenTabletDriver.Desktop.Binding.KeyBinding",
                    Identifier = KeyBindingPlugin?.Identifier ?? -1,
                    Settings = [
                        new SerializablePluginSettings()
                        {
                            Identifier = KeyBindingPlugin?.Identifier ?? -1,
                            Property = "Key",
                            Value = e.Key.ToString()
                        }
                    ]
                });
            }
        }

        base.OnKeyDown(e);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (DataContext is BindingEditorDialogViewModel &&
            e.Pointer.Type == PointerType.Mouse)
        {
            Close(new SerializablePluginSettingsStore()
            {
                PluginName = "Mouse Button Binding",
                FullName = "OpenTabletDriver.Desktop.Binding.MouseBinding",
                Identifier = MouseBindingPlugin?.Identifier ?? -1,
                Settings = [
                    new SerializablePluginSettings()
                    {
                        Identifier = MouseBindingPlugin?.Identifier ?? -1,
                        Property = "Button",
                        Value = ParseMouseClick(e)
                    }
                ]
            });
        }

        base.OnPointerPressed(e);
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
}
