using JsonNet.PrivateSettersContractResolvers;  // Install-Package JsonNet.PrivateSettersContractResolvers.Source
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Posy.V2.WF.Helpers
{
    public static class FuncaoJson
    {
        public static object CopyObj(Object obj)
        {
            return CloneObject(obj);
        }

        private static T CloneObject<T>(T obj)
        {
            if (obj == null) return obj;

            #region OUTRAS CONFIGS

            //var jsonSerializerSettings = new JsonSerializerSettings
            //{
            //    //NullValueHandling = NullValueHandling.Ignore,
            //    //MissingMemberHandling = MissingMemberHandling.Ignore,
            //    //PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //    //TypeNameHandling = TypeNameHandling.Auto,
            //    //TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            //    Converters = new[] { new ByteArrayConverter() }
            //    //MetadataPropertyHandling = MetadataPropertyHandling.Ignore
            //};

            #endregion

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                //TypeNameHandling = TypeNameHandling.Objects,
                //Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateSetterContractResolver() // Install-Package JsonNet.PrivateSettersContractResolvers.Source
            };

            var serialized = JsonConvert.SerializeObject(obj, settings);
            //var deserialized = JsonConvert.DeserializeObject<T>(serialized, settings);
            var deserialized = (T)JsonConvert.DeserializeObject(serialized, obj.GetType(), settings);

            return deserialized;
        }

        public static T CloneJson<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}