using Photon.Pun;
using TMPro;
using UnityEngine;

/// <summary>
/// Klasa Scoreboard realizuj¹ca abstrakt tablicy wyników 
/// </summary>
public class Scoreboard : MonoBehaviour
{
    /// <summary>
    /// Zmienna przechowuj¹ca referencjê do obiektu tablicy wyników
    /// </summary>
    public GameObject scoreboard;
    /// <summary>
    /// Zmienna przechowuj¹ca referencjê do obiektu realizujacego przechowywanie wyników
    /// </summary>
    public GameObject scoreEntry;

    /// <summary>
    /// Metoda Start wywo³ywana przed pierwsz¹ aktualizacj¹ klatki. Przechowuje wywo³ania metod oraz inicjalizacje zmiennych.
    /// Stworzenie instancji obiektu newScoreEntry, pobranie danych oraz zapis na tablicê wyników
    /// </summary>
    void Start()
    {
        scoreboard.SetActive(false);

        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject newScoreEntry = Instantiate(scoreEntry, scoreboard.transform);
            TMP_Text[] texts = newScoreEntry.GetComponentsInChildren<TMP_Text>();

            texts[0].text = "9th";
            texts[1].text = player.NickName;
            texts[2].text = "99";
            texts[3].text = "59:59";
        }
    }

    /// <summary>
    /// Metoda update realizuj¹ca pokazanie tablicy wyników na ¿¹danie gracza, po wciœniêciu przycisku tab
    /// </summary>
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
