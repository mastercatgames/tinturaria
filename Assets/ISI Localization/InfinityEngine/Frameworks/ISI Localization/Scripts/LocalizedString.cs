/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                            *                                                                                                          *
*************************************************************************************************************************************/


using UnityEngine;
using InfinityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using InfinityEngine.Attributes;
using InfinityEngine.Utils;
using TMPro;

namespace InfinityEngine.Localization
{

    /// <summary>
    ///     Localized string component.
    ///     Add this component to a GameObject with a 
    ///     <see cref="Text"/>,  <see cref="TextMesh"/>, <see cref="TextMeshPro"/> or <see cref="TextMeshProUGUI"/>
    ///     component to Translate the text at runtime 
    /// </summary>
    [AddComponentMenu("InfinityEngine/Localization/Localized String")]
    public class LocalizedString : MonoBehaviour
    {
        /// <summary>
        /// The type of component to localize.
        /// </summary>
        private enum TextComponentType
        {

            /// <summary>
            /// UnityEngine.UI.Text
            /// </summary>
            Text,

            /// <summary>
            /// UnityEngine.TextMesh
            /// </summary>
            TextMesh,

            /// <summary>
            /// TMPro.TextMeshPro
            /// </summary>
            TextMeshPro,

            /// <summary>
            /// TMPro.TextMeshProUGUI
            /// </summary>
            TextMeshProUGUI,
        }

        [SerializeField]
        private TextComponentType type;

        [Popup(R3.strings.Names, PopupValueTypes.String, true)]
        [SerializeField]
        private string key;

        private Text label;
        private TextMesh textMesh;
        private TextMeshPro textMeshPro;
        private TextMeshProUGUI textMeshProUGUI;

        private static List<LocalizedString> LocalizedStrings;

        void Start()
        {
            FindComponent();

            if (ISILocalization.LoadIfNotInScene())
                Infinity.When(() => ISILocalization.IsInitialized, _OnLanguageChanged);

            if (LocalizedStrings == null)
                LocalizedStrings = new List<LocalizedString>();

            if (!LocalizedStrings.Contains(this))
                LocalizedStrings.Add(this);
        }

        private void FindComponent()
        {
            object component = null;
            switch (type)
            {
                case TextComponentType.Text:
                    component = label = GetComponent<Text>();
                    break;
                case TextComponentType.TextMesh:
                    component = textMesh = GetComponent<TextMesh>();
                    break;
                case TextComponentType.TextMeshPro:
                    component = textMeshPro = GetComponent<TextMeshPro>();
                    break;
                case TextComponentType.TextMeshProUGUI:
                    component = textMeshProUGUI = GetComponent<TextMeshProUGUI>();
                    break;
            }

            if (component == null)
            {
                Debugger.LogError($"There is no {type} component attached to this GameObject", this);
                return;
            }
        }

        private void _OnLanguageChanged()
        {
            if (label != null)
                label.text = ISILocalization.GetValueOf(key);

            if (textMesh != null)
                textMesh.text = ISILocalization.GetValueOf(key);

            if (textMeshPro != null)
                textMeshPro.text = ISILocalization.GetValueOf(key);

            if (textMeshProUGUI != null)
                textMeshProUGUI.text = ISILocalization.GetValueOf(key);
        }

        /// <summary>
        /// Callback function invoked when the application language change.
        /// </summary>
        public static void OnLanguageChanged()
        {
            if (LocalizedStrings != null)
            {
                LocalizedStrings.ForEach(arg => arg._OnLanguageChanged());
            }
        }
    }

}