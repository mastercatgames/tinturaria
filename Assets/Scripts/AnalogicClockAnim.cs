using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogicClockAnim : MonoBehaviour
{
    public float minuteSpeed;
    public float hourSpeed;
    public GameObject minuteHand;
    public GameObject hourHand;
    public bool isRectTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        minuteSpeed = 400f;
        hourSpeed = 50f;

        isRectTransform = minuteHand.GetComponent<RectTransform>() != null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRectTransform)
        {
            minuteHand.GetComponent<RectTransform>().Rotate(Vector3.back * minuteSpeed * Time.deltaTime);
            hourHand.GetComponent<RectTransform>().Rotate(Vector3.back * hourSpeed * Time.deltaTime);
        }
        else
        {
            minuteHand.transform.Rotate(Vector3.back * minuteSpeed * Time.deltaTime);
            hourHand.transform.Rotate(Vector3.back * hourSpeed * Time.deltaTime);
        }
    }
}
