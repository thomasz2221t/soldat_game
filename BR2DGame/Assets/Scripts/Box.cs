using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Box : MonoBehaviour
{
    [SerializeField] private int health = 100;
   // [SerializeField] private GameObject boxPrefab;

    //private void Start()
    //{
    //    PhotonNetwork.Instantiate(boxPrefab.name, boxPrefab.transform.position, boxPrefab.transform.rotation); //<-should work
    //}

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
