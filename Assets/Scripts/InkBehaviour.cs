using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBehaviour : MonoBehaviour
{
    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        gameController.isPainting = false;
        gameController.limitToFill -= 0.25f; //TODO: I think that limitToFill could be in other script, like BottleController, to keep the value of each bottle
        Destroy(this.gameObject);        
    }
}
