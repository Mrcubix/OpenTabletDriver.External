using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public SerializablePluginSettings(object? value, int identifier, SerializableProperty property)
        {
            Identifier = identifier;
            Property = property.Name;
            Value = value == null ? null : JToken.FromObject(value);
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
    }
}