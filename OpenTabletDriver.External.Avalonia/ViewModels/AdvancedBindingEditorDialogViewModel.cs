using System;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

public class AdvancedBindingEditorDialogViewModel : BindingEditorDialogViewModel
{
    private ObservableCollection<string> _types = null!;
    private string _selectedType = null!;

    private ObservableCollection<string> _validProperties = null!;
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

    public ObservableCollection<string> Types
    {
        get => _types;
        set => this.RaiseAndSetIfChanged(ref _types, value);
    }

    public string SelectedType
    {
        get => _selectedType;
        set => this.RaiseAndSetIfChanged(ref _selectedType, value);
    }

    public ObservableCollection<string> ValidProperties
    {
        get => _validProperties;
        set => this.RaiseAndSetIfChanged(ref _validProperties, value);
    }

    public string SelectedProperty
    {
        get => _selectedProperty;
        set => this.RaiseAndSetIfChanged(ref _selectedProperty, value);
    }

    public event EventHandler ApplyRequested = null!;

    public void Apply()
    {
        ApplyRequested?.Invoke(this, EventArgs.Empty);
    }
}