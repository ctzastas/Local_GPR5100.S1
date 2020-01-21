using UnityEngine;

public class SpinCars : MonoBehaviour {
    
    [SerializeField] private float spinSpeed = 0.3f;
    
    // Update is called once per frame
    void Update() {
        transform.Rotate(0,spinSpeed,0);
    }
}
