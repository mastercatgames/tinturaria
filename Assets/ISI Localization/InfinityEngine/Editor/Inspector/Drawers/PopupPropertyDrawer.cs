/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *                                                                                                          *
*************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine.Utils;
using InfinityEngine.Extensions;
using UnityEditor;
using InfinityEngine.Attributes;
using System;
using System.Linq;

namespace InfinityEditor
{
    /// <summary>
    ///   Custom <see cref="PropertyDrawer"/> of <see cref="PopupAttribute"/>
    /// </summary>
    [CustomPropertyDrawer(typeof(PopupAttribute))]
    public class PopupPropertyDrawer : PropertyDrawer
    {
        private Type serializedObjectType;
        private object serializedObjectReference;
        private PopupAttribute target;
        private float searchBoxHeight;
        private bool isInSearch;
        private string searchValue;
        private string[] searchResult;

        private string[] m_options;

        private string[] Options
        {
            get
            {
                if (m_options == null)
                {
                    switch (target.valueType)
                    {
                        case PopupValueTypes.String:
                            m_options = target.values.Split(new char[] { target.separator }, StringSplitOptions.RemoveEmptyEntries);
                            break;
                        case PopupValueTypes.PlayerPref:
                            m_options = PlayerPrefs.GetString(target.values, "").Split(new char[] { target.separator }, StringSplitOptions.RemoveEmptyEntries);
                            break;
                        case PopupValueTypes.Method:
                            var method = ReflectionUtils.GetCachedMethod(serializedObjectType, target.values);
                            if (method.ReturnType == typeof(string[]))
                            {
                                m_options = (string[])method.Invoke(serializedObjectReference, null);
                            }
                            else if (method.ReturnType == typeof(string))
                            {
                                var tmp = (string)method.Invoke(serializedObjectReference, null);
                                m_options = tmp.Split(new char[] { target.separator }, StringSplitOptions.RemoveEmptyEntries);
                            }

                            break;
                    }
                }
                return m_options;
            }
        }


        /// <summary>
        /// Called when unity draws the attribute.
        /// </summary>
        /// <param name="position">The position where to draw the attribute</param>
        /// <param name="property">The SerializedProperty of the attribute</param>
        /// <param name="label">The label of the attribute</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (serializedObjectReference == null)
            {
                serializedObjectReference = property.serializedObject.targetObject;
                serializedObjectType = property.serializedObject.targetObject.GetType();
            }

            target = attribute as PopupAttribute;
            Initialize(property.stringValue);

            EditorGUI.BeginProperty(position, GUIContent.none, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            if (!isInSearch)
            {
                target.selectedIndex = EditorGUI.Popup(position, target.selectedIndex, Options);
            }
            if (target.enableSearch)
            {
                if (GUI.Button(new Rect(position.x - 20, position.y, 20, 15), AssetReferences.SearchIcon, EditorStyles.label))
                {
                    isInSearch = !isInSearch;

                }
                if (isInSearch)
                {
                    var rect = new Rect(position.x, position.y, position.width, position.height);
                    rect.height = EditorGUIUtility.singleLineHeight;
                    if (string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = String.Empty;
                    }
                    searchValue = GUI.TextField(rect, searchValue);
                    if (!string.IsNullOrEmpty(searchValue) && searchResult != null)
                    {
                        rect.height = EditorGUIUtility.singleLineHeight;
                        var searchBox = new Rect(rect.x, rect.y, rect.width, position.height);
                        GUI.Box(searchBox, GUIContent.none, EditorStyles.helpBox);
                        foreach (var s in searchResult)
                        {
                            rect.y += EditorGUIUtility.singleLineHeight;
                            if (GUI.Button(rect, s, EditorStyles.boldLabel))
                            {
                                target.selectedIndex = Options.IndexOf(s);
                                isInSearch = false;
                            }
                        }
                    }
                }
            }

            if (target.selectedIndex >= Options.Length || Options.Length == 0)
            {
                property.stringValue = String.Empty;
            }
            else
            {
                property.stringValue = Options[target.selectedIndex];
            }
            EditorGUI.EndProperty();
        }


        /// <summary>
        /// Called when unity calcuates the height necessaty to display the attribute.
        /// </summary>
        /// <param name="property">The SerializedProperty of the attribute</param>
        /// <param name="label">The label of the attribute</param>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (serializedObjectReference == null)
            {
                serializedObjectReference = property.serializedObject.targetObject;
                serializedObjectType = property.serializedObject.targetObject.GetType();
            }
            target = attribute as PopupAttribute;
            if (isInSearch && !string.IsNullOrEmpty(searchValue))
            {
                searchResult = Options
                    .Where(s => !string.IsNullOrEmpty(s) && s.ToLower().Contains(searchValue.ToLower()))
                    .Take(5)
                    .ToArray();

                if (searchResult.Length > 0)
                {
                    searchBoxHeight = EditorGUIUtility.singleLineHeight * searchResult.Length;
                    return searchBoxHeight + EditorGUIUtility.singleLineHeight;
                }
            }

            searchResult = null;
            return base.GetPropertyHeight(property, label);
        }

        /// <summary>
        /// Finds the index of the current selected item in the enum based on the value of the given string.      
        ///  </summary>
        /// <param name="value">The value. </param>
        private void Initialize(string value)
        {
            target.selectedIndex = 0;
            if (!string.IsNullOrEmpty(value) && Options.Length > 0)
            {
                target.selectedIndex = Options.IndexOf(value);
            }
            if (target.selectedIndex < 0)
            {
                target.selectedIndex = 0;
            }
        }
    }
}