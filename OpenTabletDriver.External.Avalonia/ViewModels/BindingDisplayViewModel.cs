using System;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

#nullable enable

public partial class BindingDisplayViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private string? _content;

    [ObservableProperty]
    private SerializablePluginSettings? _pluginProperty;

    public BindingDisplayViewModel()
    {
        Description = "PlaceHolder";
        Content = "";
        PluginProperty = null;
    }

    public BindingDisplayViewModel(SerializablePluginSettings? pluginProperty)
    {
        PluginProperty = pluginProperty;
    }

    public BindingDisplayViewModel(string description, string content, SerializablePluginSettings? pluginProperty)
    {
        Description = description;
        Content = content;
        PluginProperty = pluginProperty;
    }

    public event EventHandler<BindingDisplayViewModel>? ShowBindingEditorDialogRequested;
    public event EventHandler<BindingDisplayViewModel>? ShowAdvancedBindingEditorDialogRequested;

    public void ShowBindingEditorDialog()
    {
        ShowBindingEditorDialogRequested?.Invoke(this, this);
    }

    public void ShowAdvancedBindingEditorDialog()
    {
        ShowAdvancedBindingEditorDialogRequested?.Invoke(this, this);
    }
}
