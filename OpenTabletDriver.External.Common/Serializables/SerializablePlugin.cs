using Newtonsoft.Json;

namespace OpenTabletDriver.External.Common.Serializables
{
    /// <summary>
    ///   A serializable plugin.
    /// </summary>
    /// <remarks>
    ///   This is made with bindings in mind.
    /// </remarks>
    /// TODO: Implement SerializablePluginStore as well as a way to serialize properties and their types.
    public class SerializablePlugin
    {
        public SerializablePlugin()
        {
            PluginName = "Not Set";
            FullName = string.Empty;
            Identifier = -1;
            ValidProperties = new string[0];
            Property = string.Empty;
        }

        public SerializablePlugin(string? pluginName, string? fullName, int identifier, string[] validProperties, string property = "")
        {
            PluginName = pluginName;
            FullName = fullName;
            Identifier = identifier;
            ValidProperties = validProperties;
            Property = property;
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
        ///   The valid values for a property.
        /// </summary>
        [JsonProperty("ValidProperties")]
        public string[] ValidProperties { get; set; }

        /// <summary>
        ///   The name of the property.
        /// </summary>
        [JsonProperty("Property")]
        public string Property { get; set; }
    }
}