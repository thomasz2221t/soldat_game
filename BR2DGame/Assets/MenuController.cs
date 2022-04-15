using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviourPunCallbacks
{

    [SerializeField] private string versionName = "0.0.1";
    [SerializeField] private GameObject usernameMenu;
    [SerializeField] private GameObject connectPanel;

    [SerializeField] private GameObject CreateGameInput;
    [SerializeField] private GameObject JoinGameInput;
    [SerializeField] private GameObject UsernameInput;

    [SerializeField] private GameObject StartButton;

    private void Awake() {
        Debug.Log("Connecting to server");
        PhotonNetwork.GameVersion = versionName;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start() {
        usernameMenu.SetActive(true);
    }

    /// <summary>
    ///  
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log("Disconnected from server because: " + cause.ToString());
    }

    public void ChangeUserNameInput() {

        //Debug.Log(UsernameInput.GetComponent<Text>().text.Length);
        if (UsernameInput.GetComponent<InputField>().text.Length >= 3) {
            StartButton.SetActive(true);
        }
        else {
            StartButton.SetActive(false);
        }
    }

    public void SetUserName() {
        usernameMenu.SetActive(false);
        PhotonNetwork.NickName = UsernameInput.GetComponent<Text>().text;
    }
}
