using System;
using System.Collections.Generic;
using NUnit.Framework;
using JavaUtilities.Collections;

namespace JavaUtilitiesTests.Collections{
    [TestFixture]
    public class TableTests{
        private Table<string, int, int> testTable = null!;
        private static readonly string[] nums = ["zero", "one", "two", "three"];
        
        
        [SetUp]
        public void SetUp(){
            testTable = [];
            for(int r=1; r<=3; r++){
                for(int c=1; c<=3; c++){
                    testTable.Add(nums[r], c, r*c);
                }
            }
        }
        
        
        [Test]
        public void Count(){
            Assert.AreEqual(9, testTable.Count);
        }
        
        
        [Test]
        public void GetThis(){
            Assert.AreEqual(9, testTable[nums[3], 3]);
        }
        
        
        [Test]
        public void GetThisNotPresent(){
            Assert.Throws<KeyNotFoundException>(() => ++testTable[nums[3], 0]);
        }
        
        
        [Test]
        public void SetThis(){
            testTable[nums[3], 4] = 12;
            Assert.AreEqual(12, testTable[nums[3], 4]);
        }
        
        
        [Test]
        public void RowKeys(){
            List<string> rowKeys = new(testTable.RowKeys);
            Assert.AreEqual(3, rowKeys.Count);
            for(int i=1; i<=3; i++){
                Assert.IsTrue(rowKeys.Contains(nums[i]));
            }
        }
        
        
        [Test]
        public void ColKeys(){
            List<int> colKeys = new(testTable.ColKeys);
            Assert.AreEqual(3, colKeys.Count);
            for(int i=1; i<=3; i++){
                Assert.IsTrue(colKeys.Contains(i));
            }
        }
        
        
        [Test]
        public void Values(){
            ICollection<int> values = testTable.Values;
            Assert.AreEqual(9, values.Count);
            for(int r=1; r<=3; r++){
                for(int c=1; c<=3; c++){
                    Assert.IsTrue(values.Contains(r*c));
                }
            }
        }
        
        
        [Test]
        public void Contains(){
            Assert.IsTrue(testTable.Contains(nums[1], 1));
        }
        
        
        [Test]
        public void ContainsNotPresent(){
            Assert.IsFalse(testTable.Contains(nums[1], 4));
        }
        
        
        [Test]
        public void ContainsRow(){
            Assert.IsTrue(testTable.ContainsRow(nums[1]));
        }
        
        
        [Test]
        public void ContainsRowNotPresent(){
            Assert.IsFalse(testTable.ContainsRow("four"));
        }
        
        
        [Test]
        public void ContainsColumn(){
            Assert.IsTrue(testTable.ContainsColumn(1));
        }
        
        
        [Test]
        public void ContainsColumnNotPresent(){
            Assert.IsFalse(testTable.ContainsColumn(4));
        }
        
        
        [Test]
        public void ContainsValue(){
            Assert.IsTrue(testTable.ContainsValue(9));
        }
        
        
        [Test]
        public void ContainsValueNotPresent(){
            Assert.IsFalse(testTable.ContainsValue(12));
        }
        
        
        [Test]
        public void Clear(){
            testTable.Clear();
            Assert.AreEqual(0, testTable.Count);
        }
        
        
        [Test]
        public void Add(){
            testTable.Add(nums[0], 4, 12);
            Assert.AreEqual(12, testTable[nums[0], 4]);
        }
        
        
        [Test]
        public void AddAll(){
            List<Tuple<string, int, int>> values = [
                new Tuple<string, int, int>(nums[1], 4, 4),
                new Tuple<string, int, int>(nums[2], 4, 8),
                new Tuple<string, int, int>(nums[3], 4, 12)
            ];
            testTable.AddAll(values);
            Assert.AreEqual(12, testTable.Count);
            Assert.AreEqual(12, testTable[nums[3], 4]);
        }
        
        
        [Test]
        public void Remove(){
            testTable.Remove(nums[3], 3);
            Assert.IsFalse(testTable.Contains(nums[3], 3));
        }
        
        
        [Test]
        public void GetHashCodeAndEquals(){
            Table<string, int, int> otherTable = new(testTable);
            Assert.IsFalse(testTable.Equals(otherTable));
            Assert.AreNotEqual(testTable.GetHashCode(), otherTable.GetHashCode());
        }
    }
}
