using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] static private float shotCooldown = 0.1f;
    [SerializeField] private int magazineSize = 30;

    float timeStamp = 0;
    float timeStamp2 = 0;

    PhotonView pv;

    void Start()
    {
        pv = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1")&& pv.IsMine)
        {
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

    //function realizing releasing the bullet from barell
    //[RPC] - obsolete
    [PunRPC]
    void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.transform.position, firePoint.transform.rotation); //Instantiation of a new bullet
    }
}
