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
    private TutorialController tutorialController; //Added by Augusto Polonio
    public float targetPitch;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.Find("Gameplay").transform.Find("GameController").GetComponent<GameController>();
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        tutorialController = uiController.transform.parent.Find("Tutorial").GetComponent<TutorialController>();
    }

    // Update is called once per frame
    private void Update()
    {
        //MOBILE INPUT
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            startTouchPosition = Input.GetTouch(0).position;

        if (!gameController.isPainting
        && !gameController.isChangingRepository
        && !uiController.somePanelIsOpen
        && uiController.isInGamePlay
        && !uiController.blockSwipe
        && !GameObject.Find("AudioController").GetComponent<AudioController>().AudioIsPlaying("InkMachineMove")
        && Input.touchCount > 0
        && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            gameController.isChangingRepository = true;
            endTouchPosition = Input.GetTouch(0).position;

            if ((endTouchPosition.x < startTouchPosition.x) && transform.position.x > -maxLeftScreenPositionX)
                StartCoroutine(Fly("left"));
            else if (!uiController.blockRightSwipe &&
                (endTouchPosition.x > startTouchPosition.x && transform.position.x < maxRightScreenPositionX))
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

                GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("InkMachineMove");
                Invoke("PlayInkMachineMoveSFXTwice", 0.42f);

                while (flyTime < flightDuration)
                {
                    flyTime += Time.deltaTime;
                    transform.position = Vector2.Lerp
                        (startRocketPosition, endRocketPosition, flyTime / flightDuration);
                    yield return null;
                }
                downPitch = false;
                gameController.isChangingRepository = false;

                gameController.transform.parent.GetComponent<ZoomObject>().machineLastPosition = new Vector3(transform.position.x, 3.028f, 0f);                

                if (uiController.isTutorial)
                {
                    print("Swipe Tutorial completed! Now it's blocked again! \n Show step 6!");
                    //Show Step 6 after time (to avoid painting before time)
                    tutorialController.Invoke("NextStep", 0.2f);
                }

                break;

            case "right":
                flyTime = 0f;
                startRocketPosition = transform.position;
                endRocketPosition = new Vector3
                    (startRocketPosition.x + maxMovePositionX, transform.position.y, transform.position.z);

                GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("InkMachineMove");
                Invoke("PlayInkMachineMoveSFXTwice", 0.42f);

                while (flyTime < flightDuration)
                {
                    flyTime += Time.deltaTime;
                    transform.position = Vector2.Lerp
                        (startRocketPosition, endRocketPosition, flyTime / flightDuration);
                    yield return null;
                }
                downPitch = false;
                gameController.isChangingRepository = false;

                gameController.transform.parent.GetComponent<ZoomObject>().machineLastPosition = new Vector3(transform.position.x, 3.028f, 0f);
                break;
        }

    }

    //Added by Augusto Polonio
    private void PlayInkMachineMoveSFXTwice()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("InkMachineMove");
    }

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
