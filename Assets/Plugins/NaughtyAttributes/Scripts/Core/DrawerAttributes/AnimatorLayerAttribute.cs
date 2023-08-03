using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AnimatorLayerAttribute : DrawerAttribute
    {
        public string AnimatorName { get; private set; }

        public AnimatorLayerAttribute(string animatorName)
        {
            AnimatorName = animatorName;
        }
    }
}