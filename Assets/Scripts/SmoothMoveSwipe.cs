using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMoveSwipe : MonoBehaviour
{

    private Vector2 startTouchPosition, endTouchPosition;
    private Vector3 startRocketPosition, endRocketPosition;
    private float flyTime;
    public float flightDuration = 0.1f; //Move speed (We can modify as a special power in the game)
    private float maxMovePositionX = 2.42f; //Added by Augusto Polonio
    private float maxScreenPositionX = 5f; //Added by Augusto Polonio
    AudioSource audioSource; //Added by Augusto Polonio
    public bool downPitch; //Added by Augusto Polonio
    public float targetPitch;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            startTouchPosition = Input.GetTouch(0).position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;

            if ((endTouchPosition.x < startTouchPosition.x) && transform.position.x > -maxScreenPositionX)
                StartCoroutine(Fly("left"));

            if ((endTouchPosition.x > startTouchPosition.x) && transform.position.x < maxScreenPositionX)
                StartCoroutine(Fly("right"));
        }
    }

    private IEnumerator Fly(string whereToFly)
    {
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
}
