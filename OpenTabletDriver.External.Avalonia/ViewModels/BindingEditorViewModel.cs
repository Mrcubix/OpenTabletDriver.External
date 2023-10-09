using System;
using OpenTabletDriver.External.Common.Serializables;
using ReactiveUI;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

public class BindingEditorDialogViewModel : ViewModelBase
{
    private SerializablePluginSettings? _property = null!;

    public event EventHandler CloseRequested = null!;

    public SerializablePluginSettings? Property
    {
        get => _property;
        set => this.RaiseAndSetIfChanged(ref _property, value);
    }

    public void Clear()
    {
        _property = null!;

        CloseRequested?.Invoke(this, null!);
    }
}
