using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SOInheritedFromAttribute : ValidatorAttribute
    {
        public Type TargetInterface { get; private set; }

        public SOInheritedFromAttribute(Type targetInterface)
        {
            TargetInterface = targetInterface;
        }
    }
}
