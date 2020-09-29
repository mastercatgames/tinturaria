/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using InfinityEngine.Attributes;
using UnityEditorInternal;
using InfinityEngine.Utils;

namespace InfinityEditor
{

    internal class SerializedField : SerializedMember
    {
        private static Type CustomDrawerType = typeof(CustomDrawerAttribute);

        private ReorderableList m_reorderableList;
        private ReorderableAttribute m_reordableAttribute;
        private SimpleAccordion m_classDrawerAccordion;
        private InspectorDrawer m_classDrawer;

        private FieldInfo m_fieldInfo;
        private Type m_fieldType;

        public SerializedField(object declaringObject, SerializedProperty serializedProperty, MemberInfo memberInfo) : base(declaringObject, serializedProperty, memberInfo)
        {
            this.m_fieldInfo = memberInfo as FieldInfo;
            this.m_fieldType = m_fieldInfo.FieldType;
            this.m_reordableAttribute = ReflectionUtils.GetAttribute<ReorderableAttribute>(m_fieldInfo);
        }

        internal override void DrawMember()
        {
            if(m_fieldType.IsDefined(CustomDrawerType, true))
            { 
                if(m_classDrawer == null)
                {
                    m_classDrawer = InspectorDrawer.Create(m_fieldInfo.GetValue(DeclaringObject), SerializedProperty.Copy(), false);
                    m_classDrawerAccordion = new SimpleAccordion(DisplayName, () =>
                    {
                        m_classDrawer.OnEnable();
                        m_classDrawer.OnInspectorGUI();

                    });
                    m_classDrawerAccordion.IsExpanded = SerializedProperty.isExpanded;
                    m_classDrawerAccordion.expandStateChangeCallback = acc => SerializedProperty.isExpanded = acc.IsExpanded;

                    m_classDrawerAccordion.drawHeaderCallback = ()=> SimpleAccordion.DrawDefaultAccordionHeader(m_classDrawerAccordion, FA.code, 14, 8);
                }

                DrawMember(m_classDrawerAccordion.OnGUI);
                return;
            }

            if (IsListOrArray(m_fieldType))
            {
                if(m_reordableAttribute != null)
                {
                    if (m_reorderableList == null)
                    {
                        m_reorderableList = CreateReordableList(SerializedProperty.serializedObject, SerializedProperty);
                    }
                    DrawMember(m_reorderableList.DoLayoutList);
                }
                else
                {
                    DrawMember(DrawCollection);                 
                }
            }
            else
            {
                DrawMember(DrawPropertyField);
            }
        }

        private ReorderableList CreateReordableList(SerializedObject obj, SerializedProperty prop)
        {
            var list = new ReorderableList(obj, prop, true, true, true, true);

            list.drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, prop.displayName);
            };

            list.drawElementCallback = (rect, index, active, focused) => {
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
              
                rect.x += 10;
                rect.width -= 10;
                EditorGUI.PropertyField(rect, element, new GUIContent(element.displayName), true);
            };

            list.elementHeightCallback = (index) => {
                return EditorGUI.GetPropertyHeight(list.serializedProperty.GetArrayElementAtIndex(index),GUIContent.none, true);
            };       
            return list;
        }
    }

}