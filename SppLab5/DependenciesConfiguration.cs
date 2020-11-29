using System;
using System.Collections.Generic;

namespace SppLab5
{
    public class DependenciesConfiguration
    {
        public readonly Dictionary<Type, List<ImplementationInfo>> dependencies;
        public DependenciesConfiguration()
        {
            dependencies = new Dictionary<Type, List<ImplementationInfo>>();
        }

        public void Register<TDependency, TImplementation>(Lifetime lifetime = Lifetime.Transient)
            where TDependency : class
            where TImplementation : TDependency
        {
            var newImplInfo = new ImplementationInfo(typeof(TImplementation), lifetime);
            if (!dependencies.ContainsKey(typeof(TDependency)))
            {
                dependencies.Add(typeof(TDependency), new List<ImplementationInfo>());
            }

            dependencies[typeof(TDependency)].Add(newImplInfo);
        }

        public void Register(Type dependency, Type implementation, Lifetime lifetime = Lifetime.Transient)
        {
            var newImplInfo = new ImplementationInfo(implementation, lifetime);
            if (!dependencies.ContainsKey(dependency))
            {
                dependencies.Add(dependency, new List<ImplementationInfo>());
            }

            dependencies[dependency].Add(newImplInfo);
        }
    }
}