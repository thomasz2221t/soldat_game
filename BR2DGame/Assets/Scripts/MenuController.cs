using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Klasa MenuController realizująca abstrakt kontrolera początkowej fazy gry, udostępniająca graczowi możliwość konfiguracji
/// parametrów gry multiplayer.
/// </summary>
public class MenuController : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Zmienna przechowująca wersję gry
    /// </summary>
    [SerializeField] private string versionName = "0.0.1";
    /// <summary>
    /// Zmienna przechowująca referencje do obiektu menu użytkownika
    /// </summary>
    [SerializeField] private GameObject usernameMenu;
    /// <summary>
    /// Zmienna przechowująca referencje do obiektu panelu umożliwiający nawiązanie połączenia
    /// </summary>
    [SerializeField] private GameObject connectPanel;
    /// <summary>
    /// Zmienna przechowująca referencje do obiektu ekran sygnalizującego łączenie z serwerem
    /// </summary>
    [SerializeField] private GameObject connectingScreen;
    /// <summary>
    /// Zmienna przechowująca referencje do pola tekstowego umożliwiające nadaniu grze identyfikatora
    /// </summary>
    [SerializeField] private GameObject CreateGameInput;
    /// <summary>
    /// Zmienna przechowująca referencje do obiektu przycisku dołączenia do gry
    /// </summary>
    [SerializeField] private GameObject JoinGameInput;
    /// <summary>
    /// Zmienna przechowująca referencje do obiektu pola tekstowego do nadania graczowi przezwiska
    /// </summary>
    [SerializeField] private GameObject UsernameInput;
    /// <summary>
    /// Zmienna przechowująca referencje do obiektu przycisku start
    /// </summary>
    [SerializeField] private GameObject StartButton;

    /// <summary>
    /// Metoda awake ustawiająca wersję gry oraz inicjująca zadanie ustawień dla połączenia
    /// </summary>
    private void Awake() {
        Debug.Log("Connecting to server");
        PhotonNetwork.GameVersion = versionName;
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Metoda start odkrywające menu użytkownika
    /// </summary>
    private void Start() {
        usernameMenu.SetActive(true);
    }

    /// <summary>
    /// Metoda realizująca dołączenie do poczekalni
    /// </summary>
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected to server");
    }

    /// <summary>
    /// Metoda wywoływana w chwili rozłączenia się z serwerem
    /// </summary>
    /// <param name="cause">Przyczyna rozłączenia się z serwerem</param>
    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log("Disconnected from server: " + cause.ToString());
    }

    /// <summary>
    /// Metoda wprowadzająca nick gracza do programu
    /// </summary>
    public void ChangeUserNameInput() {

        if (UsernameInput.GetComponent<InputField>().text.Length >= 3) {
            StartButton.SetActive(true);
        }
        else {
            StartButton.SetActive(false);
        }
    }

    /// <summary>
    /// Metoda ustawiająca nick gracza w rozgrywce
    /// </summary>
    public void SetUserName() {
        usernameMenu.SetActive(false);
        PhotonNetwork.NickName = UsernameInput.GetComponent<InputField>().text;
        Debug.Log(UsernameInput.GetComponent<InputField>().text);
        connectPanel.SetActive(true);
    }

    /// <summary>
    /// Metoda obsługująca ekran łączenia z serwerem
    /// </summary>
    public override void OnJoinedLobby() {
        PhotonNetwork.NickName = UsernameInput.GetComponent<InputField>().text;
        connectingScreen.SetActive(false);
    }

    /// <summary>
    /// Metoda realizująca tworzenie nowego pokoju
    /// </summary>
    public void CreateRoom() {
        PhotonNetwork.CreateRoom(CreateGameInput.GetComponent<InputField>().text);
    }

    /// <summary>
    /// Metoda realizująca dołączanie do nowego pokoju
    /// </summary>
    public void JoinRoom() {
        PhotonNetwork.JoinRoom(JoinGameInput.GetComponent<InputField>().text);
    }

    /// <summary>
    /// Metoda obsługująca zmianę sceny ze sceny poczekalni na scenę gry
    /// </summary>
    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("Game");
    }
}
