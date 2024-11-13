using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class SerializablePluginSettingsStore
    {
        public SerializablePluginSettingsStore()
        {
            PluginName = "Not Set";
            FullName = string.Empty;
            Identifier = -1;
            Settings = new ObservableCollection<SerializablePluginSettings>();
        }

        public SerializablePluginSettingsStore(string? pluginName, string? fullName, int identifier, 
                                               ObservableCollection<SerializablePluginSettings> settings)
        {
            PluginName = pluginName;
            FullName = fullName;
            Identifier = identifier;
            Settings = settings;
        }

        public SerializablePluginSettingsStore(string? pluginName, string? fullName, int identifier, IList<SerializablePluginSettings> settings)
            : this(pluginName, fullName, identifier, new ObservableCollection<SerializablePluginSettings>(settings)) { }

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
        ///   A collection of property names and their values.<br/>
        ///   Represent the settings of the plugin.
        /// </summary>
        [JsonProperty("Settings")]
        public ObservableCollection<SerializablePluginSettings> Settings { get; set; }
    }
}