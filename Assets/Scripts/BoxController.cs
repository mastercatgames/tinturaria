using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Color currentColor;
    [Range (0f,1f)]public float percentage = 0; //only to know how much is inside box

    public void PlayDropBoxSFX()
    {
        gameObject.transform.parent.GetComponent<AudioSource>().Play();
    }
}
