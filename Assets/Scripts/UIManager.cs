using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Random = UnityEngine.Random;

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

    [Header("Prefab")]
    [Space]
    public GameObject[] carPrefabs;
    public ParticleSystem [] finishLine;
    
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
    public static bool isRacing;

    // Start is called before the first frame update
    void Start() {
        finishLine = GetComponents<ParticleSystem>();
        for (int i = 0; i < finishLine.Length; i++) {
            finishLine[i].Play();
        }
        
        isRacing = false;
        // Disable countdown images
        foreach (GameObject images  in countdownImages) {
            images.SetActive(false);
        }
        raceEnd.SetActive(false);
        startRace.SetActive(false);
        waitingText.SetActive(false);
        
        ConnectingPlayers();
    }
    
    // Set countdown timer when the player push StartRace button 
    IEnumerator Countdown(float timer) {
        yield return new WaitForSeconds(timer + 1);
        foreach (GameObject images in countdownImages) {
            images.SetActive(true);
            yield return new WaitForSeconds(timer);
            images.SetActive(false);
        }
        isRacing = true;
    }

    void ConnectingPlayers() {
        playerCar = PlayerPrefs.GetInt("PlayerCar");
        // Set randomly the position of the players
        int randomStartPosition = Random.Range(0, spawnPosition.Length);
        Vector3 startPosition = spawnPosition[randomStartPosition].position;
        Quaternion startRotation = spawnPosition[randomStartPosition].rotation;
        GameObject selectCar = null;
        
        //Set spawn positions of the players 
        if (PhotonNetwork.IsConnected) {
            startPosition = spawnPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1].position;
            startRotation = spawnPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1].rotation;
            
            if (NetworkPlayer.LocalPlayerInstance == null) {
                selectCar = PhotonNetwork.Instantiate(carPrefabs[playerCar].name, startPosition, startRotation, 0);
            }
            // If player is the Master client enable StartRace button
            if (PhotonNetwork.IsMasterClient) {
                startRace.SetActive(true);
                Debug.Log("Is Master");
            }
            else {
                waitingText.SetActive(true);
                startRace.SetActive(false);
            }
        }
        
        // When the player connect to network enable car components 
        selectCar.GetComponent<Drive>().enabled = true;
        selectCar.GetComponent<PlayerController>().enabled = true;
        selectCar.GetComponent<AntiRollBar>().enabled = true;
        selectCar.GetComponent<FlipCar>().enabled = true;
    }

    [PunRPC]
    public void StartGame() {
        StartCoroutine(Countdown(timer));
        startRace.SetActive(false);
        waitingText.SetActive(false);
    }
    
    [PunRPC]
    public void BeginGame() {
        if (PhotonNetwork.IsMasterClient) {
            photonView.RPC("StartGame", RpcTarget.All,null);
        }
    }
    
    [PunRPC]
    public void NewGame() {
        isRacing = false;
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
