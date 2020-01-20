using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour {

    public Text name;
    public Transform target;

    void Awake() {
        name = GetComponent<Text>();
        transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }
    
    // Start is called before the first frame update
    private void Start() {
       
    }

    // Update is called once per frame
    void LateUpdate() {
        transform.position = Camera.main.WorldToScreenPoint(target.position);
    }
}
