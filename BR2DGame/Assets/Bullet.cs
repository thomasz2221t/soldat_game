using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bullets collisions and dmg or blowing up- can be particle effect
public class Bullet : MonoBehaviour
{
    [SerializeField] private float DestroyTime = 2f;

    private void Awake()
    {
        StartCoroutine("DestroyByTime");
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);
    }

    //collisions
    void onCollisionEnter2D(Collision2D collision)
    {

    }
}
