using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenTabletDriver.External.Common.Serializables.Properties
{
    public class SerializableValidatedProperty : SerializableProperty
    {
        [JsonConstructor]
        private SerializableValidatedProperty() { }

        public SerializableValidatedProperty(string name, JTokenType type, System.Collections.IList values, IEnumerable<SerializableAttributeModifier> modifiers) 
            : base(name, type, modifiers)
        {
            Values = values;
        }

        public System.Collections.IList Values { get; set; } = Array.Empty<object>();
    }
}