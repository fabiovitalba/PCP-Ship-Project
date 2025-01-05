using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSearchLight : MonoBehaviour
{
    public int minMillisBetweenToggles = 250;

    private GameObject[] searchLights;
    private bool currLightsEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        if (searchLights == null)
            searchLights = GameObject.FindGameObjectsWithTag("ShipSearchlight");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleSearchLights(bool lightsEnabled)
    {
        if (currLightsEnabled != lightsEnabled) {
            foreach (GameObject searchLight in searchLights) {
                searchLight.GetComponent<Light>().enabled = lightsEnabled;
            }
            currLightsEnabled = lightsEnabled;
        }
    }
}
