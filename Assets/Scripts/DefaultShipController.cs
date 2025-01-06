using System;
using Unity.VisualScripting;
using UnityEngine;

public class DefaultShipController : MonoBehaviour
{
    private ShipMovement shipMovement;
    private ShipSearchLight shipSearchLight;
    private WaterBoilerShipController waterBoilerShipController;

    // Use this for initialization
    void Start()
    {
        shipMovement = GetComponent<ShipMovement>();
        shipSearchLight = GetComponent<ShipSearchLight>();
        waterBoilerShipController = GetComponent<WaterBoilerShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!waterBoilerShipController.waterBoilerConnected) {
            // Get default Inputs and call the Ship's methods
            shipMovement.Steer(Input.GetAxis("Horizontal"));
            shipMovement.Accelerate(Input.GetAxis("Vertical"));
            if (Input.GetKey(KeyCode.Space)) {
                shipSearchLight.ToggleSearchLights(true);
            } else {
                shipSearchLight.ToggleSearchLights(false);
            }
        }
    }
}
