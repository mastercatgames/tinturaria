/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using InfinityEngine.Attributes;
using InfinityEngine.Utils;

namespace InfinityEditor
{


    internal class InspectorTabGroup : InspectorDrawer
    {

        private Dictionary<string, TabDrawer> m_tabs;
        private string[] m_toolbar;
        private int m_currentTab;

        public bool CanBeDrawed => serializedMembers.Any(m => m.ShouldBeDrawed);

        public InspectorTabGroup()
        {
            this.serializedMembers = new List<SerializedMember>();
        }

        internal override void OnEnable()
        {
            if (m_tabs == null)
            {
                m_tabs = BuilTabs(serializedMembers.GroupBy(member => ReflectionUtils.GetAttribute<TabAttribute>(member.MemberInfo)));
                BuildToolbar();
            }
        }

        internal override void OnInspectorGUI()
        {
            m_currentTab = GUILayout.Toolbar(m_currentTab, m_toolbar, EditorStyles.toolbarButton);
            m_tabs[m_toolbar[m_currentTab]].OnInspectorGUI();
        }

        private Dictionary<string, TabDrawer> BuilTabs(IEnumerable<IGrouping<TabAttribute, SerializedMember>> enumerable)
        {
            var dict = new Dictionary<string, TabDrawer>();
            TabDrawer tab;
            var isFirstItem = false;
            var groupDrawOrder = 0;
            foreach (var group in enumerable)
            {
                groupDrawOrder = group.Key.DrawOrder;
                if (!dict.TryGetValue(group.Key.Title, out tab))
                {
                    tab = new TabDrawer(group.Key.Title);
                    tab.DrawOrder = groupDrawOrder;
                    dict.Add(group.Key.Title, tab);
                }

                if (groupDrawOrder != 0 && groupDrawOrder != tab.DrawOrder)
                {
                    tab.DrawOrder = groupDrawOrder;
                }

                if (groupDrawOrder < DrawOrder)
                {
                    DrawOrder = group.Key.DrawOrder;
                }

                foreach (var item in group)
                {
                    if (isFirstItem)
                    {
                        DeclaringOrder = item.DeclaringOrder;
                        isFirstItem = false;
                    }
                    tab.PushMember(item);
                }
            }

            return dict;
        }

        private void BuildToolbar()
        {
            m_toolbar = m_tabs.Values.OrderBy(tab => tab.DrawOrder).Select(tab => tab.title).ToArray();
        }
    }
}