using UnityEngine;
using UnityEngine.UI;

namespace InfinityEngine
{
    public class PlaySoundOnClick : MonoBehaviour
    {

        public AudioClip clip;

        // Use this for initialization
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayEffect(clip);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}