using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviourPunCallbacks {

   public static GameObject LocalPlayerInstance;
   public GameObject playerNamePrefab;
   public Rigidbody carRigidbody;
   public Renderer carMesh;

   private void Awake() {
      if (photonView.IsMine) {
         LocalPlayerInstance = gameObject;
      }
      else {
         GameObject playerName = Instantiate(playerNamePrefab);
         playerName.GetComponent<PlayerName>().target = carRigidbody.gameObject.transform;
         playerName.GetComponent<Text>().text = photonView.Owner.NickName;
      }
   }
}
