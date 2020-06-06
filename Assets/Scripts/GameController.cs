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
    public GameObject lastColorUsed;
    public AudioClip[] liquidClips;
    public AudioSource audioSource_sfx;

    //public GameObject InkShopButton; // *** M ***

    void Start()
    {
        //Initialize values
        paintSpeed = 2.3f;
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
        if (lastColorUsed != null)
        {
            if (lastColorUsed.name != currentRepository.name)
            {      
                //Destroy all metaballs inside the box (because the color was changed)
                //Destroy only clones, keeping the original
                foreach (Transform item in currentBox.transform.Find("InsideBox").transform)
                {
                    if (item.GetSiblingIndex() > 0)
                        GameObject.Destroy(item.gameObject);
                }
            }
        }
        if (currentRepository.GetComponent<InkRepositoryController>().inkfillAmount > 0f && !isPainting)
        {
            isPainting = true;
            Water2D.Water2D_Spawner.instance.RunSpawnerOnce(currentBox.transform.Find("InsideBox").gameObject, currentRepository);
            lastColorUsed = currentRepository;

            audioSource_sfx.clip = liquidClips[Random.Range(0, 2)];
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
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
