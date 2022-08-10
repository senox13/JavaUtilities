namespace JavaUtilitiesTests.Stubs{
    public class StubServiceInvalid: IStubService{
        public string Name => "Invalid service";

#pragma warning disable IDE0060 // Remove unused parameter
        public StubServiceInvalid(object arg){}
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
