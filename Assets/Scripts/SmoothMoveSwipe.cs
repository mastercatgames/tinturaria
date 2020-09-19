using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMoveSwipe : MonoBehaviour
{

    private Vector2 startTouchPosition, endTouchPosition;
    private Vector3 startRocketPosition, endRocketPosition;
    private float flyTime;
    public float flightDuration = 0.1f; //Move speed (We can modify as a special power in the game)
    private float maxMovePositionX = 2.40f; //Added by Augusto Polonio
    private float maxLeftScreenPositionX = 5f; //Added by Augusto Polonio
    private float maxRightScreenPositionX = 4f; //Added by Augusto Polonio
    AudioSource audioSource; //Added by Augusto Polonio
    public bool downPitch; //Added by Augusto Polonio
    private GameController gameController; //Added by Augusto Polonio
    private UIController uiController; //Added by Augusto Polonio
    public float targetPitch;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.Find("Gameplay").transform.Find("GameController").GetComponent<GameController>();
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            startTouchPosition = Input.GetTouch(0).position;

        if (!gameController.isPainting
        && !audioSource.isPlaying
        && !uiController.somePanelIsOpen
        && Input.touchCount > 0
        && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            gameController.isChangingRepository = true;
            endTouchPosition = Input.GetTouch(0).position;

            if ((endTouchPosition.x < startTouchPosition.x) && transform.position.x > -maxLeftScreenPositionX)
                StartCoroutine(Fly("left"));
            else if ((endTouchPosition.x > startTouchPosition.x) && transform.position.x < maxRightScreenPositionX)
                StartCoroutine(Fly("right"));
            else
                gameController.isChangingRepository = false;
        }
    }

    private IEnumerator Fly(string whereToFly)
    {
        transform.parent.Find("Machine").Find("Borders").GetComponent<Animator>().Play("MoveMachineBorders");

        switch (whereToFly)
        {
            case "left":
                flyTime = 0f;
                startRocketPosition = transform.position;
                endRocketPosition = new Vector3
                    (startRocketPosition.x - maxMovePositionX, transform.position.y, transform.position.z);

                audioSource.Play();
                while (flyTime < flightDuration)
                {
                    ChangeAudioPitch();
                    flyTime += Time.deltaTime;
                    transform.position = Vector2.Lerp
                        (startRocketPosition, endRocketPosition, flyTime / flightDuration);
                    yield return null;
                }
                downPitch = false;
                audioSource.pitch = 1f;
                audioSource.Stop();
                gameController.isChangingRepository = false;

                gameController.transform.parent.GetComponent<ZoomObject>().machineLastPosition = new Vector3(transform.position.x, 3.028f, 0f);
                break;

            case "right":
                flyTime = 0f;
                startRocketPosition = transform.position;
                endRocketPosition = new Vector3
                    (startRocketPosition.x + maxMovePositionX, transform.position.y, transform.position.z);

                audioSource.Play();
                while (flyTime < flightDuration)
                {
                    ChangeAudioPitch();
                    flyTime += Time.deltaTime;
                    transform.position = Vector2.Lerp
                        (startRocketPosition, endRocketPosition, flyTime / flightDuration);
                    yield return null;
                }
                downPitch = false;
                audioSource.pitch = 1f;
                audioSource.Stop();
                gameController.isChangingRepository = false;

                gameController.transform.parent.GetComponent<ZoomObject>().machineLastPosition = new Vector3(transform.position.x, 3.028f, 0f);
                break;
        }

    }

    //Added by Augusto Polonio
    private void ChangeAudioPitch()
    {
        if (audioSource.pitch < 1.8 && !downPitch)
            targetPitch = 3;
        else
        {
            targetPitch = 0;
            downPitch = true;
        }

        audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, flightDuration * Time.deltaTime);
    }

    public void CallFly(string direction)
    {
        StartCoroutine(Fly(direction));
    }
}
