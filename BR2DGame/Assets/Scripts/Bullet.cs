using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bullets collisions and dmg or blowing up- can be particle effect
public class Bullet : MonoBehaviourPun
{
    [SerializeField] private GameObject shooter;
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private float damage = 15;
    [SerializeField] private float bulletForce = 50f;
    [SerializeField] PhotonView pv;

    private Rigidbody2D bulletRigidBody;

    private Transform firePoint;


    private void Start()
    {
        pv = GetComponent<PhotonView>();
        bulletRigidBody = this.GetComponent<Rigidbody2D>(); 
        bulletRigidBody.AddForce(this.transform.up * bulletForce, ForceMode2D.Impulse); //Adding force to the bullet, making it move
    }

    private void Awake()
    {
        StartCoroutine("DestroyByTime");
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
    }

    //Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool hit = false;
        Box destroyable = collision.GetComponent<Box>();
        Barrel barrel = collision.GetComponent<Barrel>();
        Player playerBody = collision.GetComponent<Player>();

        if (destroyable != null)
        {
            destroyable.TakeDamage(damage);
            hit = true;
        }

        if(barrel != null) {
            barrel.TakeDamage(damage);
            hit = true;
        }

        if ((playerBody != null)&&(!collision.gameObject.GetPhotonView().IsMine))
        {
            playerBody.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damage);
            hit = true;
        }

        if (hit)
        {
            StopCoroutine("DestroyByTime");
            this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
        }
        

    }

    [PunRPC]
    public void destroyBullet()
    {
        Destroy(this.gameObject);
    }
}
