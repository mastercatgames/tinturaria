using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestBox : MonoBehaviour
{
    private Image bar;
    private RequestPanelController requestPanelController;
    public bool isDestroyingBox;
    private Image maskColor;
    private float barSpeed;    
    public Color colorRequested;
    public Sprite boxRequested;
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("BarBG").Find("Bar").GetComponent<Image>();
        maskColor = transform.Find("MaskColor").GetComponent<Image>();
        requestPanelController = transform.parent.GetComponent<RequestPanelController>();
        //TODO: Test the best time to fit in gameplay
        barSpeed = 0.02f;
    }

    // Update is called once per frame
    void Update()
    {
        if (bar.fillAmount > 0.009f)
        {
            bar.fillAmount -= Time.deltaTime * barSpeed;//Mathf.Lerp(bar.fillAmount, 0f, Time.deltaTime * barSpeed);

            if (bar.fillAmount < 0.5f)
            {
                bar.color = requestPanelController.YellowColor;
            }
            
            if (bar.fillAmount < 0.2f)
            {
                bar.color = requestPanelController.RedColor;
            }
        }
        else
        {
            //Execute once time
            if (!isDestroyingBox)
            {
                isDestroyingBox = true;
                maskColor.color = requestPanelController.RedColor;
                var tempColor = maskColor.color;
                tempColor.a = 0.5f;
                maskColor.color = tempColor;

                GetComponent<Animator>().Play("deliveryFailed");
            }
        }
    }

    void DestroyBox()
    {
        //TODO: Discount from a new variable numberOfFailedDeliveries (in GameController) 
        //TODO: Discount coins
        transform.parent.GetComponent<RequestPanelController>().numOfRequests--;
        Destroy(this.gameObject);
    }
}
