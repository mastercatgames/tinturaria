using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestBox : MonoBehaviour
{
    private Image bar;
    private RequestPanelController requestPanelController;
    private GameController gameController;
    public bool isDestroyingBox;
    private Image maskColor;
    private float barSpeed;
    public Color colorRequested;
    public Sprite boxRequested;
    public GameObject colorRepositoryObj;
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("BarBG").Find("Bar").GetComponent<Image>();
        maskColor = transform.Find("MaskColor").GetComponent<Image>();
        requestPanelController = transform.parent.GetComponent<RequestPanelController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        //TODO: Test the best time to fit in gameplay
        //0.01f = 01:40m
        //0.02f = 00:50m (50 sec)
        //0.015f = 01:07m
        //0.016f = 01:02m
        //0.0166f = //01:00m (1/60 = 0.0166)
        barSpeed = 0.0166f; //(1/60) 
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
                requestPanelController.DeliveryFailed_AS.Play();
                gameController.DiscountCoins();
            }
        }
    }

    void DestroyBox()
    {
        //TODO: Discount from a new variable numberOfFailedDeliveries (in GameController) 
        //TODO: Discount coins
        transform.parent.GetComponent<RequestPanelController>().numOfRequests--;
        Destroy(this.gameObject);
        bool removed = requestPanelController.colorsRequested.Remove(colorRepositoryObj);

        if (removed)
            print(colorRepositoryObj + " was removed from ColorsRequested list!");
        else
            print("(BUG)" + colorRepositoryObj + " wasn't removed from ColorsRequested list!");

    }

    public void DeliveryBox()
    {
        maskColor.color = requestPanelController.GreenColor;
        var tempColor = maskColor.color;
        tempColor.a = 0.5f;
        maskColor.color = tempColor;

        GetComponent<Animator>().Play("deliverySuccess");
        requestPanelController.DeliverySuccessful_AS.Play();

        gameController.EarnCoins();

        // print(transform.parent);
        // print(transform.parent.GetComponent<RequestPanelController>().colorsRequested.Count);
        //Broke a repository
        // GameObject repositoryToBroke = transform.parent.GetComponent<RequestPanelController>().colorsRequested[transform.parent.GetComponent<RequestPanelController>().colorsRequested.Count -1];

        // gameController.BrokeRepository(repositoryToBroke);
        // print("Must broke the *" + repositoryToBroke.name + "*");

    }
}
