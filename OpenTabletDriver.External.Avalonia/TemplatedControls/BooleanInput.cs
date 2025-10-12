using Avalonia;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace OpenTabletDriver.External.Avalonia.TemplatedControls;

/// <summary>
///   A control that allows the user to toggle a boolean value.
/// </summary>
/// <remarks>
///   Also shamelessly stolen from OpenTabletDriver's UI revamp.
/// </remarks>
[TemplatePart("PART_Input", typeof(ToggleButton))]
public class BooleanInput : DescribedInput
{
    private bool _value;

    public static readonly DirectProperty<BooleanInput, bool> ValueProperty =
        AvaloniaProperty.RegisterDirect<BooleanInput, bool>(
            nameof(Value),
            o => o.Value,
            (o, v) => o.Value = v,
            defaultBindingMode: BindingMode.TwoWay
        );

    public bool Value
    {
        get => _value;
        set => SetAndRaise(ValueProperty, ref _value, value);
    }
}