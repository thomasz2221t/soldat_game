using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Klasa MenuController realizuj¹ca abstrakt kontrolera pocz¹tkowej fazy gry, udostêpniaj¹ca graczowi mo¿liwoœæ konfiguracji
/// parametrów gry multiplayer.
/// </summary>
public class MenuController : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Zmienna przechowuj¹ca wersjê gry
    /// </summary>
    [SerializeField] private string versionName = "0.0.1";
    /// <summary>
    /// Zmienna przechowuj¹ca referencje do obiektu menu u¿ytkownika
    /// </summary>
    [SerializeField] private GameObject usernameMenu;
    /// <summary>
    /// Zmienna przechowuj¹ca referencje do obiektu panelu umo¿liwiaj¹cy nawi¹zanie po³¹czenia
    /// </summary>
    [SerializeField] private GameObject connectPanel;
    /// <summary>
    /// Zmienna przechowuj¹ca referencje do obiektu ekran sygnalizuj¹cego ³¹czenie z serwerem
    /// </summary>
    [SerializeField] private GameObject connectingScreen;
    /// <summary>
    /// Zmienna przechowuj¹ca referencje do pola tekstowego umo¿liwiaj¹ce nadaniu grze identyfikatora
    /// </summary>
    [SerializeField] private GameObject CreateGameInput;
    /// <summary>
    /// Zmienna przechowuj¹ca referencje do obiektu przycisku do³¹czenia do gry
    /// </summary>
    [SerializeField] private GameObject JoinGameInput;
    /// <summary>
    /// Zmienna przechowuj¹ca referencje do obiektu pola tekstowego do nadania graczowi przezwiska
    /// </summary>
    [SerializeField] private GameObject UsernameInput;
    /// <summary>
    /// Zmienna przechowuj¹ca referencje do obiektu przycisku start
    /// </summary>
    [SerializeField] private GameObject StartButton;

    /// <summary>
    /// Metoda awake ustawiaj¹ca wersjê gry oraz inicjuj¹ca zadanie ustawieñ dla po³¹czenia
    /// </summary>
    private void Awake() {
        Debug.Log("Connecting to server");
        PhotonNetwork.GameVersion = versionName;
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Metoda start odkrywaj¹ce menu u¿ytkownika
    /// </summary>
    private void Start() {
        usernameMenu.SetActive(true);
    }

    /// <summary>
    /// Metoda realizuj¹ca do³¹czenie do poczekalni
    /// </summary>
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected to server");
    }

    /// <summary>
    /// Metoda wywo³ywana w chwili roz³¹czenia siê z serwerem
    /// </summary>
    /// <param name="cause">Przyczyna roz³¹czenia siê z serwerem</param>
    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log("Disconnected from server: " + cause.ToString());
    }

    /// <summary>
    /// Metoda wprowadzaj¹ca nick gracza do programu
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
    /// Metoda ustawiaj¹ca nick gracza w rozgrywce
    /// </summary>
    public void SetUserName() {
        usernameMenu.SetActive(false);
        PhotonNetwork.NickName = UsernameInput.GetComponent<InputField>().text;
        Debug.Log(UsernameInput.GetComponent<InputField>().text);
        connectPanel.SetActive(true);
    }

    /// <summary>
    /// Metoda obs³uguj¹ca ekran ³¹czenia z serwerem
    /// </summary>
    public override void OnJoinedLobby() {
        PhotonNetwork.NickName = UsernameInput.GetComponent<InputField>().text;
        connectingScreen.SetActive(false);
    }

    /// <summary>
    /// Metoda realizuj¹ca tworzenie nowego pokoju
    /// </summary>
    public void CreateRoom() {
        PhotonNetwork.CreateRoom(CreateGameInput.GetComponent<InputField>().text);
    }

    /// <summary>
    /// Metoda realizuj¹ca do³¹czanie do nowego pokoju
    /// </summary>
    public void JoinRoom() {
        PhotonNetwork.JoinRoom(JoinGameInput.GetComponent<InputField>().text);
    }

    /// <summary>
    /// Metoda obs³uguj¹ca zmianê sceny ze sceny poczekalni na scenê gry
    /// </summary>
    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("Game");
    }
}
