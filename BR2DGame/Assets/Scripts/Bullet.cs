using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bullets collisions and dmg or blowing up- can be particle effect
public class Bullet : MonoBehaviourPun
{
    [SerializeField] private GameObject shooter;
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float bulletForce = 20f;

    private Rigidbody2D bulletRigidBody;

    private Transform firePoint;


    private void Start()
    {
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
        Box destroyable = collision.GetComponent<Box>();

        if (destroyable != null)
        {
            destroyable.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    [PunRPC]
    public void destroyBullet()
    {
        Destroy(this.gameObject);
    }
}
