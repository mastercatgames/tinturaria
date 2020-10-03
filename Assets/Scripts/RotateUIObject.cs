using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUIObject : MonoBehaviour
{
    public float speed;

    void Start()
    {
        if (speed == 0f)
            speed = 25f;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().Rotate(Vector3.back * speed * Time.deltaTime);
    }
}
