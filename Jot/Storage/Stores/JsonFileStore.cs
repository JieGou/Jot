﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jot.Storage.Stores
{
    public class JsonFileStore : PersistentStoreBase
    {
        #region custom serialization (for object type handling)
        private class StoreItemConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(StoreItem);
            }

            public override bool CanRead
            {
                get { return true; }
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                reader.Read();//read "Type" attribute name
                reader.Read();//read "Type" attribute value
                Type t = serializer.Deserialize<Type>(reader);

                var x = reader.Read();//read "Name" attribute name
                var name = reader.ReadAsString();//read "Name" attribute value

                reader.Read();//read "Value" attribute name
                reader.Read();//read "value" attribute value
                var res = serializer.Deserialize(reader, t);

                reader.Read();//position to next item

                return new StoreItem() { Name = name, Type = t, Value = res };
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                //nothing fancy, standard serialization
                var converters = serializer.Converters.ToArray();
                var jObject = JObject.FromObject(value);
                jObject.WriteTo(writer, converters);
            }
        }

        private class StoreItem
        {
            [JsonProperty(Order = 1)]
            public Type Type { get; set; }
            [JsonProperty(Order = 2)]
            public string Name { get; set; }
            [JsonProperty(Order = 3)]
            public object Value { get; set; }
        }
        #endregion

        public string FilePath { get; private set; }

        public JsonFileStore(string filePath)
        {
            FilePath = filePath;
        }

        protected override Dictionary<string, object> LoadValues()
        {
            List<StoreItem> storeItems = null;
            if (File.Exists(FilePath))
            {
                try
                {
                    storeItems = JsonConvert.DeserializeObject<List<StoreItem>>(File.ReadAllText(FilePath), new StoreItemConverter());
                }
                catch { }
            }

            if (storeItems == null)
                storeItems = new List<StoreItem>();

            return storeItems.ToDictionary(item => item.Name, item => item.Value);
        }

        protected override void SaveValues(Dictionary<string, object> _values)
        {
            var list = _values.Select(kvp => new StoreItem() { Name = kvp.Key, Value = kvp.Value, Type = kvp.Value?.GetType() });
            string serialized = JsonConvert.SerializeObject(list, new JsonSerializerSettings() { Formatting = Formatting.Indented, TypeNameHandling=TypeNameHandling.None });

            string directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(FilePath, serialized);
        }
    }
}