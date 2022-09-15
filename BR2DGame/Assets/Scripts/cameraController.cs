using Photon.Pun;
using UnityEngine;

/// <summary>
/// Klasa cameraController realizuj¹ca abstrakt kontrolera kamery
/// </summary>
public class cameraController : MonoBehaviour
{
    /// <summary>
    /// Referencja do komponentu widoku PhotonView
    /// </summary>
    [SerializeField] PhotonView view;

    // Start is called before the first frame update
    private void Start() {
        
    }
}
