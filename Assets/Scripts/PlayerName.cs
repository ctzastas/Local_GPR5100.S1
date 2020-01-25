using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour {

    public Text name;
    public Text speed;
    public Transform target;
    public Transform speedTarget;
    public Renderer renderer;

    void Awake() {
        name = GetComponent<Text>();
        speed = GetComponent<Text>();
        transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }
    
    // Update is called once per frame
    void LateUpdate() {
        if (renderer == null) {
            return;
        }
        transform.position = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 1f);
        transform.position = Camera.main.WorldToScreenPoint(speedTarget.position + Vector3.up * 2f);
    }
}
