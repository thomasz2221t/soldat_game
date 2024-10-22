using UnityEngine;

/// <summary>
/// Klasa realizująca abstrakt minimapy, GUI gracza
/// </summary>
public class Minimap : MonoBehaviour
{

    private void Start() {

    }

    /// <summary>
    /// Metoda realizująca aktualizację minimapy zgodnie z położeniem gracza
    /// </summary>
    private void LateUpdate() {
        if (SpawnPlayers.getLocalPlayerReference() != null) {
            Vector3 newPosition = SpawnPlayers.getLocalPlayerReference().transform.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
    }
}
