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
    private int _gunIndex;
    private int _numGuns;
    private bool _isGunEquipped;
    public bool isGunEquipped {
        get { return _isGunEquipped; }
        set { _isGunEquipped = value; }
    }
    private float accuracyMod;
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
        _numGuns = gunPosition.childCount - 1;
        _gunIndex = 0;
        _isGunEquipped = true;
        accessChosenGun(_gunIndex);
        accuracyMod = 1;
        gunController.accuracyMod = this.accuracyMod;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        playerMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _uimanager.updateStates(gunController.getCanFire(), gunController.getIsReloading(), gunController.getIsEquippable(), _numGuns, gunController.name);
        if(_isGunEquipped) {
            if(Input.GetButton("Fire1") && gunController.getCanFire() && !gunController.getIsReloading()) {
                shootRoutine = StartCoroutine(gunController.shoot());
            }
            if(Input.GetButtonDown("Reload") && !gunController.getIsReloading()) {
                reloadRoutine = StartCoroutine(gunController.reload());
            }
            if(Input.GetButtonDown("Throw")) {
                if(gunController.getIsReloading()) {
                    StopCoroutine(reloadRoutine);
                }
                throwCurrentGun();
            }
            if(Input.GetAxisRaw("Mouse ScrollWheel") > 0) {
                nextWeapon();
            }
            if(Input.GetAxisRaw("Mouse ScrollWheel") < 0) {
                lastWeapon();
            }
        }
        
    }

    void FixedUpdate() {
        handleAim();
        handleMovement();
    }

    void nextWeapon() {
        _numGuns = gunPosition.childCount - 1;
        if(_gunIndex == _numGuns) {
            _gunIndex = 0;
        } else {
            _gunIndex++;
        }
        accessChosenGun(_gunIndex);
    }
    void lastWeapon() {
        _numGuns = gunPosition.childCount - 1;
        if(_gunIndex == 0) {
            _gunIndex = _numGuns;
        } else {
            _gunIndex--;
        }
        accessChosenGun(_gunIndex);
    }

    private void throwCurrentGun() {
        StartCoroutine(gunController.throwGun());
        _numGuns = gunPosition.childCount - 1;
        accessChosenGun(_gunIndex);
        gun = equippedGun;
        equippedGun = null;
    }

    private void handleMovement() {
        rb.MovePosition(rb.position + playerMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void handleAim() {
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    private void accessChosenGun(int _gunIndex)
    {
        if (gunPosition.childCount > 0)
        {
            if(_gunIndex > _numGuns) _gunIndex = _numGuns;
            gun = gunPosition.GetChild(_gunIndex).gameObject;
            gunController = gun.GetComponent<GunController>();
        } else {
            _isGunEquipped = false;
        }
        gunController.updateAmmoUI(_isGunEquipped);
    }

    public void setInteractableName(string tag) {
        interactableName = tag;
    }
    public void set_IsGunEquipped(bool _isGunEquipped) {
        this._isGunEquipped = _isGunEquipped;
    }
}
