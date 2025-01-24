using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ValueToTrack {
    ActualRudder,
    InputRudder
}

public class UIRudderController : MonoBehaviour
{
    public GameObject target;
    public ValueToTrack valueToTrack;
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
            switch(valueToTrack) {
                case ValueToTrack.ActualRudder:
                    transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, -shipMovement.rudder * 10);
                    break;
                case ValueToTrack.InputRudder:
                    transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, -shipMovement.GetInputRudder() * 10);
                    break;
            }
        }
    }
}
