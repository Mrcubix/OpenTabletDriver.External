using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenTabletDriver.External.Common.Serializables.Properties
{
    public class SerializableProperty
    {
        [JsonConstructor]
        protected SerializableProperty() { }

        public SerializableProperty(string name, JTokenType type, IEnumerable<SerializableAttributeModifier> modifiers)
        {
            Name = name;
            Type = type;
            Modifiers = modifiers;
        }

        [JsonProperty]
        public string Name { get; set; } = string.Empty;

        [JsonProperty]
        public JTokenType Type { get; set; }

        [JsonProperty]
        public IEnumerable<SerializableAttributeModifier> Modifiers { get; set; } = [];
    }
}