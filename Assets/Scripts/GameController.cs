﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    private UIController uiController;
    private LevelManager levelManager;
    private TutorialController tutorialController;
    private PowerUpsController powerUpsController;
    private SmoothMoveSwipe inputManager;
    public float[] repositoryXPositions = { -160f, -80f, 0f, 80f, 160f, 240f };
    public float paintSpeed, originalPaintSpeed;
    public int numCoins, numDeliveredBoxes, numFailedBoxes, earnCoinsValue, discountCoinsValue;
    public GameObject currentRepository, currentBox, PanelForms, Panel_Ink_Buckets, RequestPanel, lastOpenPanel;
    public GameObject[] boxes, colors;
    public bool isPainting, emptyTutorialIsFinished, isChangingRepository;
    EventSystem m_EventSystem;

    void OnEnable()
    {
        //Fetch the current EventSystem. Make sure your Scene has one.
        m_EventSystem = EventSystem.current;
    }

    void Start()
    {
        //Initialize values
        originalPaintSpeed = 1.5f;
        paintSpeed = originalPaintSpeed;
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        tutorialController = uiController.transform.parent.Find("Tutorial").GetComponent<TutorialController>();

        emptyTutorialIsFinished = false;

        //If is in tutorial, keep the repository positions, else...
        if (!uiController.isTutorial && !uiController.isToolTutorial)
        {
            //...Load repository position randomically
            ShuffleArray(repositoryXPositions);

            GameObject[] repositories = GameObject.FindGameObjectsWithTag("Repository");
            for (int i = 0; i < repositories.Length; i++)
            {
                repositories[i].transform.localPosition = new Vector3(repositoryXPositions[i], repositories[i].transform.localPosition.y, repositories[i].transform.localPosition.z);
            }
        }

        //Init UI variables
        Transform UI = uiController.transform.parent;

        PanelForms = UI.Find("Panel_Forms").gameObject;
        Panel_Ink_Buckets = UI.Find("Panel_Ink_Buckets").gameObject;
        RequestPanel = UI.Find("RequestPanel").gameObject;

        powerUpsController = uiController.transform.parent.Find("PowerUps").GetComponent<PowerUpsController>();

        earnCoinsValue = powerUpsController.DoubleCash_Flag ? 200 : 100;
        discountCoinsValue = 150;

        if (powerUpsController.BoosterFilling_Box_Flag)
        {
            Water2D.Water2D_Spawner.instance.initSpeed = new Vector2(-0.15f, -10f);
            //Water2D.Water2D_Spawner.instance.size = 0.25f;
            paintSpeed = 10f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (uiController.timeRemaining > 0)
        {
            if (isPainting)
                Paint();

            #region PC Input            
            if (Input.GetButtonDown("Jump"))
                NewPaintFluid(false);

            if (Input.GetAxis("Horizontal") != 0
               && !isChangingRepository
               && !isPainting
               && !uiController.somePanelIsOpen
               && !uiController.blockSwipe
               && !GameObject.Find("AudioController").GetComponent<AudioController>().AudioIsPlaying("InkMachineMove")
               && uiController.isInGamePlay)
            {
                if (Input.GetAxisRaw("Horizontal") > 0 && inputManager.transform.position.x < 4.8f && !uiController.blockRightSwipe)
                {
                    inputManager.CallFly("right");
                    isChangingRepository = true;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0 && inputManager.transform.position.x > -7f)
                {
                    inputManager.CallFly("left");
                    isChangingRepository = true;
                }
            }

            //Controller Input
            if (Input.GetButtonDown("L1") && lastOpenPanel == null)
            {
                lastOpenPanel = uiController.transform.parent.Find("Panel_Ink_Buckets").gameObject;
                uiController.OpenPanel(lastOpenPanel);
                m_EventSystem.SetSelectedGameObject(lastOpenPanel.transform.Find("Buckets").Find("Red").gameObject);
                lastOpenPanel.transform.Find("Buckets").Find("Red").GetComponent<Button>().Select();
            }
            if (Input.GetButtonDown("R1") && lastOpenPanel == null)
            {
                lastOpenPanel = uiController.transform.parent.Find("Panel_Forms").gameObject;
                uiController.OpenPanel(lastOpenPanel);
                m_EventSystem.SetSelectedGameObject(lastOpenPanel.transform.Find("Forms").Find("Circle").gameObject);
            }
            // if (Input.GetButtonDown("Fire2") && lastOpenPanel != null)
            // {
            //     uiController.ClosePanel(lastOpenPanel);
            //     lastOpenPanel = null;
            // }

            #endregion            
        }
    }

    public void NewPaintFluid(bool autoFill)
    {
        if (autoFill
         || (!isChangingRepository
        && !isPainting
        && !currentRepository.GetComponent<InkRepositoryController>().isFilling
        && !uiController.somePanelIsOpen
        // && !uiController.blockSwipe
        // && !uiController.blockRightSwipe
        && !uiController.blockPainting
        //&& !currentRepository.GetComponent<InkRepositoryController>().isBroken
        && currentBox))
        {
            if (currentRepository.GetComponent<InkRepositoryController>().isBroken)
            {
                //print("Its broken!");
                currentRepository.transform.Find("HourGlass_Broken_SVG").GetComponent<Animator>().Play("ShakeRepository");
                GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("Glitch-Error");
                return;
            }

            if (currentBox.GetComponent<BoxController>().currentColor != currentRepository.transform.Find("Ink").GetComponent<SpriteRenderer>().color
             && currentRepository.GetComponent<InkRepositoryController>().inkfillAmount > 0f)
            {
                DestroyAllMetaballs();

                //Reset percentage
                currentBox.GetComponent<BoxController>().percentage = 0f;
            }
            if (currentRepository.GetComponent<InkRepositoryController>().inkfillAmount > 0f)
            {
                isPainting = true;
                Water2D.Water2D_Spawner.instance.RunSpawnerOnce(currentBox.transform.Find("InsideBox").gameObject, currentRepository);
                GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("PaintLiquid" + Random.Range(1, 3)); // 1 to 2

                currentRepository.GetComponent<InkRepositoryController>().CallTurnOffLight();
                InvokeRepeating("Vibrate", 1.0f, 0.2f);

                //Block Box Changing and the bucket button with same color
                Transform FormBtn = uiController.transform.parent.Find("ButtonsGrid").Find("FormBtn");
                FormBtn.gameObject.GetComponent<Button>().interactable = false;
                FormBtn.Find("Clock").gameObject.SetActive(true);
                uiController.SetActiveBucketButton(currentRepository.name, false);

                if (!powerUpsController.BoosterFilling_Box_Flag)
                {
                    currentRepository.transform.Find("ClockSprite").gameObject.SetActive(true);
                }

                if (uiController.isTutorial)
                {
                    tutorialController.transform.Find("Step-6").Find("Dialog").gameObject.SetActive(false);
                    tutorialController.transform.Find("Step-6").Find("HandIcon").gameObject.SetActive(false);
                    tutorialController.Invoke("ShowTapHereAgain", 4f);
                    tutorialController.transform.Find("Step-11").Find("HandIcon").gameObject.SetActive(false);
                    tutorialController.Invoke("ShowHandIcon", 4f);
                }
            }
            else
            {
                //print("Empty!");
                StartCoroutine(currentRepository.GetComponent<InkRepositoryController>().EmptyRepositoryIndicator());
                GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("Glitch-Error");

                if (uiController.isTutorial && !emptyTutorialIsFinished)
                {
                    tutorialController.NextStep();
                    emptyTutorialIsFinished = true;
                }
            }
        }
    }

    void Paint()
    {
        if (currentRepository.GetComponent<InkRepositoryController>().inkfillAmount >= currentRepository.GetComponent<InkRepositoryController>().limitToFill
        && currentRepository.GetComponent<InkRepositoryController>().inkfillAmount > 0f)
        {
            currentRepository.GetComponent<InkRepositoryController>().inkfillAmount = currentRepository.transform.Find("InkMask").Find("Mask").localScale.y;
            currentRepository.transform.Find("InkMask").Find("Mask").localScale += Vector3.down * paintSpeed * 0.001f; //0.001f to more accuracy            
        }
        else
        {
            currentRepository.GetComponent<InkRepositoryController>().limitToFill -= 0.25f;
            currentBox.GetComponent<BoxController>().currentColor = Water2D.Water2D_Spawner.instance.WaterMaterial.color;
            currentBox.GetComponent<BoxController>().percentage += 0.25f;

            if (currentBox.GetComponent<BoxController>().percentage == 1)
            {
                //Verify if exists this box request
                Transform requestedBox = null;

                foreach (Transform child in RequestPanel.transform)
                {
                    //Debug.Log("Form: " + child.Find("Form").GetComponent<Image>().sprite.name + "/ Color: " + child.Find("Color").GetComponent<Image>().color);

                    if (child.Find("Form").GetComponent<Image>().sprite.name == currentBox.transform.Find("Box").GetComponent<SpriteRenderer>().sprite.name
                    && child.Find("Color").GetComponent<Image>().color == currentBox.GetComponent<BoxController>().currentColor)
                    {
                        requestedBox = child;
                    }
                }

                if (requestedBox)
                {
                    //Debug.Log("Delivered!");
                    requestedBox.GetComponent<RequestBox>().DeliveryBox();

                    if (uiController.isTutorial)
                    {
                        uiController.timeRemaining = 0f;
                    }
                }
                else
                {
                    Debug.Log("Doesn't Match!");
                    //TODO: Coins UI must have a simple animation, showing that coins was discounted (and maybe a red border image on entire screen)
                    DiscountCoins();
                }

                DestroyAllMetaballs();
                currentBox.GetComponent<BoxController>().percentage = 0;
                currentBox.SetActive(false);
                currentBox = null;

                uiController.InkBtn_BoosterFillingBox_Icon_SetActive(false);
                //Reset to default paint speed
                // Water2D.Water2D_Spawner.instance.initSpeed = new Vector2(-0.15f, 2f);                
                //Reset power up status (reactivate button and hide icon)
                //paintSpeed = originalPaintSpeed;
                // powerUpsController.BoosterFilling_Box_Flag = false;
                // Water2D.Water2D_Spawner.instance.size = 0.15f;

                // uiController.Panel_PowerUps_SetInteractable("BoosterFilling_Box", true);
            }

            isPainting = false;
            CancelInvoke("Vibrate");

            //Unlock Box Changing and the bucket button with same color
            Transform FormBtn = uiController.transform.parent.Find("ButtonsGrid").Find("FormBtn");
            FormBtn.gameObject.GetComponent<Button>().interactable = true;
            FormBtn.Find("Clock").gameObject.SetActive(false);
            uiController.SetActiveBucketButton(currentRepository.name, true);
            currentRepository.transform.Find("ClockSprite").gameObject.SetActive(false);

            if (powerUpsController.BoosterFilling_Box_Flag)
            {
                if (currentBox != null)
                {
                    NewPaintFluid(true);
                    //print("Power up to fill box faster!");
                }
            }
        }
    }

    public void ChangeCurrentBox(GameObject newBox)
    {
        lastOpenPanel = null;

        if (currentBox)
            currentBox.SetActive(false);

        //Get the real box
        newBox = transform.parent.Find("BottomInkMachine").Find(newBox.name).gameObject;

        Water2D.Water2D_Spawner.instance.SetWaterColor(newBox.GetComponent<BoxController>().currentColor, Color.Lerp(newBox.GetComponent<BoxController>().currentColor, Color.white, .2f));
        newBox.SetActive(true);
        currentBox = newBox;
        uiController.ClosePanel(PanelForms);

        uiController.InkBtn_BoosterFillingBox_Icon_SetActive(false);
        //uiController.Panel_PowerUps_SetInteractable("BoosterFilling_Box", true);

        //Avoid the ink (metadata) out of the box when change
        currentBox.transform.Find("InsideBox").gameObject.SetActive(false);
        Invoke("ActiveInsideBox", 0.8f);
    }

    void ActiveInsideBox()
    {
        currentBox.transform.Find("InsideBox").gameObject.SetActive(true);
    }

    void DestroyAllMetaballs()
    {
        //Destroy all metaballs inside the box (because the color was changed)
        //Destroy only clones, keeping the original
        foreach (Transform item in currentBox.transform.Find("InsideBox").transform)
        {
            if (item.name.Contains("Clone"))
                GameObject.Destroy(item.gameObject);
        }
    }

    void ShuffleArray<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }

    public void Vibrate()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (PlayerPrefs.GetString("vibration") == "on")
            Vibration.Vibrate(20);
#endif
    }

    public void EarnCoins()
    {
        numCoins += earnCoinsValue;
        uiController.numCoinsText.text = numCoins.ToString();
        numDeliveredBoxes++;
    }

    public void DiscountCoins()
    {
        numCoins -= 150;
        uiController.numCoinsText.text = numCoins.ToString();
        numFailedBoxes++;
    }

    public void FixRepository(GameObject brokenRepository)
    {
        if (powerUpsController.FixInTime_Flag == true)
        {
            //Fix In time if using power up
            FinishRepair(brokenRepository);
        }
        else
        {
            brokenRepository.GetComponent<InkRepositoryController>().isFixing = true;
            brokenRepository.transform.Find("Tool").gameObject.SetActive(true);
            brokenRepository.transform.Find("ClockSprite").gameObject.SetActive(true);

            //print("Fixing the *" + brokenRepository.name + "* repository!");
        }
    }

    public void BrokeRepository(GameObject repositoryToBroke)
    {
        repositoryToBroke.GetComponent<InkRepositoryController>().isBroken = true;

        //Panel_Ink_Buckets - management
        GameObject bucketButton = Panel_Ink_Buckets.transform.Find("Buckets").Find(repositoryToBroke.name).gameObject;
        //bucketButton.GetComponent<Button>().interactable = false;

        if (PlayerPrefs.GetInt("toolsCount") == 0)
        {
            bucketButton.GetComponent<Button>().interactable = false;
        }

        Image buttonImage = bucketButton.transform.Find("Ink").GetComponent<Image>();
        bucketButton.transform.Find("Ink").GetComponent<Image>().color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0.6f);
        bucketButton.transform.Find("Tool").gameObject.SetActive(true);

        repositoryToBroke.transform.Find("HourGlass_SVG").gameObject.SetActive(false);
        repositoryToBroke.transform.Find("InkMask").gameObject.SetActive(false);
        repositoryToBroke.transform.Find("HourGlass_Broken_SVG").gameObject.SetActive(true);
        repositoryToBroke.transform.Find("Reflection").gameObject.SetActive(false);
    }

    public void FinishRepair(GameObject FixedRepository)
    {
        //Debug.Log("Repository *" + FixedRepository.name + "* was fixed!");

        FixedRepository.GetComponent<InkRepositoryController>().isFixing = false;
        FixedRepository.GetComponent<InkRepositoryController>().isBroken = false;
        FixedRepository.GetComponent<InkRepositoryController>().FixingTimeInSeconds = FixedRepository.GetComponent<InkRepositoryController>().OriginalTimeInSeconds;
        FixedRepository.GetComponent<InkRepositoryController>().transform.Find("Tool").gameObject.SetActive(false);
        FixedRepository.GetComponent<InkRepositoryController>().transform.Find("ClockSprite").gameObject.SetActive(false);

        //Panel_Ink_Buckets - management
        GameObject bucketButton = Panel_Ink_Buckets.transform.Find("Buckets").Find(FixedRepository.name).gameObject;
        //bucketButton.GetComponent<Button>().interactable = true;
        Image buttonImage = bucketButton.transform.Find("Ink").GetComponent<Image>();
        bucketButton.transform.Find("Ink").GetComponent<Image>().color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
        bucketButton.transform.Find("Tool").gameObject.SetActive(false);

        FixedRepository.transform.Find("HourGlass_SVG").gameObject.SetActive(true);
        FixedRepository.transform.Find("InkMask").gameObject.SetActive(true);
        FixedRepository.transform.Find("HourGlass_Broken_SVG").gameObject.SetActive(false);
        FixedRepository.transform.Find("Reflection").gameObject.SetActive(true);

        // powerUpsController.FixInTime_Flag = false;
        // uiController.InkBtn_FixInTime_Icon_SetActive(false);
    }
    public void InitSmoothMoveSwipe()
    {
        inputManager = (SmoothMoveSwipe)FindObjectOfType(typeof(SmoothMoveSwipe));
    }
}
