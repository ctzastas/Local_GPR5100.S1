using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class UIManager : MonoBehaviourPunCallbacks {

    [Header("Countdown")]
    [Space]
    [SerializeField] private GameObject[] countdownImages;
    
    [Header("Display Images Timer")]
    [Space]
    [SerializeField] private float timer = 1f;
    
    [Header("Panels")]
    [Space]
    public GameObject raceEnd;
    //public GameObject HUD;
    
    [Header("Prefab")]
    [Space]
    public GameObject[] carPrefabs;
    
    [Header("Spawn")]
    [Space]
    public Transform[] spawnPosition;
    
    [Space]
    private int playerCar;
    
    [Header("Button")]
    [Space]
    public GameObject startRace;
    
    [Header("Text")]
    [Space]
    public GameObject waitingText;
    
    [Space]
    public static bool racing;
    public static int totalLaps = 1;

    // Start is called before the first frame update
    void Start() {
        racing = false;
        foreach (GameObject images  in countdownImages) {
            images.SetActive(false);
        }

        raceEnd.SetActive(false);
        startRace.SetActive(true);
        waitingText.SetActive(false);
        
        playerCar = PlayerPrefs.GetInt("PlayerCar");
        int randomStartPosition = Random.Range(0, spawnPosition.Length);
        Vector3 startPosition = spawnPosition[randomStartPosition].position;
        Quaternion startRotation = spawnPosition[randomStartPosition].rotation;
        GameObject selectCar = null;
        
        // for multi player
        if (PhotonNetwork.IsConnected) {
            startPosition = spawnPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1].position;
            startRotation = spawnPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1].rotation;
            if (NetworkPlayer.LocalPlayerInstance == null) {
                selectCar = PhotonNetwork.Instantiate(carPrefabs[playerCar].name, startPosition, startRotation, 0);
            }
            if (PhotonNetwork.IsMasterClient) {
                startRace.SetActive(true);
                Debug.Log("Is Master");
            }
            else {
                waitingText.SetActive(true);
            }
        }

        
        selectCar.GetComponent<Drive>().enabled = true;
        selectCar.GetComponent<PlayerController>().enabled = true;
        selectCar.GetComponent<AntiRollBar>().enabled = true;
        selectCar.GetComponent<FlipCar>().enabled = true;
    }
    
    void LateUpdate() {
        if (!racing) {
            return;
        }
    }

    IEnumerator Countdown(float timer) {
        yield return new WaitForSeconds(timer + 1);
        foreach (GameObject images in countdownImages) {
            images.SetActive(true);
            yield return new WaitForSeconds(timer);
            images.SetActive(false);
        }
        racing = true;
    }
    
    [PunRPC]
    public void StartGame() {
        StartCoroutine(Countdown(timer));
        startRace.SetActive(false);
        waitingText.SetActive(false);
    }
    
    public void BeginGame() {
        if (PhotonNetwork.IsMasterClient) {
            photonView.RPC("StartGame", RpcTarget.All,null);
        }
    }
    
    public void NewGame() {
        racing = false;
        if (PhotonNetwork.IsConnected) {
            photonView.RPC("RestartGame", RpcTarget.All, null);
        }
        else {
            SceneManager.LoadScene("MainMenu");   
        }
    }
    
    [PunRPC]
    public void RestartGame() {
        PhotonNetwork.LoadLevel("GameScene");
    }
    
    [PunRPC]
    public void MainMenu() {
        PhotonNetwork.LoadLevel("MainMenu");
    }
    
    [PunRPC]
    public void DisconnectGame() {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("MainMenu");
    }
}
