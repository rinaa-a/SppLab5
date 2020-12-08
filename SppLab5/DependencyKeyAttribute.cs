using System;

namespace SppLab5
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class DependencyKeyAttribute : Attribute
    {
        public readonly ServiceImplementations implementationName;

        public DependencyKeyAttribute(ServiceImplementations name)
        {
            implementationName = name;
        }
    }
}
