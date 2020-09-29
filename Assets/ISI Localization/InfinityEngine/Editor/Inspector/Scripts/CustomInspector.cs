/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
using UnityEngine;
using UnityEditor;
using System;
using InfinityEngine.Attributes;

namespace InfinityEditor
{

    /// <summary>
    /// Override the ways unity draw the inspector of a component
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class CustomInspector : Editor
    {

        private Type targetType;

        private InspectorDrawer inspectorDrawer;
        private DontDrawInspectorIfAttribute dontDrawAttribute;

        private bool shouldBeDrawed;
        private bool shouldOverrideInspector;

        public virtual void OnEnable()
        {
            targetType = target.GetType();
            shouldOverrideInspector = targetType.IsDefined(typeof(OverrideInspectorAttribute), true);
            if (!shouldOverrideInspector)
                return;

            shouldBeDrawed = !DontDrawInspectorIfAttribute.TryFindInvalidAttribute(target, out dontDrawAttribute);
            if (!shouldBeDrawed)
                return;

            inspectorDrawer = InspectorDrawer.Create(target, serializedObject.GetIterator());
            inspectorDrawer.OnEnable();

            EditorApplication.update += Repaint;
        }

        public void OnDisable()
        {
            if (!shouldOverrideInspector || (dontDrawAttribute != null && !shouldBeDrawed))
                return;

            EditorApplication.update -= Repaint;
        }

        public override void OnInspectorGUI()
        {
            if (dontDrawAttribute != null && !shouldBeDrawed)
            {
                if (dontDrawAttribute.IsMissingFunction)
                {
                    EditorGUILayout.HelpBox($"{nameof(DontDrawInspectorIfAttribute)} attribute required a function", MessageType.Error);
                }
                else
                {
                    EditorGUILayout.HelpBox(dontDrawAttribute.Message, MessageType.Error);
                }
                return;
            }

            if (!shouldOverrideInspector)
            {
                base.OnInspectorGUI();
                return;
            }

            serializedObject?.Update();
            inspectorDrawer?.OnInspectorGUI();

            if (GUI.changed)
                serializedObject.ApplyModifiedProperties();
        }

    }

}