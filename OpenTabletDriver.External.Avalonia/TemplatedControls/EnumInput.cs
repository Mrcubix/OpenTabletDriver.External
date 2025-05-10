
using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Data;

namespace OpenTabletDriver.External.Avalonia.TemplatedControls;

/// <summary>
///   A control that allows the user to pick a value from a list.
/// </summary>
/// <remarks>
///   Also shamelessly stolen from OpenTabletDriver's UI revamp.
/// </remarks>
[TemplatePart("PART_Input", typeof(ComboBox))]
public class EnumInput : DescribedInput
{
    private object? _selectedItem;
    private IEnumerable? _itemsSource;

    public static readonly DirectProperty<EnumInput, object?> SelectedItemProperty =
        AvaloniaProperty.RegisterDirect<EnumInput, object?>(
            nameof(SelectedItem),
            o => o.SelectedItem,
            (o, v) => o.SelectedItem = v,
            defaultBindingMode: BindingMode.TwoWay
        );

    public static readonly DirectProperty<EnumInput, IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.RegisterDirect<EnumInput, IEnumerable?>(
            nameof(ItemsSource),
            o => o.ItemsSource,
            (o, v) => o.ItemsSource = v
        );

    public object? SelectedItem
    {
        get => _selectedItem;
        set => SetAndRaise(SelectedItemProperty, ref _selectedItem, value);
    }

    public IEnumerable? ItemsSource
    {
        get => _itemsSource;
        set => SetAndRaise(ItemsSourceProperty, ref _itemsSource, value);
    }
}