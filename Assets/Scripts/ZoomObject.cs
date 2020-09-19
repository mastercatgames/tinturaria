using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomObject : MonoBehaviour
{
    public Vector3 ScaleZoomOut;
    public Vector3 ScaleZoomIn;
    public Vector3 machineCenterPosition;
    public Vector3 machineLastPosition;
    public float scalingRate;
    public bool zoomOut;
    public bool zoomIn;
    public Transform rail;
    private UIController uiController;

    private void Start()
    {
        scalingRate = 5f;
        ScaleZoomOut = new Vector3(0.38f, 0.38f, 0.38f);
        ScaleZoomIn = new Vector3(1f, 1f, 1f);

        rail = transform.Find("TopInkMachine").Find("Rail");
        machineCenterPosition = new Vector3(0f, rail.localPosition.y, 0f);
        machineLastPosition = new Vector3(0f, 3.028f, 0f);

        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
    }

    private void Update()
    {
        if (zoomOut)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, ScaleZoomOut, scalingRate * Time.deltaTime);
            rail.localPosition = Vector3.Lerp(rail.localPosition, machineCenterPosition, scalingRate * Time.deltaTime);

            if (Vector3.Distance(transform.localScale, ScaleZoomOut) < 0.01f
            && Vector3.Distance(rail.localPosition, machineCenterPosition) < 0.01f)
            {
                transform.localScale = ScaleZoomOut;
                rail.localPosition = machineCenterPosition;
                zoomOut = false;
                //uiController.somePanelIsOpen = false;
            } 
        }
        else if (zoomIn)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), scalingRate * Time.deltaTime);
            rail.localPosition = Vector3.Lerp(rail.localPosition, machineLastPosition, scalingRate * Time.deltaTime);

            if (Vector3.Distance(transform.localScale, ScaleZoomIn) < 0.01f
            && Vector3.Distance(rail.localPosition, machineLastPosition) < 0.01f)
            {
                transform.localScale = ScaleZoomIn;
                rail.localPosition = machineLastPosition;
                zoomIn = false;
                uiController.somePanelIsOpen = false;
                uiController.transform.parent.Find("GameplayMenuButtons").gameObject.SetActive(false);
            }
        }
    }
}
