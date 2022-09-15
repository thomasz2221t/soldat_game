using UnityEngine;

/// <summary>
/// Klasa Crosshair realizuj¹ca abstrakt celownika widocznego w GUI gracza
/// </summary>
public class Crosshair : MonoBehaviour
{
    /// <summary>
    /// Referencja do obiektu celownika
    /// </summary>
    public GameObject crosshair;

    /// <summary>
    /// Metoda Start wywo³ywana przed pierwsz¹ aktualizacj¹ klatki. 
    /// Przechowuje wywo³ania metod oraz inicjalizacje zmiennych.
    /// </summary>
    void Start()
    {
        crosshair.SetActive(true);
        Cursor.visible = false;
    }

    /// <summary>
    /// W metodzie update obs³uga transformacji celownika zgodnie z kursorem myszy
    /// </summary>
    void Update()
    {
        crosshair.transform.position = Input.mousePosition;
    }
}
