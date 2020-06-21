using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestPanelController : MonoBehaviour
{
    public GameObject requestBoxPrefab;
    public int numOfRequests;
    public Color RedColor;
    public Color GreenColor;
    public Color YellowColor;
    public GameObject[] boxes;
    public GameObject[] colors;    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("RequestBox", 1f, 15f);
    }

    void RequestBox()
    {
        if (numOfRequests < 3)
        {
            GameObject box = Instantiate(requestBoxPrefab);

            box.GetComponent<RequestBox>().boxRequested = boxes[Random.Range(0,6)].transform.Find("Box").GetComponent<SpriteRenderer>().sprite;
            box.GetComponent<RequestBox>().colorRequested = colors[Random.Range(0,6)].transform.Find("Ink").GetComponent<SpriteRenderer>().color;

            box.transform.Find("Form").GetComponent<Image>().sprite = box.GetComponent<RequestBox>().boxRequested;
            box.transform.Find("Color").GetComponent<Image>().color = box.GetComponent<RequestBox>().colorRequested;
            
            //Instantiate the requested box inside the RequestPanel
            box.transform.SetParent(transform);
            numOfRequests++;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
