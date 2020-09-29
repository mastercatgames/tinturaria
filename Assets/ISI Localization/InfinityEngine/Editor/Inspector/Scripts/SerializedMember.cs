/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using UnityEditor;
using UnityEngine;
using InfinityEngine.Utils;
using InfinityEngine.Attributes;
using InfinityEngine.Extensions;

namespace InfinityEditor
{

    /// <summary>
    /// Class for editing class fields and properties in inspector window.
    /// </summary>
    [Serializable]
    internal abstract class SerializedMember
    {
        #region Fields

        private object declaringObject;
        private string displayName;

        private Type declaringType;

        private MemberInfo memberInfo;
        private MemberInfo checkIfShouldBeDrawed;

        private DisplayAsAttribute displayAsAttribute;
        private DrawOrderAttribute drawOrderAttribute;
        private VisibleIfAttribute visibleIfAttribute;
        private ProgressBarAttribute progressBarAttribute;
        private MessageAttribute[] messageAttributes;

        private InspectorArrayDrawer collectionDrawer;
        private InspectorButtonMethod[] inspectorFunctions;
        private SerializedProperty helpProperty;
        private SerializedProperty serializedProperty;

        #endregion Fields

        #region Properties

        public object DeclaringObject => declaringObject;

        public int DrawOrder => drawOrderAttribute?.Order ?? 0;
        public int DeclaringOrder { get; set; }
        public int SerializedObjectInstanceId => SerializedProperty.serializedObject.targetObject.GetInstanceID();

        public MemberInfo MemberInfo => memberInfo;
        public SerializedProperty SerializedProperty => serializedProperty;

        public string Name => SerializedProperty?.name ?? MemberInfo?.Name;
        public string DisplayName => displayName ?? (displayName = (displayAsAttribute?.DisplayName ?? ObjectNames.NicifyVariableName(Name)));

        public bool ShouldBeDrawed
        {
            get
            {
                if (DeclaringObject != null && checkIfShouldBeDrawed != null)
                {
                    var condition = (bool)ReflectionUtils.GetValue(DeclaringObject, checkIfShouldBeDrawed);
                    return visibleIfAttribute.Negate ? !condition : condition;
                }

                return true;
            }
        }

        private bool ShouldBeDisplayed => helpProperty != null && helpProperty.boolValue;

        #endregion Properties

        protected SerializedMember(object declaringObject, SerializedProperty serializedProperty, MemberInfo memberInfo)
        {

            this.declaringObject = declaringObject;
            this.declaringType = declaringObject.GetType();
            this.serializedProperty = serializedProperty;
            this.memberInfo = memberInfo;
            this.helpProperty = serializedProperty.serializedObject.FindProperty("__help__");

            CheckAttribute(ref displayAsAttribute);
            CheckAttribute(ref drawOrderAttribute);
            CheckAttribute(ref progressBarAttribute);

            CheckMessageAttributes();
            CheckButtonAttributes();
            CheckVisibleIfAttribute();
        }

        private void CheckAttribute<T>(ref T attribute)
        {
            attribute = ReflectionUtils.GetAttribute<T>(MemberInfo, true);
        }

        private void CheckButtonAttributes()
        {
            var buttonAttributes = ReflectionUtils.GetAttributes<InspectorButtonAttribute>(MemberInfo);
            if (buttonAttributes != null)
            {
                inspectorFunctions = buttonAttributes.Select(it =>
                {
                    var method = ReflectionUtils.GetCachedMethod(declaringType, it.MethodName);
                    if (method != null)
                    {
                        var func = new InspectorButtonMethod(it, DeclaringObject);
                        return func;
                    }
                    Debug.Log($"The attribute InspectorButton of the member {Name} in the class {declaringType.Name} refers to a function that does not exist : {it.MethodName}");
                    return null;

                }).Where(it => it != null).ToArray();
            }
        }

        private void CheckMessageAttributes()
        {
            this.messageAttributes = ReflectionUtils.GetAttributes<MessageAttribute>(MemberInfo, true);
        }

        private void CheckVisibleIfAttribute()
        {
            visibleIfAttribute = ReflectionUtils.GetAttribute<VisibleIfAttribute>(MemberInfo);
            if (visibleIfAttribute != null)
            {
                visibleIfAttribute.TryGetReferencedMember(DeclaringObject, out checkIfShouldBeDrawed);
            }
        }

        private bool HasButtonAttribute()
        {
            return inspectorFunctions != null && inspectorFunctions.Any();
        }

        protected bool IsListOrArray(Type type)
        {
            return type.IsArray || typeof(IList).IsAssignableFrom(type);
        }

        protected void DrawPropertyField()
        {
            if (SerializedProperty.type == nameof(DecoratorField))
            {
                EditorGUILayout.PropertyField(SerializedProperty, GUIContent.none, true, GUILayout.Height(5));
                return;
            }

            if (progressBarAttribute != null && SerializedProperty.type == "float")
            {
                var rect = GUILayoutUtility.GetRect(0, 20, GUILayout.ExpandWidth(true));
                EditorGUI.ProgressBar(rect, SerializedProperty.floatValue, string.Empty);
            }
            else
            {
                EditorGUILayout.PropertyField(SerializedProperty, new GUIContent(DisplayName), true);
            }
        }

        protected void DrawMessagesIfNeeded()
        {
            if (messageAttributes != null)
            {
                var fieldInfo = MemberInfo as FieldInfo;
                MessageType messageType;
                foreach (var it in messageAttributes)
                {
                    it.MessageType.TryParse(out messageType);

                    if (it.HasProblem(DeclaringObject, fieldInfo))
                    {
                        EditorGUILayout.HelpBox(it.Message, messageType);
                    }
                    else if (ShouldBeDisplayed && !it.HasCondition)
                    {
                        EditorGUILayout.HelpBox(it.Message, messageType);
                    }
                }
            }
        }

        protected void DrawCollection()
        {
            collectionDrawer = collectionDrawer ?? new InspectorArrayDrawer(SerializedProperty);
            collectionDrawer.OnGUI();
        }

        protected void DrawMember(Action drawFunction)
        {
            if (!ShouldBeDrawed)
                return;

            if (!HasButtonAttribute())
            {
                drawFunction?.Invoke();
            }
            else
            {
                var buttonLocation = inspectorFunctions.FirstOrDefault(it => it != null).Location;
                var isFirstItem = true;
                foreach (var it in inspectorFunctions)
                {
                    switch (buttonLocation)
                    {
                        case InspectorButtonLocations.Top:
                            it.Draw();
                            if (isFirstItem)
                            {
                                drawFunction?.Invoke();
                            }
                            break;
                        case InspectorButtonLocations.Right:
                            EditorGUILayout.BeginHorizontal();
                            if (isFirstItem)
                            {
                                drawFunction?.Invoke();
                            }
                            it.Draw();
                            EditorGUILayout.EndHorizontal();
                            break;
                        case InspectorButtonLocations.Bottom:
                            if (isFirstItem)
                            {
                                drawFunction?.Invoke();
                            }
                            it.Draw();
                            GUILayout.Space(5);
                            break;
                        case InspectorButtonLocations.Left:
                            EditorGUILayout.BeginHorizontal();
                            it.Draw();
                            if (isFirstItem)
                            {
                                drawFunction();
                            }
                            EditorGUILayout.EndHorizontal();
                            break;
                    }
                    isFirstItem = false;
                }
            }
            DrawMessagesIfNeeded();
        }

        internal abstract void DrawMember();

    }
}