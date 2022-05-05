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
    [SerializeField] static private float shotCooldown = 0.1f;
    [SerializeField] private int magazineSize = 30;

    float timeStamp = 0;

    float timeStamp2 = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            //GetComponent<PhotonView>().RPC("Shoot", PhotonTargets.All);
            if((timeStamp <= Time.time)&&(magazineSize>0))
            {
                Shoot();
                timeStamp = Time.time + shotCooldown;
                magazineSize--;
            }

            //[DELETE] AutoReload cooldown not working anyway
            if(magazineSize <= 0)
            {
                if(timeStamp2 <= Time.time)
                {
                    magazineSize = 30;
                    timeStamp2 = Time.time + 3f;
                }
            }
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
