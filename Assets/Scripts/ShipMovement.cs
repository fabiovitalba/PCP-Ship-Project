using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [Header("Acceleration")]
    public float speed = 0.0f;
    public float acceleration = 1.0f;
    public float decceleration = 0.75f;
    public float maxspeed = 7.0f;
    public float minspeed = 0.0f;

    [Header("Steering")]
    public float heading = 0.0f; // Where is the Ship heading to
    public float rudder = 0.0f; // What is the current steering input
    public float rudderDelta = 2.0f;
    public float maxRudder = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Accelerate(float inputSpeed)
    {
        if ((inputSpeed <= 0.01f) && (inputSpeed >= -0.01f)) {
            if (speed > 0) {
                speed -= decceleration * Time.deltaTime;
            }
            if (speed < 0) {
                speed = 0;
            }
        } else {
            speed += inputSpeed * acceleration * Time.deltaTime;
            if (speed > maxspeed) {
                speed = maxspeed;
            }
            else if (speed < minspeed) {
                speed = minspeed;
            }
        }

        if (speed > maxspeed) {
            speed = maxspeed;
        }
        else if (speed < minspeed) {
            speed = minspeed;
        }

        // Sail / Forward Thrust
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    public void Steer(float inputRudder)
    {
        rudder += inputRudder * rudderDelta * Time.deltaTime;
        if (rudder > maxRudder) {
            rudder = maxRudder;
        } else if (rudder < -maxRudder) {
            rudder = -maxRudder;
        }

        // Rudder / Steering
        if (speed <= 0.01f)
            heading %= 360;
        else
            heading = (heading + rudder * Time.deltaTime * Mathf.Sqrt(speed)) % 360;
        Vector3 rotation = transform.eulerAngles;
        rotation.y = heading;
        rotation.z = -rudder;
        transform.eulerAngles = rotation;
    }
}
