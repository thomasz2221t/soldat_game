
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{

    public GameObject gameObject;

    public float radius = 0.2f;

    private void Start()
    {
        Vector2 position = getRandomPosition();
        this.transform.position = position;
        this.transform.rotation = randomizeExplosionRotation();
    }

    protected Vector2 getRandomPosition()
    {
        return Random.insideUnitCircle * radius + (Vector2)transform.position;
    }
  
    protected Quaternion randomizeExplosionRotation()
    {
        return Quaternion.Euler(0,0,Random.Range(0,360));
    }

    [PunRPC]
    public void destroyAnimation()
    {
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
