using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Data;

namespace OpenTabletDriver.External.Avalonia.TemplatedControls;

/// <summary>
///   A control that allows the user to input a string value.
/// </summary>
/// <remarks>
///   Also shamelessly stolen from OpenTabletDriver's UI revamp.
/// </remarks>
[TemplatePart("PART_Input", typeof(TextBox))]
public class StringInput : DescribedInput
{
    private string? _value;

    public static readonly DirectProperty<StringInput, string?> ValueProperty =
        AvaloniaProperty.RegisterDirect<StringInput, string?>(
            nameof(Value),
            o => o.Value,
            (o, v) => o.Value = v,
            defaultBindingMode: BindingMode.TwoWay
        );

    public string? Value
    {
        get => _value;
        set => SetAndRaise(ValueProperty, ref _value, value);
    }
}
