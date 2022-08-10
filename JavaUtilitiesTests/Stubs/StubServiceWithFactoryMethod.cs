namespace JavaUtilitiesTests.Stubs{
    public class StubServiceWithFactoryMethod : IStubService{
        private readonly int value;

        public string Name => $"Service with factory method: {value}";

        public static StubServiceWithFactoryMethod Provider(){
            return new StubServiceWithFactoryMethod(10);
        }

        public StubServiceWithFactoryMethod(int valueIn){
            value = valueIn;
        }
    }
}
