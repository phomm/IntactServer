using System.Text.Json;
using System.Text.Json.Serialization;
using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Converters;

public class JsonPrefixEnumConverter<T>: JsonConverter<T> where T : Enum
{
    private static readonly Dictionary<Type, string> _prefixes = new()
    {
        {typeof(SpellKind), "sk"},
        {typeof(SpellTargetType), "st"},
        {typeof(SpellUseType), "su"},
    };
    public override bool CanConvert(Type typeToConvert) => typeof(T).IsEnum && _prefixes.ContainsKey(typeToConvert);

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // TODO:
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var key = _prefixes[value.GetType()] + value;
        writer.WriteStringValue(key);
    }
}