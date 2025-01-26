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

public enum RotationValue {
    Relative,
    Absolute
}

public enum SteeringMode {
    Direct,
    Indirect
}

public class WaterBoilerShipController : MonoBehaviour
{
    [Header("Input Tuning")]
    public bool waterBoilerConnected = false;
    public RotationValue rotationValue = RotationValue.Absolute;
    public SteeringMode steeringMode = SteeringMode.Direct;
    public float cutoffRelativeRotationValue = 40f;
    public float cutoffAbsoluteRotationValue = 90f; // The value actually goes to 127, but users likely never turn that hard.
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
            if (steeringMode == SteeringMode.Direct) {
                shipMovement.SetRudder(currentSteerInput * shipMovement.maxRudder);
            } else {
                shipMovement.Steer(currentSteerInput);
            }

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
                if (rotationValue == RotationValue.Absolute) {
                        HandleAbsoluteRotationValue(float.Parse(values[1]));
                    }
                    break;
                case "relativerotationvalue":
                    if (rotationValue == RotationValue.Relative) {
                        HandleRelativeRotationValue(float.Parse(values[1]));
                    }
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
    private void HandleRelativeRotationValue(float rotationValue)
    {
        float inputValue = rotationValue * -1;
        // First we ceil the value before mapping it.
        if (Mathf.Abs(inputValue) > cutoffRelativeRotationValue) {
            inputValue = Mathf.Sign(inputValue) * cutoffRelativeRotationValue;
        }

        // Now we map from [-ceil, ceil] to [0,1]
        inputValue = Mathf.InverseLerp(-cutoffRelativeRotationValue,cutoffRelativeRotationValue,inputValue);

        // And finally we map from [0,1] to [-1,1];
        inputValue = Mathf.Lerp(-1,1,inputValue);

        // The mapped value is now our Steering input
        currentSteerInput = inputValue;

        debugRelativeRotationValue = rotationValue;
    }

    private void HandleAbsoluteRotationValue(float rotationValue)
    {
        float inputValue = rotationValue * -1;
        // First we ceil the value before mapping it.
        if (Mathf.Abs(inputValue) > cutoffAbsoluteRotationValue) {
            inputValue = Mathf.Sign(inputValue) * cutoffAbsoluteRotationValue;
        }

        // Now we map from [-ceil, ceil] to [0,1]
        inputValue = Mathf.InverseLerp(-cutoffAbsoluteRotationValue,cutoffAbsoluteRotationValue,inputValue);

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
