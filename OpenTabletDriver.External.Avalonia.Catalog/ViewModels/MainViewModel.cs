using OpenTabletDriver.External.Avalonia.ViewModels;
using ReactiveUI;

namespace OpenTabletDriver.External.Avalonia.Catalog.ViewModels;

public class MainViewModel : ReactiveObject
{
    private static readonly Area _AvailableArea = new(0, 0, 152.0, 95.0);
    private static readonly Area _FullMappedArea = new(76, 47.5, 152.0, 95.0, true);
    private static readonly Area _HalfMappedArea = new(76, 47.5, 76, 47.5, true);
    private static readonly Area _QuarterMappedArea = new(76, 47.5, 38, 23.75, true);

    public MainViewModel() {}

    public BindingDisplayViewModel LeftMouseClick { get; set; } = new("Tip Button", "Mouse Left Click", null);
    public BindingDisplayViewModel RightMouseClick { get; set; } = new("Side Button", "Mouse Right Click", null);

    public BindingDisplayViewModel AKeyboardKey { get; set; } = new("Auxiliary Button 1", "Key Binding A", null);
    public BindingDisplayViewModel ZKeyboardKey { get; set; } = new("Auxiliary Button 2", "Key Binding Z", null);

    public AreaDisplayViewModel FullArea { get; set; } = new(_AvailableArea, _FullMappedArea);
    public AreaDisplayViewModel HalfArea { get; set; } = new(_AvailableArea, _HalfMappedArea);
    public AreaDisplayViewModel QuarterArea { get; set; } = new(_AvailableArea, _QuarterMappedArea);
    public AreaDisplayViewModel CustomArea { get; set; } = new();
}
