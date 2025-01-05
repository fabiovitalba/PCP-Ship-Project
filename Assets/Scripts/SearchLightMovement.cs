using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis {
    XAxis,
    YAxis,
    ZAxis
}

public class SearchLightMovement : MonoBehaviour
{
    public Axis rotationAxis = Axis.YAxis;
    public float maxRotationAngle = 30;
    public float rotationSpeed = 20.0f;

    private float startAngle;
    private float currAngle;
    private bool increasingAngle = true;

    // Start is called before the first frame update
    void Start()
    {
        startAngle = GetCurrentAngle();
        currAngle = GetCurrentAngle();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: The rotation ends apruptly instead of flowing from one end to the other
        if (GetComponent<Light>().enabled) {
            currAngle = GetCurrentAngle();
            float newAngle = increasingAngle ? currAngle + (rotationSpeed * Time.deltaTime) : currAngle - (rotationSpeed * Time.deltaTime);
            if (Mathf.Abs(newAngle) >= maxRotationAngle) {
                newAngle = maxRotationAngle * Mathf.Sign(newAngle);
                increasingAngle = !increasingAngle;
            }
            SetCurrentAngle(startAngle + newAngle);
        }
    }

    private float GetCurrentAngle() {
        switch(rotationAxis) {
            case Axis.XAxis:
                return transform.eulerAngles.x;
            case Axis.YAxis:
                return transform.eulerAngles.y;
            case Axis.ZAxis:
                return transform.eulerAngles.z;
            default:
                return 0;
        }
    }

    private void SetCurrentAngle(float newAngle) {
        Vector3 rotation = transform.eulerAngles;
        switch (rotationAxis)
        {
            case Axis.XAxis:
                rotation.x = newAngle;
                break;
            case Axis.YAxis:
                rotation.y = newAngle;
                break;
            case Axis.ZAxis:
                rotation.z = newAngle;
                break;
            default:
                break;
        }
        transform.eulerAngles = rotation;
    }
}
