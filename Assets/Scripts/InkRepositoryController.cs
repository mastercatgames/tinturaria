using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkRepositoryController : MonoBehaviour
{
    [Range(0f, 1f)] public float inkfillAmount;
    [Range(0f, 1f)] public float limitToFill;
    private GameController gameController;
    private UIController uiController;
    private PowerUpsController powerUpsController;
    private AudioController audioController;
    private GameObject BucketPanel;
    private float[] startInkfillAmount = { 0f, 0.25f, 0.5f, 0.75f, 1f };
    public float fillSpeed, originalFillSpeed, lightSpeed, OriginalTimeInSeconds, FixingTimeInSeconds;
    public int currentLight;
    public bool isFilling, isBroken, isFixing; 

    void Start()
    {
        originalFillSpeed = 0.2f;
        lightSpeed = originalFillSpeed;
        fillSpeed = 0.15f; //TODO: Think if its better to make the fill slower, like 0.1f (then you can use the power up to fill faster)
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        powerUpsController = uiController.transform.parent.Find("PowerUps").GetComponent<PowerUpsController>();
        audioController = uiController.transform.parent.Find("AudioController").GetComponent<AudioController>();
        BucketPanel = uiController.transform.parent.Find("Panel_Ink_Buckets").gameObject;

        if (uiController.isTutorial && name == "Green")
        {
            inkfillAmount = 0.25f;
            print("Green repository is loaded 25% (tutorial)...");
        }
        else
        {
            if (powerUpsController.BoosterFilling_AllBottles_Flag)
            {
                inkfillAmount = 1f;
            }
            else
            {
                inkfillAmount = startInkfillAmount[Random.Range(0, 5)];
            }
        }

        limitToFill = inkfillAmount - 0.25f;
        FixingTimeInSeconds = OriginalTimeInSeconds = 6f;

        if (inkfillAmount == 1f)
            currentLight = 4;
        else if (inkfillAmount == 0.75f)
            currentLight = 3;
        else if (inkfillAmount == 0.5f)
            currentLight = 2;
        else if (inkfillAmount == 0.25f)
            currentLight = 1;
        else if (inkfillAmount == 0f)
            currentLight = 0;

        transform.Find("InkMask").Find("Mask").localScale = new Vector3(1f, inkfillAmount, 1f);

        gameController = GameObject.Find("Gameplay").transform.Find("GameController").GetComponent<GameController>();

        initLights();
    }

    private void initLights()
    {
        //Turn on the lights according to inkfillAmount variable
        for (int i = 1; i <= currentLight; i++)
        {
            StartCoroutine(TurnOnLight(i));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFilling)
        {
            inkfillAmount += Time.deltaTime * fillSpeed;
            transform.Find("InkMask").Find("Mask").localScale = new Vector3(1f, inkfillAmount, 1f);

            if (!audioController.AudioIsPlaying("timerRunning"))
            {
                audioController.PlaySFX("timerRunning");
                audioController.PlaySFX("reloadingBottle");

                audioController.transform.Find("SFX").Find("reloadingBottle").GetComponent<AudioSource>().pitch = 0.4f;
            }

            ChangeAudioPitch();


            if (inkfillAmount >= 1f)
            {
                isFilling = false;
                inkfillAmount = 1f; //fix the value to 1 instead of 1.054f, e.g.
                limitToFill = inkfillAmount - 0.25f;
                uiController.SetActiveBucketButton(gameObject.name, true);
                transform.Find("ClockSprite").gameObject.SetActive(false);
                audioController.StopSFX("timerRunning");
                audioController.StopSFX("reloadingBottle");
                audioController.transform.Find("SFX").Find("reloadingBottle").GetComponent<AudioSource>().pitch = 0.4f;

                audioController.PlaySFX("timerDone");
                //Reset power up status (reactivate button and hide icon)
                // fillSpeed = originalFillSpeed;
                // powerUpsController.BoosterFilling_OneBottle_Flag = false;
                // powerUpsController.BoosterFilling_AllBottles_Flag = false;                
                // uiController.Panel_PowerUps_SetInteractable("BoosterFilling_OneBottle", true);
                // uiController.Panel_PowerUps_SetInteractable("BoosterFilling_AllBottles", true);
            }
        }

        if (isFixing)
        {
            if (!audioController.AudioIsPlaying("fixBottle"))
            {
                audioController.PlaySFX("fixBottle");                
            }

            if (FixingTimeInSeconds > 0)
            {
                FixingTimeInSeconds -= Time.deltaTime;
            }
            //If fixing time is over
            else
            {
                audioController.StopSFX("fixBottle"); 
                gameController.FinishRepair(gameObject);
                uiController.SetActiveBucketButton(gameObject.name, true);
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
    }

    public void FillOrFixRepository()
    {
        //Fill repository if it isn't broken
        if (!isBroken)
        {
            isFilling = true;
            StartCoroutine(AutoTurnOnLightsOnFill());
            uiController.ClosePanel(BucketPanel);

            if (powerUpsController.BoosterFilling_OneBottle_Flag)
            {
                fillSpeed = 2f;
            }
            else
            {
                transform.Find("ClockSprite").gameObject.SetActive(true);                
            }
        }
        else
        {
            //Fix repository!
            int toolsCount = PlayerPrefs.GetInt("toolsCount");

            if (toolsCount > 0 && !isFixing)
            {
                uiController.ClosePanel(BucketPanel);
                gameController.FixRepository(gameObject);                

                //Discount toolsCount
                PlayerPrefs.SetInt("toolsCount", toolsCount - 1);
                uiController.RefreshToolsCount();
            }
            else
            {
                print("You have no fixing tools!");
                //TODO: Alert
            }
        }
    }

    IEnumerator AutoTurnOnLightsOnFill()
    {
        while (currentLight < 4)
        {
            if (inkfillAmount >= 0.25f && currentLight < 1)
            {
                currentLight = 1;
                StartCoroutine(TurnOnLight(currentLight));
            }
            else if (inkfillAmount >= 0.5f && currentLight < 2)
            {
                currentLight = 2;
                StartCoroutine(TurnOnLight(currentLight));
            }
            else if (inkfillAmount >= 0.75f && currentLight < 3)
            {
                currentLight = 3;
                StartCoroutine(TurnOnLight(currentLight));
            }
            else if (inkfillAmount >= 1f && currentLight < 4)
            {
                currentLight = 4;
                StartCoroutine(TurnOnLight(currentLight));
            }

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        //print("Finished Coroutine");
    }

    public IEnumerator TurnOnLight(int lightNum)
    {
        //print("Turning On the Light: " + lightNum);
        SpriteRenderer lightSprite = transform.Find("Lights").Find("Light_" + lightNum).GetComponent<SpriteRenderer>();
        for (float alphaValue = 0f; alphaValue <= 1f; alphaValue += lightSpeed)
        {
            //print("Working in progress...");
            lightSprite.color = new Color(1f, 1f, 1f, alphaValue);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        //print("Finished Coroutine");
    }

    public void CallTurnOffLight()
    {
        StartCoroutine(TurnOffLight(currentLight));
        currentLight--;
    }

    IEnumerator TurnOffLight(int lightNum)
    {
        //print("Turning Off the Light: " + lightNum);
        SpriteRenderer lightSprite = transform.Find("Lights").Find("Light_" + lightNum).GetComponent<SpriteRenderer>();
        for (float alphaValue = 1f; alphaValue >= 0f; alphaValue -= lightSpeed)
        {
            //print("Working in progress...");
            lightSprite.color = new Color(1f, 1f, 1f, alphaValue);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        //print("Finished Coroutine");
    }

    public IEnumerator EmptyRepositoryIndicator()
    {
        //print("Empty Repo Indicator!");
        for (int i = 1; i <= 4; i++)
        {
            StartCoroutine(TurnOnLight(i));
        }
        for (int i = 1; i <= 4; i++)
        {
            StartCoroutine(TurnOffLight(i));
        }

        yield return new WaitForSeconds(0.4f);

        for (int i = 1; i <= 4; i++)
        {
            StartCoroutine(TurnOnLight(i));
        }
        for (int i = 1; i <= 4; i++)
        {
            StartCoroutine(TurnOffLight(i));
        }
        //print("Finished Coroutine");
    }

    private void ChangeAudioPitch()
    {
        // if (audioSource.pitch < 1.8 && !downPitch)
        //     targetPitch = 3;
        // else
        // {
        //     targetPitch = 0;
        //     downPitch = true;
        // }

        audioController.transform.Find("SFX").Find("reloadingBottle").GetComponent<AudioSource>().pitch = Mathf.Lerp(audioController.transform.Find("SFX").Find("reloadingBottle").GetComponent<AudioSource>().pitch, 2f, 0.2f * Time.deltaTime);
    }
}
