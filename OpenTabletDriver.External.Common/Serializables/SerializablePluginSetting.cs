namespace OpenTabletDriver.External.Common.Serializables
{
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

        public int Identifier { get; set; }
        public string? Value { get; set; }
    }
}