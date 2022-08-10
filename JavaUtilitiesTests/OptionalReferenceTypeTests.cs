using System;
using NUnit.Framework;
using JavaUtilities;

namespace JavaUtilitiesTests{
    [TestFixture]
    public class OptionalReferenceTypeTests{
        /*
         * Fields
         */
        private static readonly string TEST_STRING = "Hello, world!";
        private Optional<string> testOptional;


        /*
         * Setup method
         */
        [SetUp]
        public void SetUp(){
            testOptional = Optional.Of(TEST_STRING);
        }


        /*
         * Test methods
         */
        [Test]
        public void OfNullableNonNull(){
            Optional<string> result = Optional.OfNullable(TEST_STRING);
            Assert.AreSame(TEST_STRING, result.Get());
        }

        [Test]
        public void OfNullableNull(){
            Optional<string> result = Optional.OfNullable<string>(null);
            Assert.AreEqual(Optional.Empty<string>(), result);
        }

        [Test]
        public void OfNull(){
            Assert.Throws<ArgumentNullException>(() => Optional.Of<string>(null));
        }

        [Test]
        public void GetFromEmpty(){
            testOptional = Optional.Empty<string>();
            Assert.Throws<InvalidOperationException>(() => testOptional.Get());
        }

        [Test]
        public void IsPresent(){
            Assert.IsTrue(testOptional.IsPresent());
        }

        [Test]
        public void IsPresentEmpty(){
            Assert.IsTrue(Optional.Empty<string>().IsEmpty());
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
            Optional.Empty<string>().IfPresent(s => called=true);
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
            testOptional = Optional.Empty<string>();
            bool presentCalled = false;
            bool emptyCalled = false;
            testOptional.IfPresentOrElse(
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
            Optional<string> result = testOptional.Filter(s => true);
            Assert.AreEqual(testOptional, result);
        }

        [Test]
        public void FilterFalse(){
            Optional<string> result = testOptional.Filter(s => false);
            Assert.AreEqual(Optional.Empty<string>(), result);
        }

        [Test]
        public void FilterNullPredicate(){
            Assert.Throws<ArgumentNullException>(() => testOptional.Filter(null));
        }

        [Test]
        public void FilterEmpty(){
            testOptional = Optional.Empty<string>();
            Optional<string> result = testOptional.Filter(s => true);
            Assert.IsTrue(result.IsEmpty());
        }

        [Test]
        public void Map(){
            Optional<int> result = testOptional.Map(s => s.Length);
            Assert.AreEqual(TEST_STRING.Length, result.Get());
        }

        [Test]
        public void MapNullPredicate(){
            Assert.Throws<ArgumentNullException>(() => testOptional.Map<string>(null));
        }

        [Test]
        public void MapEmpty(){
            testOptional = Optional.Empty<string>();
            Optional<int> result = testOptional.Map(s => 1);
            Assert.IsTrue(result.IsEmpty());
        }

        [Test]
        public void FlatMap(){
            Optional<int> result = testOptional.FlatMap(s => Optional.Of(s.Length));
            Assert.AreEqual(TEST_STRING.Length, result.Get());
        }

        [Test]
        public void FlatMapNullPredicate(){
            Assert.Throws<ArgumentNullException>(() => testOptional.FlatMap<string>(null));
        }

        [Test]
        public void FlatMapEmpty(){
            testOptional = Optional.Empty<string>();
            Optional<int> result = testOptional.FlatMap(s => Optional.Of(1));
            Assert.IsTrue(result.IsEmpty());
        }

        [Test]
        public void Or(){
            Optional<string> result = testOptional.Or(() => Optional.Of("Goodbye, world!"));
            Assert.AreSame(TEST_STRING, result.Get());
        }

        [Test]
        public void OrNullSupplier(){
            Assert.Throws<ArgumentNullException>(() => testOptional.Or(null));
        }
        
        [Test]
        public void OrEmpty(){
            string emptyString = "Goodbye, world!";
            Optional<string> result = Optional.Empty<string>().Or(() => Optional.Of(emptyString));
            Assert.AreSame(emptyString, result.Get());
        }

        [Test]
        public void OrElse(){
            string result = testOptional.OrElse("Goodbye, world!");
            Assert.AreSame(TEST_STRING, result);
        }

        [Test]
        public void OrElseEmpty(){
            string emptyString = "Goodbye, world!";
            string result = Optional.Empty<string>().OrElse(emptyString);
            Assert.AreSame(emptyString, result);
        }

        [Test]
        public void OrElseGet(){
            string result = testOptional.OrElseGet(() => "Goodbye, world!");
            Assert.AreSame(TEST_STRING, result);
        }

        [Test]
        public void OrElseGetEmpty(){
            string emptyString = "Goodbye, world!";
            string result = Optional.Empty<string>().OrElseGet(() => emptyString);
            Assert.AreSame(emptyString, result);
        }

        [Test]
        public void OrElseThrow(){
            string result = testOptional.OrElseThrow();
            Assert.AreSame(TEST_STRING, result);
        }

        [Test]
        public void OrElseThrowEmpty(){
            Assert.Throws<InvalidOperationException>(() => Optional.Empty<string>().OrElseThrow());
        }

        [Test]
        public void OrElseThrowSupplier(){
            string result = testOptional.OrElseThrow(() => new Exception("supplier"));
            Assert.AreSame(TEST_STRING, result);
        }

        [Test]
        public void OrElseThrowSupplierEmpty(){
            string exceptionMessage = "supplier";
            Assert.Throws<Exception>(
                () => Optional.Empty<string>().OrElseThrow(() => new Exception(exceptionMessage)),
                exceptionMessage
            );
        }

        [Test]
        public void Equals(){
            Optional<string> other = Optional.Of(TEST_STRING);
            Assert.IsTrue(testOptional.Equals(other));
        }

        [Test]
        public void EqualsNotEqual(){
            Assert.IsFalse(testOptional.Equals(Optional.Empty<string>()));
        }

        [Test]
        public void EqualsDifferentType(){
            Assert.IsFalse(testOptional.Equals(Optional.Of(1)));
        }

        [Test]
        public void TestGetHashCode(){
            Optional<string> secondOptional = Optional.Of(TEST_STRING);
            Assert.AreEqual(testOptional.GetHashCode(), secondOptional.GetHashCode());
        }

        [Test]
        public void TestGetHashCodeEmpty(){
            testOptional = Optional.Empty<string>();
            Optional<string> secondOptional = Optional.Empty<string>();
            Assert.AreEqual(testOptional.GetHashCode(), secondOptional.GetHashCode());
        }

        [Test]
        public void TestToString(){
            string result = testOptional.ToString();
            Assert.AreEqual("Optional<String>[Hello, world!]", result);
        }

        [Test]
        public void TestToStringEmpty(){
            string result = Optional.Empty<string>().ToString();
            Assert.AreEqual("Optional<String>.empty", result);
        }
    }
}
