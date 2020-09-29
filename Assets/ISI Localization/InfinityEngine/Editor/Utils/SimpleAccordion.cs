/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

namespace InfinityEditor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using InfinityEngine.Utils;
    using UnityEditor.AnimatedValues;

    /// <summary>
    /// Represents a simple collapsible area
    /// </summary>
    [Serializable]
    public class SimpleAccordion
    {

        #region Fields

        private Color backgroundColor;
        private float backgroundAlpha;
        private GUIStyle headerStyle;

        private Rect headerRect;
        private AnimBool animation;

        /// <summary>
        /// Callback action invoked when the expand state of the accordion changes
        /// </summary>
        public Action<SimpleAccordion> expandStateChangeCallback;
   
        /// <summary>
        /// Invoked when the accordion is opened
        /// </summary>
        public Action onOpenedCallback;

        /// <summary>
        /// Invoked when the accordion is closed
        /// </summary>
        public Action onClosedCallback;

        /// <summary>
        /// Use this field to overrides the draw function of the header (the function must return the position of the header)
        /// </summary>
        public Func<Rect> drawHeaderCallback;

        /// <summary>
        /// The draw function of the contents.
        /// </summary>
        public Action drawCallback;

        private bool isFirstExpand;

        #endregion Fields
        
        #region Properties

        /// <summary>
        /// Gets or sets the speed of the animation
        /// </summary>
        public float Speed { get { return animation.speed; } set { animation.speed = value; } }

        /// <summary>
        /// The title of the accordion
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the expand state of the accordion
        /// </summary>
        public bool IsExpanded
        {
            get { return animation.target; }
            set { animation.target = value;  }
        }

        /// <summary>
        /// Returns the float value of the fade animation in the range [0,1]
        /// </summary>
        public float Faded
        {
            get { return animation.faded; }
        }

        #endregion Properties

        #region Constructors

        public SimpleAccordion() : this(string.Empty, null, null) { }

        public SimpleAccordion(string title) : this(title, null, null) { }

        public SimpleAccordion(Action drawCallback) : this(string.Empty, null, drawCallback) { }

        public SimpleAccordion(string title, Action drawCallback) : this(title, null, drawCallback) { }

        public SimpleAccordion(Func<Rect> drawHeaderCallback,  Action drawCallback) : this(string.Empty, drawHeaderCallback, drawCallback){}

        public SimpleAccordion(string title, Func<Rect> drawHeaderCallback, Action drawCallback)
        {
            this.Title = title;
            this.drawHeaderCallback = drawHeaderCallback;
            this.drawCallback = drawCallback;
            animation = new AnimBool();
            animation.target = false;
            isFirstExpand = true;
        }

        #endregion Constructors

        /// <summary>
        /// Draws the body of the accordion with <see cref="EditorStyles.helpBox"/> style
        /// </summary>
        public void OnGUI()
        {
            OnGUI(EditorStyles.helpBox);
        }

        /// <summary>
        /// Draws the body of the accordion with the given style
        /// </summary>
        /// <param name="style">The style to use</param>
        public void OnGUI(GUIStyle style)
        {
            backgroundColor = GUI.backgroundColor;
            backgroundAlpha = backgroundColor.a;
            backgroundColor.a = 1f;
            GUI.backgroundColor = backgroundColor;


            EditorGUILayout.BeginVertical(style);
     
            if(drawHeaderCallback != null)
            {
                headerRect = drawHeaderCallback.Invoke();
            }
            else
            {
               headerRect = DrawDefaultAccordionHeader(this);              
            }

            ProcessEvents(Event.current);

            if(IsExpanded && isFirstExpand)
            {
                if(Faded >= 1)
                {

                    isFirstExpand = false;
                }
                if (drawCallback != null)
                {
                    drawCallback.Invoke();
                }

            }
            else
            {
                if (EditorGUILayout.BeginFadeGroup(animation.faded))
                {
                    if (drawCallback != null)
                    {
                        drawCallback.Invoke();
                    }

                }
                EditorGUILayout.EndFadeGroup();
            }

            EditorGUILayout.EndVertical();

            backgroundColor.a = backgroundAlpha;
            GUI.backgroundColor = backgroundColor;
        }

        private void ProcessEvents(Event evt)
        {
            if (evt.type == EventType.MouseDown && headerRect.Contains(evt.mousePosition))
            {
                IsExpanded = !IsExpanded;
                if(IsExpanded && onOpenedCallback != null)
                {
                    onOpenedCallback.Invoke();
                }
                if (!IsExpanded && onClosedCallback != null)
                {
                    onClosedCallback.Invoke();
                }
                if (expandStateChangeCallback != null)
                {
                    expandStateChangeCallback.Invoke(this);
                }
                
                evt.Use();
            }
        }

        public Rect DrawDefaultAccordionHeader(int height = 20, int fontSize = 14)
        {
            return DrawDefaultAccordionHeader(this, height, fontSize);
        }

        public Rect DrawDefaultAccordionHeader(string icon, int height = 20, int fontSize = 14)
        {
            return DrawDefaultAccordionHeader(this, height, fontSize);
        }

        public static Rect DrawDefaultAccordionHeader(SimpleAccordion accordion, int height = 20, int fontSize = 14)
        {
            var icon = accordion.IsExpanded ? FA.angle_double_down : FA.angle_double_right;
            return DrawDefaultAccordionHeader(accordion, icon, height, fontSize);
        }

        public static Rect DrawDefaultAccordionHeader(SimpleAccordion accordion, string icon, int height = 20, int fontSize = 14)
        {
            if(accordion.headerStyle == null)
            {
                accordion.headerStyle = new GUIStyle(AssetReferences.AccordionHeader);
            }

            accordion.headerStyle.fontSize = fontSize;
            GUILayout.Box(accordion.Title, accordion.headerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(height));
            var rect = GUILayoutUtility.GetLastRect();

            DrawerHelper.FAIcon(rect, icon, FAOption.TextAnchor(TextAnchor.MiddleLeft),
                //FAOption.FontSize(fontSize),
                FAOption.Padding(new RectOffset(5, 0, 0, 0))

                );
            return rect;
        }

    }

}
