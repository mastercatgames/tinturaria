/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                            *                                                                                                          *
*************************************************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using InfinityEngine;
using System.Collections.Generic;
using InfinityEngine.Attributes;
using InfinityEngine.Utils;

namespace InfinityEngine.Localization
{

    /// <summary>
    ///     Localized sprite component. 
    ///     
    ///     Add this component to a GameObject which has an <see cref="Image"/> or a <see cref="SpriteRenderer"/> Component to change it sprite at runtime depending on current language
    /// </summary>
    [AddComponentMenu("InfinityEngine/Localization/Localized Sprite")]
    public class LocalizedSprite : MonoBehaviour
    {

        /// <summary>
        /// The type of the Sprite component to localize.
        /// </summary>
        public enum SpriteComponentType
        {
            /// <summary>
            /// UnityEngine.UI.Image
            /// </summary>
            Image,

            /// <summary>
            /// UnityEngine.SpriteRenderer
            /// </summary>
            SpriteRenderer
        }

        /// <summary>
        /// this component type
        /// </summary>
        [SerializeField] private SpriteComponentType type;

        [Popup(R3.sprites.Names, PopupValueTypes.String, true)]
        [SerializeField]
        private string key;

        private Image image;
        private SpriteRenderer spriteRenderer;

        private static List<LocalizedSprite> LocalizedSprites;

        void Start()
        {
            if (type == SpriteComponentType.Image)
            {
                image = GetComponent<Image>();
                if (image == null)
                {
                    Debugger.LogError("There is no Image component attached to this GameObject", gameObject);
                    return;
                }
            }
            else
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer == null)
                {
                    Debugger.LogError("There is no SpriteRenderer component attached to this GameObject", gameObject);
                    return;
                }
            }

            if (ISILocalization.LoadIfNotInScene())
                Infinity.When(() => ISILocalization.IsInitialized, _OnLanguageChanged);

            if (LocalizedSprites == null)
            {
                LocalizedSprites = new List<LocalizedSprite>();
            }

            if (!LocalizedSprites.Contains(this))
            {
                LocalizedSprites.Add(this);
            }
        }

        private void _OnLanguageChanged()
        {
            if (type == SpriteComponentType.Image)
            {
                image.sprite = ISILocalization.GetSprite(key);
            }
            else
            {
                spriteRenderer.sprite = ISILocalization.GetSprite(key);
            }
        }

        /// <summary>
        /// Callback function invoked when the application language change.
        /// </summary>
        public static void OnLanguageChanged()
        {
            if (LocalizedSprites != null)
            {
                LocalizedSprites.ForEach(arg => arg._OnLanguageChanged());
            }
        }
    }
}