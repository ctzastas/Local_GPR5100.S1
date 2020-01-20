using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour {

    [SerializeField] private float antiRoll = 5000.0f;
    [SerializeField] private WheelCollider wheelLFront;   
    [SerializeField] private WheelCollider wheelRFront;   
    [SerializeField] private WheelCollider wheelLBack;   
    [SerializeField] private WheelCollider wheelRBack;
    [SerializeField] private GameObject centerOfMass;
    private Rigidbody car;
    
    // Start is called before the first frame update
    void Start() {
        car = GetComponent<Rigidbody>();
        car.centerOfMass = centerOfMass.transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate() {
        GroundWheels(wheelLFront,wheelRFront);
        GroundWheels(wheelLBack,wheelRBack);
    }

    void GroundWheels(WheelCollider wheelLeft, WheelCollider wheelRight) {
        WheelHit hit;
        float travelLeft = 1f;
        float travelRight = 1f;
        bool groundedLeft = wheelLeft.GetGroundHit(out hit);
        if (groundedLeft) {
            travelLeft = (-wheelLeft.transform.InverseTransformPoint(hit.point).y - wheelLeft.radius) / wheelLeft.suspensionDistance;
        }
        bool groundedRight = wheelRight.GetGroundHit(out hit);
        if (groundedRight) {
            travelRight = (-wheelRight.transform.InverseTransformPoint(hit.point).y - wheelRight.radius) / wheelRight.suspensionDistance;
        }

        float antiRollForce = (travelLeft - travelRight) * antiRoll;
        if (groundedLeft) {
            car.AddForceAtPosition(wheelLeft.transform.up * -antiRollForce, wheelLeft.transform.position);
        }

        if (groundedRight) {
            car.AddForceAtPosition(wheelRight.transform.up * -antiRollForce, wheelRight.transform.position);
        }
    }
}
