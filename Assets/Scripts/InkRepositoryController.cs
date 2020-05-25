using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkRepositoryController : MonoBehaviour
{
    public float inkfillAmount;
    public float limitToFill;
    private GameController gameController;

    void Start()
    {
        //TODO: Each hourglass can be initialized "broken" or "empty"

        inkfillAmount = 1;
        limitToFill = 0.75f; //init value. After we can subtract 0.25f of each print until get 0, that means the ink of the bottle is over

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
        // Water2D.Water2D_Spawner.instance.FillColor = Water2D.Water2D_Spawner.instance.StrokeColor = gameObject.transform.Find("Ink").GetComponent<SpriteRenderer>().color;
        // //StrokeColor To lighten by 20%
        // Water2D.Water2D_Spawner.instance.StrokeColor = Color.Lerp(Water2D.Water2D_Spawner.instance.StrokeColor, Color.white, .2f);

        // if (gameObject.name == "Blue")
		// 	Water2D.Water2D_Spawner.instance.WaterMaterial = Water2D.Water2D_Spawner.instance.WaterMaterials[0];
		// 	else
		// 	Water2D.Water2D_Spawner.instance.WaterMaterial = Water2D.Water2D_Spawner.instance.WaterMaterials[1];

    }
}
