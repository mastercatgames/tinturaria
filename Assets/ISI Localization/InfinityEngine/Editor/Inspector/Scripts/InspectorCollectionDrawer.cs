/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
using InfinityEngine.Utils;
using System;
using UnityEngine;

namespace InfinityEditor
{

    /// <summary>
    /// Bases drawer class of all <see cref="System.Collections.Generic"/> and <see cref="System.Array"/> objects 
    /// </summary>
    [Serializable]
    internal abstract class InspectorCollectionDrawer
    {

        private Event currentEvent;

        internal Rect LastRect { get; set; }

        internal SimpleAccordion Accordion { get; set; }

        internal int ElementCount { get; set; }

        internal bool IsExpanded
        {
            get { return Accordion.IsExpanded; }
            set { Accordion.IsExpanded = value; }
        }

        internal InspectorCollectionDrawer()
        {
            this.Accordion = new SimpleAccordion(OnHeaderGUI, OnDrawElements);
        }

        internal virtual Rect OnHeaderGUI()
        {
            return SimpleAccordion.DrawDefaultAccordionHeader(Accordion, FA.list_alt, 14, 8);          
        }

        internal abstract void OnDrawElements();

        internal bool IsLastRectContainsClickPosition()
        {
            if (currentEvent.type == EventType.MouseDown && LastRect.Contains(currentEvent.mousePosition))
            {
                currentEvent.Use();
                return true;
            }
            return false;
        }

        internal virtual void OnGUI()
        {
            Accordion.OnGUI();
        }

    }

}