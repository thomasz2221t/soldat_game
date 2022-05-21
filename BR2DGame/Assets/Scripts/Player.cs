using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 50;
    [SerializeField] PhotonView view;
    [SerializeField] TMP_Text playerName;

    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private GameObject playerCamera;
    private GameObject weapon;
    private GameObject firePoint;

    private GameObject sceneCamera;

    public Vector2 inputPosition;
    public Vector2 mousePosition;


    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine) {
            sceneCamera = GameObject.Find("Main Camera");
            weapon = GameObject.Find("Weapon");
            firePoint = GameObject.Find("FirePoint");

            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
        }
        Debug.Log(view.Owner.NickName);
        playerName.text = view.Owner.NickName;
    }

    private void Update()
    {
        inputPosition.x = Input.GetAxis("Horizontal");
        inputPosition.y = Input.GetAxis("Vertical");

        Camera playerCam = playerCamera.GetComponent<Camera>();
        mousePosition = playerCam.ScreenToWorldPoint(Input.mousePosition); //Getting the coordinates of mouse cursor as world's point
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (view.IsMine)
        {
            //Character movement
            playerRigidbody.MovePosition(playerRigidbody.position + inputPosition * speed * Time.fixedDeltaTime);

            //Character rotation
            float lookDirX = mousePosition.x - weapon.transform.position.x;
            float lookDirY = mousePosition.y - weapon.transform.position.y;
            float currentAngle = playerRigidbody.rotation;
            float angle = Mathf.Atan2(lookDirY, lookDirX) * Mathf.Rad2Deg - 95f; //95 degrees - offset, which should be changed after creating final player model
            weapon.transform.rotation = Quaternion.Euler(0, 0, angle); //Rotation of the weapon, it should point to the local cursor

            //More elaborate way to smoothe the angle is written below. Should be used at a later time

            /*float angleDiff = angle - currentAngle;
            angleDiff = Mathf.Repeat(angleDiff + 180f, 360f) - 180f;
            angle = currentAngle + angleDiff;
            float smoothedAngle = Mathf.Lerp(currentAngle, angle, 0.2f);*/
        }
    }
}
