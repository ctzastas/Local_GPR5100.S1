using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviourPunCallbacks {

   public static GameObject LocalPlayerInstance;
   public GameObject playerNamePrefab;
   //public GameObject speedTextPrefab;
   public Rigidbody carRigidbody;

   private void Awake() {
      if (photonView.IsMine) {
         LocalPlayerInstance = gameObject;
      }
      else {
         GameObject playerName = Instantiate(playerNamePrefab);
         playerName.GetComponent<PlayerName>().target = carRigidbody.gameObject.transform;
         playerName.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;

         /*GameObject playerSpeed = Instantiate(speedTextPrefab);
         playerSpeed.GetComponent<PlayerSpeed>().target = carRigidbody.gameObject.transform;
         float showSpeed = Mathf.Round(carRigidbody.velocity.magnitude * 3.6f);
         playerSpeed.GetComponent<Text>().text = "" + showSpeed + " khm/h";
         Debug.Log(showSpeed);*/
      }
   }
}
