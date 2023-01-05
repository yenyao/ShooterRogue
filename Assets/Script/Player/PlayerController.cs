using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cam;
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

    void Start() {
        if(GameObject.FindGameObjectWithTag("equippedGun")) {
            equippedGun = GameObject.FindGameObjectWithTag("equippedGun");
            gunController = equippedGun.GetComponent<GunController>();
        }
        if(GameObject.FindGameObjectWithTag("Gun")) {
            gun = GameObject.FindGameObjectWithTag("Gun");
            gunController = gun.GetComponent<GunController>();
        }
        if(equippedGun) isGunEquipped = true;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        playerMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        canGunFire = isGunEquipped && gunController.getCanFire();
        isGunReloading = isGunEquipped && !gunController.getIsReloading();
        isEquipabble = gunController.getIsEquippable();
        if(Input.GetButtonDown("Fire1") && canGunFire && isGunReloading) {
            StartCoroutine(gunController.shoot());
        }
        if(Input.GetButtonDown("Reload") && isGunReloading) {
            StartCoroutine(gunController.reload());
        }
        if(Input.GetButtonDown("Throw")) {
            StartCoroutine(gunController.throwGun());
            equippedGun.tag = "Gun";
            isGunEquipped = false;
            gun = equippedGun;
            equippedGun = null;
        }
        if(Input.GetButtonDown("Equip") && isEquipabble) {
            gunController.equip();
            isGunEquipped = true;
            equippedGun = gun;
            gun.tag = "equippedGun";
            gun = null;
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
}
