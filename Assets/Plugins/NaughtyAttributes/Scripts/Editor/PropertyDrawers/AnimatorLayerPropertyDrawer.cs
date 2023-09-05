using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorLayerAttribute))]
    public class AnimatorLayerPropertyDrawer : PropertyDrawerBase
    {
        private const string InvalidAnimatorControllerWarningMessage = "Target animator controller is null";
        private const string InvalidTypeWarningMessage = "{0} must be an int";

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            AnimatorLayerAttribute animatorLayerAttribute = PropertyUtility.GetAttribute<AnimatorLayerAttribute>(property);
            bool validAnimatorController = GetAnimatorController(property, animatorLayerAttribute.AnimatorName) != null;
            bool validPropertyType = property.propertyType == SerializedPropertyType.Integer;

            return (validAnimatorController && validPropertyType)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            AnimatorLayerAttribute animatorLayerAttribute = PropertyUtility.GetAttribute<AnimatorLayerAttribute>(property);

            AnimatorController animatorController = GetAnimatorController(property, animatorLayerAttribute.AnimatorName);
            if (animatorController == null)
            {
                DrawDefaultPropertyAndHelpBox(rect, property, InvalidAnimatorControllerWarningMessage, MessageType.Warning);
                return;
            }

            List<AnimatorControllerLayer> animatorLayers = new List<AnimatorControllerLayer>(animatorController.layers.Length);
            for (int i = 0; i < animatorController.layers.Length; i++)
            {
                animatorLayers.Add(animatorController.layers[i]);
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    DrawPropertyForLayers(rect, property, label, animatorLayers);
                    break;
                default:
                    DrawDefaultPropertyAndHelpBox(rect, property, string.Format(InvalidTypeWarningMessage, property.name), MessageType.Warning);
                    break;
            }

            EditorGUI.EndProperty();
        }

        private static void DrawPropertyForLayers(Rect rect, SerializedProperty property, GUIContent label, List<AnimatorControllerLayer> animatorLayers)
        {
            int index = property.intValue;

            string[] displayOptions = GetLayerDisplayOptions(animatorLayers);

            int newIndex = EditorGUI.Popup(rect, label.text, index, displayOptions);

            if (index != newIndex)
            {
                property.intValue = newIndex;
            }
        }

        private static string[] GetLayerDisplayOptions(List<AnimatorControllerLayer> animatorLayers)
        {
            string[] displayOptions = new string[animatorLayers.Count];

            for (int i = 0; i < animatorLayers.Count; i++)
            {
                displayOptions[i] = animatorLayers[i].name;
            }

            return displayOptions;
        }

        private static AnimatorController GetAnimatorController(SerializedProperty property, string animatorName)
        {
            object target = PropertyUtility.GetTargetObjectWithProperty(property);

            FieldInfo animatorFieldInfo = ReflectionUtility.GetField(target, animatorName);
            if (animatorFieldInfo != null &&
                animatorFieldInfo.FieldType == typeof(Animator))
            {
                Animator animator = animatorFieldInfo.GetValue(target) as Animator;
                if (animator != null)
                {
                    AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
                    return animatorController;
                }
            }

            PropertyInfo animatorPropertyInfo = ReflectionUtility.GetProperty(target, animatorName);
            if (animatorPropertyInfo != null &&
                animatorPropertyInfo.PropertyType == typeof(Animator))
            {
                Animator animator = animatorPropertyInfo.GetValue(target) as Animator;
                if (animator != null)
                {
                    AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
                    return animatorController;
                }
            }

            MethodInfo animatorGetterMethodInfo = ReflectionUtility.GetMethod(target, animatorName);
            if (animatorGetterMethodInfo != null &&
                animatorGetterMethodInfo.ReturnType == typeof(Animator) &&
                animatorGetterMethodInfo.GetParameters().Length == 0)
            {
                Animator animator = animatorGetterMethodInfo.Invoke(target, null) as Animator;
                if (animator != null)
                {
                    AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
                    return animatorController;
                }
            }

            return null;
        }
    }
}

