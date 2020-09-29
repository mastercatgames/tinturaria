/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

using System.Linq;
using System.Collections.Generic;
using InfinityEngine.Attributes;
using InfinityEngine.Utils;

namespace InfinityEditor
{

    internal class InspectorAccordionGroup : InspectorDrawer
    {
        private Dictionary<string, InspectorAccordion> m_accordionDrawers;

        public bool isSingleMode;

        public InspectorAccordionGroup()
        {
            this.serializedMembers = new List<SerializedMember>();
        }

        internal override void OnEnable()
        {
            if (m_accordionDrawers == null)
            {
                m_accordionDrawers = BuildDict(serializedMembers.GroupBy(member => ReflectionUtils.GetAttribute<AccordionAttribute>(member.MemberInfo)));

                if (isSingleMode && m_accordionDrawers.Any())
                {
                    AddListeners();
                }
            }

            if (m_accordionDrawers != null && m_accordionDrawers.Any())
            {
                foreach (var drawer in m_accordionDrawers)
                {
                    drawer.Value.OnEnable();
                }
            }

            serializedMembers.Clear();
        }

        internal override void OnInspectorGUI()
        {
            if (CreateDrawersIfNot())
            {
                if (m_accordionDrawers != null && m_accordionDrawers.Any())
                {
                    foreach (var accordion in m_accordionDrawers)
                        drawers.Add(new Drawer(accordion.Value.DeclaringOrder, accordion.Value.DrawOrder, accordion.Value.OnInspectorGUI));
                }

                drawers.Sort(CompareDrawers);
                drawersCount = drawers.Count;
            }

            for (var i = 0; i < drawersCount; i++)
                drawers[i].Draw();
        }

        private Dictionary<string, InspectorAccordion> BuildDict(IEnumerable<IGrouping<AccordionAttribute, SerializedMember>> enumerable)
        {
            var dict = new Dictionary<string, InspectorAccordion>();
            InspectorAccordion accordion;
            var isFirstItem = false;
            var groupDrawOrder = 0;
            foreach (var group in enumerable)
            {
                groupDrawOrder = group.Key.DrawOrder;
                if (!dict.TryGetValue(group.Key.Title, out accordion))
                {
                    accordion = new InspectorAccordion(group.Key.Title);
                    accordion.DrawOrder = groupDrawOrder;
                    dict.Add(group.Key.Title, accordion);
                }

                if (groupDrawOrder != 0 && groupDrawOrder != accordion.DrawOrder)
                    accordion.DrawOrder = groupDrawOrder;

                if (groupDrawOrder < DrawOrder)
                    DrawOrder = group.Key.DrawOrder;

                if (group.Key.IsSingleMode)
                    isSingleMode = true;

                foreach (var item in group)
                {
                    if (isFirstItem)
                    {
                        DeclaringOrder = item.DeclaringOrder;
                        isFirstItem = false;
                    }
                    accordion.PushMember(item);
                }
            }
            return dict;
        }

        private void AddListeners()
        {
            foreach (var accordion in m_accordionDrawers)
                accordion.Value.onExpandStateChanged = Handle;

            void Handle(InspectorAccordion clickedItem)
            {
                if (clickedItem.IsExpanded)
                {
                    foreach (var it in m_accordionDrawers)
                    {
                        if (clickedItem != it.Value)
                            it.Value.IsExpanded = !clickedItem.IsExpanded;
                    }
                }
            }
        }

    }
}