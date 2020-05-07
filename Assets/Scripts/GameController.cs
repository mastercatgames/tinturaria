using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update    
    private GameObject inkLiquidClone;
    private Transform inkMask;
    public float inkfillAmount;
    public float formfillAmount;
    public float limitToFill;
    public float paintSpeed;
    public bool isPainting;
    public GameObject currentBottle;
    public GameObject currentForm;

    void Start()
    {
        //Initialize values
        inkMask = currentBottle.transform.Find("InkMask");
        inkfillAmount = inkMask.transform.localScale.y;
        limitToFill = 0.75f; //init value. After we can subtract 0.25f of each print until get 0, that means the ink of the bottle is over
        paintSpeed = 3f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            if (inkfillAmount > 0f)
                        isPainting = true;
        }

        if (isPainting)
            Paint();

    }

    void Paint()
    {
        if (inkfillAmount >= limitToFill && inkfillAmount > 0f)
        {
            inkfillAmount = inkMask.localScale.y;

            inkMask.localScale += Vector3.down * paintSpeed * 0.001f; //0.001f to more accuracy

            if (inkLiquidClone == null)
            {

                inkLiquidClone = Instantiate(currentBottle.transform.Find("InkLiquid").gameObject, currentBottle.transform.Find("InkLiquid"), true);
                inkLiquidClone.transform.parent = null;
            }

            inkLiquidClone.transform.Find("LiquidLeft").GetComponent<SpriteRenderer>().size += Vector2.up * paintSpeed * 0.035f;
            inkLiquidClone.transform.Find("LiquidRight").GetComponent<SpriteRenderer>().size += Vector2.up * paintSpeed * 0.035f;

        }
        else
        {
            if (inkLiquidClone != null)
            {
                inkLiquidClone.transform.localPosition += Vector3.down * paintSpeed * 0.006f;
                PaintForm();
            }
        }
    }

    void PaintForm()
    {
        currentForm.transform.Find("InkMask").localScale += Vector3.up * paintSpeed * 0.001f;
    }
}
