using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkRepositoryController : MonoBehaviour
{
    [Range(0f, 1f)] public float inkfillAmount;
    [Range(0f, 1f)] public float limitToFill;
    private GameController gameController;
    private UIController uiController;
    public GameObject BucketPanel;
    private float[] startInkfillAmount = { 0f, 0.25f, 0.5f, 0.75f, 1f };
    public bool isFilling = false;
    [Range(0.3f, 3f)] public float fillSpeed = 0.03f;
    public int currentLight;

    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        //TODO: Each hourglass can be initialized "broken" or "empty"
        inkfillAmount = startInkfillAmount[Random.Range(0, 5)];
        limitToFill = inkfillAmount - 0.25f;

        if (inkfillAmount == 1f)
            currentLight = 0;
        else if (inkfillAmount == 0.75f)
            currentLight = 1;
        else if (inkfillAmount == 0.5f)
            currentLight = 2;
        else if (inkfillAmount == 0.25f)
            currentLight = 3;
        else if (inkfillAmount == 0f)
            currentLight = 4;

        transform.Find("InkMask").Find("Mask").localScale = new Vector3(1f, inkfillAmount, 1f);

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFilling)
        {
            inkfillAmount += Time.deltaTime * fillSpeed;
            transform.Find("InkMask").Find("Mask").localScale = new Vector3(1f, inkfillAmount, 1f);

            if (inkfillAmount >= 1f)
            {
                isFilling = false;
                inkfillAmount = 1f; //fix the value to 1 instead of 1.054f, e.g.
                limitToFill = inkfillAmount - 0.25f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameController.currentRepository = gameObject;
        //Change to current color
        Water2D.Water2D_Spawner.instance.FillColor = Water2D.Water2D_Spawner.instance.StrokeColor = gameObject.transform.Find("Ink").GetComponent<SpriteRenderer>().color;
        //StrokeColor To lighten by 20%
        Water2D.Water2D_Spawner.instance.StrokeColor = Color.Lerp(Water2D.Water2D_Spawner.instance.StrokeColor, Color.white, .2f);
        //gameController.ChangeLightColor();
    }

    public void FillRepository()
    {
        isFilling = true;
        uiController.ClosePanel(BucketPanel);
    }
}
