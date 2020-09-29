#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.InfiniteEngine
#pragma warning disable 0414 // private field assigned but not used.

/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
************************************************************************************************************************************/


using UnityEngine;
using UnityEditor;
using InfinityEngine.Extensions;
using InfinityEngine.Utils;
using InfinityEngine;
using System.Runtime.CompilerServices;

/// <summary>
/// This namespace provides access to tools which extends unity editor.
/// </summary>
[assembly: InternalsVisibleTo("Assembly-CSharp")]
namespace InfinityEditor
{

    /// <summary>
    ///   InfinityEngine Welcome window
    /// </summary>
    [InitializeOnLoad]
    public class InfinityEditor : EditorWindow
    {

        #region Fields

        private const int RateUsMessageInterval = 15;
        private const string RateUSPreferenceKey = "___RATE_US_KEY___";

        private const string ONLINE_DOC_URL = "https://infinity-engine-f6f33.firebaseapp.com";
        private const string RATEUS_URL = "http://u3d.as/riS";
        private const string FACEBOOK_URL = "https://facebook.com/mceinc";
        private const string TWITTER_URL = "https://twitter.com/IncMce";
        private const string REQUEST_URL = "mailto:mciissee@gmail.com?subject=Asset Store";
        private const string ASSET_STORE = "http://u3d.as/riS";

        private Vector2 scrollPosition;

        private const int WIDTH = 450;
        private const int HEIGHT = 600;

        private const string PREFSHOWATSTARTUP = "SHOW_AT_STARTUP";
        private static bool showAtStartup;

        private static GUIStyle font;
        private static bool resourceLoaded;

        private static double lastTime;
        private static double currentTime;

        /// <summary>
        /// Gets the time the editor took in seconds to update.
        /// </summary>
        public static float deltaTime { get; private set; } // used be InfinityEngine.dll

        #endregion Fields

        static InfinityEditor()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;

            EditorApplication.update -= UpdateDeltaTime;
            EditorApplication.update += UpdateDeltaTime;

            EditorApplication.update -= InvokeInfinityEditorCallback;
            EditorApplication.update += InvokeInfinityEditorCallback;

            showAtStartup = PlayerPrefs.GetInt(PREFSHOWATSTARTUP, 1).ToBool();

            if (showAtStartup)
            {
                EditorApplication.update -= OpenAtStartup;
                EditorApplication.update += OpenAtStartup;
            }

            RequestRateUs();
        }

        private static void UpdateDeltaTime()
        {
            currentTime = EditorApplication.timeSinceStartup;
            deltaTime = (float)(currentTime - lastTime);
            lastTime = currentTime;
        }

        private void OnEnable()
        {
            if (!resourceLoaded)
            {
                font = new GUIStyle();
                font.normal.textColor = Color.white;
                resourceLoaded = true;
            }
        }

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            font.fontSize = 25;

            GUI.Box(new Rect(0, 10, WIDTH, 74), $"Infinity Engine\n\nVersion {Infinity.Version}\n\nby Mamadou Cisse");
            GUI.Box(new Rect(0, 10, 64, 64), AssetReferences.Logo, GUI.skin.label);

            GUILayoutUtility.GetRect(WIDTH, 74);
            GUILayout.Space(20);

            bool show = GUILayout.Toggle(showAtStartup, "Show at startup");
            if (show != showAtStartup)
            {
                showAtStartup = show;
                PlayerPrefs.SetInt(PREFSHOWATSTARTUP, show.ToInt());
            }
            GUILayout.Space(20);

            if (Button(AssetReferences.DocIcon, "SHOW DOCUMENTATION", "Read the full documentation of Infinity Engine."))
            {
                Application.OpenURL(ONLINE_DOC_URL);
            }
            if (Button(AssetReferences.RateIcon, "Rate this asset", "Write me a review ."))
            {
                Application.OpenURL(RATEUS_URL);
            }
            if (Button(AssetReferences.MoreIcon, "More Assets", "More assets."))
            {
                Application.OpenURL(ASSET_STORE);
            }
            if (Button(AssetReferences.TwitterIcon, "Twitter", "Follow me on twitter."))
            {
                Application.OpenURL(TWITTER_URL);
            }
            if (Button(AssetReferences.SupportIcon, "CONTACT ME", "Send me an email."))
            {
                Application.OpenURL(REQUEST_URL);
            }

            GUILayout.EndScrollView();
        }

        private static void OpenAtStartup()
        {
            MenuWelcome();
            EditorApplication.update -= OpenAtStartup;
        }

        private static void OnPlayModeChanged(PlayModeStateChange stateChange)
        {
            EditorApplication.update -= OpenAtStartup;
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        }

        private static bool Button(Texture texture, string heading, string body, int space = 10)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(54);
            GUILayout.Box(texture, GUIStyle.none, GUILayout.Width(48));
            GUILayout.Space(10);

            GUILayout.BeginVertical();
            GUILayout.Space(1);
            font.fontSize = 20;
            GUILayout.Label(heading, font);
            font.fontSize = 10;
            GUILayout.Label(body, font);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            var rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

            bool returnValue = false;
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                returnValue = true;
            }
            GUILayout.Space(space);

            return returnValue;
        }

        private static void RequestRateUs()
        {
            int count = PlayerPrefs.GetInt(RateUSPreferenceKey, 0);
            if (count == -1)
            {
                return;
            }

            count++;
            if (count >= RateUsMessageInterval)
            {
                var frame = Frame.Create(new Vector2(400, 180), new GUIContent("Rate US"), false);

                frame.onGUI = () =>
                {
                    GUILayout.Label(AssetReferences.Logo, GUILayout.Width(64), GUILayout.Height(64));
                    GUILayout.Label("If you like this asset please rate us in the the asset store page.\nThis encourages us to improve it consistently.\n\n Would you rate us ?");
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Yes"))
                    {
                        Application.OpenURL(RATEUS_URL);
                        PlayerPrefs.SetInt(RateUSPreferenceKey, -1);
                        PlayerPrefs.Save();
                        frame.Close();
                    }
                    if (GUILayout.Button("Later"))
                    {
                        PlayerPrefs.SetInt(RateUSPreferenceKey, 0);
                        PlayerPrefs.Save();
                        frame.Close();
                    }
                    if (GUILayout.Button("Never"))
                    {
                        PlayerPrefs.SetInt(RateUSPreferenceKey, -1);
                        PlayerPrefs.Save();
                        frame.Close();
                    }
                    GUILayout.EndHorizontal();
                };
                frame.Show();
            }
            else
            {
                PlayerPrefs.SetInt(RateUSPreferenceKey, count);
                PlayerPrefs.Save();
            }



        }

        private static void InvokeInfinityEditorCallback()
        {
            if (Infinity.editorUpdate != null)
            {
                Infinity.editorUpdate.Invoke();
            }
        }

        #region Menu Items

        [MenuItem("Tools/Infinity Engine/Welcome &w", priority = -10)]
        static void MenuWelcome()
        {
            var window = CreateInstance<InfinityEditor>();
            window.minSize = new Vector2(WIDTH, HEIGHT);
            window.maxSize = window.minSize;
            window.titleContent = new GUIContent(" Welcome ");

            window.Show();
        }

        [MenuItem("Tools/Infinity Engine/Take Screenshoot", false)]
        static void MenuScreenShoot()
        {
            ScreenCapture.CaptureScreenshot("screen " + PlayerPrefs.GetInt("SCREEN_SHOOT") + ".png");
            PlayerPrefs.SetInt("SCREEN_SHOOT", PlayerPrefs.GetInt("SCREEN_SHOOT") + 1);
        }

        [MenuItem("Tools/Infinity Engine/Find All Missing Scripts", false)]
        static void MenuFindAllMissingComponents()
        {
            EditorUtils.FindAllMissingComponents();
        }

        [MenuItem("Tools/Infinity Engine/Validate Scene")]
        static void MenuValidateSceneObjects()
        {
            EditorUtils.ValidateSceneObjects();
        }

        [MenuItem("CONTEXT/Transform/To RectTransform")]
        static void MenuToRectTransform()
        {
            Selection.activeGameObject.AddComponent<RectTransform>();
        }

        #endregion Menu Items

    }
}