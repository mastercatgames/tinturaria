/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

namespace InfinityEditor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using InfinityEngine.Utils;
    using UnityEngine.Events;
    using InfinityEngine.Extensions;
    using System.Linq;

    /// <summary>
    /// Helper class that provides static functions for drawing object fields
    /// </summary>
    public static class DrawerHelper
    {

        private static HashSet<string> FaStrings;

        private static Dictionary<Type, Action<object, FieldInfo>> FieldDrawers = new Dictionary<Type, Action<object, FieldInfo>>()
        {
            { typeof(int), IntField }, { typeof(float), FloatField },  { typeof(double), DoubleField },

            { typeof(long), LongField },{ typeof(bool), BoolField },  { typeof(string), TextField },

            { typeof(Vector2) , Vector2Field}, { typeof(Vector3) , Vector3Field}, { typeof(Vector4) , Vector4Field},

            { typeof(Color) , ColorField},  { typeof(Quaternion) , QuaternionField},  { typeof(Bounds) , BoundsField},

            { typeof(AnimationCurve) , CurveField}, {typeof(Rect), RectField}, {typeof(LayerMask), LayerField}
        };

        private static Dictionary<Type, Func<string, object, object>> StaticDrawers = new Dictionary<Type, Func<string, object, object>>()
        {
            { typeof(int), IntField }, { typeof(float), FloatField },  { typeof(double), DoubleField },

            { typeof(long), LongField },{ typeof(bool), BoolField },  { typeof(string), TextField },

            { typeof(Vector2) , Vector2Field}, { typeof(Vector3) , Vector3Field}, { typeof(Vector4) , Vector4Field},

            { typeof(Color) , ColorField},  { typeof(Quaternion) , QuaternionField},  { typeof(Bounds) , BoundsField},

            { typeof(AnimationCurve) , CurveField}, {typeof(Rect), RectField}, {typeof(LayerMask), LayerField}
        };

        private static Action<object, FieldInfo> FieldDrawer;
        private static Func<string, object, object> StaticDrawer;
        private static Type FieldType;

        internal static bool IsFontAwesomeString(string value)
        {
            if (FaStrings == null)
            {
                FaStrings = ReflectionUtils.GetCachedFields(typeof(FA))
                                           .Select(f => (string)f.GetValue(null))
                                           .ToArray()
                                           .ToHashSet();
            }
            return FaStrings.Contains(value);
        }


        internal static bool ContainsFontAwesomeString(string value)
        {
            if (FaStrings == null)
            {
                FaStrings = ReflectionUtils.GetCachedFields(typeof(FA)).Select(f => (string)f.GetValue(null))
                                           .ToArray()
                                           .ToHashSet();
            }
            return value.Split().Any(elem => FaStrings.Contains(elem));
        }


        internal static bool CanDraw(Type type)
        {
            return StaticDrawers.ContainsKey(type);
        }

        public static void Draw(object target, FieldInfo field)
        {

            FieldType = field.FieldType;
            if (TypeOF.Object.IsAssignableFrom(FieldType))
            {
                ObjectField(target, field);
            }
            else if (FieldType.IsEnum)
            {
                if (FieldType.IsDefined(TypeOF.FlagsAttribute, false))
                {
                    EnumMaskField(target, field);
                }
                else
                {
                    EnumPopup(target, field);
                }
            }
            else
            {
                if (FieldDrawers.TryGetValue(field.FieldType, out FieldDrawer))
                {
                    ProcessSpaceAttribute(field);
                    FieldDrawer.Invoke(target, field);
                }
            }

        }
        public static object Draw(string label, object value, Type type)
        {
            label = string.IsNullOrEmpty(label) ? String.Empty : ObjectNames.NicifyVariableName(label);

            if (TypeOF.Object.IsAssignableFrom(type))
            {
                return ObjectField(label, value, type);
            }

            if (type.IsEnum)
            {
                if (type.IsDefined(TypeOF.FlagsAttribute, false))
                {
                    return EnumMaskField(label, value);
                }
                return EnumPopup(label, value);
            }

            if (StaticDrawers.TryGetValue(type, out StaticDrawer))
            {
                return StaticDrawer.Invoke(label, value);
            }
            return null;
        }

        private static object IntField(string label, object value)
        {
            return EditorGUILayout.IntField(label, (int)value);
        }
        private static object FloatField(string label, object value)
        {
            return EditorGUILayout.FloatField(label, (float)value);
        }
        private static object DoubleField(string label, object value)
        {
            return EditorGUILayout.DoubleField(label, (double)value);
        }
        private static object LongField(string label, object value)
        {
            return EditorGUILayout.LongField(label, (long)value);
        }
        private static object BoolField(string label, object value)
        {
            return EditorGUILayout.Toggle(label, (bool)value);
        }
        private static object TextField(string label, object value)
        {
            return EditorGUILayout.TextField(label, (string)value);
        }
        private static object Vector2Field(string label, object value)
        {
            return EditorGUILayout.Vector2Field(label, (Vector2)value);
        }
        private static object Vector3Field(string label, object value)
        {
            return EditorGUILayout.Vector3Field(label, (Vector3)value);
        }
        private static object Vector4Field(string label, object value)
        {
            return EditorGUILayout.Vector4Field(label, (Vector4)value);
        }
        private static object QuaternionField(string label, object value)
        {
            return Quaternion.Euler(EditorGUILayout.Vector3Field(label, ((Quaternion)value).eulerAngles));
        }
        private static object ColorField(string label, object value)
        {
            return EditorGUILayout.ColorField(label, (Color)value);
        }
        private static object RectField(string label, object value)
        {
            return EditorGUILayout.RectField(label, (Rect)value);
        }
        private static object BoundsField(string label, object value)
        {
            return EditorGUILayout.BoundsField(label, (Bounds)value);
        }
        private static object CurveField(string label, object value)
        {
            return EditorGUILayout.CurveField(label, (AnimationCurve)value);
        }
        private static object LayerField(string label, object value)
        {
            return (LayerMask)EditorGUILayout.LayerField(label, (LayerMask)value);
        }
        private static object EnumPopup(string label, object value)
        {
            return EditorGUILayout.EnumPopup(label, (Enum)value);
        }
        private static object EnumMaskField(string label, object value)
        {
            return EditorGUILayout.EnumFlagsField(label, (Enum)value);
        }
        private static object EnumMaskPopup(string label, object value)
        {
            return EditorGUILayout.EnumFlagsField(string.IsNullOrEmpty(label) ? GUIContent.none : new GUIContent(label), (Enum)value);
        }

        private static object ObjectField(string label, object value, Type type)
        {
            return EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, type, true);

        }


        private static void IntField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            object newValue;
            var rangeAttribute = ReflectionUtils.GetAttribute<RangeAttribute>(field);
            if (rangeAttribute == null)
            {
                newValue = EditorGUILayout.IntField(label, (int)oldValue);
            }
            else
            {
                newValue = EditorGUILayout.IntSlider(label, (int)oldValue, (int)rangeAttribute.min, (int)rangeAttribute.max);
            }
            field.SetValue(target, newValue);
        }
        private static void FloatField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            object newValue;
            var rangeAttribute = ReflectionUtils.GetAttribute<RangeAttribute>(field);
            if (rangeAttribute == null)
            {
                newValue = EditorGUILayout.FloatField(label, (float)oldValue);
            }
            else
            {
                newValue = EditorGUILayout.Slider(label, (float)oldValue, rangeAttribute.min, rangeAttribute.max);
            }
            field.SetValue(target, newValue);
        }
        private static void DoubleField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.DoubleField(label, (double)oldValue));
        }
        private static void LongField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.LongField(label, (long)oldValue));
        }
        private static void BoolField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            object newValue;
            newValue = EditorGUILayout.Toggle(label, (bool)oldValue);
            field.SetValue(target, newValue);
        }
        private static void TextField(object target, FieldInfo field)
        {

            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            object newValue;

            if (oldValue == null)
            {
                oldValue = String.Empty;
            }
            var textAreaAttribute = ReflectionUtils.GetAttribute<TextAreaAttribute>(field);
            if (textAreaAttribute == null)
            {
                newValue = EditorGUILayout.TextField(label, (string)oldValue);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(label);
                newValue = EditorGUILayout.TextArea((string)oldValue);
                EditorGUILayout.EndHorizontal();
            }
            field.SetValue(target, newValue);
        }
        private static void Vector2Field(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.Vector2Field(label, (Vector2)oldValue));
        }
        private static void Vector3Field(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.Vector3Field(label, (Vector3)oldValue));
        }
        private static void Vector4Field(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.Vector4Field(label, (Vector4)oldValue));
        }
        private static void QuaternionField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, Quaternion.Euler(EditorGUILayout.Vector3Field(label, ((Quaternion)oldValue).eulerAngles)));
        }
        private static void ColorField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.ColorField(label, (Color)oldValue));
        }
        private static void CurveField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            if (oldValue == null)
            {
                oldValue = new AnimationCurve();
            }
            field.SetValue(target, EditorGUILayout.CurveField(label, (AnimationCurve)oldValue));
        }
        private static void RectField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.RectField(label, (Rect)oldValue));
        }
        private static void BoundsField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.BoundsField(label, (Bounds)oldValue));
        }
        private static void LayerField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, (LayerMask)EditorGUILayout.LayerField(label, (LayerMask)oldValue));
        }
        private static void EnumPopup(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.EnumPopup(label, (Enum)oldValue));
        }
        private static void EnumMaskField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.EnumFlagsField(label, (Enum)oldValue));
        }
        private static void EnumMaskPopup(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.EnumFlagsField(new GUIContent(label), (Enum)oldValue));
        }
        private static void ObjectField(object target, FieldInfo field)
        {
            var label = ObjectNames.NicifyVariableName(field.Name);
            var oldValue = field.GetValue(target);
            field.SetValue(target, EditorGUILayout.ObjectField(label, (UnityEngine.Object)oldValue, field.FieldType, true));
        }

        public static bool FAButton(string icon, params FAOption[] options)
        {
            var style = new GUIStyle(AssetReferences.FontAwesome);
            style.normal.textColor = Color.black;
            foreach (var op in options)
            {
                op.ApplyTo(style);
            }
            return GUILayout.Button(icon, style);
        }

        public static bool FAButton(Rect rect, string icon, params FAOption[] options)
        {
            var style = new GUIStyle(AssetReferences.FontAwesome);
            style.normal.textColor = Color.black;
            foreach (var op in options)
            {
                op.ApplyTo(style);
            }
            var e = Event.current;
            if (rect.Contains(e.mousePosition))
            {
                style.normal.textColor = Color.white;
            }
            return GUI.Button(rect, icon, style);
        }

        public static void FAIcon(string icon, params FAOption[] options)
        {
            var style = new GUIStyle(AssetReferences.FontAwesome);
            style.normal.textColor = Color.black;
            foreach (var op in options)
            {
                op.ApplyTo(style);
            }

            GUILayout.Label(icon, style);
        }

        public static void FAIcon(Rect rect, string icon, params FAOption[] options)
        {
            var style = new GUIStyle(AssetReferences.FontAwesome);
            style.normal.textColor = Color.black;
            foreach (var op in options)
            {
                op.ApplyTo(style);
            }
            GUI.Label(rect, icon, style);
        }

        private static void ProcessSpaceAttribute(FieldInfo field)
        {
            var space = ReflectionUtils.GetAttribute<SpaceAttribute>(field);
            if (space != null)
            {
                GUILayout.Space(space.height);
            }
        }
    }
}