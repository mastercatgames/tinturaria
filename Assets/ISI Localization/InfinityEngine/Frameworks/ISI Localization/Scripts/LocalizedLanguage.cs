/************************************************************************************************************************************
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* 							                                                                                                        *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace InfinityEngine.Localization
{

    /// <summary>
    ///   Represents a localized language.  
    /// </summary>
    [Serializable]
    public class LocalizedLanguage
    {

        #region Use By The Editor Script

        private int translationStep;

        public bool isExpanded;
        public bool isBeingAutoTranslated;
        public float translationProgress;

        #endregion Used By The Editor Script

        [SerializeField] private Texture2D m_flag;
        [SerializeField] private Language m_language;
        [SerializeField] private List<StringKV> m_localizedStrings;
        [SerializeField] private List<AudioClipKV> m_localizedAudio;
        [SerializeField] private List<SpriteKV> m_localizedSprites;
        [SerializeField] private string m_languageName;
        [SerializeField] private string m_languageCode;

        #region Properties

        /// <summary>
        /// Gets or sets the language
        /// </summary>
        public Language Language
        {
            get { return m_language; }
            set
            {
                m_flag = Resources.Load<Texture2D>(string.Format("Flags/{0}_FLAG", value).ToLower());
                m_language = value;
            }
        }

        /// <summary>
        /// Gets the name of the language
        /// </summary>
        public string LanguageName
        {
            get
            {
                if (string.IsNullOrEmpty(m_languageName))
                    m_languageName = Language.ToString();

                return m_languageName;
            }
        }

        /// <summary>
        /// Gets the iso code of the language (en, fr, es..)
        /// </summary>
        public string LanguageCode
        {
            get
            {
                if (m_languageCode == null)
                    m_languageCode = Language.Code();
                return m_languageCode;
            }
        }

        /// <summary>
        /// The flag texture of the language
        /// </summary>
        public Texture2D Flag { get { return m_flag; } }

        /// <summary>
        /// Gets or sets the localized strings
        /// </summary>
        public List<StringKV> LocalizedStrings { get { return m_localizedStrings; } set { m_localizedStrings = value; } }

        /// <summary>
        /// Gets or sets the localized audo clips
        /// </summary>
        public List<AudioClipKV> LocalizedAudio { get { return m_localizedAudio; } set { m_localizedAudio = value; } }

        /// <summary>
        /// Gets or sets the localized sprites
        /// </summary>
        public List<SpriteKV> LocalizedSprites { get { return m_localizedSprites; } set { m_localizedSprites = value; } }

        #endregion Properties

        public LocalizedLanguage() : this(new List<StringKV>()) { }

        /// <summary>
        /// Creates new instance of the class and setup the localized strings of the language as the given <paramref name="strings"/>. 
        /// </summary>
        /// <param name="strings">  List of all localized string. </param>
        public LocalizedLanguage(List<StringKV> strings)
        {
            m_localizedStrings = strings;
            m_localizedAudio = new List<AudioClipKV>();
            m_localizedSprites = new List<SpriteKV>();
        }

        /// <summary>    
        /// Sets the value of the localized string identified by key. 
        /// </summary>
        /// <param name="key">The key of the localized string.</param>
        /// <param name="value">The localized text associated to the key.</param>
        public void SetString(string key, string value)
        {
            var existing = m_localizedStrings.Find(e => e.Key == key);
            if (existing == null)
            {
                m_localizedStrings.Add(new StringKV(key, value));
            }
            else
            {
                existing.Value = value;
            }
        }

        /// <summary> 
        /// Gets the localized string identified by the given key if it exists. 
        /// </summary>
        /// <param name="key">The key of the localized string.</param>
        /// <returns>The string if it exists <c>null</c> otherwise.</returns>
        public string GetString(string key)
        {
            var existing = LocalizedStrings.Find(e => e.Key == key);
            return existing == null ? null : existing.Value;
        }

        /// <summary>
        /// Sets the value of the localized audio identified by key. 
        /// </summary>
        /// <param name="key">The key of the localized audio.</param>
        /// <param name="value">The localized audio associated to the key.</param>
        public void SetAudio(string key, AudioClip value)
        {
            var existing = m_localizedAudio.Find(e => e.Key == key);
            if (existing == null)
            {
                m_localizedAudio.Add(new AudioClipKV(key, value));
            }
            else
            {
                existing.Value = value;
            }
        }

        /// <summary> Returns the AudioClip identified by key if it exists. </summary>
        /// <param name="key">The key of the localized audio.</param>
        /// <returns>The AudioClip if it exists <c>null</c> otherwise. </returns>
        public AudioClip GetAudio(string key)
        {
            var existing = m_localizedAudio.Find(e => e.Key == key);
            return existing == null ? null : existing.Value;
        }

        /// <summary>    Sets the value of the localized Sprite identified by key. </summary>
        /// <param name="key">The key of the localized sprite.</param>
        /// <param name="value">The localized sprite associated to the key.</param>
        public void SetSprite(string key, Sprite value)
        {
            var existing = m_localizedSprites.Find(e => e.Key == key);
            if (existing == null)
            {
                m_localizedSprites.Add(new SpriteKV(key, value));
            }
            else
            {
                existing.Value = value;
            }
        }

        /// <summary> Returns the Sprite identified by key if it exists. </summary>
        /// <param name="key">The key of the localized sprite.</param>
        /// <returns>The Sprite if it exists <c>null</c> otherwise. </returns>
        public Sprite GetSprite(string key)
        {
            var existing = m_localizedSprites.Find(e => e.Key == key);
            return existing == null ? null : existing.Value;
        }


        /// <summary>
        /// Gets the name of the language
        /// </summary>
        public override string ToString()
        {
            return m_language.ToString();
        }

        // used by the editor script
        public void UpdateTranslationProgress(int totalSteps)
        {
            translationStep++;

            translationProgress = (float)translationStep / (float)totalSteps;
            if (translationStep >= totalSteps)
            {
                isBeingAutoTranslated = false;
                translationProgress = 0;
                translationProgress = 0;
            }
        }
    }
}