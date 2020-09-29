using UnityEngine;
using UnityEditor;
using InfinityEngine.Attributes;
using System.Reflection;
using InfinityEngine.Utils;
using System;
namespace InfinityEditor
{

    /// <summary>
    /// Property drawer of <see cref="MinMaxRangeAttribute"/>
    /// </summary>
    [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
    public class MinMaxRangeDrawer : PropertyDrawer
    {

        private MethodInfo minLimitGetterFunction;
        private MethodInfo maxLimitGetterFunction;
        private object serializedObjectReference;
        private Type serializedObjectType;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.type != "MinMax")
            {
                Debug.LogWarning("Use only with MinMax type");
            }
            else
            {
                var range = attribute as MinMaxRangeAttribute;

                if (range.isDynamic)
                {
                    if (serializedObjectReference == null)
                    {
                        serializedObjectReference = property.serializedObject.targetObject;
                        serializedObjectType = property.serializedObject.targetObject.GetType();
                    }

                    minLimitGetterFunction = ReflectionUtils.GetCachedMethod(serializedObjectType, range.minLimitGetterFunction);
                    maxLimitGetterFunction = ReflectionUtils.GetCachedMethod(serializedObjectType, range.maxLimitGetterFunction);

                    if (minLimitGetterFunction == null && maxLimitGetterFunction == null)
                    {
                        Debug.Log(range.maxLimitGetterFunction);
                        Debug.Log(range.minLimitGetterFunction);
                        EditorGUI.HelpBox(position, $"The attribute 'MinMaxAttribute' of the field {property.name} is not valid", MessageType.Error);
                        return;
                    }
                    else
                    {
                        object min = range.minLimit;
                        object max = range.maxLimit;
                        if (minLimitGetterFunction != null)
                        {
                            min = minLimitGetterFunction.Invoke(serializedObjectReference, null);
                        }
                        if (maxLimitGetterFunction != null)
                        {
                            max = maxLimitGetterFunction.Invoke(serializedObjectReference, null);
                        }
                        if (min.GetType() == TypeOF.Int32 || max.GetType() == TypeOF.Int32)
                        {
                            range.isIntegerRange = true;
                            range.minLimit = Convert.ToInt32(min);
                            range.maxLimit = Convert.ToInt32(max);
                        }
                        else
                        {

                            range.minLimit = (float)min;
                            range.maxLimit = (float)max;
                        }
                    }
                }

                var minValue = property.FindPropertyRelative("min");
                var maxValue = property.FindPropertyRelative("max");

                var newMin = minValue.floatValue;
                var newMax = maxValue.floatValue;

                var xDivision = position.width * 0.33f;
                var yDivision = position.height * 0.5f;
                EditorGUI.LabelField(new Rect(position.x, position.y, xDivision, yDivision), label);

                EditorGUI.LabelField(new Rect(position.x, position.y + yDivision, position.width, yDivision), range.minLimit.ToString("0.##"));
                EditorGUI.LabelField(new Rect(position.x + position.width - 20f, position.y + yDivision, position.width, yDivision), range.maxLimit.ToString("0.##"));

                EditorGUI.MinMaxSlider(new Rect(position.x + 24f, position.y + yDivision, position.width - 48f, yDivision), ref newMin, ref newMax, range.minLimit, range.maxLimit);

                EditorGUI.LabelField(new Rect(position.x + xDivision - 15, position.y, xDivision, yDivision), "From : ");
                if (range.isIntegerRange)
                {
                    newMin = Mathf.Clamp(EditorGUI.IntField(new Rect(position.x + xDivision + 30, position.y, xDivision - 30, yDivision), (int)newMin), range.minLimit, newMax);
                    EditorGUI.LabelField(new Rect(position.x + xDivision * 2f, position.y, xDivision, yDivision), "To : ");
                    newMax = Mathf.Clamp(EditorGUI.IntField(new Rect(position.x + xDivision * 2f + 24, position.y, xDivision - 24, yDivision), (int)newMax), newMin, range.maxLimit);

                }
                else
                {
                    newMin = Mathf.Clamp(EditorGUI.FloatField(new Rect(position.x + xDivision + 30, position.y, xDivision - 30, yDivision), newMin), range.minLimit, newMax);
                    EditorGUI.LabelField(new Rect(position.x + xDivision * 2f, position.y, xDivision, yDivision), "To : ");
                    newMax = Mathf.Clamp(EditorGUI.FloatField(new Rect(position.x + xDivision * 2f + 24, position.y, xDivision - 24, yDivision), newMax), newMin, range.maxLimit);

                }

                minValue.floatValue = newMin;
                maxValue.floatValue = newMax;
            }
        }

        public void GetMinLimit()
        {

        }

        public void GetMaxLimit()
        {

        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 16;
        }
    }

}