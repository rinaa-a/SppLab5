using System;

namespace SppLab5
{
    public class ImplementationInfo
    {
        public readonly Lifetime lifetime;
        public readonly Type implementationType;
        public readonly ServiceImplementations implementationName;

        public ImplementationInfo(Type implType, Lifetime lifetime, ServiceImplementations implementation)
        {
            this.lifetime = lifetime;
            implementationType = implType;
            implementationName = implementation;
        }
    }
}
