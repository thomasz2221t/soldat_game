using Photon.Pun;
using UnityEngine;

/// <summary>
/// Klasa ExplosionAnimation realizuj¹ca abstrakt animacji wybuchu
/// </summary>
public class ExplosionAnimation : MonoBehaviour
{
    /// <summary>
    /// Zmienna przechowuj¹ca referencjê do obiektu animacji 
    /// </summary>
    public GameObject animation;
    /// <summary>
    /// Zmienna przechowuj¹ca promieñ animacji
    /// </summary>
    public float radius = 0.2f;

    /// <summary>
    /// Metoda Start wywo³ywana przed pierwsz¹ aktualizacj¹ klatki. 
    /// Przechowuje wywo³ania metod oraz inicjalizacje zmiennych.
    /// </summary>
    private void Start()
    {
        //pobranie pozycji startu animacji
        Vector2 position = getRandomPosition();
        this.transform.position = position;
        this.transform.rotation = randomizeExplosionRotation();
    }

    /// <summary>
    /// Metoda realizuj¹ca losowanie pozycji wywo³ania animacji, dziêki czemu efekt chaotyczny jest mniej przewidywalny i bardziej naturalny
    /// </summary>
    /// <returns>Zwraca wektor z wylosowan¹ pozycj¹ animacji</returns>
    protected Vector2 getRandomPosition()
    {
        return Random.insideUnitCircle * radius + (Vector2)transform.position;
    }

    /// <summary>
    /// Metoda realizuj¹ca losowanie rotacji wywo³anej animacji, dziêki czemu efekt chaotyczny jest mniej przewidywalny i bardziej naturalny
    /// </summary>
    /// <returns>Zwraca kwaternion z wylosowan¹ rotacj¹ animacji</returns>
    protected Quaternion randomizeExplosionRotation()
    {
        return Quaternion.Euler(0,0,Random.Range(0,360));
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC realizuj¹ca dekontrukcje animacji
    /// </summary>
    [PunRPC]
    public void destroyAnimation()
    {
        Destroy(this.animation);
    }

    /// <summary>
    /// Metoda rysuj¹ca sferê 
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
