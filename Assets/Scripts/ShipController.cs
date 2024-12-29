using System;
using Unity.VisualScripting;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float speed = 0.0f;
    public float acceleration = 1.0f;
    public float decceleration = 0.75f;
    public float maxspeed = 2.0f;
    public float minspeed = -0.25f;
    public float heading = 0.0f;
    public float rudder = 0.0f;
    public float rudderDelta = 2.0f;
    public float maxRudder = 6.0f;
    public float bob = 0.1f;
    public float bobFrequency = 0.2f;

    private float elapsed = 0.0f;
    private float seaLevel = 0.0f;

    // Use this for initialization
    void Start()
    {
        seaLevel = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Bobbing
        elapsed += Time.deltaTime;
        Vector3 position = transform.position;
        position.y = seaLevel + bob * Mathf.Sin(elapsed * bobFrequency * (Mathf.PI * 2));
        transform.position = position;

        // Get Inputs
        rudder += Input.GetAxis("Horizontal") * rudderDelta * Time.deltaTime;
        if (rudder > maxRudder) {
            rudder = maxRudder;
        } else if (rudder < -maxRudder) {
            rudder = -maxRudder;
        }
        float verticalInput = Input.GetAxis("Vertical");
        if ((verticalInput <= 0.01f) && (verticalInput >= -0.01f)) {
            if (speed > 0) {
                speed -= decceleration * Time.deltaTime;
            }
            if (speed < 0) {
                speed = 0;
            }
        } else {
            speed += verticalInput * acceleration * Time.deltaTime;
            if (speed > maxspeed) {
                speed = maxspeed;
            }
            else if (speed < minspeed) {
                speed = minspeed;
            }
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

        // Sail / Forward Thrust
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        // Debug-draw all contact points and normals
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        Debug.Log("NEW CONTACT with " + collisionInfo.transform.tag);
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        Debug.Log("STOP CONTACT with " + collisionInfo.transform.tag);
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        Debug.Log("CONTINUE CONTACT with " + collisionInfo.transform.tag);
    }
}
