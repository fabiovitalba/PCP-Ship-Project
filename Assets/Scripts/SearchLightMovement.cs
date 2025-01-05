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
    public float deltaAngle = 2.0f;

    private float startAngle;
    private bool increasingAngle = true;

    // Start is called before the first frame update
    void Start()
    {
        startAngle = GetCurrentAngle();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Light>().enabled) {
            float currAngle = GetCurrentAngle();
            if (Mathf.Abs(currAngle - startAngle) >= maxRotationAngle) {
                increasingAngle = !increasingAngle;
            }
            float newAngle = increasingAngle ? currAngle + deltaAngle : currAngle - deltaAngle;
            if (Mathf.Abs(newAngle) >= maxRotationAngle) {
                newAngle = maxRotationAngle * Mathf.Sign(newAngle);
                increasingAngle = !increasingAngle;
            }
            SetCurrentAngle(newAngle);
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
