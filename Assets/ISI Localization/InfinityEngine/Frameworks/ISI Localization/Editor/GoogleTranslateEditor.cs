/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

namespace InfinityEngine.Localization
{
    
    using System;
    using InfinityEditor;
    using UnityEditor;
    using UnityEngine;
    using InfinityEngine.Utils;

    /// <summary>
    /// Provides a static functions allowing to translates a text using Google Translate api.
    /// </summary>
    [Serializable]
    public class GoogleTranslateEditor : EditorWindow
    {

        [SerializeField] private Language sourceLanguage = Language.English;
        [SerializeField] private Language targetLanguage = Language.French;


        [SerializeField] private string sourceLangCode = "fr";
        [SerializeField] private string targetLangCode = "en";
        [SerializeField] private string text = "";
        [SerializeField] private string translation = "";

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();

            sourceLanguage = (Language) EditorGUILayout.EnumPopup("Source : ", sourceLanguage);
            targetLanguage = (Language) EditorGUILayout.EnumPopup("Target : ", targetLanguage);


            sourceLangCode = sourceLanguage.Code();
            targetLangCode = targetLanguage.Code();

            if (GUILayout.Button("Translate") && !string.IsNullOrEmpty(text))
            {
                Translate(sourceLangCode, targetLangCode, text, (info) =>
                {
                    sourceLangCode = info.SourceLanguage;
                    translation = info.Text;
                    Repaint();
                });
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            text = EditorGUILayout.TextArea(text, GUILayout.MinHeight(100));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            translation = EditorGUILayout.TextArea(translation, GUILayout.MinHeight(100));
            GUILayout.EndHorizontal();

        }
     
        /// <summary>
        /// Translates the given text from <paramref name="sourceLang"/> to <paramref name="targetLang"/>
        /// </summary>
        /// <param name="sourceLang">The source language</param>
        /// <param name="targetLang">The target language</param>
        /// <param name="sourceText">The text to translate</param>
        /// <param name="callback">A callback function invoked after the translation</param>
        public static void Translate(string sourceLang, string targetLang, string sourceText, Action<TranslationInfo> callback)
        {
            EditorCoroutine.Start(GoogleTranslate.Process(sourceLang, targetLang, sourceText, callback));
        }
       
        /// <summary>
        /// Translates the given text to <paramref name="targetLang"/>. The function detects automatically the souce language
        /// </summary>
        /// <param name="targetLang">The target language</param>
        /// <param name="sourceText">The text to translate</param>
        /// <param name="callback">A callback function invoked after the translation</param>
        public static void Translate(string targetLang, string sourceText, Action<TranslationInfo> callback)
        {
            EditorCoroutine.Start(GoogleTranslate.Process("auto", targetLang, sourceText, callback));
        }

        [MenuItem("Tools/Infinity Engine/Localization/Google Translate", false, -7)]
        static void OpenEditor()
        {

            var window = GetWindow<GoogleTranslateEditor>();
            window.titleContent = new GUIContent(AssetReferences.GoogleTranslateIcon);
            window.minSize = new Vector2(720, 250);
        }

    }
}