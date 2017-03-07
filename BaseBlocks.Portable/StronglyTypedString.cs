using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBlocks
{
    public abstract class StronglyTypedString<T> : IComparable,
        IEnumerable, IComparable<T>, IEnumerable<char>,
        IEquatable<T> where T : StronglyTypedString<T>
    {
        public StronglyTypedString(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            return Value.Equals(obj as T);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as T);
        }

        public int CompareTo(T other)
        {
            return Value.CompareTo(other?.Value);
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < Value.Length; i++)
            {
                yield return Value[i];
            }
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            for (int i=0; i < Value.Length; i++)
            {
                yield return Value[i];
            }
        }

        public bool Equals(T other)
        {
            return Value.Equals(other?.Value);
        }

        public static implicit operator string(StronglyTypedString<T> v)
        {
            return v.Value;
        }
    }
}
