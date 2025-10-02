using System;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenTabletDriver.External.Common.Serializables;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

#nullable enable

public partial class BindingEditorDialogViewModel(bool doConvertKeysToEto = false) : ViewModelBase
{
    [ObservableProperty]
    private SerializablePluginSettingsStore? _store = null!;

    public event EventHandler ClearRequested = null!;

    public bool DoConvertKeysToEto { get; } = doConvertKeysToEto;

    public void Clear()
    {
        Store = null!;
        ClearRequested?.Invoke(this, null!);
    }
}
