using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using OpenTabletDriver.External.Avalonia.Extensions;
using OpenTabletDriver.External.Avalonia.Models;
using OpenTabletDriver.External.Avalonia.ViewModels;
using OpenTabletDriver.External.Common.Enums;
using OpenTabletDriver.External.Common.Serializables;
using OpenTabletDriver.External.Common.Serializables.Properties;

namespace OpenTabletDriver.External.Avalonia.Catalog.ViewModels;

public partial class MainViewModel : ObservableObject
{
    #region Constants

    #region Areas

    private static readonly Area _AvailableArea = new(0, 0, 152.0, 95.0);
    private static readonly Area _FullMappedArea = new(76, 47.5, 152.0, 95.0, true);
    private static readonly Area _HalfMappedArea = new(76, 47.5, 76, 47.5, true);
    private static readonly Area _QuarterMappedArea = new(76, 47.5, 38, 23.75, true);

    #endregion

    #region Properties Attributes Values

    private static readonly string[] _choices = ["Choice 1", "Choice 2", "Choice 3"];
    private static readonly string[] _bindingValues = ["a", "b", "c"];
    private static readonly string[] _mouseButtonValues = ["Left", "Middle", "Right", "Backward", "Forward"];

    private static readonly IEnumerable<SerializableAttributeModifier> _exampleModifiers =
    [
        new SerializableAttributeModifier(AttributeModifierType.Tooltip, "Tooltip Here"),
        new SerializableAttributeModifier(AttributeModifierType.Unit, "ms"),
    ];

    private static readonly IEnumerable<SerializableAttributeModifier> _exampleModifiersWithStringDefault =
    [
        new SerializableAttributeModifier(AttributeModifierType.Tooltip, "Tooltip Here"),
        new SerializableAttributeModifier(AttributeModifierType.Unit, "ms"),
        new SerializableAttributeModifier(AttributeModifierType.DefaultValue, "Hello"),
    ];

    private static readonly IEnumerable<SerializableAttributeModifier> _exampleModifiersWithDoubleDefault =
    [
        new SerializableAttributeModifier(AttributeModifierType.Tooltip, "Tooltip Here"),
        new SerializableAttributeModifier(AttributeModifierType.Unit, "ms"),
        new SerializableAttributeModifier(AttributeModifierType.DefaultValue, 5d),
    ];

    #endregion

    #region Properties

    private static readonly SerializableProperty _ExampleBoolProperty = new("Example Bool", JTokenType.Boolean, _exampleModifiers);
    private static readonly SerializableProperty _ExampleDoubleProperty = new("Example Float", JTokenType.Float, _exampleModifiers);
    private static readonly SerializableProperty _ExampleIntProperty = new("Example Int", JTokenType.Integer, _exampleModifiers);
    private static readonly SerializableProperty _ExampleStringProperty = new("Example String", JTokenType.String, _exampleModifiers);
    private static readonly SerializableValidatedProperty _ExampleValidatedStringProperty = new("Example Validated String", JTokenType.Array, _choices, _exampleModifiers);
    private static readonly SerializableSliderProperty _ExampleSliderProperty = new("Example Slider", JTokenType.Float, _exampleModifiers)
    {
        Minimum = 0,
        Maximum = 100
    };
    private static readonly SerializableSliderProperty _ExampleIntegerSliderProperty = new("Example Integer Slider", JTokenType.Integer, _exampleModifiers)
    {
        Minimum = 0,
        Maximum = 100
    };

    private static readonly SerializableProperty[] _ExampleProperties =
    [
        _ExampleBoolProperty,
        _ExampleDoubleProperty,
        _ExampleIntProperty,
        _ExampleStringProperty,
        _ExampleValidatedStringProperty,
        _ExampleSliderProperty,
        _ExampleIntegerSliderProperty
    ];

    #endregion

    #region Settings

    private static readonly SerializablePluginSettings _ExampleBoolSetting = new(_ExampleBoolProperty, 1, true);
    private static readonly SerializablePluginSettings _ExampleDoubleSetting = new(_ExampleDoubleProperty, 1, 0.5d);
    private static readonly SerializablePluginSettings _ExampleIntSetting = new(_ExampleIntProperty, 1, 42);
    private static readonly SerializablePluginSettings _ExampleStringSetting = new(_ExampleStringProperty, 1, "Hello World");
    private static readonly SerializablePluginSettings _ExampleValidatedStringSetting = new(_ExampleValidatedStringProperty, 1, "Choice 2");
    private static readonly SerializablePluginSettings _ExampleSliderSetting = new(_ExampleSliderProperty, 1, 50d);
    private static readonly SerializablePluginSettings _ExampleIntegerSliderSetting = new(_ExampleIntegerSliderProperty, 1, 50);

    private static readonly SerializablePluginSettings[] _ExampleSettings =
    [
        _ExampleBoolSetting,
        _ExampleDoubleSetting,
        _ExampleIntSetting,
        _ExampleStringSetting,
        _ExampleValidatedStringSetting,
        _ExampleSliderSetting,
        _ExampleIntegerSliderSetting
    ];

    #endregion

    #endregion

    [ObservableProperty]
    private ObservableCollection<SerializablePlugin> _plugins = new();

    public MainViewModel() 
    {
        ExamplePluginSettingsStoreEditor.Store = ExamplePluginSettingsStore;

        Plugins.AddRange(
        [
            new SerializablePlugin()
            {
                PluginName = "Plugin X",
                FullName = "yes",
                Identifier = 1,
                Type = PluginType.Binding,
                Properties = new()
                {
                    new SerializableValidatedProperty("Validated String", JTokenType.Array, _bindingValues, _exampleModifiers),
                    new SerializableProperty("Example Double", JTokenType.Float, _exampleModifiersWithDoubleDefault),
                    new SerializableProperty("Example String", JTokenType.String, _exampleModifiersWithStringDefault)
                }
            },
            new SerializablePlugin()
            {
                PluginName = "Mouse Button Binding",
                FullName = "OpenTabletDriver.Desktop.Binding.MouseBinding",
                Identifier = 2,
                Type = PluginType.Binding,
                Properties = new()
                {
                    new SerializableValidatedProperty("Button", JTokenType.Array, _mouseButtonValues, _exampleModifiers)
                }
            }
        ]);
    }

    


    #region Bindings

    public BindingDisplayViewModel LeftMouseClick { get; set; } = new("Tip Button", "Mouse Left Click", null!);
    public BindingDisplayViewModel RightMouseClick { get; set; } = new("Side Button", "Mouse Right Click", null!);

    public BindingDisplayViewModel AKeyboardKey { get; set; } = new("Auxiliary Button 1", "Key Binding A", null!);
    public BindingDisplayViewModel ZKeyboardKey { get; set; } = new("Auxiliary Button 2", "Key Binding Z", null!);

    #endregion

    #region Area Displays

    public AreaDisplayViewModel FullArea { get; set; } = new(_AvailableArea, _FullMappedArea);
    public AreaDisplayViewModel HalfArea { get; set; } = new(_AvailableArea, _HalfMappedArea);
    public AreaDisplayViewModel QuarterArea { get; set; } = new(_AvailableArea, _QuarterMappedArea);
    public AreaDisplayViewModel CustomArea { get; set; } = new();

    #endregion

    #region Plugin Settings

    public PropertySettingPair ExampleBoolSettingPair { get; set; } = new(_ExampleBoolProperty, _ExampleBoolSetting);
    public PropertySettingPair ExampleDoubleSettingPair { get; set; } = new(_ExampleDoubleProperty, _ExampleDoubleSetting);
    public PropertySettingPair ExampleIntSettingPair { get; set; } = new(_ExampleIntProperty, _ExampleIntSetting);
    public PropertySettingPair ExampleStringSettingPair { get; set; } = new(_ExampleStringProperty, _ExampleStringSetting);
    public PropertySettingPair ExampleValidatedStringSettingPair { get; set; } = new(_ExampleValidatedStringProperty, _ExampleValidatedStringSetting);
    public PropertySettingPair ExampleSliderSettingPair { get; set; } = new(_ExampleSliderProperty, _ExampleSliderSetting);
    public PropertySettingPair ExampleIntegerSliderSettingPair { get; set; } = new(_ExampleIntegerSliderProperty, _ExampleIntegerSliderSetting);

    #endregion

    #region Plugin Settings Store

    public SerializablePluginSettingsStore ExamplePluginSettingsStore { get; set; } = new()
    {
        PluginName = "Example Plugin",
        FullName = "Example.Plugin",
        Identifier = 1,
        Settings = [.. _ExampleSettings]
    };

    public PluginSettingStoreEditorViewModel ExamplePluginSettingsStoreEditor { get; set; } = new()
    {
        Properties = new(_ExampleProperties)
    };

    #endregion

    public static Area FullAreaModel => _FullMappedArea;
}
