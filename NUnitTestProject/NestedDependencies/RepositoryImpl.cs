namespace NUnitTestProject.NestedDependencies
{
    class RepositoryImpl : IRepository
    {
        public RepositoryImpl() { }

        public string Test()
        {
            return "test";
        }
    }
}
