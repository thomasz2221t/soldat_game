using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Box : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private GameObject akPrefab;
    [SerializeField] private GameObject pistolPrefab;
    [SerializeField] private GameObject shotgunPrefab;
    [SerializeField] private GameObject ammoPrefab1;
    [SerializeField] private GameObject ammoPrefab2;
    [SerializeField] private GameObject ammoPrefab3;
    [SerializeField] private GameObject healthpack;
    private int dropNumberWeapon;
    private int dropNumberAmmo;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            dropItems();
            this.GetComponent<PhotonView>().RPC("destroyBox", RpcTarget.AllBuffered);
        }
    }

    //Drop items after the box is destroyed
    private void dropItems() {
        dropNumberWeapon = Random.Range(1, 4);
        switch (dropNumberWeapon) {
            case 1:
                PhotonNetwork.Instantiate(pistolPrefab.name, this.transform.position, this.transform.rotation);
                break;
            case 2:
                PhotonNetwork.Instantiate(akPrefab.name, this.transform.position, this.transform.rotation);
                break;
            case 3:
                PhotonNetwork.Instantiate(shotgunPrefab.name, this.transform.position, this.transform.rotation);
                break;
        }

        Vector3 newPos = new Vector3(this.transform.position.x + 2, this.transform.position.y + 2, this.transform.position.z + 2 );
        dropNumberAmmo = Random.Range(1, 6);
        switch (dropNumberAmmo) {
            case 1:
                PhotonNetwork.Instantiate(ammoPrefab1.name, newPos, this.transform.rotation);
                break;
            case 2:
                PhotonNetwork.Instantiate(ammoPrefab2.name, newPos, this.transform.rotation);
                break;
            case 3:
                PhotonNetwork.Instantiate(ammoPrefab3.name, newPos, this.transform.rotation);
                break;
            case 4:
                PhotonNetwork.Instantiate(healthpack.name, newPos, this.transform.rotation);
                break;
        }
    }

    [PunRPC]
    public void destroyBox() {
        Destroy(this.gameObject);
    }
}
