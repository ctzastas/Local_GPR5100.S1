﻿using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private Drive drive;
    
    // Start is called before the first frame update
    protected void Start() {
        drive = GetComponent<Drive>();
    }
    
    // Update is called once per frame
    void Update() {
        if (!UIManager.isRacing) {
            drive.MoveCar(0, 0, 1);
            return;
        }
        
        float moveY = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        float brake = Input.GetAxis("Jump");
        
        drive.MoveCar(moveY, moveX, brake);
        drive.Skidding();
        drive.EngineSound();
        drive.SpeedInKHM();
    }
}
