using System;
using System.Collections.Generic;
using NUnit.Framework;
using JavaUtilities.Collections;

namespace JavaUtilitiesTests.Collections{
    [TestFixture]
    public class BiDictionaryTests{
        private readonly Dictionary<string, int> testDictContents = new(){
            {"one", 1},
            {"two", 2},
            {"three", 3}
        };
        private BiDictionary<string, int> testDict = null!;


        [SetUp]
        public void SetUp(){
            testDict = new(testDictContents);
        }

        [Test]
        public void GetSetByValue(){
            testDict[1] = "won";
            Assert.AreEqual("won", testDict[1]);
        }

        [Test]
        public void GetSetByKey(){
            testDict["one"] = 0;
            Assert.AreEqual(0, testDict["one"]);

        }

        [Test]
        public void Keys(){
            ICollection<string> result = testDict.Keys;
            Assert.Multiple(() => {
                Assert.AreEqual(3, result.Count);
                Assert.IsTrue(result.Contains("one"));
                Assert.IsTrue(result.Contains("two"));
                Assert.IsTrue(result.Contains("three"));
            });
        }

        [Test]
        public void Values(){
            ICollection<int> result = testDict.Values;
            Assert.Multiple(() => {
                Assert.AreEqual(3, result.Count);
                Assert.IsTrue(result.Contains(1));
                Assert.IsTrue(result.Contains(2));
                Assert.IsTrue(result.Contains(3));
            });
        }

        [Test]
        public void Add(){
            testDict.Add("four", 4);
            Assert.Multiple(() => {
                Assert.AreEqual(4, testDict.Count);
                Assert.AreEqual(4, testDict["four"]);
                Assert.AreEqual("four", testDict[4]);
            });
        }

        [Test]
        public void AddDuplicateKey(){
            Assert.Throws<ArgumentException>(() => testDict.Add("one", 0));
        }

        [Test]
        public void AddDuplicateValue(){
            Assert.Throws<ArgumentException>(() => testDict.Add("zero", 1));
        }

        [Test]
        public void AddPair(){
            testDict.Add(KeyValuePair.Create("four", 4));
            Assert.Multiple(() => {
                Assert.AreEqual(4, testDict.Count);
                Assert.AreEqual(4, testDict["four"]);
                Assert.AreEqual("four", testDict[4]);
            });
        }

        [Test]
        public void AddPairDuplicateKey(){
            Assert.Throws<ArgumentException>(() => testDict.Add(KeyValuePair.Create("one", 0)));
        }

        [Test]
        public void AddPairDuplicateValue(){
            Assert.Throws<ArgumentException>(() => testDict.Add(KeyValuePair.Create("zero", 1)));
        }

        [Test]
        public void Contains(){
            Assert.IsTrue(testDict.Contains(new KeyValuePair<string, int>("one", 1)));
        }

        [Test]
        public void ContainsNotPresent(){
            Assert.IsFalse(testDict.Contains(new KeyValuePair<string, int>("zero", 0)));
        }

        [Test]
        public void ContainsKey(){
            Assert.IsTrue(testDict.ContainsKey("one"));
        }

        [Test]
        public void ContainsKeyNotPresent(){
            Assert.IsFalse(testDict.ContainsKey("zero"));
        }

        [Test]
        public void ContainsValue(){
            Assert.IsTrue(testDict.ContainsValue(1));
        }

        [Test]
        public void ContainsValueNotPresent(){
            Assert.IsFalse(testDict.ContainsValue(0));
        }

        [Test]
        public void TryGetKey(){
            bool result = testDict.TryGetKey(1, out string resultValue);
            Assert.Multiple(() => {
                Assert.IsTrue(result);
                Assert.AreEqual("one", resultValue);
            });
        }

        [Test]
        public void TryGetKeyNotPresent(){
            bool result = testDict.TryGetKey(0, out string resultValue);
            Assert.Multiple(() => {
                Assert.IsFalse(result);
                Assert.IsNull(resultValue);
            });
        }

        [Test]
        public void TryGetValue(){
            bool result = testDict.TryGetValue("one", out int resultValue);
            Assert.Multiple(() => {
                Assert.IsTrue(result);
                Assert.AreEqual(1, resultValue);
            });
        }

        [Test]
        public void TryGetValueNotPresent(){
            bool result = testDict.TryGetValue("zero", out int resultValue);
            Assert.Multiple(() => {
                Assert.IsFalse(result);
                Assert.AreEqual(default(int), resultValue);
            });
        }

        [Test]
        public void CopyTo(){
            KeyValuePair<string, int>[] pairs = new KeyValuePair<string, int>[testDict.Count];
            testDict.CopyTo(pairs, 0);
            Assert.Multiple(() => {
                Assert.AreEqual("one", pairs[0].Key);
                Assert.AreEqual(1, pairs[0].Value);
                Assert.AreEqual("two", pairs[1].Key);
                Assert.AreEqual(2, pairs[1].Value);
                Assert.AreEqual("three", pairs[2].Key);
                Assert.AreEqual(3, pairs[2].Value);
            });
        }

        [Test]
        public void RemovePair(){
            KeyValuePair<string, int> pair = new("one", 1);
            bool result = testDict.Remove(pair);
            Assert.Multiple(() => {
                Assert.IsTrue(result);
                Assert.IsFalse(testDict.Contains(pair));
            });
        }

        [Test]
        public void RemovePairNotPresent(){
            KeyValuePair<string, int> pair = new("zero", 0);
            Assert.IsFalse(testDict.Remove(pair));
        }

        [Test]
        public void RemoveKey(){
            bool result = testDict.Remove("one");
            Assert.Multiple(() => {
                Assert.IsTrue(result);
                Assert.IsFalse(testDict.ContainsKey("one"));
            });
        }

        [Test]
        public void RemoveKeyNotPresent(){
            Assert.IsFalse(testDict.Remove("zero"));
        }

        [Test]
        public void RemoveValue(){
            bool result = testDict.Remove(1);
            Assert.Multiple(() => {
                Assert.IsTrue(result);
                Assert.IsFalse(testDict.ContainsValue(1));
            });
        }

        [Test]
        public void RemoveValueNotPresent(){
            Assert.IsFalse(testDict.Remove(0));
        }

        [Test]
        public void Clear(){
            testDict.Clear();
            Assert.AreEqual(0, testDict.Count);
        }

        [Test]
        public void GetEnumerator(){
            IEnumerator<KeyValuePair<string, int>> enumerator = testDict.GetEnumerator();
            List<KeyValuePair<string, int>> result = [];
            while(enumerator.MoveNext()){
                result.Add(enumerator.Current);
            }
            Assert.Multiple(() => {
                Assert.AreEqual(3, result.Count);
                Assert.IsTrue(result.Contains(new("one", 1)));
                Assert.IsTrue(result.Contains(new("two", 2)));
                Assert.IsTrue(result.Contains(new("three", 3)));
            });
        }
    }
}
