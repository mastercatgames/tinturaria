using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update  
    public float paintSpeed;
    public bool isPainting;
    public GameObject currentRepository;
    public GameObject currentBox;
    public AudioClip[] liquidClips;
    public AudioSource audioSource_sfx;
    public bool isChangingRepository;
    private UIController uiController;
    public GameObject PanelForms;

    //public GameObject InkShopButton; // *** M ***

    void Start()
    {
        //Initialize values
        paintSpeed = 2.3f;
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
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
        if (!isChangingRepository && !isPainting)
        {
            if (currentBox.GetComponent<BoxController>().currentColor != currentRepository.transform.Find("Ink").GetComponent<SpriteRenderer>().color
             && currentRepository.GetComponent<InkRepositoryController>().inkfillAmount > 0f)
            {
                //Destroy all metaballs inside the box (because the color was changed)
                //Destroy only clones, keeping the original
                foreach (Transform item in currentBox.transform.Find("InsideBox").transform)
                {
                    if (item.name.Contains("Clone"))
                        GameObject.Destroy(item.gameObject);
                }

                //Reset percentage
                currentBox.GetComponent<BoxController>().percentage = 0f;
            }
            if (currentRepository.GetComponent<InkRepositoryController>().inkfillAmount > 0f)
            {
                isPainting = true;
                Water2D.Water2D_Spawner.instance.RunSpawnerOnce(currentBox.transform.Find("InsideBox").gameObject, currentRepository);

                audioSource_sfx.clip = liquidClips[Random.Range(0, 2)];
                audioSource_sfx.Play();
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
            isPainting = false;
            currentRepository.GetComponent<InkRepositoryController>().limitToFill -= 0.25f;
            currentBox.GetComponent<BoxController>().percentage += 0.25f;
            currentBox.GetComponent<BoxController>().currentColor = Water2D.Water2D_Spawner.instance.WaterMaterial.color;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ChangeCurrentBox(GameObject newBox)
    {
        currentBox.SetActive(false);
        Water2D.Water2D_Spawner.instance.SetWaterColor(newBox.GetComponent<BoxController>().currentColor, Color.Lerp(newBox.GetComponent<BoxController>().currentColor, Color.white, .2f));
        newBox.SetActive(true);
        currentBox = newBox;    
        uiController.ClosePanel(PanelForms);  
    }
}
