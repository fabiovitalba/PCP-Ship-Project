using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [Header("Acceleration")]
    public float speed = 0.0f;
    public float acceleration = 1.0f;
    public float decceleration = 0.75f;
    public float maxSpeed = 7.0f;
    public float minSpeed = 0.0f;

    [Header("Steering")]
    public float heading = 0.0f; // Where is the Ship heading to
    public float rudder = 0.0f; // What is the current steering input
    public float rudderDelta = 2.0f;
    public float maxRudder = 6.0f;
    private float inputRudderUI = 0.0f; // used in UI

    [Header("Land Collision")]
    public int maxLandContactMillis = 1500;
    // This variable stores the last Stand Checkpoint in order to return to the Stand if the player ever touches land for too long.
    public Vector3 lastStandPosition;
    public Vector3 lastStandEulerAngles;
    public Quaternion lastStandRotation;
    public DateTime lastLandContact = new DateTime();
    public double millisSinceContactStart = 0;
    
    [Header("Others")]
    public MessageDisplay messageDisplay;

    private Rigidbody shipRigidbody = null;

    // Start is called before the first frame update
    void Start()
    {
        shipRigidbody = GetComponent<Rigidbody>();
        lastStandPosition = transform.position;
        lastStandRotation = transform.rotation;
        lastStandEulerAngles = transform.eulerAngles;
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
            if (speed > maxSpeed) {
                speed = maxSpeed;
            }
            else if (speed < minSpeed) {
                speed = minSpeed;
            }
        }

        if (speed > maxSpeed) {
            speed = maxSpeed;
        }
        else if (speed < minSpeed) {
            speed = minSpeed;
        }

        // Sail / Forward Thrust
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    public void Steer(float inputRudder)
    {
        inputRudderUI = inputRudder;
        rudder += inputRudder * rudderDelta * Time.deltaTime;
        UpdateHeading(rudder);
    }

    public void SetRudder(float newRudder)
    {
        inputRudderUI = newRudder;
        rudder = newRudder;
        UpdateHeading(rudder);
    }

    private void UpdateHeading(float rudder)
    {
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

    public void ResetAcceleration() {
        speed = 0.0f;
    }

    public void ResetSteering() {
        rudder = 0.0f;
        heading = 0.0f;
    }

    public void SetLastStandPosition(Vector3 newPosition, Vector3 newAngles, Quaternion newRotation) {
        newPosition.y = lastStandPosition.y; // We want to maintain the same water-level, independently of the checkpoint height.
        lastStandPosition = newPosition;
        lastStandEulerAngles = newAngles;
        lastStandRotation = newRotation;
    }

    public void ResetToLastStandPosition() {
        ResetAcceleration();
        ResetSteering();
        shipRigidbody.velocity = Vector3.zero;
        shipRigidbody.angularVelocity = Vector3.zero;
        transform.position = lastStandPosition;
        transform.rotation = lastStandRotation;
        transform.eulerAngles = lastStandEulerAngles;
        heading = transform.eulerAngles.y;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        // Debug-draw all contact points and normals
        // foreach (ContactPoint contact in collisionInfo.contacts) {
        //     Debug.DrawRay(contact.point, contact.normal, Color.white);
        // }
        if (collisionInfo.transform.CompareTag("Land")) {
            lastLandContact = DateTime.Now;
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        lastLandContact = new DateTime();
        millisSinceContactStart = 0;
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        // Debug-draw all contact points and normals
        // foreach (ContactPoint contact in collisionInfo.contacts) {
        //     Debug.DrawRay(contact.point, contact.normal, Color.white);
        // }
        if (collisionInfo.transform.CompareTag("Land")) {
            TimeSpan millisPassedSinceContact = DateTime.Now - lastLandContact;
            millisSinceContactStart = millisPassedSinceContact.TotalMilliseconds;
            if (millisSinceContactStart >= maxLandContactMillis) {
                ResetToLastStandPosition();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShipStand"))
        {
            Debug.Log("Port updated!");
            messageDisplay.ShowMessage("Port updated!");
            Transform checkpoint = FindChildWithTag(other.transform,"ShipStandCheckpoint");
            if (checkpoint != null) {
                Vector3 newStandPosition = checkpoint.position;
                Vector3 newStandEulerAngles = checkpoint.eulerAngles;
                Quaternion newStandRotation = checkpoint.rotation;
                SetLastStandPosition(newStandPosition, newStandEulerAngles, newStandRotation);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("ShipStand"))
        // {
        //     Debug.Log("Player left the Ship Stand area!");
        // }
    }

    public float GetInputRudder()
    {
        return inputRudderUI * rudderDelta;
    }

    private Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
        }

        return null; // Return null if no child with the tag is found
    }
}
