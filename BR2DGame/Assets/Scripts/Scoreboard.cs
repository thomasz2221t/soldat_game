using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Scoreboard : MonoBehaviour
{
    public GameObject scoreboard;
    public GameObject scoreEntryGameObject;

    private class _scoreEntry
    {
        public string nickName;
        public string livingStatus;
        public int score;
    }

    private List<_scoreEntry> _scoreEntries = new List<_scoreEntry>();
    private GameObject [] arrayOfGameObjects = new GameObject[9];

    // Start is called before the first frame update
    void Start()
    {
        scoreboard.SetActive(false);
        CreateScoreboard();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
            UpdateScoreboard();
        }
        else
        {
            scoreboard.SetActive(false);
        }

        //foreach(var player in PhotonNetwork.PlayerList)
        //    Debug.Log(player?.NickName + " " + player?.CustomProperties["health"]?.ToString());
        
    }

    private void CreateScoreboard() 
    {
        /*foreach (var player in PhotonNetwork.PlayerList)
        {
            _scoreEntries.Add(new _scoreEntry()
            {
                nickName = player.NickName,
                livingStatus = (bool)player.CustomProperties["livingStatus"] ? "Alive" : "Dead",
                score = (int)player.CustomProperties["score"],
            });
        }

        _scoreEntries = _scoreEntries.OrderByDescending(scoreEntry => scoreEntry.score).Take(9).ToList();*/

        for (int i = 0; i < 9; i++)
        {
            arrayOfGameObjects[i] = Instantiate(scoreEntryGameObject, scoreboard.transform);
            TMP_Text[] texts = arrayOfGameObjects[i].GetComponentsInChildren<TMP_Text>();

            switch (i)
            {
                case 0:
                    texts[0].text = "1st";
                    break;

                case 1:
                    texts[0].text = "2nd";
                    break;

                case 2:
                    texts[0].text = "3rd";
                    break;

                default: texts[0].text = (i + 1).ToString() + "th";
                         break;

            }

            texts[1].text = "";//_scoreEntries.ElementAt(i).nickName;
            texts[2].text = "";//_scoreEntries.ElementAt(i).livingStatus;
            texts[3].text = "";//_scoreEntries.ElementAt(i).score.ToString();
        }

        
    }
    private void UpdateScoreboard()
    {
        _scoreEntries = new List<_scoreEntry>();

        foreach (var player in PhotonNetwork.PlayerList)
        {
            _scoreEntries.Add(new _scoreEntry()
            {
                nickName = player.NickName,
                livingStatus = (bool)player.CustomProperties["livingStatus"] ? "Alive" : "Dead",
                score = (int)player.CustomProperties["score"],
            });
        }

        _scoreEntries = _scoreEntries.OrderByDescending(scoreEntry => scoreEntry.score).Take(9).ToList();

        int rowsToUpdate = _scoreEntries.Count();

        for (int i = 0; i < rowsToUpdate; i++)
        {
            TMP_Text[] texts = arrayOfGameObjects[i].GetComponentsInChildren<TMP_Text>();

            texts[1].text = _scoreEntries.ElementAt(i).nickName;
            texts[2].text = _scoreEntries.ElementAt(i).livingStatus;
            texts[3].text = _scoreEntries.ElementAt(i).score.ToString();
            //Debug.Log("Scoreboard log: " + _scoreEntries.ElementAt(i).score.ToString());
        }

        for (int i = rowsToUpdate; i < 9; i++)
        {
            TMP_Text[] texts = arrayOfGameObjects[i].GetComponentsInChildren<TMP_Text>();

            texts[1].text = "";
            texts[2].text = "";
            texts[3].text = "";
        }
    }
}
