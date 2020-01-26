using UnityEngine;

public class RaceEnd : MonoBehaviour {
    
    public GameObject newGameButton;
    public GameObject mainMenuButton;

    private void Start() {
        newGameButton.SetActive(false);
        mainMenuButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        UIManager.isRacing = false;
        if (!UIManager.isRacing) {
            newGameButton.SetActive(true);
            mainMenuButton.SetActive(true);
        }
    }
}
