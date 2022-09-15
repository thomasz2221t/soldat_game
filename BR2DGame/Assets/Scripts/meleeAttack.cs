using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// Klasa meleeAttack realizuj¹ca abstrakt walki wrêcz
/// </summary>
public class meleeAttack : MonoBehaviour
{
    /// <summary>
    /// Referencja do komponentu animatora
    /// </summary>
    [SerializeField] Animator animator;

    /// <summary>
    /// Metoda Start wywo³ywana przed pierwsz¹ aktualizacj¹ klatki. Uruchamia animacjê ataku wrêcz
    /// </summary>
    void Start()
    {
        animator.Play("MeleeAttack");
    }

    /// <summary>
    /// W metodzie Awake, kontrolowanie animacji, uruchomienie korutyny odmierzaj¹cej czas animacji
    /// </summary>
    private void Awake() {
        StartCoroutine("DestroyByTime");
    }

    /// <summary>
    /// Korutyna kontroluj¹ca czas ¿ycia animacji.
    /// </summary>
    /// <returns>obiekt IEnumerator</returns>
    IEnumerator DestroyByTime() {
        yield return new WaitForSeconds(2f);
        this.GetComponent<PhotonView>().RPC("destroyAnimation", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC inicjuj¹ca dekonstrukcje obiektu animacji
    /// </summary>
    [PunRPC]
    public void destroyAnimation() {
        Destroy(this.gameObject);
    }
}
