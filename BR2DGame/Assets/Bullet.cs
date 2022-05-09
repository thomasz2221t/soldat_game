using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bullets collisions and dmg or blowing up- can be particle effect
public class Bullet : MonoBehaviour
{
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private int damage = 15;

    private void Awake()
    {
        StartCoroutine("DestroyByTime");
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
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
}
