using System;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

#nullable enable

public partial class BindingEditorDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private SerializablePluginSettingsStore? _store = null!;

    public event EventHandler ClearRequested = null!;

    public void Clear()
    {
        Store = null!;
        ClearRequested?.Invoke(this, null!);
    }
}
