using OpenTabletDriver.External.Common.Enums;

namespace OpenTabletDriver.External.Common.Serializables
{
    public class SerializableAttributeModifier(AttributeModifierType type, object? value)
    {
        public AttributeModifierType Type { get; } = type;
        public object? Value { get; } = value;
    }
}