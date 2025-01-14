using System;
using System.Collections.Generic;
using NUnit.Framework;
using JavaUtilities;

namespace JavaUtilitiesTests{
    [TestFixture]
    public class DictUtilsTests{
        [Test]
        public void CreateDict(){
            int[] keys = [1, 2, 3];
            string[] values = ["one", "two", "three"];
            Dictionary<int, string> result = DictUtils.CreateDict(keys, values);
            Assert.Multiple(() => {
                Assert.AreEqual(3, result.Count);
                Assert.Contains(new KeyValuePair<int, string>(1, "one"), result);
                Assert.Contains(new KeyValuePair<int, string>(2, "two"), result);
                Assert.Contains(new KeyValuePair<int, string>(3, "three"), result);
            });
        }

        [Test]
        public void CreateDictDifferentLengths(){
            int[] keys = [1, 2, 3, 4, 5];
            string[] values = ["one", "two", "three"];
            Dictionary<int, string> result = DictUtils.CreateDict(keys, values);
            Assert.Multiple(() => {
                Assert.AreEqual(3, result.Count);
                Assert.Contains(new KeyValuePair<int, string>(1, "one"), result);
                Assert.Contains(new KeyValuePair<int, string>(2, "two"), result);
                Assert.Contains(new KeyValuePair<int, string>(3, "three"), result);
            });
        }

        //There is no need to test PopulateDict's standard behaviour, as it is always invoked by CreateDict
        [Test]
        public void PopulateDictDuplicateKey(){
            Dictionary<int, string> dict = new(){
                {1, "one"},
                {2, "two"},
                {3, "three"}
            };
            Assert.Throws<ArgumentException>(() => DictUtils.PopulateDict([3, 4], ["three", "four"], dict));
        }

        [Test]
        public void ContentsHashCode(){
            Dictionary<int, string> dict = new(){
                {1, "one"},
                {2, "two"},
                {3, "three"}
            };
            Dictionary<int, string> otherDict = new(){
                {1, "one"},
                {2, "two"},
                {3, "three"}
            };
            int expected = DictUtils.ContentsHashCode(dict);
            int result = DictUtils.ContentsHashCode(otherDict);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ContentsHashCodeNull(){
            int result = DictUtils.ContentsHashCode<int, string>(null);
            Assert.Zero(result);
        }
    }
}
