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
    private int gunIndex;
    private int numGuns;
    private bool isGunEquipped;
    private bool canGunFire;
    private bool isGunReloading;
    private bool isEquipabble;
    private Transform gunPosition;
    private Coroutine shootRoutine;
    private Coroutine reloadRoutine;
    private string interactableName;
    private UIManager _uimanager;

    void Start()
    {
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        shootRoutine = null;
        reloadRoutine = null;
        gunPosition = transform.Find("GunPosition");
        numGuns = gunPosition.childCount - 1;
        gunIndex = 0;
        accessChosenGun(gunIndex);
        isGunEquipped = true;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        playerMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        canGunFire = isGunEquipped && gunController.getCanFire();
        isGunReloading = isGunEquipped && gunController.getIsReloading();
        isEquipabble = gunController.getIsEquippable();
        _uimanager.updateStates(canGunFire, isGunReloading, isEquipabble, numGuns, gunController.name);
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
            numGuns = gunPosition.childCount - 1;
            gun = equippedGun;
            equippedGun = null;
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0) {
            nextWeapon();
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") < 0) {
            lastWeapon();
        }
    }

    void nextWeapon() {
        numGuns = gunPosition.childCount - 1;
        if(gunIndex == numGuns) {
            gunIndex = 0;
        } else {
            gunIndex++;
        }
        accessChosenGun(gunIndex);
        print("gunIndex: " + gunIndex + " : " + numGuns);
    }
    void lastWeapon() {
        numGuns = gunPosition.childCount - 1;
        if(gunIndex == 0) {
            gunIndex = numGuns;
        } else {
            gunIndex--;
        }
        accessChosenGun(gunIndex);
        print("gunIndex: " + gunIndex + " : " + numGuns);
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

    private void accessChosenGun(int gunIndex)
    {
        if (gunPosition.childCount > 0)
        {
            gun = gunPosition.GetChild(gunIndex).gameObject;
            gunController = gun.GetComponent<GunController>();
        }
    }

    public void setInteractableName(string tag) {
        interactableName = tag;
    }
    public void setIsGunEquipped(bool isGunEquipped) {
        this.isGunEquipped = isGunEquipped;
    }
}
