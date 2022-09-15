using UnityEngine;

/// <summary>
/// Klasa realizuj¹ca abstrakt minimapy, GUI gracza
/// </summary>
public class Minimap : MonoBehaviour
{

    private void Start() {

    }

    /// <summary>
    /// Metoda realizuj¹ca aktualizacjê minimapy zgodnie z po³o¿eniem gracza
    /// </summary>
    private void LateUpdate() {
        if (SpawnPlayers.getLocalPlayerReference() != null) {
            Vector3 newPosition = SpawnPlayers.getLocalPlayerReference().transform.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
    }
}
