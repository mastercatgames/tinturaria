using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkMachine : MonoBehaviour
{
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //RectTransform myRectTransform = GetComponent<RectTransform>();
 
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Vector3.left * moveSpeed;
    }
}
