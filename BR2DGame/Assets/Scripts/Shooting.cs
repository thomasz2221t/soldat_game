using Photon.Pun;
using UnityEngine;

/// <summary>
/// Klasa Shooting reprezentuj¹ca abstrakt kontrolera oddawania strza³u
/// </summary>
public class Shooting : MonoBehaviour
{
    /// <summary>
    /// Zmienna przechowuj¹ca pozycjê oraz rotacjê lufy broni
    /// </summary>
    [SerializeField] private Transform firePoint;
    /// <summary>
    /// Zmienna przechowuj¹ca prefab pocisku
    /// </summary>
    [SerializeField] private GameObject bulletPrefab;
    /// <summary>
    /// Zmienna kontroluj¹ca czas opóŸnienia po strzale
    /// </summary>
    [SerializeField] static private float shotCooldown = 0.1f;
    /// <summary>
    /// Zmienna okreœlaj¹ca rozmiar magazynka karabinu szturmowego
    /// </summary>
    [SerializeField] private int magazineSize = 30;

    /// <summary>
    /// Referencja do obiektu karabinu szturmowego
    /// </summary>
    [SerializeField] private GameObject ak;
    /// <summary>
    /// Referencja do obiektu pistoletu
    /// </summary>
    [SerializeField] private GameObject pistol;

    float timeStamp = 0;
    float timeStamp2 = 0;

    PhotonView pv;

    /// <summary>
    /// Metoda Start wywo³ywana przed pierwsz¹ aktualizacj¹ klatki. 
    /// </summary>
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
    }


    /// <summary>
    /// Metoda Update wywo³ywana po ka¿dej klatce, kontroluje wp³yw logiki gry na stan obiektu
    /// </summary>
    void Update()
    {
        if (ak.activeInHierarchy && Input.GetButton("Fire1")&& pv.IsMine)
        {
            if((timeStamp <= Time.time)&&(magazineSize>0))
            {
                Shoot();
                timeStamp = Time.time + shotCooldown;
                magazineSize--;
            }
        }
        else if(pistol.activeInHierarchy && Input.GetButtonDown("Fire1") && pv.IsMine)
        {
            Shoot();
        }

    }

    //function realizing releasing the bullet from barell
    /// <summary>
    /// Metoda synchronizowana PunRPC obs³uguj¹ca logikê oddania strza³u z broni
    /// </summary>
    [PunRPC]
    void Shoot()
    {
        //Utworzenie instancji pocisku
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.transform.position, firePoint.transform.rotation);

    }
}
