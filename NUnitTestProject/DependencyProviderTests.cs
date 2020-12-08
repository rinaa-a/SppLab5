using NUnit.Framework;
using NUnitTestProject.MultipleImplementations;
using NUnitTestProject.NestedDependencies;
using SppLab5;
using System.Collections.Generic;
using System.Linq;

namespace NUnitTestProject
{
    public class Tests
    {
        public DependenciesConfiguration Configuration { get; set; }

        [SetUp]
        public void Setup()
        {
            Configuration = new DependenciesConfiguration();
        }

        [Test]
        public void TestAsSelfDependency()
        {
            Configuration.Register<TestClass_AsSelf, TestClass_AsSelf>();
            var provider = new DependencyProvider(Configuration);

            var testClass = provider.Resolve<TestClass_AsSelf>();

            Assert.AreEqual("test", testClass.Test());
        }

        [Test]
        public void TestInterfaceDependency()
        {
            Configuration.Register<ITestInterface1, TestClass1>();
            var provider = new DependencyProvider(Configuration);

            var testClass = provider.Resolve<ITestInterface1>();

            Assert.AreEqual(0, testClass.Test());
        }

        [Test]
        public void TestInterfaceDependency_MultipleImplementations()
        {
            Configuration.Register<ITestInterface1, TestClass1>();
            Configuration.Register<ITestInterface1, TestClass1_2>();

            var provider = new DependencyProvider(Configuration);
            var services = provider.Resolve<IEnumerable<ITestInterface1>>();

            Assert.AreEqual(2, services.Count());
        }

        [Test]
        public void TestInterfaceDependency_NestedDependencies()
        {
            Configuration.Register<IService, ServiceImpl>();
            Configuration.Register<IRepository, RepositoryImpl>();

            var provider = new DependencyProvider(Configuration);
            var service = provider.Resolve<IService>();

            Assert.AreEqual("test", service.GetMessage());
        }

        [Test]
        public void TestInterfaceDependency_InstancePerDependency()
        {
            Configuration.Register<TestClass_AsSelf, TestClass_AsSelf>();
            var provider = new DependencyProvider(Configuration);

            var service = provider.Resolve<TestClass_AsSelf>();
            service.Inc();
            service = provider.Resolve<TestClass_AsSelf>();
            service.Inc();

            Assert.AreEqual(1, service.Count);
        }

        [Test]
        public void TestInterfaceDependency_Singleton()
        {
            Configuration.Register<TestClass_AsSelf, TestClass_AsSelf>(Lifetime.Singleton);
            var provider = new DependencyProvider(Configuration);

            var service = provider.Resolve<TestClass_AsSelf>();
            service.Inc();
            service = provider.Resolve<TestClass_AsSelf>();
            service.Inc();

            Assert.AreEqual(2, service.Count);
        }

        [Test]
        public void TestInterfaceDependency_Generic()
        {
            Configuration.Register<IRepository, RepositoryImpl>();
            Configuration.Register<IServiceGeneric<IRepository>, ServiceImplGeneric<IRepository>>();
            var provider = new DependencyProvider(Configuration);

            var service = provider.Resolve<IServiceGeneric<IRepository>>();

            Assert.AreEqual("test", service.Test());
        }

        [Test]
        public void TestInterfaceDependency_OpenGeneric()
        {
            Configuration.Register<IRepository, RepositoryImpl>();
            Configuration.Register(typeof(IServiceGeneric<>), typeof(ServiceImplGeneric<>));
            var provider = new DependencyProvider(Configuration);

            var service = provider.Resolve<IServiceGeneric<IRepository>>();

            Assert.AreEqual("test", service.Test());
        }

        [Test]
        public void TestInterfaceDependency_NamedDependencies()
        {
            Configuration.Register<ITestInterface1, TestClass1>(implementation: ServiceImplementations.First);
            Configuration.Register<ITestInterface1, TestClass1_2>(implementation: ServiceImplementations.Second);

            var provider = new DependencyProvider(Configuration);
            var service1 = provider.Resolve<ITestInterface1>(ServiceImplementations.First);
            var service2 = provider.Resolve<ITestInterface1>(ServiceImplementations.Second);

            Assert.AreEqual(0, service1.Test());
            Assert.AreEqual(1, service2.Test());
        }

        [Test]
        public void TestInterfaceDependency_NamedDependenciesConstructor()
        {
            Configuration.Register<ITestInterface1, TestClass1>(implementation: ServiceImplementations.First);
            Configuration.Register<ITestInterface1, TestClass1_2>(implementation: ServiceImplementations.Second);
            Configuration.Register<ISomeInterface<ITestInterface1>, SomeAnotherService<ITestInterface1>>();

            var provider = new DependencyProvider(Configuration);
            var service = provider.Resolve<ISomeInterface<ITestInterface1>>();

            Assert.AreEqual(1, service.SomeTest());
        }

        [Test]
        public void TestInterfaceDependency_NoDependencyReturnsNull()
        {
            var provider = new DependencyProvider(Configuration);
            var service = provider.Resolve<ISomeInterface<ITestInterface1>>();

            Assert.IsNull(service);
        }
    }
}