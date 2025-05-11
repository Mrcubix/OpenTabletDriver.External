using System;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

#nullable enable

public partial class BindingDisplayViewModel : ViewModelBase
{
    /// <summary>
    ///   The description of the binding. <br>
    ///   This is the text on the left column.
    /// </summary>
    [ObservableProperty]
    private string? _description;

    /// <summary>
    ///   The content of the binding. <br>
    ///   This is the text contained in the button leading to the binding editor dialog.
    /// </summary>
    [ObservableProperty]
    private string? _content;

    [ObservableProperty]
    private SerializablePluginSettingsStore? _store;

    public BindingDisplayViewModel()
    {
        Description = "PlaceHolder";
        Content = "";
        Store = null;
    }

    public BindingDisplayViewModel(SerializablePluginSettingsStore store)
    {
        Store = store;
    }

    public BindingDisplayViewModel(string description, string content, SerializablePluginSettingsStore store)
    {
        Description = description;
        Content = content;
        Store = store;
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
