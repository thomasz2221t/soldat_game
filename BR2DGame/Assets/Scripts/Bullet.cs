using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// Klasa Bullet realizuj¹ca abstrakt standardowego pocisku
/// </summary>
public class Bullet : MonoBehaviourPun
{
    /// <summary>
    /// Zmienna przechowuj¹ca referencjê do obiektu strzelaj¹cego
    /// </summary>
    [SerializeField] private GameObject shooter;
    /// <summary>
    /// Zmienna przechowuj¹ca czas ¿ycia pocisku
    /// </summary>
    [SerializeField] private float destroyTime;
    /// <summary>
    /// Zmienna przechowuj¹ca obra¿enia zadawane poprzez trafienie pociskiem
    /// </summary>
    [SerializeField] private float damage;
    /// <summary>
    /// Zmienna przechowuj¹ca si³ê nadawan¹ pociskowi
    /// </summary>
    [SerializeField] private float bulletForce;
    [SerializeField] PhotonView pv;

    private Rigidbody2D bulletRigidBody;

    /// <summary>
    /// Zmienna przechowuj¹ca po³o¿enie oraz rotacjê lufy broni, z której pocisk jest wystrzeliwany
    /// </summary>
    private Transform firePoint;

    /// <summary>
    /// Metoda Start wywo³ywana przed pierwsz¹ aktualizacj¹ klatki. 
    /// Przechowuje wywo³ania metod oraz inicjalizacje zmiennych.
    /// </summary>
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        bulletRigidBody = this.GetComponent<Rigidbody2D>();
        //Dodanie si³y do RigidBody pocisku w celu nadania ruchu
        bulletRigidBody.AddForce(this.transform.up * bulletForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// W metodzie Awake, kontrolowanie fizyki pocisku - uruchomienie korutyny odmierzaj¹cej czas ¿ycia pocisku
    /// </summary>
    private void Awake()
    {
        StartCoroutine("DestroyByTime");
    }

    /// <summary>
    /// Korutyna odmierzaj¹ca czas ¿ycia pocisku. Po up³yniêciu czasu ¿ycia obiekt pocisku jest dekontruuowany
    /// </summary>
    /// <returns>obiekt IEnumerator</returns>
    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Metoda realizuj¹ca logikê kolizji pocisku z obiektami na mapie
    /// </summary>
    /// <param name="collision">Kolizjator obiektu z którym zachodzi kolizja</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        bool hit = false;
        Box destroyable = collision.GetComponent<Box>();
        Barrel barrel = collision.GetComponent<Barrel>();
        Player playerBody = collision.GetComponent<Player>();
        Wall wall = collision.GetComponent<Wall>();

        //logika po trafieniu w skrzynkê
        if (destroyable != null)
        {
            destroyable.TakeDamage(damage);
            hit = true;
        }

        //logika po trafieniu beczki
        if (barrel != null) {
            barrel.TakeDamage(damage);
            hit = true;
        }

        //logika po trafieniu w obiekt gracza
        if ((playerBody != null)&&(!collision.gameObject.GetPhotonView().IsMine))
        {
            playerBody.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damage);
            hit = true;
        }

        //logika po trafieniu pocisku
        if (hit)
        {
            StopCoroutine("DestroyByTime");
            this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Metoda realizuj¹ca logikê gry po wejœciu w kolizjê z obiektem œciany
    /// </summary>
    /// <param name="collision">Kolizjator obiektu z którym zachodzi kolizja</param>
    private void OnCollisionEnter2D(Collision2D collision) {
        Wall wall = collision.gameObject.GetComponent<Wall>();
        if (collision.gameObject.tag == "wall") {
            //dekonstrukcja pocisku
            StopCoroutine("DestroyByTime");
            this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC odpowiedzialna za destrukcjê obiektu pocisku
    /// </summary>
    [PunRPC]
    public void destroyBullet()
    {
        Destroy(this.gameObject);
    }
}
