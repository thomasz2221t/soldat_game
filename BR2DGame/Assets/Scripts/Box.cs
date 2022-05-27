using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Box : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private GameObject akPrefab;
    [SerializeField] private GameObject pistolPrefab;
    private int dropNumber;
    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            dropItems();
            Die();
        }
    }

    // drop items after the box is destroyed
    private void dropItems() {
        dropNumber = Random.Range(1, 3);
        switch (dropNumber) {
            case 1:
                PhotonNetwork.Instantiate(pistolPrefab.name, this.transform.position, this.transform.rotation);
                break;
            case 2:
                PhotonNetwork.Instantiate(akPrefab.name, this.transform.position, this.transform.rotation);
                break;
        }
    }

    private void Die()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
