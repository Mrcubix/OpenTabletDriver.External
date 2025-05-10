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

        public string Name { get; set; } = string.Empty;
        public JTokenType Type { get; set; }
        public IEnumerable<SerializableAttributeModifier> Modifiers { get; set; } = [];
    }
}