using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

/// <summary>
/// Klasa player reprezentuj¹ca abstrakt gracza.
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// Tablica hashów przekazuj¹ca dane o graczu do tabeli wyników
    /// </summary>
    private ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();
    /// <summary>
    /// Zmienna zliczaj¹ca czas ¿ycia gracza
    /// </summary>
    private Stopwatch _livingTime = new Stopwatch();
    /// <summary>
    /// Zmienna przechowuj¹ca punkty ¿ycia gracza
    /// </summary>
    [SerializeField] private float health = 1000;
    /// <summary>
    /// Zmienna przechowuj¹ca maksymalne ¿ycie gracza
    /// </summary>
    private float maxHealth;
    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject ui;
    [SerializeField] Animator animator;
    /// <summary>
    /// Zmienna przechowuj¹ca prêdkoœæ ruchu gracza
    /// </summary>
    [SerializeField] private float speed;
    /// <summary>
    /// Widok komponentu PhotonView - widok gracza w rozgrywce multiplayer
    /// </summary>
    [SerializeField] PhotonView view;
    /// <summary>
    /// Pole tekstowe przechowuj¹ce nick gracza
    /// </summary>
    [SerializeField] TMP_Text playerName;
    /// <summary>
    /// Pole tekstowe przechowuj¹ce iloœæ amunicji podstawowej
    /// </summary>
    [SerializeField] TMP_Text ammoCountText1;
    /// <summary>
    /// Pole tekstowe przechowuj¹ce iloœæ amunicji odbijaj¹cej siê
    /// </summary>
    [SerializeField] TMP_Text ammoCountText2;
    /// <summary>
    /// Pole tekstowe przechowuj¹ce iloœæ amunicji eksploduj¹cej
    /// </summary>
    [SerializeField] TMP_Text ammoCountText3;
    /// <summary>
    /// Pole tekstowe przechowuj¹ce iloœæ amunicji w magazynu
    /// </summary>
    [SerializeField] TMP_Text weaponMagazine;
    /// <summary>
    /// Powiadomienie, ¿e trwa prze³adowywanie broni
    /// </summary>
    [SerializeField] TMP_Text reloadingNotification;
    /// <summary>
    /// Komponent Rigidbody gracza
    /// </summary>
    [SerializeField] private Rigidbody2D playerRigidbody;
    /// <summary>
    /// Zmienna przechowuj¹ca referencje do komponentu kamery gracza
    /// </summary>
    [SerializeField] private GameObject playerCamera;
    /// <summary>
    /// Prefab karabinu szturmowego
    /// </summary>
    [SerializeField] private GameObject akSymbolPrefab;
    /// <summary>
    /// Prefab pistoletu
    /// </summary>
    [SerializeField] private GameObject pistolSymbolPrefab;
    /// <summary>
    /// Prefab strzelby
    /// </summary>
    [SerializeField] private GameObject shotgunSymbolPrefab;
    /// <summary>
    /// Referencja do obiektu karabinu szturmowego
    /// </summary>
    [SerializeField] private GameObject ak;
    /// <summary>
    /// Referencja do obiektu pistoletu
    /// </summary>
    [SerializeField] private GameObject pistol;
    /// <summary>
    /// Referencja do obiektu strzelby
    /// </summary>
    [SerializeField] private GameObject shotgun;
    /// <summary>
    /// Referencja do obiektu ataku krótkodystansowego
    /// </summary>
    [SerializeField] private GameObject melee;
    /// <summary>
    /// Zmienna zliczaj¹ca iloœæ amunicji podstawowej
    /// </summary>
    private uint ammoCountNormal = 60;
    /// <summary>
    /// Zmienna zliczaj¹ca iloœæ amunicji odbijaj¹cej siê
    /// </summary>
    private uint ammoCountBouncy = 15;
    /// <summary>
    /// Zmienna zliczaj¹ca iloœæ amunicji eksploduj¹cej
    /// </summary>
    private uint ammoCountExplo = 5;
    /// <summary>
    /// Referencja do obiektu symbolu karabinu szturmowego
    /// </summary>
    private GameObject weaponSymbol;
    /// <summary>
    /// Referencja do obiektu symbolu amunicji
    /// </summary>
    private GameObject ammoSymbol;
    /// <summary>
    /// Rwferencja do obiektu apteczki
    /// </summary>
    private GameObject healthpackObject;
    /// <summary>
    /// Referencja do obiektu po³o¿enia lufy aktulanie wybranej broni
    /// </summary>
    private GameObject firePoint;
    /// <summary>
    /// Referencja do obiektu po³o¿enia lufy karabinu szturmowego
    /// </summary>
    private GameObject akFirePoint;
    /// <summary>
    /// Referencja do obiektu po³o¿enia lufy pistoletu
    /// </summary>
    private GameObject pistolFirePoint;
    /// <summary>
    /// Referencja do obiektu g³owy gracza
    /// </summary>
    private GameObject head;
    /// <summary>
    /// Referencja do obiektu po³o¿enia lufy strzelby
    /// </summary>
    private GameObject shotgunFirePoint;
    /// <summary>
    /// Referencja do obiektu kamery sceny
    /// </summary>
    private GameObject sceneCamera;
    /// <summary>
    /// Wektor przechowuj¹cy pozyjcê gracza
    /// </summary>
    private Vector2 inputPosition;
    /// <summary>
    /// Wektor przechowuj¹cy aktuln¹ pozycjê kursora myszy
    /// </summary>
    private Vector2 mousePosition;
    /// <summary>
    /// Wektor przechowuj¹cy po³o¿enie obiektu g³owy modelu gracza
    /// </summary>
    private Vector2 headPosition;
    /// <summary>
    /// Wektor przechowuj¹cy pozycjê lufy wybranej broni
    /// </summary>
    private Vector2 firePointPosition;
    /// <summary>
    /// Dystans dziel¹cy g³owê gracza oraz pozyjcê lufy karabinu, wartoœæ potrzebna przy okreœlaniu rotacji modelu gracza
    /// </summary>
    private float firePointHeadDistance;
    /// <summary>
    /// Dystans dziel¹cy g³owê gracza oraz pozyjcê kursoru myszy
    /// </summary>
    private float mouseHeadDistance;
    /// <summary>
    /// Zmienna definiuj¹ca aktualnie wybran¹ przez gracza broñ.
    /// Wartoœæ -1 okreœla, ¿e gracz nie podniós³ jeszcze broni.
    /// Wartoœæ 0 okreœla, ¿e gracz aktualnie posiada karabin szturmowy.
    /// Wartoœæ 1 okreœla, ¿e gracz aktualnie posiada pistolet.
    /// Wartoœæ 2 okreœla, ¿e gracz aktualnie posiada strzelbê
    /// </summary>
    private int weaponId = -1; //0 - holdingAk, 1 - holdingPistol, 2 - holdingShotgun, -1 nothing
    /// <summary>
    /// Zmienna logczina kontroluj¹ca logikê podnoszenia itemów na mapie
    /// </summary>
    private bool pickUpAllowed = false;
    /// <summary>
    /// Zmienna logiczna kontroluj¹ca przebieg gry wieloosobowej
    /// </summary>
    private bool isMultiplayer = false;

    //////////////////////////////// Shooting ////////////////////////////////
    
    /// <summary>
    /// Referencja do obiektu prefaba pocisku standardowego
    /// </summary>
    [SerializeField] private GameObject bulletPrefab;
    /// <summary>
    /// Referencja do obiektu prefaba pocisku odbijaj¹cego siê
    /// </summary>
    [SerializeField] private GameObject bouncyBulletPrefab;
    /// <summary>
    /// Referencja do obiektu prefaba pocisku eksploduj¹cego
    /// </summary>
    [SerializeField] private GameObject exploBulletPrefab;
    /// <summary>
    /// Zmienna kontroluj¹ca czas opóŸnienia po strzale
    /// </summary>
    [SerializeField] static private float shotCooldown = 0.1f;
    float timeStamp = 0;
    /// <summary>
    /// Zmienna kontroluj¹ca rozmiar magazynka karabinu szturmowego
    /// </summary>
    [SerializeField] private int akMagazineSize = 30;
    /// <summary>
    /// Zmienna kontroluj¹ca rozmiar magazynka pistoletu
    /// </summary>
    [SerializeField] private int pistolMagazineSize = 12;
    /// <summary>
    /// Zmienna kontroluj¹ca rozmiar magazynka strzelby
    /// </summary>
    [SerializeField] private int shotgunMagazineSize = 2;
    /// <summary>
    /// Zmienna kontroluj¹ca czas prze³adowania karabinu szturmowego
    /// </summary>
    private float reloadAkTime = 2f;
    /// <summary>
    /// Zmienna kontroluj¹ca czas prze³adowania pistoletu
    /// </summary>
    private float reloadPistolTime = 1f;
    /// <summary>
    /// Zmienna kontroluj¹ca czas prze³adowania strzelby
    /// </summary>
    private float reloadShotgunTime = 0.5f;
    /// <summary>
    /// Zmienna kontroluj¹ca liczbê pocisków w magazynku broni
    /// </summary>
    private int bulletsInWeaponMagazine;
    /// <summary>
    /// Zmienna logiczna kontroluj¹ca czy zachodzi prze³adowanie broni i nie ma mo¿liwoœci strza³u
    /// </summary>
    private bool inReload = false;
    /// <summary>
    /// Zmienna zliczaj¹ca iloœæ naciœniêæ przycisków strza³u
    /// </summary>
    private uint shotClicksCounter = 0;
    /// <summary>
    /// Zmienna przechowuj¹ca iloœæ punktów ¿ycia przyznawanych po zebraniu apteczki
    /// </summary>
    private float healthpackPlusHealth = 150;
    /// <summary>
    /// Zmienna przechowuj¹ca referencje do obiektu ataku wrêcz
    /// </summary>
    [SerializeField] private GameObject meleeAnimationPrefab;
    /// <summary>
    /// Zmienna przechouj¹ca zakres obra¿eñ ataku wrêcz
    /// </summary>
    [SerializeField] private float meleeAttackRange;
    /// <summary>
    /// Zmienna przechowuj¹ca obra¿enia uzyskiwane po ataku wrêcz
    /// </summary>
    private float meleeDamage = 40f;
    /// <summary>
    /// Zmienne przechowuj¹ce grafiki mo¿liwych do wybrania typów pocisków 
    /// </summary>
    [SerializeField] private Image normalAmmoBackground;
    [SerializeField] private Image bouncyAmmoBackground;
    [SerializeField] private Image exploAmmoBackground;
    /// <summary>
    /// Kolory pocisków odbijaj¹cych siê i wybuchaj¹cych
    /// </summary>
    Color greenColor = new Color(72, 224, 113, 255);
    Color yellowColor = new Color(242, 163, 46, 255);

    /// <summary>
    /// Enum bulletType przechowuj¹cy typy pocisków, którymi dysponuj¹ gracze
    /// </summary>
    enum bulletType {
        NORMAL,
        BOUNCY,
        EXPLO
    }

    /// <summary>
    /// Zmienna typu bulletType przechouj¹ca wybrany przez gracza typ pocisku
    /// </summary>
    bulletType ammoTypeUsed = Player.bulletType.NORMAL;

    /// <summary>
    /// Metoda start wywo³ywana przed pierwsz¹ aktualizacj¹ klatki. 
    /// Przechowuje wywo³ania metod oraz inicjalizacje zmiennych potrzebnych zaraz po utworzeniu obiektu gracza
    /// </summary>
    void Start()
    {
        //Pobranie komponentu PhotonView
        view = GetComponent<PhotonView>();

        maxHealth = health;

        _customProperties.Add("health", health);

        PhotonNetwork.LocalPlayer.SetCustomProperties(_customProperties);

        if (view.IsMine) {
            //przypisanie referencji do potrzebnych komponentów na planszy gry
            sceneCamera = GameObject.Find("Main Camera");
            head = GameObject.Find("Head");
            akFirePoint = GameObject.Find("FirePoint");
            pistolFirePoint = GameObject.Find("PistolFirePoint");
            shotgunFirePoint = GameObject.Find("ShotgunFirePoint");

            this.GetComponent<PhotonView>().RPC("setWeaponsNotActive", RpcTarget.AllBuffered);

            firePoint = akFirePoint;
            bulletsInWeaponMagazine = akMagazineSize;

            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
        } else {
            Destroy(ui);
        }
        //Ustawienie nazwy gracza
        playerName.text = view.Owner.NickName;

        _livingTime.Start();
    }
    
    /// <summary>
    /// Metoda kontroluj¹ca liczbê graczy w rozgrywce. Uniemo¿liwia wygranie gry je¿eli w sesji nie by³o wiêcej ni¿ dwóch graczy.
    /// Ustawia zmienn¹ logiczn¹ isMultiplayer je¿eli rozgrywka spe³nia warunki do zwyciestwa.
    /// </summary>
    public void OnLobbyStatisticsUpdate() {
        if(PhotonNetwork.PlayerList.Length >= 2) 
            isMultiplayer = true;
        if(isMultiplayer && PhotonNetwork.PlayerList.Length == 1) {
            PhotonNetwork.Destroy(view);
            if (view.IsMine)
                PhotonNetwork.LoadLevel("Winner");
            _livingTime.Stop();
        }
    }

    /// <summary>
    /// Metoda PunRPC wywo³ywana w metodzie Start() ukrywaj¹ca obiekty broni gracza przy inicjalizacji
    /// </summary>
    [PunRPC]
    public void setWeaponsNotActive() {
        pistol.SetActive(false);
        shotgun.SetActive(false);
        ak.SetActive(false);
    }

    /// <summary>
    /// Metoda update wywo³ywana po ka¿dej klatce, kontroluje wp³yw logiki gry na stan gracza oraz czynnoœci u¿ytkownika na rozgrywkê
    /// </summary>
    private void Update()
    {
        if (!view.IsMine)
            return;

        OnLobbyStatisticsUpdate(); //sprawdzenie ilu graczy jest w rogrywce

        //Kontrola UI
        ammoCountText1.text = ammoCountNormal.ToString();
        ammoCountText2.text = ammoCountBouncy.ToString();
        ammoCountText3.text = ammoCountExplo.ToString();
        weaponMagazine.text = bulletsInWeaponMagazine.ToString()+ "/" + ((ak.activeSelf) ? akMagazineSize.ToString() : (pistol.activeSelf ? pistolMagazineSize.ToString() : shotgunMagazineSize.ToString()));

        //Wybranie typów amunicji
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            deloadAndChangeAmmoType(ammoTypeUsed, bulletType.NORMAL);
            normalAmmoBackground.color = Color.yellow;
            bouncyAmmoBackground.color = Color.white;
            exploAmmoBackground.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            deloadAndChangeAmmoType(ammoTypeUsed, bulletType.BOUNCY);
            normalAmmoBackground.color = Color.white;
            bouncyAmmoBackground.color = Color.green; 
            exploAmmoBackground.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            deloadAndChangeAmmoType(ammoTypeUsed, bulletType.EXPLO);
            normalAmmoBackground.color = Color.white;
            bouncyAmmoBackground.color = Color.white; 
            exploAmmoBackground.color = Color.red;
        }

        //Pobranie pozycji gracza
        inputPosition.x = Input.GetAxis("Horizontal");
        inputPosition.y = Input.GetAxis("Vertical");

        Camera playerCam = playerCamera.GetComponent<Camera>();
        mousePosition = playerCam.ScreenToWorldPoint(Input.mousePosition); //Pobranie koordynatów kursora myszy jako punkty na planszy rozgrywki

        //Podniesienie itemów z ziemi oraz wyrzucenie broni je¿eli gracza posiada broñ
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E)) {
            equipWeapon(weaponSymbol.name);
            this.GetComponent<PhotonView>().RPC("equipWeapon", RpcTarget.OthersBuffered, weaponSymbol.name);
            this.GetComponent<PhotonView>().RPC("destroyWeaponSymbol", RpcTarget.All);
        }

        //Pobranie pozycji dla rotacji gracza
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

        //Walka wrêcz, animacja widoczna wy³¹cznie w widoku gracza atakuj¹cego wrêcz
        if (Input.GetKeyDown(KeyCode.Mouse1)) { 
            animator.Play("MeleeAttack");
            meleeAttack();
        }

        ammoCountText1.text = ammoCountNormal.ToString();
        ammoCountText2.text = ammoCountBouncy.ToString();
        ammoCountText3.text = ammoCountExplo.ToString();

        //Pobranie liczby naboi w ekwipunku gracza
        int availableBullets = calculateAvailableBullets();

        //Prze³adowanie broni po naciœniêciu przycisku R
        if (Input.GetKeyDown(KeyCode.R))
        {
            reloadWeapon();
        }

        //Logika wystrza³u oraz prze³adowania broni po skoñczeniu siê amunicji
        if (ak.activeInHierarchy && Input.GetButton("Fire1") && view.IsMine) {
            if ((timeStamp <= Time.time) && (bulletsInWeaponMagazine > 0))
            {
                if (inReload != true) {
                    ShootAk();
                    timeStamp = Time.time + shotCooldown;
                }
            }
            else if((availableBullets > 0) && (bulletsInWeaponMagazine == 0) && inReload==false)
            {
                inReload = true;
                StartCoroutine("reloadAk");
            }
        }
        else if (pistol.activeInHierarchy && Input.GetButtonDown("Fire1") && view.IsMine) {
            if ((timeStamp <= Time.time) && (bulletsInWeaponMagazine > 0)) {
                if (inReload != true) {
                    ShootPistol();
                    timeStamp = Time.time + shotCooldown;
                }
            }
            else if(availableBullets > 0 && (bulletsInWeaponMagazine == 0) && inReload == false)
            {
                inReload = true;
                StartCoroutine("reloadPistol");
            }
        }
        else if(shotgun.activeInHierarchy && Input.GetButtonDown("Fire1") && view.IsMine){
            if ((timeStamp <= Time.time) && (bulletsInWeaponMagazine > 0) && (shotClicksCounter < bulletsInWeaponMagazine)) {
                if (inReload != true) { 
                    ShootShotgun();
                    timeStamp = Time.time + shotCooldown;
                }
            }
            else if(availableBullets > 0 && (bulletsInWeaponMagazine == 0) && inReload == false)
            {
                inReload = true;
                StartCoroutine("reloadShotgun");
            }
        }
    }

    /// <summary>
    /// Metoda obs³uguj¹ca logikê ataku wrêcz
    /// </summary>
    public void meleeAttack() {
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, meleeAttackRange);
        foreach (var hitCollider in hitColliders) {
            Player player = hitCollider.GetComponent<Player>();
            Box box = hitCollider.GetComponent<Box>();
            Barrel barrel = hitCollider.GetComponent<Barrel>();
            if (player && player.GetComponent<PhotonView>().IsMine != view.IsMine) {
                //Zadanie obra¿eñ atakiem wrêcz graczowi
                player.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, meleeDamage);
            }
            if (box) {
                //Zadanie obra¿eñ atakiem wrêcz obiektowi skrzynki
                box.TakeDamage(meleeDamage);
            }
            if (barrel) {
                //Zadanie obra¿eñ atakiem wrêcz obiektowi beczki
                barrel.TakeDamage(meleeDamage);
            }
        }
    }

    /// <summary>
    /// Metoda obs³uguj¹ca logikê prze³adowania w przypadku zmiany typu amunicji. Pociski w broni zostaj¹ wy³adowane i przekazane do 
    /// ekwipunku, a wybrany rodzaj pocisków jest ³adowany w liczbie dostêpnej w ekwipunku.
    /// </summary>
    /// <param name="currentAmmoType"> zmienna przechowuj¹ca typ amunicji obecnej w broni </param>
    /// <param name="newAmmoType"> zmienna przechowuj¹ca typ amunicji wybrany przez gracza do za³adowania</param>
    private void deloadAndChangeAmmoType(bulletType currentAmmoType, bulletType newAmmoType)
    {
        if (currentAmmoType == Player.bulletType.NORMAL)
        {
            ammoCountNormal += (uint)bulletsInWeaponMagazine;
            bulletsInWeaponMagazine = 0;
        }
        else if (currentAmmoType == Player.bulletType.BOUNCY)
        {
            ammoCountBouncy += (uint)bulletsInWeaponMagazine;
            bulletsInWeaponMagazine = 0;
        }
        else if (currentAmmoType == Player.bulletType.EXPLO)
        {
            ammoCountExplo += (uint)bulletsInWeaponMagazine;
            bulletsInWeaponMagazine = 0;
        }
        ammoTypeUsed = newAmmoType;
        if (ammoTypeUsed == Player.bulletType.NORMAL)
        {
            normalAmmoBackground.color = Color.yellow;
            bouncyAmmoBackground.color = Color.white;
            exploAmmoBackground.color = Color.white;
        }
        if (ammoTypeUsed == Player.bulletType.BOUNCY)
        {
            normalAmmoBackground.color = Color.white;
            bouncyAmmoBackground.color = Color.green;
            exploAmmoBackground.color = Color.white;
        }
        if (ammoTypeUsed == Player.bulletType.EXPLO)
        {
            normalAmmoBackground.color = Color.white;
            bouncyAmmoBackground.color = Color.white;
            exploAmmoBackground.color = Color.red;
        }
        reloadWeapon();
    }

    /// <summary>
    /// Metoda zlicza amunicjê dostêpn¹ w ekwipunku gracza
    /// </summary>
    /// <returns>Zwraca sumê pocisków, któr¹ gracz dysponuje</returns>
    private int calculateAvailableBullets()
    {
        int availableBullets = 0;
        if (ammoTypeUsed == Player.bulletType.NORMAL)
        {
            availableBullets = (int)ammoCountNormal;
        }
        else if (ammoTypeUsed == Player.bulletType.BOUNCY)
        {
            availableBullets = (int)ammoCountBouncy;
        }
        else if (ammoTypeUsed == Player.bulletType.EXPLO)
        {
            availableBullets = (int)ammoCountExplo;
        }
        return availableBullets;
    }

    /// <summary>
    /// Metoda obs³uguj¹ca logikê prze³adowania broni. Uruchamia korutyny, które s¹ kar¹ czasow¹ dla gracza za prze³adowanie.
    /// </summary>
    private void reloadWeapon()
    {
        int availableBullets = calculateAvailableBullets();

        if (ak.activeSelf)
        {
            if (availableBullets > 0 && inReload == false)
            {
                inReload = true;
                StartCoroutine("reloadAk");
            }
        }
        else if (pistol.activeSelf)
        {
            if (availableBullets > 0 && inReload == false)
            {
                inReload = true;
                StartCoroutine("reloadPistol");
            }
        }
        else if (shotgun.activeSelf)
        {
            if (availableBullets > 0 && inReload == false)
            {
                inReload = true;
                StartCoroutine("reloadShotgun");
            }
        }
    }

    /// <summary>
    /// Korutyna obs³uguj¹ca prze³adowanie karabinu szturmowego
    /// </summary>
    /// <returns>Wymusza na graczu poczekanie kary czasowej przed uzupe³nieneim amunicji w broni</returns>
    IEnumerator reloadAk()
    {
        reloadingNotification.text = "Reloading";
        yield return new WaitForSeconds(reloadAkTime);
        bulletsInWeaponMagazine = 30;
        bulletsInWeaponMagazine = subtractLoadedAmmoByType(akMagazineSize);
        inReload = false;
        reloadingNotification.text = "";
    }

    /// <summary>
    /// Korutyna obs³uguj¹ca prze³adowanie pistoletu
    /// </summary>
    /// <returns>Wymusza na graczu poczekanie kary czasowej przed uzupe³nieniem amunicji w broni</returns>
    IEnumerator reloadPistol()
    {
        reloadingNotification.text = "Reloading";
        yield return new WaitForSeconds(reloadPistolTime);
        bulletsInWeaponMagazine = 12;
        bulletsInWeaponMagazine = subtractLoadedAmmoByType(pistolMagazineSize);
        inReload = false;
        reloadingNotification.text = "";
    }

    /// <summary>
    /// Korutyna obs³uguj¹ca prze³adowanie strzelby
    /// </summary>
    /// <returns>Wymusza na graczu poczekanie kary czasowej przed uzupe³nieniem amunicji w broni</returns>
    IEnumerator reloadShotgun()
    {
        reloadingNotification.text = "Reloading";
        yield return new WaitForSeconds(reloadShotgunTime);
        bulletsInWeaponMagazine = 2;
        bulletsInWeaponMagazine = subtractLoadedAmmoByType(shotgunMagazineSize);
        inReload = false;
        reloadingNotification.text = "";
    }

    /// <summary>
    /// Metoda obs³ugujaca logikê zwi¹zan¹ z utrat¹ amunicji w ekwipunku, która jest ³adowana do broni
    /// </summary>
    /// <param name="numberQuantity">Zmienna przechowuj¹ca iloœæ mo¿liwych do za³adowania pocisków</param>
    /// <returns>Zwraca liczbê pocisków za³adowanych do broni w zale¿noœci od dostêponoœci danego typu amunicji</returns>
    private int subtractLoadedAmmoByType(int numberQuantity)
    {
        int loadedBullets = 0;
        if(ammoTypeUsed == Player.bulletType.NORMAL)
        {
            loadedBullets = (ammoCountNormal >= numberQuantity) ? numberQuantity : (int)ammoCountNormal;
            ammoCountNormal -= (uint)loadedBullets;
        }
        else if(ammoTypeUsed == Player.bulletType.BOUNCY)
        {
            loadedBullets = (ammoCountBouncy >= numberQuantity) ? numberQuantity : (int)ammoCountBouncy;
            ammoCountBouncy -= (uint)loadedBullets;
        }
        else if(ammoTypeUsed == Player.bulletType.EXPLO)
        {
            loadedBullets = (ammoCountExplo >= numberQuantity) ? numberQuantity : (int)ammoCountExplo;
            ammoCountExplo -= (uint)loadedBullets;
        }
        return loadedBullets;
    }

    /// <summary>
    /// Zmienna FixedUpdate wywo³ywana co klatkê. Przechowuje logikê gry zwi¹zan¹ z ruchem i rotacj¹ gracza
    /// </summary>
    void FixedUpdate()
    {
        if (view.IsMine)
        {
            //Ruch postaci
            playerRigidbody.MovePosition(playerRigidbody.position + inputPosition * speed * Time.fixedDeltaTime);

            //Rotacja postaci
            float lookDirX = 0.0f;
            float lookDirY = 0.0f;

            //Rotacja zwi¹zana z precyzyjnym celowaniem d³ugodystansowym
            if ((mouseHeadDistance-4.0f) > (firePointHeadDistance))
            {
                lookDirX = mousePosition.x - firePoint.transform.position.x;
                lookDirY = mousePosition.y - firePoint.transform.position.y;
                float currentAngle = playerRigidbody.rotation;
                float angle = Mathf.Atan2(lookDirY, lookDirX) * Mathf.Rad2Deg - 90;
                head.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            //Rotacja zwi¹zana z celowaniem z biodra
            else if(mouseHeadDistance != firePointHeadDistance)
            {
                lookDirX = mousePosition.x - head.transform.position.x;
                lookDirY = mousePosition.y - head.transform.position.y;
                float currentAngle = playerRigidbody.rotation;
                float angle = Mathf.Atan2(lookDirY, lookDirX) * Mathf.Rad2Deg - 85;
                head.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC obs³uguje logikê zwi¹zan¹ z przyjêciem przez gracza obra¿eñ w przypadku trafienia lub znalezienia siê w obra¿eniu obszarowym zadawanym atakiem wrêcz lub wybuchem
    /// </summary>
    /// <param name="damage">Parametr przechowuj¹cym iloœæ obra¿eñ zadawanych</param>
    [PunRPC]
    public void TakeDamage(float damage)
    {
        //Odjêcie punktów ¿ycia
        if (view.IsMine) {
            health -= damage;
            _customProperties["health"] = health;
            PhotonNetwork.LocalPlayer.SetCustomProperties(_customProperties);

            healthbarImage.fillAmount = health / maxHealth;
        }
        //Uœmiercenie gracza je¿eli punkty ¿ycia spadn¹ do 0
        if (health <= 0) {
            PhotonNetwork.Destroy(view);
            if(view.IsMine)
                PhotonNetwork.LoadLevel("Dead");
            _livingTime.Stop();
        }
    }

    /// <summary>
    /// Obs³uga kolizji obiektu gracza z innymi obiektami
    /// </summary>
    /// <param name="collision">Kolizjator obiektu z którym zachodzi kolizja</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        //Kolizja z obiektem kategorii broñ
        if (collision.gameObject.tag.Equals("WeaponSymbol")) {
            pickUpAllowed = true;
            weaponSymbol = collision.gameObject;
        //Kolizcja z obiektem kategorii amunicja
        } else if (collision.gameObject.tag.Equals("AmmoSymbol1") || collision.gameObject.tag.Equals("AmmoSymbol2") || collision.gameObject.tag.Equals("AmmoSymbol3")) {
            ammoSymbol = collision.gameObject;
            if(collision.gameObject.tag.Equals("AmmoSymbol1")) {
                ammoCountNormal += 30;
            } else if(collision.gameObject.tag.Equals("AmmoSymbol2")) {
                ammoCountBouncy += 30;
            } else if(collision.gameObject.tag.Equals("AmmoSymbol3")) {
                ammoCountExplo += 30;
            }
            this.GetComponent<PhotonView>().RPC("destroyAmmoSymbol", RpcTarget.AllBuffered);
            //Kolizja z obiektem kategorii apteczka
        } else if(collision.gameObject.tag.Equals("healthPack")){
            healthpackObject = collision.gameObject;
            if (collision.gameObject.tag.Equals("healthPack") && health < maxHealth)
            {
                float healthOffset = maxHealth - health;
                if (healthOffset < healthpackPlusHealth)
                {
                    health = maxHealth;
                    healthbarImage.fillAmount = health / maxHealth;
                }
                else
                {
                    health += healthpackPlusHealth;
                    healthbarImage.fillAmount = health / maxHealth;
                }
                this.GetComponent<PhotonView>().RPC("destroyHealthpack", RpcTarget.AllBuffered);
            }
        }
    }

    /// <summary>
    /// Obs³uga logiki wyjœcia z pola kolizji, wzbronienie mo¿liwoœci podniesienia przedmiotu
    /// </summary>
    /// <param name="collision">Kolizjator obiektu, z którego obszaru wychodzimy</param>
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("WeaponSymbol")) {
            pickUpAllowed = false;
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC obs³uguj¹ca logikê podnoszenia broni przez gracza
    /// </summary>
    /// <param name="weaponName">Obiekt symbolu broni, który zosta³ podniesiony</param>
    [PunRPC]
    public void equipWeapon(string weaponName) {
        //Wyra¿enie logiczne w przypadku podniesienia pistoletu
        if (weaponName.Contains("pistol")) {
            ak.SetActive(false);
            pistol.SetActive(true);
            shotgun.SetActive(false);
            firePoint = pistolFirePoint;
            bulletsInWeaponMagazine = pistolMagazineSize;
            //Zatrzymanie korutyn je¿eli trwa³oby prze³adowanie broni
            StopCoroutine("reloadAk");
            StopCoroutine("reloadShotgun");
            reloadingNotification.text = "";
            inReload = false;
            //Wyrzucenie aktualnie przechowywanej broni
            dropCurrentWeapon(weaponId);
            //Ustawienie pistoletu jako przechowywanej broni
            weaponId = 1;
            //Zmiana amunicji na podstawow¹ w przypadku zmiany broni
            ammoTypeUsed = Player.bulletType.NORMAL;
            normalAmmoBackground.color = Color.yellow;
            bouncyAmmoBackground.color = Color.white;
            exploAmmoBackground.color = Color.white;

        }
        //Wyra¿enie logiczne w przypadku podniesienia karabinu szturmowego
        else if (weaponName.Contains("ak")) {
            ak.SetActive(true);
            pistol.SetActive(false);
            shotgun.SetActive(false);
            firePoint = akFirePoint;
            bulletsInWeaponMagazine = akMagazineSize;
            //Zatrzymanie korutyn je¿eli trwa³oby prze³adowanie broni
            StopCoroutine("reloadPistol");
            StopCoroutine("reloadShotgun");
            reloadingNotification.text = "";
            inReload = false;
            //Wyrzucenie aktualnie przechowywanej broni
            dropCurrentWeapon(weaponId);
            //Ustawienie karabinu jako przechowywanej broni
            weaponId = 0;
            //Zmiana amunicji na podstawow¹ w przypadku zmiany broni
            ammoTypeUsed = Player.bulletType.NORMAL;
            normalAmmoBackground.color = Color.yellow;
            bouncyAmmoBackground.color = Color.white;
            exploAmmoBackground.color = Color.white;
        }
        //Wyra¿enie logiczne w przypadku podniesienia strzelby
        else if (weaponName.Contains("shotgun"))
        {
            ak.SetActive(false);
            pistol.SetActive(false);
            shotgun.SetActive(true);
            firePoint = shotgunFirePoint;
            bulletsInWeaponMagazine = shotgunMagazineSize;
            //Zatrzymanie korutyn je¿eli trwa³oby prze³adowanie broni
            StopCoroutine("reloadPistol");
            StopCoroutine("reloadAk");
            reloadingNotification.text = "";
            inReload = false;
            //Wyrzucenie aktualnie przechowywanej broni
            dropCurrentWeapon(weaponId);
            //Ustawienie strzelby jako przechowywanej broni
            weaponId = 2;
            //Zmiana amunicji na podstawow¹ w przypadku zmiany broni
            ammoTypeUsed = Player.bulletType.NORMAL;
            normalAmmoBackground.color = Color.yellow;
            bouncyAmmoBackground.color = Color.white;
            exploAmmoBackground.color = Color.white;
        }
    }

    /// <summary>
    /// Metoda obs³uguj¹ca logikê wyrzutu broni z rêki gracza przy podnoszeniu nowej
    /// </summary>
    /// <param name="weaponId">Id aktualnie przetrzymywanej broni</param>
    public void dropCurrentWeapon(int weaponId) {
        if (view.IsMine) {
            if (weaponId == 0)
                PhotonNetwork.Instantiate(akSymbolPrefab.name, this.transform.position, this.transform.rotation);
            else if(weaponId == 1)
                PhotonNetwork.Instantiate(pistolSymbolPrefab.name, this.transform.position, this.transform.rotation);
            else if(weaponId == 2)
                PhotonNetwork.Instantiate(shotgunSymbolPrefab.name, this.transform.position, this.transform.rotation);
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC obs³uguj¹ca wystrza³ z karabinu szturmowego. Tworzy now¹ instacjê obiektu pocisku po wywo³aniu
    /// </summary>
    [PunRPC]
    void ShootAk() {
        if(bulletsInWeaponMagazine != 0) {
            if(ammoTypeUsed == bulletType.NORMAL)
                PhotonNetwork.Instantiate(bulletPrefab.name, akFirePoint.transform.position, akFirePoint.transform.rotation); //Instantiation of a new bullet
            else if(ammoTypeUsed == bulletType.BOUNCY)
                PhotonNetwork.Instantiate(bouncyBulletPrefab.name, akFirePoint.transform.position, akFirePoint.transform.rotation); //Instantiation of a new bullet
            else if(ammoTypeUsed == bulletType.EXPLO)
                PhotonNetwork.Instantiate(exploBulletPrefab.name, akFirePoint.transform.position, akFirePoint.transform.rotation); //Instantiation of a new bullet
            bulletsInWeaponMagazine--;
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC obs³uguj¹ca wystrza³ z pistoletu. Tworzy now¹ instacjê obiektu pocisku po wywo³aniu
    /// </summary>
    [PunRPC]
    void ShootPistol() {
        if(bulletsInWeaponMagazine != 0) {
            if (ammoTypeUsed == bulletType.NORMAL)
                PhotonNetwork.Instantiate(bulletPrefab.name, pistolFirePoint.transform.position, pistolFirePoint.transform.rotation); //Instantiation of a new bullet
            else if (ammoTypeUsed == bulletType.BOUNCY)
                PhotonNetwork.Instantiate(bouncyBulletPrefab.name, pistolFirePoint.transform.position, pistolFirePoint.transform.rotation); //Instantiation of a new bullet
            else if (ammoTypeUsed == bulletType.EXPLO)
                PhotonNetwork.Instantiate(exploBulletPrefab.name, pistolFirePoint.transform.position, pistolFirePoint.transform.rotation); //Instantiation of a new bullet
            bulletsInWeaponMagazine--;
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC obs³uguj¹ca wystrza³ ze strzelby. Tworzy now¹ instacjê obiektu pocisku po wywo³aniu
    /// </summary>
    [PunRPC]
    void ShootShotgun() {
        if(bulletsInWeaponMagazine != 0) {
            //Losowanie rozrzutu dla pocisków
            int shotgunScatteringValueMiddle = Random.Range(0, 5);
            int shotgunScatteringValueRight = Random.Range(15, 40);
            int shotgunScatteringValueLeft = Random.Range(320, 345);
            //Stworzenie instancji pocisku wraz z rotacj¹ powiêkszon¹ o wartoœæ rozrzutu
            if (ammoTypeUsed == bulletType.NORMAL) {
                PhotonNetwork.Instantiate(bulletPrefab.name, shotgunFirePoint.transform.position, shotgunFirePoint.transform.rotation * Quaternion.Euler(0, 0, shotgunScatteringValueMiddle)); //Instantiation of a new bullet
                PhotonNetwork.Instantiate(bulletPrefab.name, shotgunFirePoint.transform.position, shotgunFirePoint.transform.rotation * Quaternion.Euler(0, 0, shotgunScatteringValueRight)); //Instantiation of a new bullet
                PhotonNetwork.Instantiate(bulletPrefab.name, shotgunFirePoint.transform.position, shotgunFirePoint.transform.rotation * Quaternion.Euler(0, 0, shotgunScatteringValueLeft)); //Instantiation of a new bullet
            }
            else if (ammoTypeUsed == bulletType.BOUNCY) { 
                PhotonNetwork.Instantiate(bouncyBulletPrefab.name, shotgunFirePoint.transform.position, shotgunFirePoint.transform.rotation * Quaternion.Euler(0, 0, shotgunScatteringValueMiddle)); //Instantiation of a new bullet
                PhotonNetwork.Instantiate(bouncyBulletPrefab.name, shotgunFirePoint.transform.position, shotgunFirePoint.transform.rotation * Quaternion.Euler(0, 0, shotgunScatteringValueRight)); //Instantiation of a new bullet
                PhotonNetwork.Instantiate(bouncyBulletPrefab.name, shotgunFirePoint.transform.position, shotgunFirePoint.transform.rotation * Quaternion.Euler(0, 0, shotgunScatteringValueLeft)); //Instantiation of a new bullet
            }
            else if (ammoTypeUsed == bulletType.EXPLO) {
                PhotonNetwork.Instantiate(exploBulletPrefab.name, shotgunFirePoint.transform.position, shotgunFirePoint.transform.rotation * Quaternion.Euler(0, 0, shotgunScatteringValueMiddle)); //Instantiation of a new bullet
                PhotonNetwork.Instantiate(exploBulletPrefab.name, shotgunFirePoint.transform.position, shotgunFirePoint.transform.rotation * Quaternion.Euler(0,0, shotgunScatteringValueRight)); //Instantiation of a new bullet
                PhotonNetwork.Instantiate(exploBulletPrefab.name, shotgunFirePoint.transform.position, shotgunFirePoint.transform.rotation * Quaternion.Euler(0, 0, shotgunScatteringValueLeft)); //Instantiation of a new bullet
            }
            bulletsInWeaponMagazine--;
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC obs³uguj¹ca destrukcjê symbolu broni
    /// </summary>
    [PunRPC]
    public void destroyWeaponSymbol() {
        Destroy(weaponSymbol);
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC obs³uguj¹ca destrukcjê symbolu amunicji
    /// </summary>
    [PunRPC]
    public void destroyAmmoSymbol() {
        Destroy(ammoSymbol);
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC obs³uguj¹ca destrukcjê symbolu pakietu apteczki
    /// </summary>
    [PunRPC]
    public void destroyHealthpack(){
        Destroy(healthpackObject);
    }

}
