/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

using System.Collections.Generic;

namespace InfinityEditor
{

    internal class TabDrawer : InspectorDrawer
    {
        public readonly string title;

        public TabDrawer(string title)
        {
            this.title = title;
            serializedMembers = new List<SerializedMember>();
        }
        internal override void OnEnable()
        {
            return;
        }
        internal override void OnInspectorGUI()
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
