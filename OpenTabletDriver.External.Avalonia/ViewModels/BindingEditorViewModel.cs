using System;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

#nullable enable

public partial class BindingEditorDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private SerializablePluginSettings? _property = null!;

    public event EventHandler CloseRequested = null!;

    public void Clear()
    {
        Property = null!;

        CloseRequested?.Invoke(this, null!);
    }
}
