using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpeedContoller : MonoBehaviour
{
    public GameObject target;
    //public ValueToTrack valueToTrack;
    private ShipMovement shipMovement;
    public GameObject knot1;
    public GameObject knot2;
    public GameObject knot3;
    public GameObject knot4;
    public GameObject knot5;
    private List<GameObject> knots;

    // Start is called before the first frame update
    void Start()
    {
        shipMovement = target.GetComponent<ShipMovement>();
        knots = new List<GameObject>() {
            knot1, knot2, knot3, knot4, knot5
        };
    }

    // Update is called once per frame
    void Update()
    {
        int knotsToShow = (int)Mathf.Floor(shipMovement.speed / shipMovement.maxSpeed * knots.Count);
        for (int i = 0; i < knots.Count; i++) {
            knots[i].SetActive(i <= knotsToShow);
        }
    }
}
