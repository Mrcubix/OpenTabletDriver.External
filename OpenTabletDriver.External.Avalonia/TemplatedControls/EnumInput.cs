
using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
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
    private int _selectedIndex = -1;
    private IList? _itemsSource;

    public static readonly DirectProperty<EnumInput, object?> SelectedItemProperty =
        AvaloniaProperty.RegisterDirect<EnumInput, object?>(
            nameof(SelectedItem),
            o => o.SelectedItem,
            (o, v) => o.SelectedItem = v,
            defaultBindingMode: BindingMode.TwoWay
        );

    public static readonly DirectProperty<EnumInput, int> SelectedIndexProperty =
        AvaloniaProperty.RegisterDirect<EnumInput, int>(
            nameof(SelectedIndex),
            o => o.SelectedIndex,
            (o, v) => o.SelectedIndex = v,
            defaultBindingMode: BindingMode.TwoWay
        );

    public static readonly DirectProperty<EnumInput, IList?> ItemsSourceProperty =
        AvaloniaProperty.RegisterDirect<EnumInput, IList?>(
            nameof(ItemsSource),
            o => o.ItemsSource,
            (o, v) => o.ItemsSource = v
        );

    public object? SelectedItem
    {
        get => _selectedItem;
        set => SetAndRaise(SelectedItemProperty, ref _selectedItem, value);
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetAndRaise(SelectedIndexProperty, ref _selectedIndex, value);
    }

    public IList? ItemsSource
    {
        get => _itemsSource;
        set => SetAndRaise(ItemsSourceProperty, ref _itemsSource, value);
    }

    // TODO : Get rid of this, Currently a Workaround for a Binding Issue in ComboBox
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        var comboBox = e.NameScope.Get<ComboBox>("PART_Input");

        comboBox.SelectedIndex = ItemsSource?.IndexOf(SelectedItem) ?? -1;

        comboBox.SelectionChanged += (sender, args) => 
        {
            // Ignore when ItemSource is empty, otherwise null values will be fed to Settings
            if (ItemsSource == null) return;
            SelectedItem = comboBox.SelectedItem;
            SelectedIndex = comboBox.SelectedIndex;
        };

        base.OnApplyTemplate(e);
    }
}