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

        public SerializableValidatedProperty(string name, JTokenType type, System.Collections.IEnumerable values, IEnumerable<SerializableAttributeModifier> modifiers) 
            : base(name, type, modifiers)
        {
            Values = values;
        }

        public System.Collections.IEnumerable Values { get; set; } = Array.Empty<object>();
    }
}