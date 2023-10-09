using System;
using System.Linq;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Input;
using OpenTabletDriver.External.Avalonia.ViewModels;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.Dialogs;

public partial class BindingEditorDialog : Window
{
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

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is BindingEditorDialogViewModel vm)
        {
            vm.CloseRequested += (s, e) => Close(new SerializablePluginSettings()
            {
                Identifier = KeyBindingPlugin?.Identifier ?? -1,
                Value = "None"
            });
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (DataContext is BindingEditorDialogViewModel vm)
        {
            if (e.Key == Key.Escape)
            {
                Close(null);
            }
            else
            {
                Close(new SerializablePluginSettings()
                {
                    Identifier = KeyBindingPlugin?.Identifier ?? -1,
                    Value = e.Key.ToString()
                });
            }
        }

        base.OnKeyDown(e);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (DataContext is BindingEditorDialogViewModel vm)
            Close(new SerializablePluginSettings()
            {
                Identifier = MouseBindingPlugin?.Identifier ?? -1,
                Value = ParseMouseClick(e)
            });

        base.OnPointerPressed(e);
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
        else
            return "None";
    }
}
