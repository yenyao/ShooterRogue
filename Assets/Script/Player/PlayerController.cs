using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float throwStrength;
    private Vector2 playerMovement;
    private Vector2 mousePos;
    private GameObject equippedGun;
    private GameObject gun;
    private GunController gunController;
    private bool isGunEquipped;
    private bool canGunFire;
    private bool isGunReloading;
    private bool isEquipabble;
    private Coroutine shootRoutine;
    private Coroutine reloadRoutine;
    private string interactableName;
    private UIManager _uimanager;

    void Start() {
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        shootRoutine = null;
        reloadRoutine = null;
        // if(GameObject.FindGameObjectWithTag("equippedGun")) {
        //     equippedGun = GameObject.FindGameObjectWithTag("equippedGun");
        //     gunController = equippedGun.GetComponent<GunController>();
        // }
        // if(GameObject.FindGameObjectWithTag("Gun")) {
        //     gun = GameObject.FindGameObjectWithTag("Gun");
        //     gunController = gun.GetComponent<GunController>();
        // }
        Transform gunPosition = gameObject.transform.Find("GunPosition");
        if(gunPosition.childCount > 0) {
            gun = gunPosition.GetChild(0).gameObject;
            // equippedGun = gun;
            gunController = gun.GetComponent<GunController>();
        }
        isGunEquipped = true;
        // if(!gameObject.transform.Find("GunPosition").GetChild(0).gameObject) {
        //     print("hell");
        //     gun = GameObject.FindGameObjectWithTag("Gun");
        //     gunController = gun.GetComponent<GunController>();
        // }
        // if(equippedGun) isGunEquipped = true;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        playerMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        print(playerMovement);
        canGunFire = isGunEquipped && gunController.getCanFire();
        // print(canGunFire);
        isGunReloading = isGunEquipped && gunController.getIsReloading();
        isEquipabble = gunController.getIsEquippable();
        _uimanager.updateStates(canGunFire, isGunReloading, isEquipabble);
        // print(!isGunReloading);
        if(Input.GetButtonDown("Fire1") && canGunFire && !isGunReloading) {
            shootRoutine = StartCoroutine(gunController.shoot());
        }
        if(Input.GetButtonDown("Reload") && !isGunReloading) {
            reloadRoutine = StartCoroutine(gunController.reload());
        }
        if(Input.GetButtonDown("Throw")) {
            if(gunController.getIsReloading()) {
                StopCoroutine(reloadRoutine);
            }
            StartCoroutine(gunController.throwGun());
            gun = equippedGun;
            equippedGun = null;
        }
    }
    void FixedUpdate() {
        handleAim();
        handleMovement();
    }

    private void handleMovement() {
        rb.MovePosition(rb.position + playerMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void handleAim() {
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    public void setInteractableName(string tag) {
        interactableName = tag;
    }
    public void setIsGunEquipped(bool isGunEquipped) {
        this.isGunEquipped = isGunEquipped;
    }
}
