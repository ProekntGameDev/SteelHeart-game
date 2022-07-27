using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class EventEditor
{
    static Object[][] obj;
    static System.Type[] type;
    static MethodInfo[][] m_info;
    static ParameterInfo[][][] param_info;

    public static void Init()
    {
        MethodInfoEqualityComparer miec = new MethodInfoEqualityComparer();
        MethodInfo[] mono_type_methods = typeof(MonoBehaviour).GetMethods();
        DirectoryInfo dir_info = new DirectoryInfo(Application.dataPath + "/Scripts/");
        FileInfo[] file_info = dir_info.GetFiles("*.cs");
        type = new System.Type[file_info.Length];

        if (m_info == null) m_info = new MethodInfo[file_info.Length][];
        if (param_info == null) param_info = new ParameterInfo[file_info.Length][][];
        for (int i = 0; i < file_info.Length; ++i)
        {
            type[i] = System.Type.GetType(file_info[i].Name.Remove(file_info[i].Name.Length - 3));
            //
            m_info[i] = type[i].GetMethods().Except(mono_type_methods, miec).ToArray();
            param_info[i] = new ParameterInfo[m_info[i].Length][];
            for (int i1 = 0; i1 < m_info[i].Length; ++i1)
            {
                Debug.Log(m_info[i][i1].ToString());
                param_info[i][i1] = m_info[i][i1].GetParameters();
            }
            //
            obj = new Object[file_info.Length][];
            obj[i] = MonoBehaviour.FindObjectsOfType(type[i]);
            //if (obj[i] != null && obj[i].Length != 0) Debug.Log(obj[i][0].name);//working correctly and useful
        }
        //string param_type_and_name = param_info[0][0][0].ParameterType + " " + param_info[0][0][0].Name;
    }
    public static void CallMethod()
    {
        //obj[0][0].GetType()); - BouncerSpecification

        {
            //object[] args = new object[0];
            //m_info[0].Invoke(obj[0][0], args);
            //type.InvokeMember(m_info[0].Name, BindingFlags.Default, null, obj[0][0], args);
        }
    }

    class MethodInfoEqualityComparer : IEqualityComparer<MethodInfo>
    {
        public bool Equals(MethodInfo m0, MethodInfo m1)
        {
            return m0.Name == m1.Name;
        }
        public int GetHashCode(MethodInfo mi)
        {
            return mi.Name.GetHashCode();
        }
    }

    public class EventTrigger
    {
        List<string> trigger;
        void Handle() { }
        void Store() { }
    }
}