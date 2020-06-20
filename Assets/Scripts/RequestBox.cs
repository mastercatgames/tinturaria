using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestBox : MonoBehaviour
{
    private Image bar;
    public bool timeIsOver;
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("BarBG").Find("Bar").GetComponent<Image>();

        //InvokeRepeating("CountdownBar", 3f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeIsOver && bar.fillAmount > 0.009f)
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, 0f, Time.deltaTime * 0.5f);
        else {
            timeIsOver = true;
            Debug.Log("TimeIsOver!");
        }
    }

    // void CountdownBar()
    // {
    //     if (!timeIsOver && bar.fillAmount > 0.009f)
    //         bar.fillAmount = Mathf.Lerp(bar.fillAmount, 0f, Time.deltaTime * 2f);
    //     else
    //     {
    //         timeIsOver = true;
    //         Debug.Log("TimeIsOver!");
    //     }
    // }

    void ClearUpBox()
    {

    }
}
