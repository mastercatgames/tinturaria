using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{

    public static DontDestroyMusic instance = null;
    public static DontDestroyMusic Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        this.gameObject.transform.parent = null; //DontDestroyOnLoad only works if gameobject is in root of the hierarchy

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
