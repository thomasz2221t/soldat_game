using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Minimap : MonoBehaviour
{

    //private GameObject localPlayer;

    private void Start() {
        //this.transform.position = new Vector3(0,0,0);


        /*GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players) {
            if (PhotonView.Get(player).IsMine) {
                this.localPlayer = player;
                Debug.Log("Znalaz³em gracza w minimap script");
                break;
            }
        }

        if (localPlayer == null) Debug.Log("DUPAAAAAAAAAAAA");*/
    }

    private void LateUpdate() {
        if (SpawnPlayers.getLocalPlayerReference() != null) {
            Vector3 newPosition = SpawnPlayers.getLocalPlayerReference().transform.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;

            //transform.rotation = Quaternion.Euler(0f, 0f, SpawnPlayers.getLocalPlayerReference().transform.eulerAngles.y);
            //transform.rotation = Quaternion.Euler(0f, SpawnPlayers.getLocalPlayerReference().transform.eulerAngles.y, 0f);
        }

        /*if (localPlayer != null) {

        Vector3 newPosition = localPlayer.transform.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;

        //transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Euler(0f, localPlayer.transform.eulerAngles.y, 0f);
    }*/
    }

}
