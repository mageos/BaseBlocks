using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BaseBlocks
{
    partial class SuperEnum<TKey, TEnum> : ISerializable
    {       
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(typeof(SuperEnumObjectRefManager<TKey, TEnum>));
            info.AddValue(nameof(Key), Key);
        }

        private static TEnum GetValue(TKey key)
        {
            return values[key];
        }

        /// <summary>
        /// Serialization Helper to make sure that the object deserialize correctly
        /// </summary>
        /// <typeparam name="TKey2">The key type for the enum</typeparam>
        /// <typeparam name="TEnum2">The value of the Enum</typeparam>
        [Serializable]
        class SuperEnumObjectRefManager<TKey2, TEnum2> : IObjectReference, ISerializable where TEnum2 : SuperEnum<TKey2, TEnum2> where TKey2 : IComparable
        {
            private TKey2 key;

            public SuperEnumObjectRefManager(SerializationInfo info, StreamingContext context)
            {
                key = (TKey2)info.GetValue("Key", typeof(TKey2));
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
            }

            public object GetRealObject(StreamingContext context)
            {
                return SuperEnum<TKey2, TEnum2>.GetValue(key);
            }
        }
    }    
}
