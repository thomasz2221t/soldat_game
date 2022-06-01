using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    //Coordinates within which player could be spawned
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private static GameObject localPlayer = null;

    public static GameObject getLocalPlayerReference() { return localPlayer;  }

    // Start is called before the first frame update
    void Start() {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity); //Spawning each player as different entity

        if (localPlayer == null) {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players) {
                if (PhotonView.Get(player).IsMine) {
                    localPlayer = player;
                    Debug.Log("Znalaz³em gracza w spwanowaniu");
                    break;
                }
            }

        }
    }
}
