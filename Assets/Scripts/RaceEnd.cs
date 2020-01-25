using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEnd : MonoBehaviour {

    public GameObject raceEnd;
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            raceEnd.SetActive(true);
        }
    }
}
