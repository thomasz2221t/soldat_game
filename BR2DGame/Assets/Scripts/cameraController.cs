using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    
    [SerializeField] PhotonView view;
    //private GameObject localPlayer;
    //private Transform target;

    // Start is called before the first frame update
    private void Start() {
        /*GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players) {
            if (PhotonView.Get(player).IsMine) {
                this.target = player.transform;
                Debug.Log("Znalaz³em gracza w camera controller");
                break;
            }
        }*/

        
    }
}
