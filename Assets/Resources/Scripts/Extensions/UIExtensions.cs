using System.Reflection;
using UnityEngine.UI;

public static class UIExtensions
{
    private static readonly MethodInfo sliderSetMethod;

    static UIExtensions()
    {
        sliderSetMethod = FindSetMethod(typeof(Slider));
    }

    public static void Set(this Slider instance, float value, bool sendCallback = false)
    {
        sliderSetMethod.Invoke(instance, new object[] { value, sendCallback });
    }

    private static MethodInfo FindSetMethod(System.Type objectType)
    {
        var methods = objectType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
        for (var i = 0; i < methods.Length; i++)
        {
            if (methods[i].Name == "Set" && methods[i].GetParameters().Length == 2)
            {
                return methods[i];
            }
        }

        return null;
    }
}
