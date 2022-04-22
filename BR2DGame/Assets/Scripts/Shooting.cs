using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float bulletForce = 20f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            //GetComponent<PhotonView>().RPC("Shoot", PhotonTargets.All);
            Shoot();
           
        }

    }

    //functaion realizing releasing the bullet from barell
    //[RPC] - obsolete
    void Shoot()
    {
 
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);//spawning bullet
        Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();//accessing rigidbody
        bulletRigidBody.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);//adding force to the bullet, making it fly
    }
}
