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
        //nice way to get child object by name
        //GameObject firePoint = shooter.transform.Find("FirePoint").gameObject;
        firePoint = shooter.transform.Find("FirePoint");
        Debug.Log(firePoint);
        bulletRigidBody = this.GetComponent<Rigidbody2D>();//accessing rigidbody
        bulletRigidBody.AddForce(this.transform.up * bulletForce, ForceMode2D.Impulse);//adding force to the bullet, making it fly
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

    //collisions
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
