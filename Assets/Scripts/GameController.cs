using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update  
    private float paintSpeed;
    public bool isPainting;
    public GameObject currentRepository;
    public GameObject currentBox;
    public AudioClip[] liquidClips;
    public AudioClip glitchClip;
    public AudioSource InkMachine_AS;
    public bool isChangingRepository;
    private UIController uiController;
    public GameObject PanelForms;
    public GameObject RequestPanel;
    public float[] repositoryXPositions = { -160f, -80f, 0f, 80f, 160f, 240f };

    //public GameObject InkShopButton; // *** M ***   

    void Start()
    {
        //Initialize values
        paintSpeed = 1.5f;
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();

        //Load repository position randomically
        ShuffleArray(repositoryXPositions);

        GameObject[] repositories = GameObject.FindGameObjectsWithTag("Repository");
        for (int i = 0; i < repositories.Length; i++)
        {
            repositories[i].transform.localPosition = new Vector3(repositoryXPositions[i], repositories[i].transform.localPosition.y, repositories[i].transform.localPosition.z);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump"))
        {
            NewPaintFluid();
        }

        if (isPainting)
            Paint();

        //InkShopButton.SetActive(true); // *** M ***
    }

    public void NewPaintFluid()
    {
        if (!isChangingRepository
        && !isPainting
        && !currentRepository.GetComponent<InkRepositoryController>().isFilling
        && currentBox)
        {
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

                InkMachine_AS.clip = liquidClips[Random.Range(0, 2)];
                InkMachine_AS.volume = 1f;
                InkMachine_AS.Play();
                currentRepository.GetComponent<InkRepositoryController>().CallTurnOffLight();
                InvokeRepeating("Vibrate", 1.0f, 0.2f);
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
                }

                DestroyAllMetaballs();
                currentBox.GetComponent<BoxController>().percentage = 0;
                currentBox.SetActive(false);
                currentBox = null;
            }

            isPainting = false;

            CancelInvoke("Vibrate");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ChangeCurrentBox(GameObject newBox)
    {
        if (currentBox)
            currentBox.SetActive(false);

        Water2D.Water2D_Spawner.instance.SetWaterColor(newBox.GetComponent<BoxController>().currentColor, Color.Lerp(newBox.GetComponent<BoxController>().currentColor, Color.white, .2f));
        newBox.SetActive(true);
        currentBox = newBox;
        uiController.ClosePanel(PanelForms);
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
}
