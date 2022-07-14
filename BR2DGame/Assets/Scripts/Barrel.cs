using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private float splashRange;
    [SerializeField] private float damage = 100;


    public void TakeDamage(float damage) {
        health -= damage;

        if (health <= 0) {
            explode();
            this.GetComponent<PhotonView>().RPC("destroyBarrel", RpcTarget.AllBuffered);
        }
    }

    public void explode() {
        //Debug.Log("inside explode");
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);
        foreach(var hitCollider in hitColliders) {
            Player player = hitCollider.GetComponent<Player>();
            //Debug.Log("inside foreach");
            if (player) {
                //Debug.Log("inside if(player)");
                //var closestPoint = hitCollider.ClosestPoint(transform.position);
                //Debug.Log("closest point: " + closestPoint);
                //var distance = Vector3.Distance(hitCollider.ClosestPoint(transform.position), transform.position);
                //Debug.Log("distance: " + distance);
                //var damagePercent = Mathf.InverseLerp(0, splashRange, distance);
                player.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 100.00f);
                //Debug.Log("all: " + damagePercent * damage * 100);
                //Debug.Log("dmg: " + damage);
                //Debug.Log("dmg prc: " + damagePercent);
                //player.TakeDamage((int)(damagePercent * damage));
            }
        }
    }

    [PunRPC]
    public void destroyBarrel() {
        Destroy(this.gameObject);
    }
}
