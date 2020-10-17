using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestBox : MonoBehaviour
{
    private Image bar;
    private RequestPanelController requestPanelController;
    private GameController gameController;
    private PowerUpsController powerUpsController;
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
        gameController = GameObject.Find("Gameplay").transform.Find("GameController").GetComponent<GameController>();
        powerUpsController = transform.parent.parent.Find("PowerUps").GetComponent<PowerUpsController>();
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
            if (!isDestroyingBox && !isPlayingAnimation("deliverySuccess"))
            {
                isDestroyingBox = true;
                maskColor.color = requestPanelController.RedColor;
                var tempColor = maskColor.color;
                tempColor.a = 0.5f;
                maskColor.color = tempColor;

                GetComponent<Animator>().Play("deliveryFailed");
                GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("DeliveryFailed");
                gameController.DiscountCoins();
            }
        }
    }

    void DestroyBox()
    {
        //Is called when the "deliveredSuccess" or "deliveredFailed" animations is finished
        transform.parent.GetComponent<RequestPanelController>().numOfRequests--;
        Destroy(this.gameObject);
        bool removed = requestPanelController.colorsRequested.Remove(colorRepositoryObj);

        // if (removed)
        //     print(colorRepositoryObj + " was removed from ColorsRequested list!");
        // else
        //     print("(BUG)" + colorRepositoryObj + " wasn't removed from ColorsRequested list!");


        //Request another box immediately
        if (powerUpsController.BoosterFilling_Box_Flag)
        {
            requestPanelController.RequestBox();
        }

    }

    public void DeliveryBox()
    {
        if (!isPlayingAnimation("deliveryFailed"))
        {
            maskColor.color = requestPanelController.GreenColor;
            var tempColor = maskColor.color;
            tempColor.a = 0.5f;
            maskColor.color = tempColor;

            GetComponent<Animator>().Play("deliverySuccess");
            GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("DeliverySuccessful");

            gameController.EarnCoins();
        }
        else
        {
            print("Too late!");
        }
    }

    bool isPlayingAnimation(string stateName)
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
}
