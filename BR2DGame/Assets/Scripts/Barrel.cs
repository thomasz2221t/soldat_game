using Photon.Pun;
using UnityEngine;

/// <summary>
/// Klasa Barrel reprezentuj¹ca obstrakt beczki z materia³em, który po trafieniu eksploduje
/// </summary>
public class Barrel : MonoBehaviour
{
    /// <summary>
    /// Zmienna przechowuj¹ca iloœci punktów ¿ycia beczki
    /// </summary>
    [SerializeField] private float health = 100;
    /// <summary>
    /// Zmienna przechowuj¹ca zakres zadawania obra¿eñ przez eksplozje
    /// </summary>
    [SerializeField] private float splashRange;
    /// <summary>
    /// Zmienna przechowuj¹ca obra¿enia zadawane w wyniku eksplozji beczki
    /// </summary>
    [SerializeField] private float damage = 100;
    /// <summary>
    /// Referencja do obiektu animacji wybuchu beczki
    /// </summary>
    [SerializeField] private GameObject animationPrefab;
    /// <summary>
    /// Zmienna przechowuj¹ca liczbê zachodz¹cych animacji
    /// </summary>
    int animationCounter = 1;

    /// <summary>
    /// Metoda realizuj¹ca logikê otrzymywania przez obiekt beczki obra¿eñ od trafienia pociskiem
    /// </summary>
    /// <param name="damage">Obra¿enia otrzymane w wyniku ataku</param>
    public void TakeDamage(float damage) {
        health -= damage;

        //Obs³uga logiki wybuchu beczki po utraceniu punktów ¿ycia
        if (health <= 0) {
            explode();
            this.GetComponent<PhotonView>().RPC("destroyBarrel", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Metoda realizuj¹ca logikê eksplozji obiektu beczki oraz zadania graczom w wyniku eksplozji obra¿eñ w obszarze wybuchu
    /// </summary>
    public void explode() {
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);
        foreach (var hitCollider in hitColliders) {
            Player player = hitCollider.GetComponent<Player>();
            if (player) {
                player.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 100.00f);
            }
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC wywo³uj¹ca eksplozjê beczki oraz dekontruuj¹ca obiekt beczki
    /// </summary>
    [PunRPC]
    public void destroyBarrel() 
    {
        barrelExplosion();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Metoda obs³ugujaca wywo³ywanie animacji wybuchu obiektu beczki
    /// </summary>
    public void barrelExplosion()
    {
        GameObject explosionAnimation = PhotonNetwork.Instantiate(animationPrefab.name,this.transform.position, this.transform.rotation);
    }
}
