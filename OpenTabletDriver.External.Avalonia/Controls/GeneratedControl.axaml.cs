using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Newtonsoft.Json.Linq;
using OpenTabletDriver.External.Avalonia.Converters;
using OpenTabletDriver.External.Avalonia.Models;
using OpenTabletDriver.External.Avalonia.TemplatedControls;
using OpenTabletDriver.External.Common.Enums;
using OpenTabletDriver.External.Common.Serializables;
using OpenTabletDriver.External.Common.Serializables.Properties;

namespace OpenTabletDriver.External.Avalonia.Controls;

public partial class GeneratedControl : UserControl
{
    public GeneratedControl()
    {
        InitializeComponent();
    }

    // --------------------------------- Methods --------------------------------- //

    /*protected override void OnDataContextChanged(EventArgs e)
    {
        Content = null;

        if (DataContext is PropertySettingPair pair)
        {
            var control = GetControlForSetting(pair.Property, pair.Setting);

            if (control != null)
            {
                // Apply attribute modifiers
                foreach (var modifier in pair.Property.Modifiers)
                    control = ApplyAttributeModifier(control, modifier);

                Content = control;
            }
            else
            {
                throw new NullReferenceException($"{nameof(control)} is null. This is likely due to {pair.Property.Type} being an unsupported type.");
            }
        }

        base.OnDataContextChanged(e);    
    }*/

    private Control? GetControlForSetting(SerializableProperty property, SerializablePluginSettings pluginSetting)
    {
        string? unit = property.Modifiers.FirstOrDefault(m => m.Type == AttributeModifierType.Unit)?.Value?.ToString();
        string? description = property.Modifiers.FirstOrDefault(m => m.Type == AttributeModifierType.Description)?.Value?.ToString();

        if (pluginSetting.Value == null || pluginSetting.Value.Type == JTokenType.Null)
            return null;

        // TODO: Replace whole function with axaml once a solution a found that conserves Compiled Bindings
        var valueBinding = new MultiBinding
        {
            Converter = new JTokenMultiConverter(),
            Bindings =
            {
                new Binding
                {
                    Source = pluginSetting,
                    Path = nameof(SerializablePluginSettings.Value),
                },
                new Binding
                {
                    Source = property,
                }
            }
        };

        if (property.Type == JTokenType.String)
        {
            if (property is SerializableValidatedProperty validatedProperty)
            {
                return new EnumInput()
                {
                    Label = property.Name,
                    ItemsSource = validatedProperty.Values,
                    [!EnumInput.SelectedItemProperty] = valueBinding,
                };
            }
            else
            {
                return new StringInput()
                {
                    Label = property.Name,
                    [!StringInput.ValueProperty] = valueBinding,
                };
            }
        }
        else if (property.Type == JTokenType.Boolean)
        {
            return new BooleanInput()
            {
                Label = property.Name,
                [!BooleanInput.ValueProperty] = valueBinding,
            };
        }
        else if (property.Type == JTokenType.Float) // Either a slider or float normal property
        {
            // TODO: Implement SliderInput
            return new DoubleInput
            {
                Label = property.Name,
                [!DoubleInput.ValueProperty] = valueBinding,
            };
        }
        throw new NotSupportedException($"'{property.Type}' is not supported for generated controls.");
    }

    private static Control ApplyAttributeModifier(Control control, SerializableAttributeModifier modifier)
    {
        switch (modifier.Type)
        {
            case AttributeModifierType.Tooltip:
            {
                ToolTip.SetTip(control, modifier.Value?.ToString());
                return control;
            }
            default:
                return control;
        }
    }
}
