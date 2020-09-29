/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
using System.Reflection;
using System.Collections;
using UnityEditor;
using UnityEngine;
using InfinityEngine.Utils;
using InfinityEngine.Attributes;
using InfinityEngine;

namespace InfinityEditor
{
    /// <summary>
    /// Encapsulates the informations of the method linked to an <see cref="InspectorButtonAttribute"/>.
    /// </summary>
    internal class InspectorButtonMethod
    {
        private readonly object declaringObject;
        private readonly SimpleAccordion accordion;
        private readonly InspectorButtonAttribute attribute;

        private readonly MethodInfo method;
        private readonly MethodInfo checkIfShouldBeDrawed;
        private readonly ParameterInfo[] paramsInfos;

        private readonly object[] arguments;
        private readonly string errorMessage;

        private bool HasParams => paramsInfos.Length > 0;
        private bool IsCoroutine { get; set; }
        private bool HasFontAwesomeTitile { get; set; }

        public InspectorButtonLocations Location => attribute.Location;

        public InspectorButtonMethod(InspectorButtonAttribute attribute, object target)
        {
            this.attribute = attribute;
            declaringObject = target;
            method = ReflectionUtils.GetCachedMethod(target.GetType(), attribute.MethodName);
            if (method == null)
            {
                errorMessage = $"{nameof(InspectorButtonAttribute)} refers to a missing method '{attribute.MethodName}' in '{target.GetType()}' class";
                return;
            }

            checkIfShouldBeDrawed = ReflectionUtils.GetCachedMethod(target.GetType(), $"__{attribute.MethodName}__");

            HasFontAwesomeTitile = DrawerHelper.ContainsFontAwesomeString(attribute.Label);
            IsCoroutine = method.ReturnType == typeof(IEnumerator);
            paramsInfos = method.GetParameters();

            arguments = new object[paramsInfos.Length];

            var title = ObjectNames.NicifyVariableName(attribute.MethodName);
            accordion = new SimpleAccordion($"{title} Function Arguments", DrawArguments);
            accordion.drawHeaderCallback = () => SimpleAccordion.DrawDefaultAccordionHeader(accordion, 14, 8);

            var index = 0;
            foreach (var param in paramsInfos)
            {
                arguments[index] = ReflectionUtils.DefaultValue(param.ParameterType);
                index++;
            }
        }

        private void DrawArguments()
        {
            EditorGUILayout.BeginVertical();
            ParameterInfo param;
            for (var i = 0; i < paramsInfos.Length; i++)
            {
                param = paramsInfos[i];
                arguments[i] = DrawerHelper.Draw(ObjectNames.NicifyVariableName(param.Name), arguments[i], param.ParameterType);
            }
            EditorGUILayout.EndVertical();
        }

        public void Draw()
        {
            if (method == null)
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
            }
            if (checkIfShouldBeDrawed != null)
            {
                if (!(bool)checkIfShouldBeDrawed.Invoke(declaringObject, null))
                    return;
            }

            if (attribute.Width != 0 && attribute.Height != 0)
            {
                if (attribute.Center)
                {
                    var rect = EditorUtils.GetCenteredRect(attribute.Width, attribute.Height);
                    if (HasFontAwesomeTitile)
                    {
                        if (DrawerHelper.FAButton(rect, attribute.Label))
                        {
                            Invoke();
                        }
                    }
                    else if (GUI.Button(rect, attribute.Label))
                    {
                        Invoke();
                    }
                }
                else
                {
                    if (HasFontAwesomeTitile)
                    {
                        if (DrawerHelper.FAButton(attribute.Label, FAOption.FontSize(attribute.Height)))
                        {
                            Invoke();
                        }
                    }
                    else if (GUILayout.Button(attribute.Label, GUILayout.Width(attribute.Width), GUILayout.Height(attribute.Height)))
                    {
                        Invoke();
                    }
                }
            }
            else
            {
                if (HasFontAwesomeTitile)
                {
                    if (DrawerHelper.FAButton(attribute.Label, FAOption.FontSize(18)))
                    {
                        Invoke();
                    }
                }
                else if (GUILayout.Button(attribute.Label))
                {
                    Invoke();
                }
            }

            if (HasParams)
            {
                accordion.OnGUI();
            }
        }

        private void Invoke()
        {
            if (IsCoroutine)
            {
                Infinity.DOEditorCoroutine((IEnumerator)method.Invoke(declaringObject, arguments));
            }
            else
            {
                method.Invoke(declaringObject, arguments);
            }
        }

    }
}