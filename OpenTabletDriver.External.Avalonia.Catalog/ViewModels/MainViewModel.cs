using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using OpenTabletDriver.External.Avalonia.Models;
using OpenTabletDriver.External.Avalonia.ViewModels;
using OpenTabletDriver.External.Common.Enums;
using OpenTabletDriver.External.Common.Serializables;
using OpenTabletDriver.External.Common.Serializables.Properties;
using ReactiveUI;

namespace OpenTabletDriver.External.Avalonia.Catalog.ViewModels;

public class MainViewModel : ReactiveObject
{
    #region Constants

    #region Areas

    private static readonly Area _AvailableArea = new(0, 0, 152.0, 95.0);
    private static readonly Area _FullMappedArea = new(76, 47.5, 152.0, 95.0, true);
    private static readonly Area _HalfMappedArea = new(76, 47.5, 76, 47.5, true);
    private static readonly Area _QuarterMappedArea = new(76, 47.5, 38, 23.75, true);

    #endregion

    #region Properties Attributes Values

    private static readonly IEnumerable<string> _Choices = new[] { "Choice 1", "Choice 2", "Choice 3" };

    private static readonly IEnumerable<SerializableAttributeModifier> _ExampleModifiers = new[]
    {
        new SerializableAttributeModifier(AttributeModifierType.Tooltip, "Tooltip Here"),
        new SerializableAttributeModifier(AttributeModifierType.Unit, "ms"),
    };

    #endregion

    #region Properties

    private static readonly SerializableProperty _ExampleBoolProperty = new("Example Bool", JTokenType.Boolean, _ExampleModifiers);
    private static readonly SerializableProperty _ExampleDoubleProperty = new("Example Float", JTokenType.Float, _ExampleModifiers);
    private static readonly SerializableProperty _ExampleIntProperty = new("Example Int", JTokenType.Integer, _ExampleModifiers);
    private static readonly SerializableProperty _ExampleStringProperty = new("Example String", JTokenType.String, _ExampleModifiers);
    private static readonly SerializableValidatedProperty _ExampleValidatedStringProperty = new("Example Validated String", JTokenType.Array, _Choices, _ExampleModifiers);

    private static readonly SerializableProperty[] _ExampleProperties =
    [
        _ExampleBoolProperty,
        _ExampleDoubleProperty,
        _ExampleIntProperty,
        _ExampleStringProperty,
        _ExampleValidatedStringProperty
    ];

    #endregion

    #region Settings

    private static readonly SerializablePluginSettings _ExampleBoolSetting = new(true, 1, _ExampleBoolProperty);
    private static readonly SerializablePluginSettings _ExampleDoubleSetting = new(0.5d, 1, _ExampleDoubleProperty);
    private static readonly SerializablePluginSettings _ExampleIntSetting = new(42, 1, _ExampleIntProperty);
    private static readonly SerializablePluginSettings _ExampleStringSetting = new("Hello World", 1, _ExampleStringProperty);
    private static readonly SerializablePluginSettings _ExampleValidatedStringSetting = new("Choice 1", 1, _ExampleValidatedStringProperty);

    private static readonly SerializablePluginSettings[] _ExampleSettings =
    [
        _ExampleBoolSetting,
        _ExampleDoubleSetting,
        _ExampleIntSetting,
        _ExampleStringSetting,
        _ExampleValidatedStringSetting
    ];

    #endregion

    #endregion

    public MainViewModel() 
    {
        ExamplePluginSettingsStoreEditor.Store = ExamplePluginSettingsStore;
        ExamplePluginSettingsStoreEditor.Properties = _ExampleProperties;
    }

    #region Bindings

    public BindingDisplayViewModel LeftMouseClick { get; set; } = new("Tip Button", "Mouse Left Click", null);
    public BindingDisplayViewModel RightMouseClick { get; set; } = new("Side Button", "Mouse Right Click", null);

    public BindingDisplayViewModel AKeyboardKey { get; set; } = new("Auxiliary Button 1", "Key Binding A", null);
    public BindingDisplayViewModel ZKeyboardKey { get; set; } = new("Auxiliary Button 2", "Key Binding Z", null);

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

    #endregion

    #region Plugin Settings Store

    public SerializablePluginSettingsStore ExamplePluginSettingsStore { get; set; } = new()
    {
        PluginName = "Example Plugin",
        FullName = "Example.Plugin",
        Identifier = 1,
        Settings = [.. _ExampleSettings]
    };

    public PluginSettingStoreEditorViewModel ExamplePluginSettingsStoreEditor { get; set; } = new();

    #endregion

    public static Area FullAreaModel => _FullMappedArea;
}
