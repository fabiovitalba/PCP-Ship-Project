using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRudderController : MonoBehaviour
{
    public GameObject target;
    private ShipMovement shipMovement;

    // Start is called before the first frame update
    void Start()
    {
        shipMovement = target.GetComponent<ShipMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shipMovement != null) {
            var eulerAngles = transform.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, -shipMovement.rudder * 10);
        }
    }
}
