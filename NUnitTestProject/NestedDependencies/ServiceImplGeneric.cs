namespace NUnitTestProject.NestedDependencies
{
    class ServiceImplGeneric<TRepository> : IServiceGeneric<TRepository>
        where TRepository : IRepository
    {
        private readonly string message;
        public string Test()
        {
            return message;
        }

        public ServiceImplGeneric(TRepository repository)
        {
            message = repository.Test();
        }
    }
}
