/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
using System;
using UnityEngine;
using UnityEditor;
using InfinityEngine.Utils;

namespace InfinityEditor
{


    /// <summary>
    /// Drawer class of <see cref="List{T}"/> and <see cref="System.Array"/> objects
    /// </summary>
    internal class InspectorArrayDrawer : InspectorCollectionDrawer
    {

        private SerializedProperty property;
        private SerializedProperty currentElement;

        internal InspectorArrayDrawer(SerializedProperty property) : base()
        {
            if (!property.isArray)
            {
                throw new ArgumentException("The argument property must be an array or list property");
            }
            this.property = property;
            this.Accordion.Title = ObjectNames.NicifyVariableName(property.name);

        }

        internal override Rect OnHeaderGUI()
        {
            var rect = base.OnHeaderGUI();

            var deleteBtnRect = new Rect(rect.width - 10, rect.y, 14, rect.height);

            if (DrawerHelper.FAButton(deleteBtnRect, FA.plus, FAOption.FontSize(14)))
            {
                property.arraySize++;
                if (!IsExpanded)
                {
                    IsExpanded = true;
                }
            }
            
            return rect;
        }

        internal override void OnDrawElements()
        {

            ElementCount = property.arraySize;
            for (int index = 0; index < ElementCount; index++)
            {
                EditorGUI.indentLevel++;
 
                LastRect = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                currentElement = property.GetArrayElementAtIndex(index);

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(currentElement, new GUIContent(currentElement.displayName), true, GUILayout.Width(((70 /100.0f) * EditorGUIUtility.currentViewWidth)));

                GUILayout.FlexibleSpace();

                if (HasClickEvent(FA.arrow_down))
                {
                    property.MoveArrayElement(index, index + 1);
                    break;
                }
                if (HasClickEvent(FA.arrow_up))
                {
                    property.MoveArrayElement(index, index - 1);
                    break;
                }
              
                if (HasClickEvent(FA.trash))
                {
                    property.DeleteArrayElementAtIndex(index);
                    break;
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);

                EditorGUILayout.EndVertical();

                LastRect = GUILayoutUtility.GetLastRect();

                EditorGUI.indentLevel--;
            }          
        }

        private bool HasClickEvent(string icon)
        {
            GUILayout.Space(5);
            return DrawerHelper.FAButton(icon, FAOption.FontSize(10), FAOption.TextAnchor(TextAnchor.MiddleCenter));
  
        }

    }
}