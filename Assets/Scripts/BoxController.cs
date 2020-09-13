using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Color currentColor;
    private UIController uiController;
    private PowerUpsController powerUpsController;
    [Range(0f, 1f)] public float percentage = 0; //only to know how much is inside box

    public void PlayDropBoxSFX()
    {
        gameObject.transform.parent.GetComponent<AudioSource>().Play();
        //powerUpsController = transform.parent.Find("Panel_PowerUps").GetComponent<PowerUpsController>();
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        powerUpsController = uiController.transform.parent.Find("Panel_PowerUps").GetComponent<PowerUpsController>();     
    }

    public void PowerUpIcon_SetActive()
    {
        if (powerUpsController.BoosterFilling_Box_Flag)
        {
            uiController.InkBtn_BoosterFillingBox_Icon_SetActive(true);
        }
    }
}
