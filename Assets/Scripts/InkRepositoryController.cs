using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkRepositoryController : MonoBehaviour
{
    [Range (0f,1f)]public float inkfillAmount;
    [Range (0f,1f)]public float limitToFill;
    private GameController gameController;
    private float [] startInkfillAmount = { 0f, 0.25f, 0.5f, 0.75f, 1f};

    void Start()
    {
        //TODO: Each hourglass can be initialized "broken" or "empty"
        inkfillAmount = startInkfillAmount[Random.Range(0, 5)];
        limitToFill = inkfillAmount - 0.25f;

        transform.Find("InkMask").Find("Mask").localScale = new Vector3(1, inkfillAmount, 1f);

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameController.currentRepository = gameObject;
        //Change to current color
        Water2D.Water2D_Spawner.instance.FillColor = Water2D.Water2D_Spawner.instance.StrokeColor = gameObject.transform.Find("Ink").GetComponent<SpriteRenderer>().color;
        //StrokeColor To lighten by 20%
        Water2D.Water2D_Spawner.instance.StrokeColor = Color.Lerp(Water2D.Water2D_Spawner.instance.StrokeColor, Color.white, .2f);

    }
}
