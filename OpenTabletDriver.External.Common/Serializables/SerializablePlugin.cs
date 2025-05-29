using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using OpenTabletDriver.External.Common.Enums;
using OpenTabletDriver.External.Common.Serializables.Properties;

namespace OpenTabletDriver.External.Common.Serializables
{
    /// <summary>
    ///   A serializable plugin.
    /// </summary>
    /// <remarks>
    ///   This is made with bindings in mind.
    /// </remarks>
    public class SerializablePlugin
    {
        public SerializablePlugin()
        {
            PluginName = "Not Set";
            FullName = string.Empty;
            Identifier = -1;
            Properties = new();
        }

        [Obsolete("Support for multiple properties has been added, use the constructor with properties instead.")]
        public SerializablePlugin(string? pluginName, string? fullName, int identifier, string[] validProperties, string property = "")
        {
            PluginName = pluginName;
            FullName = fullName;
            Identifier = identifier;
            Properties = new();
            ValidProperties = validProperties;
            Property = property;
        }

        public SerializablePlugin(string? pluginName, string? fullName, int identifier, IEnumerable<SerializableProperty> properties)
        {
            PluginName = pluginName;
            FullName = fullName;
            Identifier = identifier;
            Properties = new(properties);
        }

        /// <summary>
        ///   The display name of the plugin.
        /// </summary>
        [JsonProperty("PluginName")]
        public string? PluginName { get; set; }

        /// <summary>
        ///   The full name of the plugin.
        /// </summary>
        [JsonProperty("FullName")]
        public string? FullName { get; set; }

        /// <summary>
        ///   The identifier of the plugin.
        /// </summary>
        [JsonProperty("Identifier")]
        public int Identifier { get; set; }

        /// <summary>
        ///   The Plugin's Type
        /// </summary>
        [JsonProperty("Type")]
        public PluginType Type { get; set; }

        /// <summary>
        ///   The Properties of the plugin.
        /// </summary>
        [JsonProperty("Properties")]
        public ObservableCollection<SerializableProperty> Properties { get; set; }

        /// <summary>
        ///   The valid values for a property.
        /// </summary>
        [Obsolete("Support for multiple properties has been added, use Properties instead.")]
        [JsonProperty("ValidProperties")]
        public string[] ValidProperties { get; set; } = Array.Empty<string>();

        /// <summary>
        ///   The name of the property.
        /// </summary>
        [JsonProperty("Property")]
        [Obsolete("Support for multiple properties has been added, use Properties instead.")]
        public string Property { get; set; } = string.Empty;

        public override string ToString() => PluginName ?? FullName ?? Identifier.ToString();
    }
}