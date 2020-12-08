namespace NUnitTestProject.NestedDependencies
{
    interface IServiceGeneric<TRepository> 
        where TRepository : IRepository
    {
        public string Test();
    }
}
