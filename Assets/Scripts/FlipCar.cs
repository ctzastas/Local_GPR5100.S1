using UnityEngine;

public class FlipCar : MonoBehaviour {
    
    private Rigidbody car;
    private float lastTimeChecked;
    private int seconds = 3;
    
    // Start is called before the first frame update
    void Start() {
        car = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (transform.up.y > 0.5f || car.velocity.magnitude > 1) {
            lastTimeChecked = Time.time;
        }

        if (Time.time > lastTimeChecked + seconds) {
            RightCar();
        }
    }

    void RightCar() {
        transform.position += Vector3.up;
        transform.rotation = Quaternion.LookRotation(transform.forward);
    }
}
