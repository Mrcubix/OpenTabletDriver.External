using Newtonsoft.Json;

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
        public string? Value { get; set; }

        [JsonIgnore]
        public bool HasValue => Value != null;
    }
}