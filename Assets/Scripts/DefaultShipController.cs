using System;
using Unity.VisualScripting;
using UnityEngine;

public class DefaultShipController : MonoBehaviour
{
    private ShipMovement shipMovement;
    private WaterBoilerShipController waterBoilerShipController;

    // Use this for initialization
    void Start()
    {
        shipMovement = GetComponent<ShipMovement>();
        waterBoilerShipController = GetComponent<WaterBoilerShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!waterBoilerShipController.waterBoilerConnected) {
            // Get default Inputs and call the Ship's methods
            shipMovement.Steer(Input.GetAxis("Horizontal"));
            shipMovement.Accelerate(Input.GetAxis("Vertical"));
        }
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
