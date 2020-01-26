using UnityEngine;

public class PlayerSpeed : MonoBehaviour {
    
    public Transform target;
    public Renderer renderer;

    void Awake() {
        transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }
    
    // Update is called once per frame
    void LateUpdate() {
        if (renderer == null) {
            return;
        }
        transform.position = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 100f);
    }
}
