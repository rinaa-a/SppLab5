using SppLab5;

namespace NUnitTestProject.MultipleImplementations
{
    class SomeAnotherService <TService> : ISomeInterface<TService>
        where TService : ITestInterface1
    {
        private readonly int num;


        public int Test()
        {
            return 19;
        }

        public int SomeTest()
        {
            return num;
        }

        public SomeAnotherService([DependencyKey(ServiceImplementations.Second)] TService service)
        {
            num = service.Test();
        }
    }
}
