/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfinityEngine.Attributes;
using InfinityEngine.Utils;
using UnityEditor;
using UnityEngine;

//// <summary>
//// Provides access to tools used to override the way unity draws the inspector of a component
//// </summary>
namespace InfinityEditor
{
    [Serializable]
    internal class InspectorDrawer
    {
        private static Dictionary<Type, Action<InspectorDrawer, SerializedMember>> AttributeProcessors = new Dictionary<Type, Action<InspectorDrawer, SerializedMember>>
        {
            {typeof(AccordionAttribute), (drawer, members) => drawer.PushMemberToAccordionGroup(members) },
            {typeof(TabAttribute), (drawer, members) => drawer.PushMemberToTabGroup(members) },

        };

        // The boolean field in target with the name '__help__'
        private SerializedMember showHelp;
        private List<FieldInfo> fields;
        private object serializedObject;

        protected InspectorTabGroup tabGroup;
        protected InspectorAccordionGroup accordionGroup;

        protected List<SerializedMember> serializedMembers;
        protected List<Drawer> drawers;

        protected int drawersCount;

        public int DeclaringOrder { get; set; }
        public int DrawOrder { get; set; }

        internal static InspectorDrawer Create(object serializedObject, SerializedProperty property, bool isRoot = true)
        {
            SerializedMember serializedMember;
            var serializedMembers = new List<SerializedMember>();
            var isFirstIteration = true;
            var targetType = serializedObject.GetType();
            FieldInfo fieldInfo;
            while (property.NextVisible(isFirstIteration))
            {
                if (!isRoot || !isFirstIteration)
                {
                    fieldInfo = ReflectionUtils.GetCachedField(targetType, property.name);
                    if (fieldInfo == null)
                    {
                        continue;
                    }
                    serializedMember = new SerializedField(serializedObject, property.Copy(), fieldInfo);
                    serializedMembers.Add(serializedMember);
                }
                isFirstIteration = false;
            }

            var inspectorDrawer = new InspectorDrawer
            {
                serializedObject = serializedObject,
                serializedMembers = serializedMembers
            };
            return inspectorDrawer;
        }

        internal virtual void OnEnable()
        {
            FindFields();

            if (fields == null)
                return;

            FieldInfo fieldInfo;
            SerializedMember member;
            var numberOfFields = fields.Count;
            for (var index = 0; index < numberOfFields; index++)
            {
                fieldInfo = fields[index];
                member = serializedMembers.FirstOrDefault(m => m.Name == fieldInfo.Name);

                if (member == null)
                    continue;

                member.DeclaringOrder = index;

                if (member.Name == "__help__" && fieldInfo.FieldType == typeof(bool))
                {
                    showHelp = member;
                    serializedMembers.RemoveAt(serializedMembers.IndexOf(member));
                    continue;
                }

                ProcessAttributes(member, typeof(AccordionAttribute), typeof(TabAttribute));
            }

            accordionGroup?.OnEnable();
            tabGroup?.OnEnable();
        }

        internal virtual void OnInspectorGUI()
        {
            if (CreateDrawersIfNot())
            {
                drawers.Sort(CompareDrawers);
                drawersCount = drawers.Count;
            }

            showHelp?.DrawMember();

            for (var i = 0; i < drawersCount; i++)
                drawers[i].Draw();


        }

        public void PushMember(SerializedMember member)
        {
            this.serializedMembers.Add(member);
        }

        protected bool CreateDrawersIfNot()
        {
            if (drawers == null)
            {
                drawers = new List<Drawer>();

                foreach (var member in serializedMembers)
                    drawers.Add(new Drawer(member.DeclaringOrder, member.DrawOrder, member.DrawMember));

                if (accordionGroup != null)
                    drawers.Add(new Drawer(accordionGroup.DeclaringOrder, accordionGroup.DrawOrder, accordionGroup.OnInspectorGUI));

                if (tabGroup != null)
                    drawers.Add(new Drawer(tabGroup.DeclaringOrder, tabGroup.DrawOrder, tabGroup.OnInspectorGUI));

                return true;
            }

            return false;
        }

        protected void PushMemberToAccordionGroup(SerializedMember member)
        {
            accordionGroup = accordionGroup ?? new InspectorAccordionGroup();
            accordionGroup.serializedMembers.Add(member);
        }

        protected void PushMemberToTabGroup(SerializedMember member)
        {
            tabGroup = tabGroup ?? new InspectorTabGroup();
            tabGroup.serializedMembers.Add(member);
        }

        protected void ProcessAttributes(SerializedMember member, params Type[] types)
        {
            foreach (var type in types)
            {
                var attributes = member.MemberInfo.GetCustomAttributes(type, false);
                if (attributes.Length > 0)
                {
                    AttributeProcessors[type].Invoke(this, member);
                    serializedMembers.RemoveAt(serializedMembers.IndexOf(member));
                    break;
                }
            }
        }

        protected int CompareDrawers(Drawer a, Drawer b)
        {
            if (a.drawOrder == b.drawOrder)
                return a.declaringOrder.CompareTo(b.declaringOrder);
            return a.drawOrder.CompareTo(b.drawOrder);
        }

        private void FindFields()
        {
            bool Filter(FieldInfo field)
            {
                return field.FieldType.IsPublic || ReflectionUtils.HasAttribute(field, typeof(SerializeField));
            }
            fields = ReflectionUtils.GetCachedFields(serializedObject.GetType())
                                    .Where(Filter)
                                    .ToList();

        }

        internal class Drawer
        {
            public readonly int drawOrder;
            public readonly int declaringOrder;
            public readonly Action drawCallback;

            public Drawer(int declaringOrder, int drawOrder, Action drawCallback)
            {
                this.declaringOrder = declaringOrder;
                this.drawOrder = drawOrder;
                this.drawCallback = drawCallback;
            }

            public void Draw() => drawCallback?.Invoke();
        }
    }

}