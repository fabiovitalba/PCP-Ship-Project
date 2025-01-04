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
public class WaterBoilerMessageListener : MonoBehaviour
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
                    break;
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

    private void handleRotationValue(float rotationValue)
    {

    }

    private void handleSwitchValue(float switchValue)
    {

    }

    private void handleLightValue(float lightValue)
    {

    }
}
