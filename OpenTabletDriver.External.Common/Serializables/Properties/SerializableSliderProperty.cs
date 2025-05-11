using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenTabletDriver.External.Common.Serializables.Properties
{
    public class SerializableSliderProperty : SerializableProperty
    {
        [JsonConstructor]
        private SerializableSliderProperty() { }

        public SerializableSliderProperty(string name, JTokenType type, IEnumerable<SerializableAttributeModifier> modifiers) : base(name, type, modifiers) { }

        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double DefaultValue { get; set; }
    }
}