using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using JavaUtilities.Collections;

namespace JavaUtilitiesTests.Collections{
    [TestFixture]
    public class ImmutableHashableDictionaryTests{
        /*
         * Fields
         */
        private const string TESTDICT_STR = "{one=1, two=2}";
        private ImmutableHashableDictionary<string, int> testDict = null!;
        
        
        /*
         * Setup method
         */
        [SetUp]
        public void SetUp(){
            testDict = new ImmutableHashableDictionary<string, int>().Add("one", 1).Add("two", 2);
        }
        

        /*
         * Test methods
         */
        [Test]
        public void Count(){
            Assert.AreEqual(2, testDict.Count);
        }

        [Test]
        public void Keys(){
            List<string> result = testDict.Keys.ToList();
            Assert.Multiple(() => {
                Assert.AreEqual("one", result[0]);
                Assert.AreEqual("two", result[1]);
            });
        }

        [Test]
        public void Values(){
            List<int> result = testDict.Values.ToList();
            Assert.Multiple(() => {
                Assert.AreEqual(1, result[0]);
                Assert.AreEqual(2, result[1]);
            });
        }

        [Test]
        public void SetNewKey(){
            ImmutableHashableDictionary<string, int> result = testDict.Set("three", 3);
            Assert.Multiple(() => {
                Assert.AreEqual(3, result.Count);
                Assert.AreEqual(3, result["three"]);
            });
        }
        
        
        [Test]
        public void SetExistingKey(){
            ImmutableHashableDictionary<string, int> newDict = testDict.Set("one", -1);
            Assert.AreEqual(-1, newDict["one"]);
        }
        
        [Test]
        public void TryGetValue(){
            bool result = testDict.TryGetValue("one", out int outResult);
            Assert.Multiple(() => {
                Assert.IsTrue(result);
                Assert.AreEqual(1, outResult);
            });
        }
        
        [Test]
        public void GetHashCodeAndEquals(){
            ImmutableHashableDictionary<string, int> testDict2 = new ImmutableHashableDictionary<string, int>().Add("one", 1).Add("two", 2);
            Assert.Multiple(() => {
                Assert.IsTrue(testDict.Equals(testDict2));
                Assert.AreEqual(testDict.GetHashCode(), testDict2.GetHashCode());
            });
        }

        [Test]
        public void TestToString(){
            Assert.AreEqual(TESTDICT_STR, testDict.ToString());
        }
    }
}
