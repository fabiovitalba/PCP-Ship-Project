/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * When creating your message listeners you need to implement these two methods:
 *  - OnMessageArrived
 *  - OnConnectionEvent
 */
public class WaterBoilerShipController : MonoBehaviour
{
    [Header("Input Tuning")]
    public bool waterBoilerConnected = false;
    public float cutoffRotationValue = 40f;
    public float minLightValue = 0f;
    public float maxLightValue = 255f;

    [Header("Received Values (Debug)")]
    public float debugRelativeRotationValue = 0.0f;
    public float debugSwitchValue = 0.0f;
    public float debugLightValue = 0.0f;

    [Header("Translated Inputs (Debug)")]
    public float currentSteerInput = 0.0f;
    public float currentAccelInput = 0.0f;
    public bool currentLightInput = false;

    private ShipMovement shipMovement;
    private ShipSearchLight shipSearchLight;

    // Use this for initialization
    void Start()
    {
        shipMovement = GetComponent<ShipMovement>();
        shipSearchLight = GetComponent<ShipSearchLight>();
    }

    // Update is called once per frame
    void Update()
    {
        // unused
        if (waterBoilerConnected) {
            shipMovement.Steer(currentSteerInput);
            shipMovement.Accelerate(currentAccelInput);
            
            shipSearchLight.ToggleSearchLights(currentLightInput);
        }
    }

    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        string[] variables = msg.Split(',');
        foreach (var variable in variables) {
            string[] values = variable.Split(":");
            switch(values[0].ToLower()) {
                case "absoluterotationvalue":
                    break; // This value is received but ignored
                case "relativerotationvalue":
                    HandleRotationValue(float.Parse(values[1]));
                    break;
                case "switchvalue":
                    HandleSwitchValue(float.Parse(values[1]));
                    break;
                case "lightvalue":
                    HandleLightValue(float.Parse(values[1]));
                    break;
            }
        }
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success) {
            Debug.LogWarning("Controller over connected over serial port.");
            waterBoilerConnected = true;
        } else {
            Debug.LogWarning("Controller connection attempt failed or disconnection detected.");
            waterBoilerConnected = false;
        }
    }

    /// <summary>
    /// This Procedure receives a float with a relative Rotation value, meaning a delta from the original rotation of the water boiler.
    /// When the boiler is rotated to the right, the received value is below 0, and when the boiler is rotated to the left the value will be positive.
    /// </summary>
    /// <param name="rotationValue"></param>
    private void HandleRotationValue(float rotationValue)
    {
        float inputValue = rotationValue * -1;
        // First we ceil the value before mapping it.
        if (Mathf.Abs(inputValue) > cutoffRotationValue) {
            inputValue = Mathf.Sign(inputValue) * cutoffRotationValue;
        }

        // Now we map from [-ceil, ceil] to [0,1]
        inputValue = Mathf.InverseLerp(-cutoffRotationValue,cutoffRotationValue,inputValue);

        // And finally we map from [0,1] to [-1,1];
        inputValue = Mathf.Lerp(-1,1,inputValue);

        // The mapped value is now our Steering input
        currentSteerInput = inputValue;

        debugRelativeRotationValue = rotationValue;
    }

    /// <summary>
    /// Reads the switchValue received from the controller which will lie between 0 and 1. Anything above 0 counts as true.
    /// </summary>
    /// <param name="switchValue"></param>
    private void HandleSwitchValue(float switchValue)
    {
        currentLightInput = switchValue > 0;

        debugSwitchValue = switchValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lightValue"></param>
    private void HandleLightValue(float lightValue)
    {
        float inputValue = Mathf.InverseLerp(minLightValue, maxLightValue, lightValue);
        currentAccelInput = inputValue;

        debugLightValue = lightValue;
    }
}
