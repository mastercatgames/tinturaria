/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *
*************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine.Attributes;
using InfinityEngine.Utils;

namespace InfinityEngine.Localization
{
    /// <summary>
    ///    Localized audio component.
    /// </summary>
    [AddComponentMenu("InfinityEngine/Localization/Localized Audio")]
    public class LocalizedAudio : MonoBehaviour
    {

        [Popup(R3.audios.Names, PopupValueTypes.String, true)]
        [SerializeField]
        private string key;
        private AudioClip clip;


        /// <summary> 
        /// Plays the localized audio in the current language. 
        /// </summary>
        public void Play()
        {
            if (key == null)
            {
                Debugger.LogError("This audio key is null", gameObject);
                return;
            }

            clip = ISILocalization.GetAudio(key);
            if (clip == null)
            {
                Debugger.LogError("There is not localized audio with the  key" + key, gameObject);
                return;
            }
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }
    }
}