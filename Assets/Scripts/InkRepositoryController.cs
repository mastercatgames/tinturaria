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

    private void OnTriggerEnter2D(Collider2D other) {
        gameController.currentRepository = gameObject;
    }
}
