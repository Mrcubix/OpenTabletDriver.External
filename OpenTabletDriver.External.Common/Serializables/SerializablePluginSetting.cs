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
        }

        public SerializablePluginSettings(string value, int identifier)
        {
            Value = value;
            Identifier = identifier;
        }

        [JsonProperty]
        public int Identifier { get; set; }

        [JsonProperty]
        public string? Value { get; set; }
    }
}