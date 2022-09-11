using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public GameObject crosshair;

    // Start is called before the first frame update
    void Start()
    {
        crosshair.SetActive(true);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        crosshair.transform.position = Input.mousePosition;
    }
}
