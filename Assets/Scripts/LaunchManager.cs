using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class LaunchManager : MonoBehaviourPunCallbacks {
    
    // Photon setup
    private byte maxPlayersPerRoom = 2;
    private bool isConnecting;
    
    // Input Texts
    public InputField playerName;
    public Text feedbackText;
    private string gameVersion = "1";


    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PlayerPrefs.HasKey("PlayerName")) {
            playerName.text = PlayerPrefs.GetString("PlayerName");
        }
    }
    
    // Connect to a network
    public void ConnectNetwork() {
        feedbackText.text = "";
        isConnecting = true;
        PhotonNetwork.NickName = playerName.text;
        if (PhotonNetwork.IsConnected) {
            feedbackText.text += "\nJoining room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else {
            feedbackText.text += "\nConnecting...";
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    public void SetName(string name) {
        PlayerPrefs.SetString("PlayerName", name);
    }

    public void SelectCar() {
        if (PlayerPrefs.HasKey("PlayerName")) {
            playerName.text = PlayerPrefs.GetString("PlayerName");
        }
    }
    
    /* ============ Network Callbacks ============== */
    
    public override void OnConnectedToMaster() {
        if (isConnecting) {
            feedbackText.text += "\nOn Connecting to master...";
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        feedbackText.text += "\nFailed to joined random room...";
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = this.maxPlayersPerRoom});
    }

    public override void OnDisconnected(DisconnectCause cause) {
        feedbackText.text += "\nDisconnected because " + cause;
        isConnecting = false;
    }

    public override void OnJoinedRoom() {
        feedbackText.text += "\nJoined Room with " + PhotonNetwork.CurrentRoom.PlayerCount + " Players";
        PhotonNetwork.LoadLevel("GameScene");
    }
}