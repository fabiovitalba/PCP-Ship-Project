using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBobbing : MonoBehaviour
{
    public float bob = -0.5f;
    public float bobFrequency = 0.18f;

    private float elapsed = 0.0f;
    private float seaLevel = 0.0f;

    // Start is called before the first frame update
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
    }
}
