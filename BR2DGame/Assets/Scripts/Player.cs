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

    //control of the player's rigid body - movement while aiming
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private GameObject playerCamera;
    private GameObject sceneCamera;

    public Vector2 inputPosition;
    public Vector2 mousePosition;


    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        //sceneCamera = GameObject.Find("Camera");

        if (view.IsMine) {
            sceneCamera = GameObject.Find("Main Camera");
            //playerCamera = GameObject.Find("PlayerCamera");

            //playerCamera = sceneCamera;

            sceneCamera.SetActive(false);
            //playerCamera.SetActive(true);
        }
        //playerName = GetComponent<TMP_Text>();
        Debug.Log(view.Owner.NickName);
        playerName.text = view.Owner.NickName;
        //playerName.GetComponent<Text>().text = view.Owner.NickName;
    }

    // Update is called once per frame - function is calculating parameters needed in FixedUpdate to perform movement
    /*void Update()
    {
        if (view.IsMine) {
            inputPosition.x = Input.GetAxis("Horizontal");
            inputPosition.y = Input.GetAxis("Vertical");

            mousePosition = playerCam.ScreenToWorldPoint(Input.mousePosition);//getting the place of mouse cursor as world's point

            //Vector3 movement = new Vector3(speed * inputX, speed * inputY, 0);
            //movement *= Time.deltaTime;
            //transform.Translate(movement);

        }
    }*/

    //Moving player basing on value in Update function (input of movment)
    /*void FixedUpdate()
    {
        //moving player
        playerRigidbody.MovePosition(playerRigidbody.position + inputPosition * speed * Time.deltaTime);

        //aiming - player is aiming towards middle of the model - important while placing weapon on model
        Vector2 lookDir = mousePosition - playerRigidbody.position;
        float angle = Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg - 195f; //195 degrees - offset. offset should be changed after placing final player model
        playerRigidbody.rotation = angle;
    }*/

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            //mousePosition = playerCam.ScreenToWorldPoint(Input.mousePosition);//getting the place of mouse cursor as world's point

            Vector3 movement = new Vector3(speed * inputX, speed * inputY, 0);
            movement *= Time.deltaTime;

            //Vector2 lookDir = mousePosition - playerRigidbody.position;
            //float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 195f; //195 degrees - offset. offset should be changed after placing final player model

            transform.Translate(movement);
            //transform.rotation = Quaternion.Euler(0,0,angle);
        }
    }
}
