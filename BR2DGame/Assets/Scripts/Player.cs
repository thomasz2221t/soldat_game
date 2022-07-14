using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private float health = 1000;
    private float maxHealth;
    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject ui;
    //private int currentHealth;
    //public HealthBar healthBar;

    [SerializeField] private float speed = 50;
    [SerializeField] PhotonView view;
    [SerializeField] TMP_Text playerName;

    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private GameObject playerCamera;

    [SerializeField] private GameObject akSymbolPrefab;
    [SerializeField] private GameObject pistolSymbolPrefab;
    [SerializeField] private GameObject ak; // !!! Don't change, it has to be initialized by SerializeField !!!
    [SerializeField] private GameObject pistol; // !!! Don't change, it has to be initialized by SerializeField !!!
    private GameObject weaponSymbol;
    private GameObject firePoint;
    private GameObject akFirePoint; 
    private GameObject pistolFirePoint;
    private GameObject head;

    private GameObject sceneCamera;

    public Vector2 inputPosition;
    public Vector2 mousePosition;
    public Vector2 headPosition;
    public Vector2 firePointPosition;
    public float firePointHeadDistance;
    public float mouseHeadDistance;

    private bool isHoldingAk = true;
    private bool pickUpAllowed = false;


    // Start is called before the first frame update
    //nice way to get child object by name
    //GameObject firePoint = shooter.transform.Find("FirePoint").gameObject;
    void Start()
    {
        view = GetComponent<PhotonView>();
        maxHealth = health;
        if (view.IsMine) {
            sceneCamera = GameObject.Find("Main Camera");
            head = GameObject.Find("Head");
            akFirePoint = GameObject.Find("FirePoint"); // akFirePoint
            pistolFirePoint = GameObject.Find("PistolFirePoint");

            if (isHoldingAk) {
                pistol.SetActive(false);
                firePoint = akFirePoint;
            }

            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
        } else {
            Destroy(ui);
        }
        Debug.Log(view.Owner.NickName);
        playerName.text = view.Owner.NickName;
    }

    private void Update()
    {
        if (!view.IsMine)
            return;

        inputPosition.x = Input.GetAxis("Horizontal");
        inputPosition.y = Input.GetAxis("Vertical");

        Camera playerCam = playerCamera.GetComponent<Camera>();
        mousePosition = playerCam.ScreenToWorldPoint(Input.mousePosition); //Getting the coordinates of mouse cursor as world's point

        //Picking up items from the ground and dropping the weapon that the player is currently holding
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E)) {
            Debug.Log(weaponSymbol.name);
            equipWeapon(weaponSymbol.name);
            this.GetComponent<PhotonView>().RPC("equipWeapon", RpcTarget.OthersBuffered, weaponSymbol.name);
            this.GetComponent<PhotonView>().RPC("destroyWeaponSymbol", RpcTarget.AllBuffered);

        }

        //Getting positions for the player rotation
        if (head != null) {
            headPosition.x = head.transform.position.x;
            headPosition.y = head.transform.position.y;
        }
        if(firePoint != null) {
            firePointPosition.x = firePoint.transform.position.x;
            firePointPosition.y = firePoint.transform.position.y;
        }
        if ((head != null) && (firePoint != null))
        {
            firePointHeadDistance = Vector2.Distance(headPosition, firePointPosition);
            mouseHeadDistance = Vector2.Distance(headPosition, mousePosition);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (view.IsMine)
        {
            //Character movement
            playerRigidbody.MovePosition(playerRigidbody.position + inputPosition * speed * Time.fixedDeltaTime);

            //Character rotation
            float lookDirX = 0.0f;
            float lookDirY = 0.0f;

            //Precise aiming rotation (long range)
            if ((mouseHeadDistance-4.0f) > (firePointHeadDistance))
            {
                lookDirX = mousePosition.x - firePoint.transform.position.x;
                lookDirY = mousePosition.y - firePoint.transform.position.y;
                float currentAngle = playerRigidbody.rotation;
                float angle = Mathf.Atan2(lookDirY, lookDirX) * Mathf.Rad2Deg - 90; //95 degrees - offset, which should be changed after creating final player model
                head.transform.rotation = Quaternion.Euler(0, 0, angle); //Rotation of the weapon, it should point to the local cursor

                //More elaborate way to smoothe the angle is written below. Should be used at a later time

                /*float angleDiff = angle - currentAngle;
                angleDiff = Mathf.Repeat(angleDiff + 180f, 360f) - 180f;
                angle = currentAngle + angleDiff;
                float smoothedAngle = Mathf.Lerp(currentAngle, angle, 0.2f);*/
            }
            //From hips aiming rotation (close range)
            else if(mouseHeadDistance != firePointHeadDistance)
            {
                lookDirX = mousePosition.x - head.transform.position.x;
                lookDirY = mousePosition.y - head.transform.position.y;
                float currentAngle = playerRigidbody.rotation;
                float angle = Mathf.Atan2(lookDirY, lookDirX) * Mathf.Rad2Deg - 85; //87 degrees - offset, which should be changed after creating final player model
                head.transform.rotation = Quaternion.Euler(0, 0, angle); //Rotation of the weapon, it should point to the local cursor

                //More elaborate way to smoothe the angle is written below. Should be used at a later time

                /*float angleDiff = angle - currentAngle;
                angleDiff = Mathf.Repeat(angleDiff + 180f, 360f) - 180f;
                angle = currentAngle + angleDiff;
                float smoothedAngle = Mathf.Lerp(currentAngle, angle, 0.2f);*/
            }

        }
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthbarImage.fillAmount = health / maxHealth;
        Debug.Log("Health: " + health + " maxHealth: " + maxHealth + " divided: " + health / maxHealth);

        if (health <= 0) {
            if(view.IsMine)
                PhotonNetwork.LoadLevel("Dead");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("WeaponSymbol")) {
            pickUpAllowed = true;
            weaponSymbol = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("WeaponSymbol")) {
            pickUpAllowed = false;
        }
    }

    [PunRPC]
    public void equipWeapon(string weaponName) {
        if (weaponName.Contains("pistol")) {
            Debug.Log("Pisztolet znaleziony");
            ak.SetActive(false);
            pistol.SetActive(true);
            firePoint = pistolFirePoint;
            dropCurrentWeapon(isHoldingAk);
            isHoldingAk = false;

        }
        else if (weaponName.Contains("ak")) {
            Debug.Log("Akacz znaleziony");
            ak.SetActive(true);
            pistol.SetActive(false);
            firePoint = akFirePoint;
            dropCurrentWeapon(isHoldingAk);
            isHoldingAk = true;
        }
    }

    public void dropCurrentWeapon(bool wasHoldingAK) {
        if (view.IsMine) {
            if (wasHoldingAK)
                PhotonNetwork.Instantiate(akSymbolPrefab.name, this.transform.position, this.transform.rotation);
            else
                PhotonNetwork.Instantiate(pistolSymbolPrefab.name, this.transform.position, this.transform.rotation);
        }
    }

    [PunRPC]
    public void destroyWeaponSymbol() {
        Destroy(weaponSymbol);
    }
}
