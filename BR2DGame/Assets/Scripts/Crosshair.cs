using UnityEngine;

/// <summary>
/// Klasa Crosshair realizująca abstrakt celownika widocznego w GUI gracza
/// </summary>
public class Crosshair : MonoBehaviour
{
    /// <summary>
    /// Referencja do obiektu celownika
    /// </summary>
    public GameObject crosshair;

    /// <summary>
    /// Metoda Start wywoływana przed pierwszą aktualizacją klatki. 
    /// Przechowuje wywołania metod oraz inicjalizacje zmiennych.
    /// </summary>
    void Start()
    {
        crosshair.SetActive(true);
        Cursor.visible = false;
    }

    /// <summary>
    /// W metodzie update obsługa transformacji celownika zgodnie z kursorem myszy
    /// </summary>
    void Update()
    {
        crosshair.transform.position = Input.mousePosition;
    }
}
