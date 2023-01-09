using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject gunPosition;
    [SerializeField] private PlayerController playerController;
    private bool canFire = true;
    private bool isReloading = false;
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadRate;
    private float firedTime;
    private float reloadTime;
    private int currentAmmo;
    private bool isEquipabble = true;
    [SerializeField] private int maxMagazineAmmo = 10;
    [SerializeField] private int totalAmmo = 50;
    [SerializeField] private float bulletForce = 20f;
    private UIManager _uimanager;
    void Start() {
        currentAmmo = maxMagazineAmmo;
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading, isEquipabble);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Player" && isEquipabble) {
            equip();
            canFire = true;
        }
    }
    public void equip() {
        Destroy(GetComponent<BoxCollider2D>());
        canFire = true;
        isReloading = false;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading, true);
        gameObject.tag = "equippedGun";
        Vector3 angleOffset = new Vector3(0, 0, -90);
        isEquipabble = false;
        _uimanager.updateInteraction(false);
        gameObject.transform.SetParent(gunPosition.transform);
        gameObject.transform.position = gunPosition.transform.position;
        transform.eulerAngles = transform.parent.eulerAngles + angleOffset;
        playerController.setIsGunEquipped(true);
    }

    public IEnumerator shoot() {
        if(currentAmmo >= 1) {
            print("shoot");
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            currentAmmo--;
            canFire = false;
            _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading, true);
            yield return new WaitForSeconds(fireRate);
            canFire = true;
        } else {
            print("reload");
            StartCoroutine(reload());
        }
    }

    public IEnumerator reload() {
        isReloading = true;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading, true);
        _uimanager.setProgressBarTimer(reloadRate);
        yield return new WaitForSeconds(reloadRate);
        var diffAmmo = maxMagazineAmmo - currentAmmo;
        totalAmmo -= diffAmmo;
        currentAmmo = maxMagazineAmmo;
        isReloading = false;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading, true);
        canFire = true;
    }
    public IEnumerator throwGun() {
        canFire = false;
        isReloading = false;
        isEquipabble = false;
        transform.parent = null;
        gameObject.tag = "Gun";
        Rigidbody2D gunRB = gameObject.AddComponent<Rigidbody2D>();
        gunRB.gravityScale = 0;
        gunRB.drag = 5;
        gunRB.angularDrag = 10;
        BoxCollider2D gunCollider = gameObject.AddComponent<BoxCollider2D>();
        PhysicsMaterial2D mat = Resources.Load("Bouncy", typeof(PhysicsMaterial2D)) as PhysicsMaterial2D;
        gameObject.GetComponent<BoxCollider2D>().sharedMaterial = mat;
        gunRB.velocity = firePoint.up * bulletForce;
        playerController.setIsGunEquipped(false);
        _uimanager.updateAmmo(0, 0, false, false);
        yield return new WaitForSeconds(1);
        gunCollider.isTrigger = true;
        isEquipabble = true;
        gunRB.velocity = new Vector2(0, 0);
        Destroy(GetComponent<Rigidbody2D>());
        gameObject.GetComponent<BoxCollider2D>().sharedMaterial = null;
    }

    public bool getCanFire() {
        return canFire;
    }
    public bool getIsReloading() {
        return isReloading;
    }
    public bool getIsEquippable() {
        return isEquipabble;
    }
}
