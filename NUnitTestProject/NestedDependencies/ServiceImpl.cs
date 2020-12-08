namespace NUnitTestProject.NestedDependencies
{
    class ServiceImpl : IService
    {
        private readonly string message;
        public ServiceImpl(IRepository repository) 
        {
            message = repository.Test();
        }
        public string GetMessage()
        {
            return message;
        }
    }
}
