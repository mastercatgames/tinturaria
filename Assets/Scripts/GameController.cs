using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bottleInk;
    public GameObject formInk;

    public float inkfillAmount;
    public float formfillAmount;
    public float limitToFill;
    public float paintSpeed;

    void Start()
    {
        inkfillAmount = 1f;
        formfillAmount = 0f;
        limitToFill = 0.25f;
        //Debug.Log(bottleInk.GetComponent<Image>());
        //inkfillAmount = bottleInk.GetComponent<Image>().fillAmount * 100f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
            Paint();
        
    }

    void Paint()
    {
        //bottleInk.GetComponent<Image>().fillAmount -= Time.deltaTime * paintSpeed;
        
        if (formfillAmount <= limitToFill){
            inkfillAmount -= Time.deltaTime * paintSpeed;
            formfillAmount += Time.deltaTime * paintSpeed;

            bottleInk.GetComponent<Image>().fillAmount = inkfillAmount;
            formInk.GetComponent<Image>().fillAmount = formfillAmount;

            //Debug.Log(inkfillAmount);
        }
    }
}
