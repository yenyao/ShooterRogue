using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    private bool canFire = true;
    private bool isReloading = false;
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadRate;
    private float firedTime;
    private float reloadTime;
    private int currentAmmo;
    [SerializeField] private int maxMagazineAmmo = 10;
    [SerializeField] private int totalAmmo = 50;
    [SerializeField] private float bulletForce = 20f;
    private UIManager _uimanager;
    void Start() {
        currentAmmo = maxMagazineAmmo;
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading);
    }

    public IEnumerator shoot() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        currentAmmo--;
        canFire = false;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading);
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    public IEnumerator reload() {
        isReloading = true;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading);
        yield return new WaitForSeconds(reloadRate);
        var diffAmmo = maxMagazineAmmo - currentAmmo;
        totalAmmo -= diffAmmo;
        currentAmmo = maxMagazineAmmo;
        isReloading = false;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading);
        canFire = true;
    }
    public void throwGun() {
        Rigidbody2D gunRB = gameObject.AddComponent<Rigidbody2D>();
        gunRB.gravityScale = 0;
        gunRB.drag = 5;
        gunRB.angularDrag = 0;
        BoxCollider2D gunCollider = gameObject.AddComponent<BoxCollider2D>();
        PhysicsMaterial2D mat = Resources.Load("Bouncy", typeof(PhysicsMaterial2D)) as PhysicsMaterial2D;
        gunCollider.sharedMaterial = mat;
        gunRB.velocity = firePoint.up * bulletForce;
    }

    public bool getCanFire() {
        return canFire;
    }
    public bool getIsReloading() {
        return isReloading;
    }
}
