using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorStateAttribute))]
    public class AnimatorStatePropertyDrawer : PropertyDrawerBase
    {
        private const string InvalidAnimatorControllerWarningMessage = "Target animator controller is null";
        private const string InvalidTypeWarningMessage = "{0} must be a string";

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            AnimatorStateAttribute animatorParamAttribute = PropertyUtility.GetAttribute<AnimatorStateAttribute>(property);
            bool validAnimatorController = GetAnimatorController(property, animatorParamAttribute.AnimatorName) != null;
            bool validPropertyType = property.propertyType == SerializedPropertyType.String;

            return (validAnimatorController && validPropertyType)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            AnimatorStateAttribute animatorStateAttribute = PropertyUtility.GetAttribute<AnimatorStateAttribute>(property);

            AnimatorController animatorController = GetAnimatorController(property, animatorStateAttribute.AnimatorName);
            if (animatorController == null)
            {
                DrawDefaultPropertyAndHelpBox(rect, property, InvalidAnimatorControllerWarningMessage, MessageType.Warning);
                return;
            }

            int layer = GetLayer(property, animatorStateAttribute.LayerIndexProperty);

            int statesCount = animatorController.layers[layer].stateMachine.states.Length;
            List<ChildAnimatorState> animatorStates = new List<ChildAnimatorState>(statesCount);
            for (int i = 0; i < statesCount; i++)
            {
                animatorStates.Add(animatorController.layers[layer].stateMachine.states[i]);
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    DrawPropertyForStates(rect, property, label, animatorStates);
                    break;
                default:
                    DrawDefaultPropertyAndHelpBox(rect, property, string.Format(InvalidTypeWarningMessage, property.name), MessageType.Warning);
                    break;
            }

            EditorGUI.EndProperty();
        }

        private static void DrawPropertyForStates(Rect rect, SerializedProperty property, GUIContent label, List<ChildAnimatorState> animatorStates)
        {
            string paramName = property.stringValue;
            int index = 0;

            for (int i = 0; i < animatorStates.Count; i++)
            {
                if (paramName.Equals(animatorStates[i].state.name, System.StringComparison.Ordinal))
                {
                    index = i + 1; // +1 because the first option is reserved for (None)
                    break;
                }
            }

            string[] displayOptions = GetStateDisplayOptions(animatorStates);

            int newIndex = EditorGUI.Popup(rect, label.text, index, displayOptions);
            string newValue = newIndex == 0 ? null : animatorStates[newIndex - 1].state.name;

            if (!property.stringValue.Equals(newValue, System.StringComparison.Ordinal))
            {
                property.stringValue = newValue;
            }
        }

        private static string[] GetStateDisplayOptions(List<ChildAnimatorState> animatorStates)
        {
            string[] displayOptions = new string[animatorStates.Count + 1];
            displayOptions[0] = "(None)";

            for (int i = 0; i < animatorStates.Count; i++)
            {
                displayOptions[i + 1] = animatorStates[i].state.name;
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

        private static int GetLayer(SerializedProperty property, string layerIndexProperty)
        {
            object target = PropertyUtility.GetTargetObjectWithProperty(property);

            FieldInfo animatorFieldInfo = ReflectionUtility.GetField(target, layerIndexProperty);
            if (animatorFieldInfo != null &&
                animatorFieldInfo.FieldType == typeof(int))
            {
                return (int)animatorFieldInfo.GetValue(target);
            }

            PropertyInfo animatorPropertyInfo = ReflectionUtility.GetProperty(target, layerIndexProperty);
            if (animatorPropertyInfo != null &&
                animatorPropertyInfo.PropertyType == typeof(int))
            {
                return (int)animatorPropertyInfo.GetValue(target);
            }

            MethodInfo animatorGetterMethodInfo = ReflectionUtility.GetMethod(target, layerIndexProperty);
            if (animatorGetterMethodInfo != null &&
                animatorGetterMethodInfo.ReturnType == typeof(int) &&
                animatorGetterMethodInfo.GetParameters().Length == 0)
            {
                return (int)animatorGetterMethodInfo.Invoke(target, null);
            }

            return 0;
        }
    }
}
