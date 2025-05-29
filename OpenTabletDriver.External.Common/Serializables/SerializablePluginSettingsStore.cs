using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using OpenTabletDriver.External.Common.Extensions;
using OpenTabletDriver.External.Common.Serializables.Properties;

namespace OpenTabletDriver.External.Common.Serializables
{
    /// <summary>
    ///   A serializable plugin.
    /// </summary>
    /// <remarks>
    ///   This is made with bindings in mind.
    /// </remarks>
    public class SerializablePluginSettingsStore
    {
        public SerializablePluginSettingsStore()
        {
            PluginName = "Not Set";
            FullName = string.Empty;
            Identifier = -1;
            Settings = new ObservableCollection<SerializablePluginSettings>();
        }

        public SerializablePluginSettingsStore(string? pluginName, string? fullName, int identifier)
        {
            PluginName = pluginName;
            FullName = fullName;
            Identifier = identifier;
            Settings = new ObservableCollection<SerializablePluginSettings>();
        }

        public SerializablePluginSettingsStore(string? pluginName, string? fullName, int identifier, 
                                               ObservableCollection<SerializablePluginSettings> settings) : this(pluginName, fullName, identifier)
        {
            Settings = settings;
        }

        public SerializablePluginSettingsStore(string? pluginName, string? fullName, int identifier, IList<SerializablePluginSettings> settings)
            : this(pluginName, fullName, identifier) 
        { 
            Settings = new ObservableCollection<SerializablePluginSettings>(settings);
        }

        public SerializablePluginSettingsStore(SerializablePlugin plugin) : this(plugin.PluginName, plugin.FullName, plugin.Identifier) 
        { 
            Settings = plugin != null ? GetSettingsForType(plugin) : new ObservableCollection<SerializablePluginSettings>();
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
        ///   The value of the property.
        /// </summary>
        [Obsolete]
        [JsonProperty]
        public string? Value { get; set; }

        /// <summary>
        ///   A collection of property names and their values.<br/>
        ///   Represent the settings of the plugin.
        /// </summary>
        [JsonProperty("Settings")]
        public ObservableCollection<SerializablePluginSettings> Settings { get; set; }

        private static ObservableCollection<SerializablePluginSettings> GetSettingsForType(SerializablePlugin plugin)
        {
            var settings = plugin.Properties.Select(property => new SerializablePluginSettings(property, plugin));
            return new ObservableCollection<SerializablePluginSettings>(settings);
        }

        public SerializablePluginSettings this[string name]
        {
            get
            {
                var setting = Settings.FirstOrDefault(p => p.Property == name);

                if (setting == null)
                {
                    setting = new SerializablePluginSettings(string.Empty, -1, name);
                    Settings.Add(setting);
                }

                return setting;
            }
            set
            {
                // If the setting already exists, update it
                if (Settings.FirstOrDefault(p => p.Property == value.Property) is SerializablePluginSettings setting)
                    Settings.Replace(setting, value);
                else
                    Settings.Add(value);
            }
        }

        public SerializablePluginSettings this[SerializableProperty property]
        {
            get => this[property.Name];
            set => this[property.Name] = value;
        }

        public string GetHumanReadableString()
        {
            var name = PluginName ?? FullName ?? Identifier.ToString();
            string settings = string.Join(", ", this.Settings.Select(s => $"({s.Property}: {s.Value})"));
            string suffix = Settings.Any() ? $": {settings}" : string.Empty;
            return name + suffix;
        }

        public override string ToString() => GetHumanReadableString();
    }
}