using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    public class SOInheritedFromPropertyValidator : PropertyValidatorBase
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            SOInheritedFromAttribute inheritedFromAttribute = PropertyUtility.GetAttribute<SOInheritedFromAttribute>(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue != null)
                {
                    if (property.objectReferenceValue.GetType().IsInstanceOfType(inheritedFromAttribute.TargetInterface) == false)
                        property.objectReferenceValue = null;
                }
            }
            if (property.isArray && property.propertyType == SerializedPropertyType.Generic)
            {
                int size = property.arraySize;

                for (int i = 0; i < size; i++)
                {
                    SerializedProperty elementProperty = property.GetArrayElementAtIndex(i);

                    if (elementProperty == null)
                        continue;

                    if (elementProperty.objectReferenceValue == null)
                        continue;

                    Type elementType = elementProperty.objectReferenceValue.GetType();
                    if (elementType.GetInterface(inheritedFromAttribute.TargetInterface.Name) == null)
                    {
                        Debug.LogError($"{elementProperty.objectReferenceValue.name} isn't inherited from {inheritedFromAttribute.TargetInterface.Name}");
                        elementProperty.objectReferenceValue = null;
                    }
                }
            }
        }
    }
}
