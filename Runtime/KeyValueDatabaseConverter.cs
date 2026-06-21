using System;
using CupkekGames.KeyValueDatabases;
using Newtonsoft.Json;

namespace CupkekGames.KeyValueDatabases.Newtonsoft
{
    /// <summary>
    /// Newtonsoft converter for any <see cref="KeyValueDatabase{TKey,TValue}"/>. Serializes the database
    /// as its list of pairs (via the public <c>Pairs</c> property), so the core KeyValueDatabase type
    /// needs no Newtonsoft awareness — mirroring the Data &lt;-&gt; Newtonsoft bridge pattern.
    /// <para>
    /// Register it with the <c>SerializationManager</c> through
    /// <see cref="KeyValueDatabasesSerializationTypeProviderSO"/> (added to a SerializationManagerRegistrar).
    /// Handles the open generic, so every closed <c>KeyValueDatabase&lt;,&gt;</c> is covered with no
    /// per-type registration.
    /// </para>
    /// </summary>
    public class KeyValueDatabaseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType
                && objectType.GetGenericTypeDefinition() == typeof(KeyValueDatabase<,>);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            // The dictionary is a derived runtime cache; the pairs list is the source of truth.
            object pairs = value.GetType()
                .GetProperty(nameof(KeyValueDatabase<object, object>.Pairs))
                .GetValue(value);
            serializer.Serialize(writer, pairs);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object instance = existingValue ?? Activator.CreateInstance(objectType);
            var pairsProperty = objectType.GetProperty(nameof(KeyValueDatabase<object, object>.Pairs));

            if (reader.TokenType != JsonToken.Null)
            {
                object pairs = serializer.Deserialize(reader, pairsProperty.PropertyType);
                pairsProperty.SetValue(instance, pairs); // setter invalidates the cache; rebuilt on next access
            }

            return instance;
        }
    }
}
