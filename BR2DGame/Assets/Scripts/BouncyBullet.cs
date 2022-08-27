using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBullet : MonoBehaviour
{
    [SerializeField] private GameObject shooter;
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private float damage = 15;
    [SerializeField] private float bulletForce = 50f;
    [SerializeField] PhotonView pv;

    private Rigidbody2D bulletRigidBody;

    private Transform firePoint;

    private Vector3 lastVelocity;


    private void Start() {
        pv = GetComponent<PhotonView>();
        bulletRigidBody = this.GetComponent<Rigidbody2D>();
        bulletRigidBody.AddForce(this.transform.up * bulletForce, ForceMode2D.Impulse); //Adding force to the bullet, making it move
    }

    private void Awake() {
        StartCoroutine("DestroyByTime");
    }

    private void Update() {
        lastVelocity = bulletRigidBody.velocity;
    }

    IEnumerator DestroyByTime() {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
    }

    //Collisions
    private void OnTriggerEnter2D(Collider2D collision) {
        bool hit = false;
        Box destroyable = collision.GetComponent<Box>();
        Barrel barrel = collision.GetComponent<Barrel>();
        Player playerBody = collision.GetComponent<Player>();
        Wall wall = collision.GetComponent<Wall>();

        if (destroyable != null) {
            destroyable.TakeDamage(damage);
            hit = true;
        }

        if (barrel != null) {
            barrel.TakeDamage(damage);
            hit = true;
        }

        if ((playerBody != null) && (!collision.gameObject.GetPhotonView().IsMine)) {
            playerBody.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damage);
            hit = true;
        }

        /*if (wall != null) {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.GetContacts())

            hit = true;
        }*/

        if (hit) {
            StopCoroutine("DestroyByTime");
            StartCoroutine("DestroyByTime");
            this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Wall wall = collision.gameObject.GetComponent<Wall>();
        if (collision.gameObject.tag == "wall") {
        //if (wall != null) { 
            Debug.Log("wall2 wall22 wall222");
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            bulletRigidBody.velocity = direction * Mathf.Max(speed, 0f);

            /*StopCoroutine("DestroyByTime");
            StartCoroutine("DestroyByTime");
            this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);*/
        }
    }

    [PunRPC]
    public void destroyBullet() {
        Destroy(this.gameObject);
    }
}
