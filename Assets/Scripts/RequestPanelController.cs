using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestPanelController : MonoBehaviour
{
    private LevelManager levelManager;
    private GameController gameController;
    private UIController UIController;
    private PowerUpsController powerUpsController;
    public GameObject requestBoxPrefab;
    public int numOfRequests;
    public Color RedColor;
    public Color GreenColor;
    public Color YellowColor;
    public GameObject[] boxes;
    public GameObject[] colors;
    public AudioSource DeliveryFailed_AS;
    public AudioSource DeliverySuccessful_AS;
    public List<GameObject> colorsRequested;
    public int TotalRequests;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        gameController = GameObject.Find("Gameplay").transform.Find("GameController").GetComponent<GameController>();
        UIController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        powerUpsController = UIController.transform.parent.Find("PowerUps").GetComponent<PowerUpsController>();

        if (UIController.isTutorial)
        {
            //Request specifics box and forms
            RequestSpecificBox("green");
        }
        else if (UIController.isToolTutorial)
        {
            //Request specifics box and forms
            RequestSpecificBox("blue");
        }
        else
        {
            InvokeRepeating("RequestBox", 1f, 15f);
        }
    }

    public void RequestBox()
    {
        if (numOfRequests < 3)
        {
            GameObject box = Instantiate(requestBoxPrefab);

            box.GetComponent<RequestBox>().boxRequested = boxes[Random.Range(0, 6)].transform.Find("Box").GetComponent<SpriteRenderer>().sprite;

            //GameObject colorRepositoryObj = colors[Random.Range(0,6)];

            box.GetComponent<RequestBox>().colorRepositoryObj = RequestRamdomColor();
            //print(box.GetComponent<RequestBox>().colorRepositoryObj.name + " requested!");

            colorsRequested.Add(box.GetComponent<RequestBox>().colorRepositoryObj);

            box.GetComponent<RequestBox>().colorRequested = box.GetComponent<RequestBox>().colorRepositoryObj.transform.Find("Ink").GetComponent<SpriteRenderer>().color;

            box.transform.Find("Form").GetComponent<Image>().sprite = box.GetComponent<RequestBox>().boxRequested;
            box.transform.Find("Color").GetComponent<Image>().color = box.GetComponent<RequestBox>().colorRequested;

            //print(GameObject.FindGameObjectWithTag("Rail").transform.Find(colorRepositoryObj.name));
            //print(box.GetComponent<RequestBox>().colorRequested);

            //Instantiate the requested box inside the RequestPanel
            box.transform.SetParent(transform);
            numOfRequests++;
            TotalRequests++;

            //Verify if has to broke a repository
            if (powerUpsController.NoBrokenBottles_Flag == false)
            {
                foreach (int position in levelManager.requestedColorsPosition)
                {
                    if (position == TotalRequests)
                    {
                        gameController.BrokeRepository(box.GetComponent<RequestBox>().colorRepositoryObj);
                        break;
                    }
                }
            }
        }
    }

    private GameObject RequestRamdomColor()
    {
        GameObject repositoryToRequest = colors[Random.Range(0, 6)];
        foreach (GameObject colorObj in colorsRequested)
        {
            //Verify if color already requested 
            //(It will not repeat itself if the color already on screen)
            if (repositoryToRequest.name == colorObj.name)
            {
                print(repositoryToRequest.name + " already requested. Requesting again...");
                return RequestRamdomColor();
            }
        }
        return repositoryToRequest;
    }

    private void RequestSpecificBox(string color)
    {
        GameObject box = Instantiate(requestBoxPrefab);

        //Request a moon
        box.GetComponent<RequestBox>().boxRequested = boxes[0].transform.Find("Box").GetComponent<SpriteRenderer>().sprite;

        if (color == "green")
        {
            box.GetComponent<RequestBox>().colorRepositoryObj = colors[3];
        }
        else if (color == "blue")
        {
            box.GetComponent<RequestBox>().colorRepositoryObj = colors[0];
        }

        colorsRequested.Add(box.GetComponent<RequestBox>().colorRepositoryObj);

        box.GetComponent<RequestBox>().colorRequested = box.GetComponent<RequestBox>().colorRepositoryObj.transform.Find("Ink").GetComponent<SpriteRenderer>().color;

        box.transform.Find("Form").GetComponent<Image>().sprite = box.GetComponent<RequestBox>().boxRequested;
        box.transform.Find("Color").GetComponent<Image>().color = box.GetComponent<RequestBox>().colorRequested;

        //Instantiate the requested box inside the RequestPanel
        box.transform.SetParent(transform);
        numOfRequests++;
        TotalRequests++;

        if (UIController.isToolTutorial)
        {
            gameController.BrokeRepository(box.GetComponent<RequestBox>().colorRepositoryObj);
        }
    }
}
