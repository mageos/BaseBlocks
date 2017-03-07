using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BaseBlocks
{
    /// <summary>
    /// Base class for creating classes that act like an enum in many ways but can contain additional properties.  Sub classes of this
    /// type should be marked sealed and have private contructors to achieve the best results.
    /// </summary>
    /// <typeparam name="TKey">The type of the Key value for each item</typeparam>
    /// <typeparam name="TEnum">The type of class that is deriving from this</typeparam>    
    public abstract partial class SuperEnum<TKey, TEnum> : IComparable, IComparable<TEnum> where TEnum : SuperEnum<TKey, TEnum> where TKey : IComparable
    {
        /// <summary>
        /// Used to link instances to a key
        /// </summary>
        private static Dictionary<TKey, TEnum> values = new Dictionary<TKey, TEnum>();

        protected SuperEnum(TKey key)
        {            
            if (values.ContainsKey(key))
            {
                throw new ArgumentException("A Key with that value already exists", nameof(key));
            }

            Key = key;

            values.Add(Key, (TEnum)this);
        }

        public TKey Key { get; private set; }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var objAsEnum = obj as TEnum;
            if (objAsEnum == null)
            {
                return false;
            }

            return Key.Equals(objAsEnum.Key);
        }

        public int CompareTo(TEnum other)
        {
            return Key.CompareTo(other.Key);
        }

        public int CompareTo(object obj)
        {
            var objAsEnum = obj as TEnum;
            if (objAsEnum != null)
            {
                return Key.CompareTo(objAsEnum.Key);
            }

            return Key.CompareTo(obj);
        }
    }
}
