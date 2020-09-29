
/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace InfinityEngine.Localization
{
    /// <summary>
    ///   Flag component of an <see cref="Language"/>.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class Flag : MonoBehaviour
    {

        /// <summary>
        /// Is <see cref="ISILocalization.m_nextScene"/> will be loaded when the user click on the button attached to this flag?
        /// </summary>
        [SerializeField] private Language language;
        public bool loadNextLevel = false;
        
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                ISILocalization.ChangeLanguage(language);
                if(loadNextLevel)
                    ISILocalization.LoadNextLevel();
            });
        }
        
    }
}