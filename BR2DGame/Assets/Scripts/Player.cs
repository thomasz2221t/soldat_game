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
    [SerializeField] TMP_Text ammoCountText1;
    [SerializeField] TMP_Text ammoCountText2;
    [SerializeField] TMP_Text ammoCountText3;

    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private GameObject playerCamera;

    [SerializeField] private GameObject akSymbolPrefab;
    [SerializeField] private GameObject pistolSymbolPrefab;
    [SerializeField] private GameObject ak; // !!! Don't change, it has to be initialized by SerializeField !!!
    [SerializeField] private GameObject pistol; // !!! Don't change, it has to be initialized by SerializeField !!!
    private uint ammoCount1 = 0;
    private uint ammoCount2 = 0;
    private uint ammoCount3 = 0;
    private GameObject weaponSymbol;
    private GameObject ammoSymbol;
    private GameObject firePoint;
    private GameObject akFirePoint; 
    private GameObject pistolFirePoint;
    private GameObject head;

    private GameObject sceneCamera;

    private Vector2 inputPosition;
    private Vector2 mousePosition;
    private Vector2 headPosition;
    private Vector2 firePointPosition;
    private float firePointHeadDistance;
    private float mouseHeadDistance;

    private bool isHoldingAk = true;
    private bool pickUpAllowed = false;

    //////////////////////////////// Shooting ////////////////////////////////
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] static private float shotCooldown = 0.1f;
    [SerializeField] private int magazineSize = 30;

    float timeStamp = 0;
    float timeStamp2 = 0;


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
            this.GetComponent<PhotonView>().RPC("destroyWeaponSymbol", RpcTarget.All);
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

        ammoCountText1.text = ammoCount1.ToString();
        ammoCountText2.text = ammoCount2.ToString();
        ammoCountText3.text = ammoCount3.ToString();

        // Shooting
        if (ak.activeInHierarchy && Input.GetButton("Fire1") && view.IsMine) {
            if ((timeStamp <= Time.time) && (magazineSize > 0)) {
                ShootAk();
                timeStamp = Time.time + shotCooldown;
                magazineSize--;
            }

            //[DELETE] AutoReload cooldown not working anyway
            if (magazineSize <= 0) {
                if (timeStamp2 <= Time.time) {
                    magazineSize = 30;
                    timeStamp2 = Time.time + 3f;
                }
            }
        }
        else if (pistol.activeInHierarchy && Input.GetButtonDown("Fire1") && view.IsMine) {
            ShootPistol();
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
        if (view.IsMine) {
            health -= damage;
            healthbarImage.fillAmount = health / maxHealth;
            Debug.Log("Health: " + health + " maxHealth: " + maxHealth + " divided: " + health / maxHealth);
        }
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
        } else if (collision.gameObject.tag.Equals("AmmoSymbol1") || collision.gameObject.tag.Equals("AmmoSymbol2") || collision.gameObject.tag.Equals("AmmoSymbol3")) {
            ammoSymbol = collision.gameObject;
            Debug.Log(collision.gameObject.name);
            if(collision.gameObject.tag.Equals("AmmoSymbol1")) {
                ammoCount1 += 30;
                Debug.Log(ammoCount1);
            } else if(collision.gameObject.tag.Equals("AmmoSymbol2")) {
                ammoCount2 += 30;
                Debug.Log(ammoCount3);
            } else if(collision.gameObject.tag.Equals("AmmoSymbol3")) {
                ammoCount3 += 30;
                Debug.Log(ammoCount3);
            }
            this.GetComponent<PhotonView>().RPC("destroyAmmoSymbol", RpcTarget.AllBuffered);
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

    //function realizing releasing the bullet from barell
    //[RPC] - obsolete
    [PunRPC]
    void ShootAk() {
        PhotonNetwork.Instantiate(bulletPrefab.name, akFirePoint.transform.position, akFirePoint.transform.rotation); //Instantiation of a new bullet
    }

    [PunRPC]
    void ShootPistol() {
        PhotonNetwork.Instantiate(bulletPrefab.name, pistolFirePoint.transform.position, pistolFirePoint.transform.rotation); //Instantiation of a new bullet
    }

    [PunRPC]
    public void destroyWeaponSymbol() {
        Destroy(weaponSymbol);
    }

    [PunRPC]
    public void destroyAmmoSymbol() {
        Destroy(ammoSymbol);
    }

}
