using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour {
    
    private GameObject player;
    private Transform followTarget;
    private CinemachineFreeLook camera;
 
    // Use this for initialization
    void Start() {
        camera = GetComponent<CinemachineFreeLook>();
    }
 
    // Update is called once per frame
    void Update() {
        if (player == null) {
            player = GameObject.FindWithTag("Player");
            if (player != null) {
                followTarget = player.transform;
                camera.LookAt = followTarget;
                camera.Follow = followTarget;
            }
        }
    }
}
