namespace NUnitTestProject.MultipleImplementations
{
    interface ISomeInterface<TTestInterface>
        where TTestInterface : ITestInterface1
    {
        public int SomeTest();
    }
}
