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
        currAngle = startAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Light>().enabled) {
            currAngle = GetCurrentAngle();
            float newAngle = increasingAngle ? currAngle + (rotationSpeed * Time.deltaTime) : currAngle - (rotationSpeed * Time.deltaTime);
            if (Mathf.Abs(newAngle - startAngle) >= maxRotationAngle) {
                increasingAngle = !increasingAngle;
            }
            SetCurrentAngle(newAngle);
        }
    }

    private float GetCurrentAngle() {
        switch(rotationAxis) {
            case Axis.XAxis:
                return transform.localEulerAngles.x;
            case Axis.YAxis:
                return transform.localEulerAngles.y;
            case Axis.ZAxis:
                return transform.localEulerAngles.z;
            default:
                return 0;
        }
    }

    private void SetCurrentAngle(float newAngle) {
        Vector3 rotation = transform.localEulerAngles;
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
        transform.localEulerAngles = rotation;
    }
}
