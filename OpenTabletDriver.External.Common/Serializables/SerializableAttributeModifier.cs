using Newtonsoft.Json;
using OpenTabletDriver.External.Common.Enums;

namespace OpenTabletDriver.External.Common.Serializables
{
    public class SerializableAttributeModifier(AttributeModifierType type, object? value)
    {
        [JsonProperty]
        public AttributeModifierType Type { get; } = type;

        [JsonProperty]
        public object? Value { get; } = value;
    }
}