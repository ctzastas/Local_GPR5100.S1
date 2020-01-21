using UnityEngine;

public class SelectCar : MonoBehaviour {

    public GameObject[] cars;
    private int currentCar = 0;
    
    // Start is called before the first frame update
    void Start() {
        if (PlayerPrefs.HasKey("PlayerCar")) {
            currentCar = PlayerPrefs.GetInt("PlayerCar");
        }
        transform.LookAt(cars[currentCar].transform.position);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            currentCar++;
            if (currentCar > cars.Length - 1) {
                currentCar = 0;
            }
            PlayerPrefs.SetInt("PlayerCar", currentCar);
        }
        Quaternion lookDirection = Quaternion.LookRotation(cars[currentCar].transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, Time.deltaTime);
    }
}
