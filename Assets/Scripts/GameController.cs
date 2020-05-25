using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update    
    private GameObject inkLiquidClone;
    private Transform inkMask;
    public float inkfillAmount;
    public float limitToFill;
    public float paintSpeed;
    public bool isPainting;
    public GameObject currentRepository;
    public GameObject currentForm;
    public AudioClip[] liquidClips;
    public AudioSource audioSource_sfx;

    //public GameObject InkShopButton; // *** M ***

    void Start()
    {
        //Initialize values
        inkMask = currentRepository.transform.Find("InkMask").Find("Mask");
        //inkfillAmount = inkMask.transform.localScale.y;
        // limitToFill = 0.75f; //init value. After we can subtract 0.25f of each print until get 0, that means the ink of the bottle is over
         paintSpeed = 1.3f;
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
        if (currentRepository.GetComponent<InkRepositoryController>().inkfillAmount > 0f && !isPainting)
        {
            isPainting = true;
            Water2D.Water2D_Spawner.instance.RunSpawnerOnce();

            audioSource_sfx.clip = liquidClips[Random.Range(0,2)];
            audioSource_sfx.Play();
        }

    }

    void Paint()
    {
        if (currentRepository.GetComponent<InkRepositoryController>().inkfillAmount >= currentRepository.GetComponent<InkRepositoryController>().limitToFill && currentRepository.GetComponent<InkRepositoryController>().inkfillAmount > 0f)
        {
            currentRepository.GetComponent<InkRepositoryController>().inkfillAmount = currentRepository.transform.Find("InkMask").Find("Mask").localScale.y;

            currentRepository.transform.Find("InkMask").Find("Mask").localScale += Vector3.down * paintSpeed * 0.001f; //0.001f to more accuracy            
        }
        else
        {
            isPainting = false;
            currentRepository.GetComponent<InkRepositoryController>().limitToFill -= 0.25f; //TODO: I think that limitToFill could be in other script, like BottleController, to keep the value of each bottle
        }
        //Water2D.Water2D_Spawner.instance.RunSpawnerOnce();
        // if (inkfillAmount >= limitToFill && inkfillAmount > 0f)
        // {
        //     inkfillAmount = inkMask.localScale.y;

        //     inkMask.localScale += Vector3.down * paintSpeed * 0.001f; //0.001f to more accuracy

        //     if (inkLiquidClone == null)
        //     {

        //         inkLiquidClone = Instantiate(currentRepository.transform.Find("InkLiquid").gameObject, currentRepository.transform.Find("InkLiquid"), true);
        //         inkLiquidClone.transform.parent = null;
        //     }

        //     inkLiquidClone.transform.Find("LiquidLeft").GetComponent<SpriteRenderer>().size += Vector2.up * paintSpeed * 0.035f;
        //     inkLiquidClone.transform.Find("LiquidRight").GetComponent<SpriteRenderer>().size += Vector2.up * paintSpeed * 0.035f;

        // }
        // else
        // {
        //     if (inkLiquidClone != null)
        //     {
        //         inkLiquidClone.transform.localPosition += Vector3.down * paintSpeed * 0.006f;
        //         PaintForm();
        //     }
        // }
    }

    // void PaintForm()
    // {
    //     currentForm.transform.Find("InkMask").Find("Mask").localScale += Vector3.up * paintSpeed * 0.001f;
    // }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
