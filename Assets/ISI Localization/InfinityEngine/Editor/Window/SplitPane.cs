/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *                                                                                                          *
*************************************************************************************************************************************/

using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Events;

namespace InfinityEditor
{

    /// <summary>
    ///  Displays two components, either side by side or one on top of the other.
    /// </summary>
    [Serializable]
    public class SplitPane : ScriptableObject
    {
        #region Fields
        [SerializeField] private Color backgroundColor;
        [SerializeField] private Rect leftRect;
        [SerializeField] private Rect rightRect;

        [SerializeField] private Rect dividerRect;
        [SerializeField] private GUIStyle style;

        [SerializeField] private bool isResizing;

        /// <summary>
        /// Draw function of the left component
        /// </summary>
        public UnityAction onDrawLeftComponent;

        /// <summary>
        ///  Draw function of the right component
        /// </summary>
        public UnityAction onDrawRightComponent;

        private Event currentEvent;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating how the views are split.
        /// </summary>
        public Orientations Orientation { get; private set; }

        /// <summary>
        ///  Gets or sets the location of the divider between 0.0 and 1.0.
        ///  This value determines how space is distributed between the two contained components when the split pane's size is set.
        ///  If the value is sets to 0, the right component will fill all the available area and if it is sets to 1, 
        ///  the left component will fill all the available area.
        /// </summary>
        public float DividerLocation { get; set; }

        /// <summary>
        /// Gets or sets the max size of the left component between [0,1]
        /// </summary>
        public float DividendLimit { get; set; }

        /// <summary>
        /// Gets or sets the size of the divider.
        /// </summary>
        public float DividendSize { get; set; }

        /// <summary>
        /// The position of this split in the window
        /// </summary>
        public Rect Location { get; set; }

        /// <summary>
        /// Gets or sets the color of the divider
        /// </summary>
        public Color DividerColor { get; set; }

        /// <summary>
        /// Gets the position of the view to the left (or above) the divider.
        /// </summary>
        public Rect LeftRect { get { return leftRect; } }

        /// <summary>
        /// Gets the position of the view to the right (or below) the divider.
        /// </summary>
        public Rect RightRect { get { return rightRect; } }

        #endregion Properties

        /// <summary>
        /// Creates new split pane.
        /// </summary>
        /// <param name="orientation">The orientation of the split</param>
        /// <param name="dividendSize">The size of the dividend</param>
        /// <returns>New SplitPlane</returns>
        public static SplitPane Create(Orientations orientation, float dividendSize)
        {
            var split = CreateInstance<SplitPane>();

            split.Orientation = orientation;
            split.DividendSize = dividendSize;
            split.DividendLimit = .5f;
            split.DividerLocation = .5f;
            split.DividerColor = Color.white;

            return split;
        }


        /// <summary>
        /// Draws this splitpane
        /// </summary>
        public void Draw()
        {
            style = EditorStyles.helpBox;
            currentEvent = Event.current;

            DrawLeft();
            DrawRight();

            backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = DividerColor;
            DrawDivider();
            GUI.backgroundColor = backgroundColor;

            ProcessEvents(currentEvent);
        }

        private void DrawLeft()
        {
            if (Orientation == Orientations.Vertical)
            {
                leftRect = new Rect(Location.x, Location.y, Location.width * DividerLocation, Location.height);
            }
            else
            {
                leftRect = new Rect(0, 0, Location.width, Location.height * DividerLocation);
            }

            GUILayout.BeginArea(leftRect);
            onDrawLeftComponent?.Invoke();
            GUILayout.EndArea();
        }

        private void DrawRight()
        {
            if (Orientation == Orientations.Vertical)
            {
                rightRect = new Rect((Location.width * DividerLocation) + DividendSize + Location.x, Location.y, (Location.width * (1 - DividerLocation)) - DividendSize, Location.height);
            }
            else
            {
                rightRect = new Rect(Location.x, (Location.height * DividerLocation) + DividendSize + Location.y, Location.width, (Location.height * (1 - DividerLocation)) - DividendSize);
            }
            GUILayout.BeginArea(rightRect);

            onDrawRightComponent?.Invoke();
            GUILayout.EndArea();
        }

        private void DrawDivider()
        {
            MouseCursor cursor;
            if (Orientation == Orientations.Vertical)
            {
                dividerRect = new Rect((Location.width * DividerLocation) + DividendSize + Location.x, Location.y, DividendSize * 2, Location.height);
                GUILayout.BeginArea(new Rect(dividerRect.position + (Vector2.up * DividendSize), new Vector2(2, Location.height)), style);
                cursor = MouseCursor.ResizeHorizontal;
            }
            else
            {
                dividerRect = new Rect(Location.x, (Location.height * DividerLocation) - DividendSize + Location.y, Location.width, DividendSize * 2);
                GUILayout.BeginArea(new Rect(dividerRect.position + (Vector2.left * DividendSize), new Vector2(Location.width, 2)), style);
                cursor = MouseCursor.ResizeVertical;
            }

            GUILayout.EndArea();
            EditorGUIUtility.AddCursorRect(dividerRect, cursor);
        }

        private void ProcessEvents(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                isResizing = dividerRect.Contains(e.mousePosition);
            }
            else if (e.type == EventType.MouseUp)
            {
                isResizing = false;
            }

            if (isResizing)
            {
                if (Orientation == Orientations.Vertical)
                {
                    DividerLocation = Mathf.Clamp(e.mousePosition.x / Location.width, 0.05f, DividendLimit);
                }
                else
                {
                    DividerLocation = Mathf.Clamp(e.mousePosition.y / Location.height, 0.05f, DividendLimit);
                }
            }
        }


        /// <summary>
        /// The different type of orientations of a <see cref="SplitPane"/> object
        /// </summary>
        public enum Orientations
        {
            /// <summary>Vertical indicates the views of the split pane are split along the y axis.</summary>
            Vertical,
            /// <summary>Vertical indicates the views of the split pane are split along the x axis.</summary>
            Horizontal
        }
    }


}