using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update  
    public float paintSpeed;
    public float originalPaintSpeed;
    public bool isPainting;
    public GameObject currentRepository;
    public GameObject currentBox;
    public AudioClip[] liquidClips;
    public AudioClip glitchClip;
    public AudioSource InkMachine_AS;
    public bool isChangingRepository;
    private UIController uiController;
    private LevelManager levelManager;
    private GameObject PanelForms;
    private GameObject Panel_Ink_Buckets;
    private GameObject RequestPanel;
    public float[] repositoryXPositions = { -160f, -80f, 0f, 80f, 160f, 240f };
    public int numCoins;
    public int numDeliveredBoxes;
    public int numFailedBoxes;
    public SmoothMoveSwipe inputManager;
    private PowerUpsController powerUpsController;
    public int earnCoinsValue;
    public int discountCoinsValue;

    void Start()
    {
        //Initialize values
        originalPaintSpeed = 1.5f;
        paintSpeed = originalPaintSpeed;
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();        

        //Load repository position randomically
        ShuffleArray(repositoryXPositions);

        GameObject[] repositories = GameObject.FindGameObjectsWithTag("Repository");
        for (int i = 0; i < repositories.Length; i++)
        {
            repositories[i].transform.localPosition = new Vector3(repositoryXPositions[i], repositories[i].transform.localPosition.y, repositories[i].transform.localPosition.z);
        }

        //Init UI variables
        Transform UI = uiController.transform.parent;

        PanelForms = UI.Find("Panel_Forms").gameObject;
        Panel_Ink_Buckets = UI.Find("Panel_Ink_Buckets").gameObject;
        RequestPanel = UI.Find("RequestPanel").gameObject;

        powerUpsController = uiController.transform.parent.Find("PowerUps").GetComponent<PowerUpsController>();

        earnCoinsValue = powerUpsController.DoubleCash_Flag ? 200 : 100;
        discountCoinsValue = 150;
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
                NewPaintFluid();

            if (Input.GetButtonDown("Horizontal")
               && !isChangingRepository
               && !isPainting
               && !uiController.somePanelIsOpen
               && uiController.isInGamePlay)
            {
                if (Input.GetAxisRaw("Horizontal") > 0 && inputManager.transform.position.x < 4.8f)
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
            #endregion            
        }
    }

    public void NewPaintFluid()
    {
        if (!isChangingRepository
        && !isPainting
        && !currentRepository.GetComponent<InkRepositoryController>().isFilling
        && !uiController.somePanelIsOpen
        //&& !currentRepository.GetComponent<InkRepositoryController>().isBroken
        && currentBox)
        {
            if (currentRepository.GetComponent<InkRepositoryController>().isBroken)
            {
                print("Its broken!");
                currentRepository.transform.Find("HourGlass_Broken_SVG").GetComponent<Animator>().Play("ShakeRepository");
                InkMachine_AS.clip = glitchClip;
                InkMachine_AS.volume = 0.08f;
                InkMachine_AS.Play();
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

                //If isn't using power up, spawn the metaballs normally
                //But if is using power up, allows spawn the metaballs only once time
                if (powerUpsController.BoosterFilling_Box_Flag == false
                || (powerUpsController.BoosterFilling_Box_Flag == true && currentBox.GetComponent<BoxController>().percentage <= 0.25f))
                {
                    Water2D.Water2D_Spawner.instance.RunSpawnerOnce(currentBox.transform.Find("InsideBox").gameObject, currentRepository);
                }

                InkMachine_AS.clip = liquidClips[Random.Range(0, 2)];
                InkMachine_AS.volume = 1f;
                InkMachine_AS.Play();
                currentRepository.GetComponent<InkRepositoryController>().CallTurnOffLight();
                InvokeRepeating("Vibrate", 1.0f, 0.2f);
                //Block Box Chganging
                uiController.transform.parent.Find("ButtonsGrid").Find("FormBtn").GetComponent<Button>().interactable = false;
            }
            else
            {
                print("Empty!");
                StartCoroutine(currentRepository.GetComponent<InkRepositoryController>().EmptyRepositoryIndicator());
                InkMachine_AS.clip = glitchClip;
                InkMachine_AS.volume = 0.08f;
                InkMachine_AS.Play();
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
                    Debug.Log("Form: " + child.Find("Form").GetComponent<Image>().sprite.name + "/ Color: " + child.Find("Color").GetComponent<Image>().color);

                    if (child.Find("Form").GetComponent<Image>().sprite.name == currentBox.transform.Find("Box").GetComponent<SpriteRenderer>().sprite.name
                    && child.Find("Color").GetComponent<Image>().color == currentBox.GetComponent<BoxController>().currentColor)
                    {
                        requestedBox = child;
                    }
                }

                if (requestedBox)
                {
                    Debug.Log("Delivered!");
                    requestedBox.GetComponent<RequestBox>().DeliveryBox();
                }
                else
                {
                    Debug.Log("Doesn't Match!");
                    DiscountCoins();
                }

                DestroyAllMetaballs();
                currentBox.GetComponent<BoxController>().percentage = 0;
                currentBox.SetActive(false);
                currentBox = null;
                //Reset to default paint speed
                Water2D.Water2D_Spawner.instance.initSpeed = new Vector2(-0.15f, 2f);
                paintSpeed = originalPaintSpeed;
                //Reset power up status (reactivate button and hide icon)
                powerUpsController.BoosterFilling_Box_Flag = false;
                Water2D.Water2D_Spawner.instance.size = 0.15f;
                uiController.InkBtn_BoosterFillingBox_Icon_SetActive(false);
                uiController.Panel_PowerUps_SetInteractable("BoosterFilling_Box", true);
            }

            isPainting = false;
            CancelInvoke("Vibrate");
            uiController.transform.parent.Find("ButtonsGrid").Find("FormBtn").GetComponent<Button>().interactable = true;

            if (powerUpsController.BoosterFilling_Box_Flag)
            {
                NewPaintFluid();
                print("Power up to fill box faster!");
            }
        }
    }

    public void ChangeCurrentBox(GameObject newBox)
    {
        if (currentBox)
            currentBox.SetActive(false);

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
        Vibration.Vibrate(20);
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

            print("Fixing the *" + brokenRepository.name + "* repository!");
        }
    }

    public void BrokeRepository(GameObject repositoryToBroke)
    {
        repositoryToBroke.GetComponent<InkRepositoryController>().isBroken = true;

        //Panel_Ink_Buckets - management
        GameObject bucketButton = Panel_Ink_Buckets.transform.Find("Buckets").Find(repositoryToBroke.name).gameObject;
        bucketButton.GetComponent<Button>().interactable = false;
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
        Debug.Log("Repository *" + FixedRepository.name + "* was fixed!");

        FixedRepository.GetComponent<InkRepositoryController>().isFixing = false;
        FixedRepository.GetComponent<InkRepositoryController>().isBroken = false;
        FixedRepository.GetComponent<InkRepositoryController>().FixingTimeInSeconds = FixedRepository.GetComponent<InkRepositoryController>().OriginalTimeInSeconds;
        FixedRepository.GetComponent<InkRepositoryController>().transform.Find("Tool").gameObject.SetActive(false);

        //Panel_Ink_Buckets - management
        GameObject bucketButton = Panel_Ink_Buckets.transform.Find("Buckets").Find(FixedRepository.name).gameObject;
        bucketButton.GetComponent<Button>().interactable = true;
        Image buttonImage = bucketButton.transform.Find("Ink").GetComponent<Image>();
        bucketButton.transform.Find("Ink").GetComponent<Image>().color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
        bucketButton.transform.Find("Tool").gameObject.SetActive(false);

        FixedRepository.transform.Find("HourGlass_SVG").gameObject.SetActive(true);
        FixedRepository.transform.Find("InkMask").gameObject.SetActive(true);
        FixedRepository.transform.Find("HourGlass_Broken_SVG").gameObject.SetActive(false);
        FixedRepository.transform.Find("Reflection").gameObject.SetActive(true);

        powerUpsController.FixInTime_Flag = false;
        uiController.InkBtn_FixInTime_Icon_SetActive(false);
    }
}
