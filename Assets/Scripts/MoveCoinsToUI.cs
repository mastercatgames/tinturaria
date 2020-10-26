using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCoinsToUI : MonoBehaviour
{
    private float speed;
    public RectTransform target;
    private AudioController audioController;
    private UIController uiController;

    void Start()
    {
        if (speed == 0f)
            speed = 3000f;

        audioController = transform.parent.Find("AudioController").GetComponent<AudioController>();
        uiController = transform.parent.Find("UIController").GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().position = Vector3.MoveTowards(GetComponent<RectTransform>().position, target.position, speed * Time.deltaTime);

        //Check if the position of the cube and sphere are approximately equal. (When done..)
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            // print("Coin was collected +50!");
            uiController.currentTotalCoins -= 50;
            if (uiController.currentTotalCoins <= 0)
            {
                uiController.gameOverPanel.transform.Find("ButtonsGrid").gameObject.SetActive(true);
            }

            Text UICoinsTotal = target.parent.Find("Text").GetComponent<Text>();
            UICoinsTotal.text = (int.Parse(UICoinsTotal.text) + 50).ToString();
            audioController.PlaySFX("coinsPurchase");
            target.parent.GetComponent<Animator>().Play("UI_HighLight_Zoom");
            Destroy(gameObject);
        }
    }
}
