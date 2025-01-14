using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using JavaUtilities;
using JavaUtilitiesTests.Stubs;

namespace JavaUtilitiesTests{
    [TestFixture]
    public class ServiceLoaderTests{
        /*
         * Fields
         */
        private ServiceLoader<IStubService> loader = null!;


        /*
         * Setup
         */
        [SetUp]
        public void SetUp(){
            loader = ServiceLoader.Load<IStubService>();
        }


        /*
         * Private methods
         */
        private static void AssertServicesLoaded(List<string> serviceNames){
            Assert.Multiple(() => {
                Assert.AreEqual(4, serviceNames.Count);
                Assert.Contains("Service A", serviceNames);
                Assert.Contains("Service B", serviceNames);
                Assert.Contains("Service C", serviceNames);
            });
        }


        /*
         * Test methods
         */
        [Test]
        public void EnumerateServices(){
            List<string> serviceNames = [];
            foreach(IStubService service in loader){
                serviceNames.Add(service.Name);
            }
            AssertServicesLoaded(serviceNames);
        }

        [Test]
        public void EnumerateServiceProviders(){
            List<string> serviceNames = [];
            foreach(ServiceLoader<IStubService>.IProvider provider in loader.GetSuppliers()){
                serviceNames.Add(provider.Get().Name);
            }
            AssertServicesLoaded(serviceNames);
        }

        [Test]
        public void RepeatEnumeration(){
            List<string> serviceNamesFirst = loader.Select(s => s.Name).ToList();
            List<string> serviceNamesSecond = loader.Select(s => s.Name).ToList();
            Assert.Multiple(() => {
                AssertServicesLoaded(serviceNamesFirst);
                AssertServicesLoaded(serviceNamesSecond);
            });
        }

        [Test]
        public void ParallelEnumeration(){
            IEnumerator<IStubService> enumA = loader.GetEnumerator();
            List<string> serviceNamesA = [];
            IEnumerator<IStubService> enumB = loader.GetEnumerator();
            List<string> serviceNamesB = [];
            while(true){
                bool aHasNext = enumA.MoveNext();
                bool bHasNext = enumB.MoveNext();
                Assert.AreEqual(aHasNext, bHasNext);
                if(!aHasNext || !bHasNext)
                    break;
                serviceNamesA.Add(enumA.Current.Name);
                serviceNamesB.Add(enumB.Current.Name);
            }
            Assert.Multiple(() => {
                AssertServicesLoaded(serviceNamesA);
                AssertServicesLoaded(serviceNamesB);
            });
        }

        [Test]
        public void GetCachedServices(){
            Dictionary<string, IStubService> services = loader.ToDictionary(s => s.Name);
            Dictionary<string, IStubService> cachedServices = loader.ToDictionary(s => s.Name);
            Assert.Multiple(() => {
                Assert.AreEqual(4, services.Count);
                Assert.AreEqual(4, cachedServices.Count);
                Assert.AreSame(services["Service A"], cachedServices["Service A"]);
                Assert.AreSame(services["Service B"], cachedServices["Service B"]);
                Assert.AreSame(services["Service C"], cachedServices["Service C"]);
                Assert.AreSame(services["Service with factory method: 10"], cachedServices["Service with factory method: 10"]);
            });
        }

        [Test]
        public void GetCachedProvider(){
            Dictionary<Type, ServiceLoader<IStubService>.IProvider> suppliers = loader.GetSuppliers().ToDictionary(s => s.GetServiceImplType());
            Dictionary<Type, ServiceLoader<IStubService>.IProvider> cachedSuppliers = loader.GetSuppliers().ToDictionary(s => s.GetServiceImplType());
            Assert.Multiple(() => {
                Assert.AreEqual(4, suppliers.Count);
                Assert.AreEqual(4, cachedSuppliers.Count);
                Assert.AreSame(suppliers[typeof(StubServiceA)], cachedSuppliers[typeof(StubServiceA)]);
                Assert.AreSame(suppliers[typeof(StubServiceB)], cachedSuppliers[typeof(StubServiceB)]);
                Assert.AreSame(suppliers[typeof(StubServiceC)], cachedSuppliers[typeof(StubServiceC)]);
                Assert.AreSame(suppliers[typeof(StubServiceWithFactoryMethod)], cachedSuppliers[typeof(StubServiceWithFactoryMethod)]);
            });
        }

        [Test]
        public void ReloadCheckServices(){
            Dictionary<string, IStubService> services = loader.ToDictionary(s => s.Name);
            loader.Reload();
            Dictionary<string, IStubService> newServices = loader.ToDictionary(s => s.Name);
            Assert.Multiple(() => {
                Assert.AreEqual(4, services.Count);
                Assert.AreEqual(4, newServices.Count);
                Assert.AreNotSame(services["Service A"], newServices["Service A"]);
                Assert.AreNotSame(services["Service B"], newServices["Service B"]);
                Assert.AreNotSame(services["Service C"], newServices["Service C"]);
                Assert.AreNotSame(services["Service with factory method: 10"], newServices["Service with factory method: 10"]);
            });
        }

        [Test]
        public void ReloadCheckSuppliers(){
            Dictionary<Type, ServiceLoader<IStubService>.IProvider> suppliers = loader.GetSuppliers().ToDictionary(s => s.GetServiceImplType());
            loader.Reload();
            Dictionary<Type, ServiceLoader<IStubService>.IProvider> newSuppliers = loader.GetSuppliers().ToDictionary(s => s.GetServiceImplType());
            Assert.Multiple(() => {
                Assert.AreEqual(4, suppliers.Count);
                Assert.AreEqual(4, newSuppliers.Count);
                Assert.AreNotSame(suppliers[typeof(StubServiceA)], newSuppliers[typeof(StubServiceA)]);
                Assert.AreNotSame(suppliers[typeof(StubServiceB)], newSuppliers[typeof(StubServiceB)]);
                Assert.AreNotSame(suppliers[typeof(StubServiceC)], newSuppliers[typeof(StubServiceC)]);
                Assert.AreNotSame(suppliers[typeof(StubServiceWithFactoryMethod)], newSuppliers[typeof(StubServiceWithFactoryMethod)]);
            });
        }

        [Test]
        public void FindFirst(){
            Optional<IStubService> result = loader.FindFirst();
            Assert.AreEqual("Service A", result.Get().Name);
        }

        [Test]
        public void FindFirstNotPresent(){
            ServiceLoader<IUnimplementedStubService> emptyLoader = ServiceLoader.Load<IUnimplementedStubService>();
            Optional<IUnimplementedStubService> result = emptyLoader.FindFirst();
            Assert.IsTrue(result.IsEmpty());
        }

        [Test]
        public void TestToString(){
            Assert.AreEqual("ServiceLoader[IStubService]", loader.ToString());
        }
    }
}
