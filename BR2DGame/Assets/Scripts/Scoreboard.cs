using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public GameObject scoreboard;
    public Transform scoreboardPosition;
    public GameObject scoreEntry;

    // Start is called before the first frame update
    void Start()
    {
        scoreboard.SetActive(false);

        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject newScoreEntry = Instantiate(scoreEntry, scoreboardPosition);
            TMP_Text[] texts = newScoreEntry.GetComponentsInChildren<TMP_Text>();
            //Debug.Log(texts.Length);
            Debug.Log(player.CustomProperties["health"]?.ToString());

            //PhotonView view = PhotonView.Find(1);
            //view.

            //GetComponent<PhotonView>().owner.customProperties["property"];

            texts[0].text = "9th";
            texts[1].text = player.NickName;
            texts[2].text = "99";
            texts[3].text = "59:59";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else
        {
            scoreboard.SetActive(false);
        }

        foreach(var player in PhotonNetwork.PlayerList)
            Debug.Log(player?.NickName + " " + player?.CustomProperties["health"]?.ToString());
        
    }
}
