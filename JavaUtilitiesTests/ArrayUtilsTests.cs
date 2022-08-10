using System;
using NUnit.Framework;
using JavaUtilities;

namespace JavaUtilitiesTests{
    [TestFixture]
    public class ArrayUtilsTests{
        private readonly Random random = new Random();
        private int[] array;
        
        
        [SetUp]
        public void SetUp(){
            array = new int[]{1, 2, 3, 4, 5};
        }
        
        [Test]
        public void Insert(){
            int[] result = ArrayUtils.Insert(array, 2, 0);
            Assert.Multiple(() => {
                Assert.AreEqual(0, result[2]);
                Assert.AreEqual(array.Length+1, result.Length);
            });
        }
        
        [Test]
        public void InsertIntoNull(){
            int[] result = ArrayUtils.Insert(null, 0, 0);
            Assert.IsNull(result);
        }

        [Test]
        public void InsertNull(){
            int[] result = ArrayUtils.Insert(array, 0, null);
            Assert.AreEqual(array.Length, result.Length);
        }
        
        [Test]
        public void InsertIndexOutOfRange(){
            Assert.Throws<ArgumentOutOfRangeException>(() => ArrayUtils.Insert(array, 6, 0));
        }
        
        [Test]
        public void Remove(){
            int[] result = ArrayUtils.Remove(array, 2);
            Assert.Multiple(() => {
                Assert.AreEqual(array.Length-1, result.Length);
                Assert.AreEqual(-1, Array.IndexOf(result, 3));
            });
        }
        
        [Test]
        public void RemoveNullArray(){
            int[] result = ArrayUtils.Remove<int>(null, 0);
            Assert.AreEqual(null, result);
        }
        
        [Test]
        public void RemoveIndexOutOfRange(){
            Assert.Throws<ArgumentOutOfRangeException>(() => ArrayUtils.Remove(array, 6));
        }
        
        [Test]
        public void HashCode(){
            int result = ArrayUtils.HashCode(array);
            Assert.AreEqual(29615266, result);
        }
        
        [Test]
        public void HashCodeNull(){
            int result = ArrayUtils.HashCode<int>(null);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void ContentsEqual(){
            int[] other = {1, 2, 3, 4, 5};
            Assert.IsTrue(ArrayUtils.ContentsEqual(array, other));
        }

        [Test]
        public void ContentsEqualDifferentLength(){
            int[] other = {0, 1, 2, 3};
            Assert.IsFalse(ArrayUtils.ContentsEqual(array, other));
        }

        [Test]
        public void ContentsEqualNotEqual(){
            int[] other = {1, 2, 3, 4, 6};
            Assert.IsFalse(ArrayUtils.ContentsEqual(array, other));
        }

        [Test]
        public void ContentsEqualFirstNull(){
            Assert.IsFalse(ArrayUtils.ContentsEqual(null, array));
        }

        [Test]
        public void ContentsEqualSecondNull(){
            Assert.IsFalse(ArrayUtils.ContentsEqual(array, null));
        }

        [Test]
        public void ContentsEqualBothNull(){
            Assert.IsTrue(ArrayUtils.ContentsEqual<int>(null, null));
        }

        [Test]
        public void TestToString(){
            string result = ArrayUtils.ToString(array);
            Assert.AreEqual("[1, 2, 3, 4, 5]", result);
        }
        
        [Test]
        public void TestToStringNull(){
            string result = ArrayUtils.ToString<int>(null);
            Assert.AreEqual("null", result);
        }

        [Test]
        public void TestToStringEmpty(){
            string result = ArrayUtils.ToString(new int[]{});
            Assert.AreEqual("[]", result);
        }

        [Test]
        [Repeat(50)]
        public void GetRandom(){
            int result = ArrayUtils.GetRandom(array, random);
            Assert.Contains(result, array);
        }
    }
}
