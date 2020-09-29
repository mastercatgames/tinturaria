/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InfinityEditor
{
    /// <summary>
    /// Provides a way to creates an editor window without extending <see cref="UnityEditor.EditorWindow"/>
    /// </summary>
    [Serializable]
    public class Frame : EditorWindow
    {

        #region Events

        protected Dictionary<EventType, Action> EventMap { get; set; }

        /// <summary>
        /// <see cref="OnGUI"/> callback
        /// </summary>
        public Action onGUI;

        /// <summary>
        /// <see cref="OnRepaint"/> callback
        /// </summary>
        public Action onRepaint;

        /// <summary>
        /// <see cref="OnDestroy"/> callback
        /// </summary>
        public Action onClose;

        private Action<Keyboard, Event> KeyUpListener;
        private Action<Keyboard, Event> KeyDownListener;

        private Action<MouseButton, Vector2, Event> MouseUpListener;
        private Action<MouseButton, Vector2, Event> MouseDownListener;
        private Action<MouseButton, Vector2, Event> MouseDragListener;

        private Action<Vector2, Event> MouseScrollListener;

        private int controlId;
        private EventType controlEvent;
        private Action e;
        #endregion Events

        /// <summary>
        /// Creates new window
        /// </summary>
        /// <param name="size">The size of the window</param>
        /// <param name="title">The title of the window</param>
        /// <param name="isResizable">Is the window is resizable ?</param>
        /// <returns></returns>
        public static Frame Create(Vector2 size, GUIContent title, bool isResizable = true)
        {
            var window = CreateInstance<Frame>();
            window.maxSize = size;
            if (!isResizable)
            {
                window.minSize = size;
            }
            window.titleContent = title;
            return window;
        }


        /// <summary>
        /// Called when the window is focused.
        /// </summary>
        protected virtual void OnEnable()
        {
            void KeyUp() => OnKeyUp(new Keyboard(Event.current), Event.current);
            void KeyDown() => OnKeyDown(new Keyboard(Event.current), Event.current);

            void MouseDown() => OnMouseDown((MouseButton)Event.current.button, Event.current.mousePosition, Event.current);
            void MouseUp() => OnMouseUp((MouseButton)Event.current.button, Event.current.mousePosition, Event.current);
            void MouseDrag() => OnMouseDrag((MouseButton)Event.current.button, Event.current.delta, Event.current);
            void ScrollWheel() => OnScrollWheel(Event.current.delta, Event.current);

            if (EventMap == null)
            {
                EventMap = new Dictionary<EventType, Action>
                {
                    { EventType.Repaint, OnRepaint },
                    { EventType.KeyDown, KeyDown },
                    { EventType.KeyUp, KeyUp },
                    { EventType.MouseDown, MouseDown },
                    { EventType.MouseUp, MouseUp },
                    { EventType.MouseDrag, MouseDrag },
                    { EventType.ScrollWheel, ScrollWheel }
                };
            }
        }

        /// <summary>
        /// Called when the window is drawed
        /// </summary>
        protected virtual void OnGUI()
        {
            controlId = GUIUtility.GetControlID(FocusType.Passive);
            controlEvent = Event.current.GetTypeForControl(controlId);

            if (EventMap.TryGetValue(controlEvent, out e))
                e?.Invoke();

            onGUI?.Invoke();
        }

        /// <summary>
        /// Called before the window being destroyed
        /// </summary>
        protected virtual void OnDestroy()
        {
            onClose?.Invoke();
        }


        #region Listeners


        protected virtual void OnRepaint()
        {
            onRepaint?.Invoke();
        }

        protected virtual void OnKeyDown(Keyboard keyboard, Event e)
        {
            KeyDownListener?.Invoke(keyboard, e);

        }

        public void AddKeyDownListener(Action<Keyboard, Event> listener)
        {
            this.KeyDownListener += listener;
        }

        protected virtual void OnKeyUp(Keyboard keyboard, Event e)
        {
            KeyUpListener?.Invoke(keyboard, e);
        }

        public void AddKeyUpListener(Action<Keyboard, Event> listener)
        {
            this.KeyUpListener += listener;
        }

        protected virtual void OnMouseDown(MouseButton button, Vector2 position, Event e)
        {
            MouseDownListener?.Invoke(button, position, e);
        }

        public void AddMouseDownListener(Action<MouseButton, Vector2, Event> listener)
        {
            this.MouseDownListener += listener;
        }

        protected virtual void OnMouseUp(MouseButton button, Vector2 position, Event e)
        {
            MouseUpListener?.Invoke(button, position, e);
        }

        public void AddMouseUpListener(Action<MouseButton, Vector2, Event> listener)
        {
            this.MouseUpListener += listener;
        }

        protected virtual void OnMouseDrag(MouseButton button, Vector2 delta, Event e)
        {
            MouseDragListener?.Invoke(button, delta, e);
        }
        public void AddMouseDragListener(Action<MouseButton, Vector2, Event> listener)
        {
            this.MouseDragListener += listener;
        }

        protected virtual void OnScrollWheel(Vector2 delta, Event e)
        {
            MouseScrollListener?.Invoke(delta, e);
        }
        public void AddMouseScrollListener(Action<Vector2, Event> listener)
        {
            this.MouseScrollListener += listener;
        }

        #endregion Listeners


    }

    /// <summary>
    /// Represents the informations the system keyboard
    /// </summary>
    public class Keyboard
    {
        public Keyboard()
        {
        }

        /// <summary>
        /// Creates new instance of Keyboard class
        /// </summary>
        /// <param name="evt">The event associated to the keybord</param>
        public Keyboard(Event evt)
        {
            this.Code = evt.keyCode;
            this.IsAlt = evt.alt;
            this.IsCapsLock = evt.capsLock;
            this.IsControl = evt.control;
            this.IsFunctionKey = evt.functionKey;
            this.IsNumeric = evt.numeric;
            this.IsShift = evt.shift;
            this.Modifiers = evt.modifiers;
        }

        /// <summary>
        /// The code of the key pressed.
        /// </summary>
        public KeyCode Code { get; set; }

        /// <summary>
        /// Is the pressed key is alt key
        /// </summary>
        public bool IsAlt { get; set; }

        /// <summary>
        /// Is the pressed key is alt capslock
        /// </summary>
        public bool IsCapsLock { get; set; }

        /// <summary>
        /// Is the pressed key is alt control key
        /// </summary>
        public bool IsControl { get; set; }

        /// <summary>
        /// Is the pressed key is alt function key
        /// </summary>
        public bool IsFunctionKey { get; set; }

        /// <summary>
        /// Is the pressed key is alt numeric key
        /// </summary>
        public bool IsNumeric { get; set; }

        /// <summary>
        /// Is the pressed key is alt shift key
        /// </summary>
        public bool IsShift { get; set; }

        /// <summary>
        /// The type of the modifier key that can be active during keystroke event.
        /// </summary>
        public EventModifiers Modifiers { get; set; }
    }


    /// <summary>
    /// Represents the different type of buttons of a mouse.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// Left button
        /// </summary>
        Left = 0,
        /// <summary>
        /// Right button
        /// </summary>
        Right = 1,
        /// <summary>
        /// Middle button
        /// </summary>
        Middle = 2
    }
}