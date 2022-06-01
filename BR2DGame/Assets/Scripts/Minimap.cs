using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Minimap : MonoBehaviour
{

    private void Start() {

    }

    private void LateUpdate() {
        if (SpawnPlayers.getLocalPlayerReference() != null) {
            Vector3 newPosition = SpawnPlayers.getLocalPlayerReference().transform.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
    }
}
