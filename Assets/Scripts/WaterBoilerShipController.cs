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
    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        string[] variables = msg.Split(',');
        foreach (var variable in variables)
        {
            string[] values = variable.Split(":");
            switch(values[0].ToLower()) {
                case "absoluterotationvalue":
                    break; // This value is received but ignored
                case "relativerotationvalue":
                    handleRotationValue(float.Parse(values[1]));
                    break;
                case "switchvalue":
                    handleSwitchValue(float.Parse(values[1]));
                    break;
                case "lightvalue":
                    handleLightValue(float.Parse(values[1]));
                    break;
            }
        }
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Controller over connected over serial port.");
        else
            Debug.Log("Controller connection attempt failed or disconnection detected.");
    }

    /// <summary>
    /// This Procedure receives a float with a relative Rotation value, meaning a delta from the original rotation of the water boiler.
    /// When the boiler is rotated to the right, the received value is below 0, and when the boiler is rotated to the left the value will be positive.
    /// </summary>
    /// <param name="rotationValue"></param>
    private void handleRotationValue(float rotationValue)
    {
        Debug.Log("Rotation: " + rotationValue);
        // Negative: To the right
        // Positive: To the left
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="switchValue"></param>
    private void handleSwitchValue(float switchValue)
    {
        Debug.Log("Switch: " + switchValue);
        // Below 300 = Off
        // Above 300 = On
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lightValue"></param>
    private void handleLightValue(float lightValue)
    {
        Debug.Log("Light: " + lightValue);
        // Value between 0 and 255
    }
}
