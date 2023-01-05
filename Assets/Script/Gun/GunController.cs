using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject playerPrefab;
    private bool canFire = true;
    private bool isReloading = false;
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadRate;
    private float firedTime;
    private float reloadTime;
    private int currentAmmo;
    private bool isEquipabble = false;
    [SerializeField] private int maxMagazineAmmo = 10;
    [SerializeField] private int totalAmmo = 50;
    [SerializeField] private float bulletForce = 20f;
    private UIManager _uimanager;
    void Start() {
        currentAmmo = maxMagazineAmmo;
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading, true);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        _uimanager.updateInteraction(true);
        isEquipabble = true;
    }
    void OnTriggerExit2D(Collider2D collider) {
        _uimanager.updateInteraction(false);
        isEquipabble = false;
    }

    public void equip() {
        Vector3 angleOffset = new Vector3(0, 0, -90);
        isEquipabble = false;
        _uimanager.updateInteraction(false);
        Destroy(GetComponent<BoxCollider2D>());
        
        gameObject.transform.SetParent(playerPrefab.transform);
        gameObject.transform.position = playerPrefab.transform.position;
        transform.eulerAngles = gameObject.transform.parent.eulerAngles + angleOffset;
    }

    public IEnumerator shoot() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        currentAmmo--;
        canFire = false;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading, true);
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    public IEnumerator reload() {
        isReloading = true;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading, true);
        yield return new WaitForSeconds(reloadRate);
        var diffAmmo = maxMagazineAmmo - currentAmmo;
        totalAmmo -= diffAmmo;
        currentAmmo = maxMagazineAmmo;
        isReloading = false;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading, true);
        canFire = true;
    }
    public IEnumerator throwGun() {
        transform.parent = null;
        Rigidbody2D gunRB = gameObject.AddComponent<Rigidbody2D>();
        gunRB.gravityScale = 0;
        gunRB.drag = 5;
        gunRB.angularDrag = 10;
        BoxCollider2D gunCollider = gameObject.AddComponent<BoxCollider2D>();
        PhysicsMaterial2D mat = Resources.Load("Bouncy", typeof(PhysicsMaterial2D)) as PhysicsMaterial2D;
        gameObject.GetComponent<BoxCollider2D>().sharedMaterial = mat;
        gunRB.velocity = firePoint.up * bulletForce;
        _uimanager.updateAmmo(0, 0, false, false);
        yield return new WaitForSeconds(1);
        gunCollider.isTrigger = true;
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
