using System;
using System.Collections.Generic;
using NUnit.Framework;
using JavaUtilities;

namespace JavaUtilitiesTests{
    [TestFixture]
    public class CollectionUtilsTests{
        /*
         * Fields
         */
        private readonly Random random = new Random();
        private List<string> list;


        /*
         * Setup
         */
        [SetUp]
        public void SetUp(){
            list = new List<string>{"one", "two", "three"};
        }


        /*
         * Test methods
         */
        [Test]
        public void ContentsEqual(){
            List<string> list2 = new List<string>{"one", "two", "three"};
            Assert.IsTrue(CollectionUtils.ContentsEqual(list, list2));
        }

        [Test]
        public void ContentsEqualNotEqual(){
            List<string> list2 = new List<string>{"one", "two", "four"};
            Assert.IsFalse(CollectionUtils.ContentsEqual(list, list2));
        }

        [Test]
        public void ContentsEqualDifferentLength(){
            List<string> list2 = new List<string>{"one", "two", "three", "four"};
            Assert.IsFalse(CollectionUtils.ContentsEqual(list, list2));
        }

        [Test]
        public void ContentsEqualFirstNull(){
            Assert.IsFalse(CollectionUtils.ContentsEqual(null, list));
        }

        [Test]
        public void ContentsEqualSecondNull(){
            Assert.IsFalse(CollectionUtils.ContentsEqual(list, null));
        }

        [Test]
        public void ContentsEqualBothNull(){
            Assert.IsTrue(CollectionUtils.ContentsEqual<string>(null, null));
        }

        [Test]
        public void HashCodeEmptyCollection(){
            list.Clear();
            int result = CollectionUtils.HashCode(list);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void HashCode(){
            List<int> intList = new List<int>{1, 2, 3, 4};
            int result = CollectionUtils.HashCode(intList);
            Assert.AreEqual(955331, result);
        }

        [Test]
        [Repeat(50)]
        public void GetRandom(){
            string result = CollectionUtils.GetRandom(list, random);
            Assert.Contains(result, list);
        }
    }
}
