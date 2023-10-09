using System;
using OpenTabletDriver.External.Common.Serializables;
using ReactiveUI;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

public class BindingDisplayViewModel : ViewModelBase
{
    private string? _description;
    private string? _content;
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

    public string? Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public string? Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    public SerializablePluginSettings? PluginProperty
    {
        get => _pluginProperty;
        set => this.RaiseAndSetIfChanged(ref _pluginProperty, value);
    }
    
    public event EventHandler<BindingDisplayViewModel>? OnShowBindingEditorDialog;
    public event EventHandler<BindingDisplayViewModel>? OnShowAdvancedBindingEditorDialog;

    public void OnShowBindingEditorDialogEvent()
    {
        OnShowBindingEditorDialog?.Invoke(this, this);
    }

    public void OnShowAdvancedBindingEditorDialogEvent()
    {
        OnShowAdvancedBindingEditorDialog?.Invoke(this, this);
    }
}
