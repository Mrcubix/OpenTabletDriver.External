using Newtonsoft.Json;

namespace OpenTabletDriver.External.Common.Serializables;

public class SerializablePlugin
{
    public SerializablePlugin()
    {
        PluginName = "Not Set";
        FullName = "";
        Identifier = -1;
        ValidProperties = new string[0];
    }

    public SerializablePlugin(string? pluginName, string? fullName, int identifier, string[] validProperties)
    {
        PluginName = pluginName;
        FullName = fullName;
        Identifier = identifier;
        ValidProperties = validProperties;
    }

    [JsonProperty("PluginName")]
    public string? PluginName { get; set; }

    [JsonProperty("FullName")]
    public string? FullName { get; set; }

    [JsonProperty("Identifier")]
    public int Identifier { get; set; }

    [JsonProperty("ValidProperties")]
    public string[] ValidProperties { get; set; }
}