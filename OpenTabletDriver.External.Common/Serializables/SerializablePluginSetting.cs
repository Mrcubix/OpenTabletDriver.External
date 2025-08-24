using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTabletDriver.External.Common.Enums;
using OpenTabletDriver.External.Common.Serializables.Properties;

namespace OpenTabletDriver.External.Common.Serializables
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SerializablePluginSettings
    {
        public SerializablePluginSettings()
        {
            Value = null!;
            Identifier = -1;
            Property = string.Empty;
        }

        public SerializablePluginSettings(string value, int identifier, string property = "")
        {
            Value = value;
            Identifier = identifier;
            Property = property;
        }

        public SerializablePluginSettings(string property, int identifier, JToken? value = null)
        {
            Identifier = identifier;
            Property = property;
            Value = value;
        }

        public SerializablePluginSettings(SerializableProperty property, int identifier, object? value = null)
        {
            Identifier = identifier;
            Property = property.Name;
            Value = value == null ? GetDefaultValue(property) : JToken.FromObject(value);
        }

        public SerializablePluginSettings(SerializableProperty property, int identifier, JToken? value = null)
        {
            Identifier = identifier;
            Property = property.Name;
            Value = value ?? GetDefaultValue(property);
        }

        public SerializablePluginSettings(SerializableProperty property, SerializablePlugin plugin, object? value = null)
        {
            Identifier = plugin.Identifier;
            Property = property.Name;
            Value = value == null ? GetDefaultValue(property) : JToken.FromObject(value);
        }

        /// <summary>
        ///   The identifier of the plugin.
        /// </summary>
        [JsonProperty]
        public int Identifier { get; set; }

        /// <summary>
        ///   The name of the property.
        /// </summary>
        [JsonProperty]
        public string Property { get; set; }

        /// <summary>
        ///   The value of the property.
        /// </summary>
        [JsonProperty]
        public JToken? Value { get; set; }

        [JsonIgnore]
        public bool HasValue => Value != null;

        private static JToken? GetDefaultValue(SerializableProperty property)
        {
            var defaultValueModifier = property.Modifiers.FirstOrDefault(p => p.Type == AttributeModifierType.DefaultValue);
            return defaultValueModifier == null || defaultValueModifier.Value == null ? null : JToken.FromObject(defaultValueModifier.Value);
        }
    }
}