using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SppLab5
{
    public class DependencyProvider
    {
        private readonly Dictionary<Type, List<ImplementationInfo>> dependencies;

        public DependencyProvider(DependenciesConfiguration configuration)
        {
            dependencies = configuration.dependencies;
        }

        public T Resolve<T>(ServiceImplementations implementationName = ServiceImplementations.None) where T : class
        {
            Type dependencyType = typeof(T);

            return (T)Resolve(dependencyType, implementationName);
        }

        private object Resolve(Type dependencyType, ServiceImplementations implementationName = ServiceImplementations.None)
        {
            if (dependencyType.IsGenericType && (dependencyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                return GetAllImplementations(dependencyType.GetGenericArguments()[0]);
            }

            if (dependencyType.IsGenericType && dependencies.ContainsKey(dependencyType.GetGenericTypeDefinition()))
            {
                var implementationType = dependencies[dependencyType.GetGenericTypeDefinition()][0].implementationType;
                implementationType = implementationType.MakeGenericType(dependencyType.GetGenericArguments());

                if (dependencyType.IsAssignableFrom(implementationType))
                {
                    return CreateInstance(implementationType);
                }
            }

            if (!dependencies.ContainsKey(dependencyType))
            {
                return null;
            }

            var implementationInfo = GetImplementationInfo(dependencyType, implementationName);

            if (!dependencyType.IsAssignableFrom(implementationInfo.implementationType))
            {
                return null;
            }

            if (implementationInfo.lifetime == Lifetime.Transient)
            {
                return CreateInstance(implementationInfo.implementationType);
            }

            if (implementationInfo.lifetime == Lifetime.Singleton)
            {
                return Singleton.GetInstance(implementationInfo.implementationType, CreateInstance);
            }

            return null;
        }

        private ImplementationInfo GetImplementationInfo(Type dependencyType, ServiceImplementations name)
        {
            var implementations = dependencies[dependencyType];
            return implementations.Where(info => info.implementationName == name).First();
        }

        private IEnumerable GetAllImplementations(Type dependencyType)
        {
            List<ImplementationInfo> implementations = dependencies[dependencyType];
            Type collectionType = typeof(List<>).MakeGenericType(dependencyType);
            IList instances = (IList)Activator.CreateInstance(collectionType);

            foreach (var implementation in implementations)
            {
                instances.Add(CreateInstance(implementation.implementationType));
            }

            return instances;
        }

        private object CreateInstance(Type type)
        {
            ConstructorInfo constructor = type.GetConstructors()[0];
            var parameters = new List<object>();

            foreach (var parameter in constructor.GetParameters())
            {
                var attribute = (DependencyKeyAttribute)parameter.GetCustomAttribute(typeof(DependencyKeyAttribute));

                if (attribute == null)
                {
                    parameters.Add(Resolve(parameter.ParameterType));
                }
                else
                {
                    parameters.Add(Resolve(parameter.ParameterType, attribute.implementationName));
                }
            }

            return Activator.CreateInstance(type, parameters.ToArray());
        }
    }
}
