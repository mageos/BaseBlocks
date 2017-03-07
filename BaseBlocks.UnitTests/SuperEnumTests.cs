using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Xunit;

namespace BaseBlocks.UnitTests
{
    [Serializable]
    public sealed class TestEnum : SuperEnum<int, TestEnum>
    {
        public TestEnum(int key, string shortName, string longName) : base(key)
        {
            ShortName = shortName;
            LongName = longName;
        }

        public string ShortName
        {
            get;
        }

        public string LongName
        {
            get;
        }

        public static readonly TestEnum TestOne = new TestEnum(0, "Test0", "Test Number One");

        public static readonly TestEnum TestTwo = new TestEnum(1, "Test1", "Test Number Two");

        public static readonly TestEnum TestThree = new TestEnum(2, "Test2", "Test Number Three");
    }
    
    public class SuperEnumTests
    {
        private static IEnumerable<object[]> EqualityData(bool includeNulls)
        {
            object[] DataSet(params object[] data) => data;

            yield return DataSet(TestEnum.TestOne, TestEnum.TestOne, true);
            yield return DataSet(TestEnum.TestOne, TestEnum.TestTwo, false);
            yield return DataSet(TestEnum.TestOne, null, false);
            if (includeNulls)
            {
                yield return DataSet(null, null, true);
            }
        }

        [Theory]
        [MemberData(nameof(EqualityData), true)]
        public void SuperEnum_EqualOperator(TestEnum a, TestEnum b, bool expectedResult)
        {
            Assert.Equal(expectedResult, a == b);            
        }

        [Theory]
        [MemberData(nameof(EqualityData), false)]
        public void SuperEnum_EqualsMethod(TestEnum a, TestEnum b, bool expectedResult)
        {
            Assert.Equal(expectedResult, a.Equals(b));
        }

        [Fact]
        public void SuperEnum_ThrowsException_OnDuplicateKey()
        {
            var test = Record.Exception(() => new TestEnum(0, string.Empty, string.Empty));

            Assert.IsType<ArgumentException>(test);
        }

        [Fact]
        public void SuperEnum_Serialization_Success()
        {
            MemoryStream ms = new MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(ms, TestEnum.TestThree);
            formatter.Serialize(ms, TestEnum.TestTwo);
            formatter.Serialize(ms, TestEnum.TestOne);

            ms.Flush();

            ms.Seek(0, SeekOrigin.Begin);

            var testThree = (TestEnum)formatter.Deserialize(ms);
            var testTwo = (TestEnum)formatter.Deserialize(ms);
            var testOne = (TestEnum)formatter.Deserialize(ms);

            Assert.Equal(TestEnum.TestThree, testThree);
            Assert.Equal(TestEnum.TestTwo, testTwo);
            Assert.Equal(TestEnum.TestOne, testOne);
        }
    }
}
