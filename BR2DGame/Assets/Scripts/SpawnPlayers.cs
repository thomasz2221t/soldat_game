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
        localPlayer = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity); //Spawning each player as different entity
        
        //NIE USUWAÆ
        //SetLayerRecursively(localPlayer, LayerMask.NameToLayer("ObjectsToHide"));

        /*if (localPlayer == null) {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players) {
                if (!PhotonView.Get(player).IsMine)
                {
                    SetLayerRecursively(player, LayerMask.NameToLayer("ObjectsToHide"));
                }*/

                //if (PhotonView.Get(player).IsMine)
                //{
                //localPlayer = player;
                //Debug.Log("Znalaz³em gracza w spawanowaniu");
                //break;
                //}
                //else 
                //{
                //player.layer = LayerMask.NameToLayer("ObjectsToHide");
                //}
            //}

        //}
    }

    /*void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }*/

}
