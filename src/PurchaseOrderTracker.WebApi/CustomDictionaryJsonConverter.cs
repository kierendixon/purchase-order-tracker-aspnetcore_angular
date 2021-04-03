using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PurchaseOrderTracker.WebApi
{
    // System.Text.Json doesn't support serialisation of Dictionary objects with a non-string key in .Net Core 3.1 (it's only available in .NET 5)
    // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-core-3-1#support-dictionary-with-non-string-key
    // https://makolyte.com/system-text-json-cant-serialize-dictionary-unless-it-has-a-string-key/
    public class CustomDictionaryJsonConverter<TKey, TValue> : JsonConverter<IDictionary<TKey, TValue>> where TKey : IConvertible
    {
        public override bool CanConvert(Type typeToConvert)
        {
            /* Only use this converter if 
             * 1. It's a dictionary
             * 2. The key is not a string
             */
            if (typeToConvert != typeof(Dictionary<TKey, TValue>))
            {
                return false;
            }
            else if (typeToConvert.GenericTypeArguments.First() == typeof(string))
            {
                return false;
            }
            return true;
        }
        public override IDictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //Step 1 - Use built-in serializer to deserialize into a dictionary with string key
            var dictionaryWithStringKey = (Dictionary<string, TValue>)JsonSerializer.Deserialize(ref reader, typeof(Dictionary<string, TValue>), options);


            //Step 2 - Convert the dictionary to one that uses the actual key type we want
            var dictionary = new Dictionary<TKey, TValue>();

            foreach (var kvp in dictionaryWithStringKey)
            {
                dictionary.Add((TKey)Convert.ChangeType(kvp.Key, typeof(TKey)), kvp.Value);
            }

            return dictionary;
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<TKey, TValue> value, JsonSerializerOptions options)
        {
            //Step 1 - Convert dictionary to a dictionary with string key
            var dictionary = new Dictionary<string, TValue>(value.Count);

            foreach (var kvp in value)
            {
                dictionary.Add(kvp.Key.ToString(), kvp.Value);
            }
            //Step 2 - Use the built-in serializer, because it can handle dictionaries with string keys
            JsonSerializer.Serialize(writer, dictionary, options);

        }
    }
}
