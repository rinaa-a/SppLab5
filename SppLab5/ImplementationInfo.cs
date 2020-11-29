using System;

namespace SppLab5
{
    public class ImplementationInfo
    {
        public readonly Lifetime lifetime;
        public readonly Type implementationType;

        public ImplementationInfo(Type implType, Lifetime lifetime)
        {
            this.lifetime = lifetime;
            implementationType = implType;
        }
    }
}
