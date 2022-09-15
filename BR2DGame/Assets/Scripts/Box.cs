using UnityEngine;
using Photon.Pun;

/// <summary>
/// Klasa Box realizuj¹ca abstrakt skrzynki
/// </summary>
public class Box : MonoBehaviour
{
    /// <summary>
    /// Zmienna przechowuj¹ca punkty ¿ycia skrzynki
    /// </summary>
    [SerializeField] private float health = 100;
    /// <summary>
    /// Referencja do obiektu karabinu szturmowego
    /// </summary>
    [SerializeField] private GameObject akPrefab;
    /// <summary>
    /// Referencja do obiektu pistoletu
    /// </summary>
    [SerializeField] private GameObject pistolPrefab;
    /// <summary>
    /// Referencja do obiektu strzelby
    /// </summary>
    [SerializeField] private GameObject shotgunPrefab;
    /// <summary>
    /// Prefaby amunicji
    /// </summary>
    [SerializeField] private GameObject ammoPrefab1;
    [SerializeField] private GameObject ammoPrefab2;
    [SerializeField] private GameObject ammoPrefab3;
    /// <summary>
    /// Referencja do obiektu pakietu ¿ycia
    /// </summary>
    [SerializeField] private GameObject healthpack;
    private int dropNumberWeapon;
    private int dropNumberAmmo;

    /// <summary>
    /// Metoda realizuj¹ca logikê otrzymywania przez obiekt skrzynki obra¿eñ od trafienia pociskiem
    /// </summary>
    /// <param name="damage">Obra¿enia otrzymane w wyniku ataku</param>
    public void TakeDamage(float damage)
    {
        health -= damage;

        //Obs³uga rozpadniêcia siê skrzynki po utraceniu punktów ¿ycia
        if (health <= 0)
        {
            dropItems();
            this.GetComponent<PhotonView>().RPC("destroyBox", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Metoda obs³uguj¹ca wypadniêcie broni i/lub amunicji i/lub apteczki po zniszczeniu skrzyni
    /// </summary>
    private void dropItems() {
        //Wylosowanie broni
        dropNumberWeapon = Random.Range(1, 4);
        switch (dropNumberWeapon) {
            case 1:
                PhotonNetwork.Instantiate(pistolPrefab.name, this.transform.position, this.transform.rotation);
                break;
            case 2:
                PhotonNetwork.Instantiate(akPrefab.name, this.transform.position, this.transform.rotation);
                break;
            case 3:
                PhotonNetwork.Instantiate(shotgunPrefab.name, this.transform.position, this.transform.rotation);
                break;
        }

        Vector3 newPos = new Vector3(this.transform.position.x + 2, this.transform.position.y + 2, this.transform.position.z + 2 );
        //Wylosowanie amunicji/ apteczki
        dropNumberAmmo = Random.Range(1, 6);
        switch (dropNumberAmmo) {
            case 1:
                PhotonNetwork.Instantiate(ammoPrefab1.name, newPos, this.transform.rotation);
                break;
            case 2:
                PhotonNetwork.Instantiate(ammoPrefab2.name, newPos, this.transform.rotation);
                break;
            case 3:
                PhotonNetwork.Instantiate(ammoPrefab3.name, newPos, this.transform.rotation);
                break;
            case 4:
                PhotonNetwork.Instantiate(healthpack.name, newPos, this.transform.rotation);
                break;
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC realizuj¹ca zniszczenie obiektu beczki
    /// </summary>
    [PunRPC]
    public void destroyBox() {
        Destroy(this.gameObject);
    }
}
