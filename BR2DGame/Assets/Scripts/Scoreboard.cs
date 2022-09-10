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

        for (int i = 0; i < 9; i++)
        {
            GameObject newScoreEntry = Instantiate(scoreEntry, scoreboardPosition);
            TMP_Text[] texts = newScoreEntry.GetComponentsInChildren<TMP_Text>();
            Debug.Log(texts.Length);
            texts[0].text = i.ToString();
            texts[1].text = "nickname1234";
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
    }
}
