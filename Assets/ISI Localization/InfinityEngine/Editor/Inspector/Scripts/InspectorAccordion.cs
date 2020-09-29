/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using InfinityEngine.Attributes;

namespace InfinityEditor
{

    internal class InspectorAccordion : InspectorDrawer
    {

        private SimpleAccordion m_accordion;
        private string m_isExpandedPref;
     
        public readonly string title;
        public Action<InspectorAccordion> onExpandStateChanged;

        private string IsExpandedPref
        {
            get
            {
                if (serializedMembers.Any() && string.IsNullOrEmpty(m_isExpandedPref))
                {
                    var firstMember = serializedMembers[0];
                    if (string.IsNullOrEmpty(m_isExpandedPref))
                    {
                        m_isExpandedPref = string.Concat(firstMember.DeclaringObject.GetType(), "_", firstMember.SerializedObjectInstanceId, "_accordion_", DeclaringOrder);
                    }
                }
                return m_isExpandedPref;

            }
        }

        public bool IsExpanded
        {
            get { return m_accordion.IsExpanded; }
            set { m_accordion.IsExpanded = value; }
        }

        public InspectorAccordion(string title)
        {
            this.title = title;
            this.serializedMembers = new List<SerializedMember>();
            this.m_accordion = new SimpleAccordion(title, OnDrawAccordionContents);
            this.m_accordion.expandStateChangeCallback = (acc) =>
            {
                if (serializedMembers.Any())
                {
                    InspectorData.SetBool(IsExpandedPref, acc.IsExpanded);
                }

                if (onExpandStateChanged != null)
                {
                    onExpandStateChanged.Invoke(this);            
                }
            };
        }

        internal override void OnEnable()
        {
            var numberOfMembers = serializedMembers.Count;
            if(numberOfMembers > 0)
            {
                DeclaringOrder = serializedMembers[0].DeclaringOrder;
                m_accordion.IsExpanded = InspectorData.GetBool(IsExpandedPref);
               
            }
            var members = new List<SerializedMember>(serializedMembers);
            for (int i = 0; i < numberOfMembers; i++)
            {
                ProcessAttributes(members[i], typeof(TabAttribute));
            }

            if (tabGroup != null)
            {
                tabGroup.OnEnable();
            }
        }

        internal override void OnInspectorGUI()
        {
            if((serializedMembers != null && serializedMembers.Any(m => m.ShouldBeDrawed)) || (tabGroup != null && tabGroup.CanBeDrawed))
            {
                m_accordion.OnGUI();
            }
        }

        private void OnDrawAccordionContents()
        {
            if (CreateDrawersIfNot())
            {
                drawers.Sort(CompareDrawers);
                drawersCount = drawers.Count;
            }

            for (var i = 0; i < drawersCount; i++)
            {
                drawers[i].Draw();
            }
        }
    }

}