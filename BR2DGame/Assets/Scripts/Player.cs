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
    //[SerializeField] InputField usernameInput;


    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        //playerName = GetComponent<TMP_Text>();
        Debug.Log(view.Owner.NickName);
        playerName.text = view.Owner.NickName;
        //playerName.GetComponent<Text>().text = view.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine) {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(speed * inputX, speed * inputY, 0);
            movement *= Time.deltaTime;
            transform.Translate(movement);
        }
    }
}
