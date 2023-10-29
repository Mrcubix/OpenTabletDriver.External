using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

public partial class AdvancedBindingEditorDialogViewModel : BindingEditorDialogViewModel
{
    [ObservableProperty]
    private ObservableCollection<string> _types = null!;

    [ObservableProperty]
    private string _selectedType = null!;

    [ObservableProperty]
    private ObservableCollection<string> _validProperties = null!;

    [ObservableProperty]
    private string _selectedProperty = null!;

    public AdvancedBindingEditorDialogViewModel()
    {
        Types = new ObservableCollection<string>();
        ValidProperties = new ObservableCollection<string>();
    }

    public AdvancedBindingEditorDialogViewModel(ObservableCollection<string> types, ObservableCollection<string> validProperties)
    {
        Types = types;
        ValidProperties = validProperties;
    }

    public event EventHandler ApplyRequested = null!;

    public void Apply()
    {
        ApplyRequested?.Invoke(this, EventArgs.Empty);
    }
}