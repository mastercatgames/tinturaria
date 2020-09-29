/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *                                                                                                          *
*************************************************************************************************************************************/


using UnityEngine;
using InfinityEngine.Attributes;
using UnityEditor;
using InfinityEngine.Utils;

namespace InfinityEditor
{

    /// <summary>
    ///   Custom <see cref="PropertyDrawer"/> of <see cref="InfinityHeaderAttribute"/>
    /// </summary>
    [CustomPropertyDrawer(typeof(InfinityHeaderAttribute))]
    public class InfinityHeaderDrawer : PropertyDrawer
    {

        private string expectedPropertyName = "__help__";
        private InfinityHeaderAttribute target;

        /// <summary>
        /// Called when unity draws the attribute.
        /// </summary>
        /// <param name="position">The position where to draw the attribute</param>
        /// <param name="property">The SerializedProperty of the attribute</param>
        /// <param name="label">The label of the attribute</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            target = attribute as InfinityHeaderAttribute;
            if (property.name != expectedPropertyName)
            {
                EditorGUI.HelpBox(position, $"The name of the field {property.name} must be {expectedPropertyName}", MessageType.Error);
                return;
            }
            if (property.type != "bool")
            {
                EditorGUI.HelpBox(position, $"The type of the field {property.name} must be a boolean", MessageType.Error);
                return;
            }
            EditorGUI.BeginProperty(position, label, property);
            property.boolValue = DrawHeader(position, property.boolValue);
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Called when unity calcuate the height necessaty to display the attribute.
        /// </summary>
        /// <param name="property">The SerializedProperty of the attribute</param>
        /// <param name="label">The label of the attribute</param>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 40;
        }

        private bool DrawHeader(Rect rect, bool showHelp)
        {
            EditorGUI.indentLevel++;
            if (GUI.Button(new Rect(rect.x, rect.y, 64, 32), AssetReferences.Logo, GUI.skin.label))
            {
                Application.OpenURL("http://u3d.as/riS");
            }
            EditorGUI.indentLevel--;

            if (GUI.Button(new Rect(rect.x + 64, rect.y, 64, 32), showHelp ? AssetReferences.HelpIconEnable : AssetReferences.HelpIconDisable, GUI.skin.label))
            {
                showHelp = !showHelp;
            }

            if (!string.IsNullOrEmpty(target.OnlineDocUrl))
            {
                if (GUI.Button(new Rect(rect.x + 64 * 2, rect.y, 64, 32), AssetReferences.DocIcon, GUI.skin.label))
                {
                    Application.OpenURL(target.OnlineDocUrl);
                }
            }

            return showHelp;
        }

    }
}