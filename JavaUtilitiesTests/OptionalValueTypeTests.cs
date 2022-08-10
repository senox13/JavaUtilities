using System;
using NUnit.Framework;
using JavaUtilities;

namespace JavaUtilitiesTests{
    [TestFixture]
    public class OptionalValueTypeTests{
        /*
         * Fields
         */
        private const int TEST_NUM = 10;
        public Optional<int> testOptional;


        /*
         * Setup method
         */
        [SetUp]
        public void SetUp(){
            testOptional = Optional.Of(TEST_NUM);
        }


        /*
         * Test methods
         */
        [Test]
        public void OfNullableNonNull(){
            Optional<int> result = Optional.OfNullable(TEST_NUM);
            Assert.AreEqual(TEST_NUM, result.Get());
        }

        [Test]
        public void IsPresent(){
            Assert.IsTrue(testOptional.IsPresent());
        }

        [Test]
        public void IsPresentEmpty(){
            Assert.IsTrue(Optional.Empty<int>().IsEmpty());
        }

        [Test]
        public void IfPresent(){
            bool called = false;
            testOptional.IfPresent(s => called=true);
            Assert.IsTrue(called);
        }

        [Test]
        public void IfPresentEmpty(){
            bool called = false;
            Optional.Empty<int>().IfPresent(s => called=true);
            Assert.IsFalse(called);
        }

        [Test]
        public void IfPresentOrElse(){
            bool presentCalled = false;
            bool emptyCalled = false;
            testOptional.IfPresentOrElse(
                s => presentCalled=true,
                () => emptyCalled=true
            );
            Assert.Multiple(() => {
                Assert.IsTrue(presentCalled);
                Assert.IsFalse(emptyCalled);
            });
        }

        [Test]
        public void IfPresentOrElseEmpty(){
            bool presentCalled = false;
            bool emptyCalled = false;
            Optional.Empty<int>().IfPresentOrElse(
                s => presentCalled=true,
                () => emptyCalled=true
            );
            Assert.Multiple(() => {
                Assert.IsFalse(presentCalled);
                Assert.IsTrue(emptyCalled);
            });
        }

        [Test]
        public void FilterTrue(){
            Optional<int> result = testOptional.Filter(s => true);
            Assert.AreEqual(testOptional, result);
        }

        [Test]
        public void FilterFalse(){
            Optional<int> result = testOptional.Filter(s => false);
            Assert.AreEqual(Optional.Empty<int>(), result);
        }

        [Test]
        public void Map(){
            Optional<string> result = testOptional.Map(s => s.ToString());
            Assert.AreEqual(TEST_NUM.ToString(), result.Get());
        }

        [Test]
        public void FlatMap(){
            Optional<string> result = testOptional.FlatMap(s => Optional.Of(s.ToString()));
            Assert.AreEqual(TEST_NUM.ToString(), result.Get());
        }

        [Test]
        public void Or(){
            Optional<int> result = testOptional.Or(() => Optional.Of(-1));
            Assert.AreEqual(TEST_NUM, result.Get());
        }

        [Test]
        public void OrEmpty(){
            int emptyValue = -1;
            Optional<int> result = Optional.Empty<int>().Or(() => Optional.Of(emptyValue));
            Assert.AreEqual(emptyValue, result.Get());
        }

        [Test]
        public void OrElse(){
            int result = testOptional.OrElse(-1);
            Assert.AreEqual(TEST_NUM, result);
        }

        [Test]
        public void OrElseEmpty(){
            int emptyValue = -1;
            int result = Optional.Empty<int>().OrElse(emptyValue);
            Assert.AreEqual(emptyValue, result);
        }

        [Test]
        public void OrElseGet(){
            int result = testOptional.OrElseGet(() => -1);
            Assert.AreEqual(TEST_NUM, result);
        }

        [Test]
        public void OrElseGetEmpty(){
            int emptyValue = -1;
            int result = Optional.Empty<int>().OrElseGet(() => emptyValue);
            Assert.AreEqual(emptyValue, result);
        }

        [Test]
        public void OrElseThrow(){
            int result = testOptional.OrElseThrow();
            Assert.AreEqual(TEST_NUM, result);
        }

        [Test]
        public void OrElseThrowEmpty(){
            Assert.Throws<InvalidOperationException>(() => Optional.Empty<int>().OrElseThrow());
        }

        [Test]
        public void OrElseThrowSupplier(){
            int result = testOptional.OrElseThrow(() => new Exception("supplier"));
            Assert.AreEqual(TEST_NUM, result);
        }

        [Test]
        public void OrElseThrowSupplierEmpty(){
            string exceptionMessage = "supplier";
            Assert.Throws<Exception>(
                () => Optional.Empty<int>().OrElseThrow(() => new Exception(exceptionMessage)),
                exceptionMessage
            );
        }

        [Test]
        public void Equals(){
            Optional<int> other = Optional.Of(TEST_NUM);
            Assert.IsTrue(testOptional.Equals(other));
        }

        [Test]
        public void EqualsDefaultValue(){
            Optional<int> defaultOptional = Optional.Of<int>(default);
            Assert.AreNotEqual(Optional.Empty<int>(), defaultOptional);
        }

        [Test]
        public void EqualsNotEqual(){
            Assert.IsFalse(testOptional.Equals(Optional.Empty<int>()));
        }

        [Test]
        public void TestToString(){
            string result = testOptional.ToString();
            Assert.AreEqual("Optional<Int32>[10]", result);
        }

        [Test]
        public void TestToStringEmpty(){
            string result = Optional.Empty<int>().ToString();
            Assert.AreEqual("Optional<Int32>.empty", result);
        }
    }
}
