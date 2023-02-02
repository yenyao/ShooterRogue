using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject gunPosition;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadRate;
    [SerializeField] private float accuracy;
    [SerializeField] private int maxMagazineAmmo = 10;
    [SerializeField] private int totalAmmo = 50;
    [SerializeField] private float bulletForce = 20f;
    private bool _canFire = true;
    private bool _isReloading = false;
    private float firedTime;
    private float reloadTime;
    private int currentAmmo;
    private bool isEquipabble = true;
    private float _accuracyMod;
    public float accuracyMod {
        get { return _accuracyMod; }
        set { _accuracyMod = value; }
    }
    private UIManager _uimanager;
    void Start() {
        currentAmmo = maxMagazineAmmo;
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        print(currentAmmo);
        _uimanager.updateAmmo(currentAmmo, totalAmmo, _isReloading, isEquipabble);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Player" && isEquipabble) {
            equip();
            _canFire = true;
        }
    }

    public void updateAmmoUI(bool isGunEquipped) {
        _uimanager.updateAmmo(currentAmmo, totalAmmo, _isReloading, isGunEquipped);
    }

    public void equip() {
        Destroy(GetComponent<BoxCollider2D>());
        _canFire = true;
        _isReloading = false;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, _isReloading, true);
        gameObject.tag = "equippedGun";
        Vector3 angleOffset = new Vector3(0, 0, 0);
        isEquipabble = false;
        _uimanager.updateInteraction(false);
        gameObject.transform.SetParent(gunPosition.transform);
        gameObject.transform.position = gunPosition.transform.position;
        transform.eulerAngles = transform.parent.eulerAngles + angleOffset;
        // playerController.setIsGunEquipped(true);
        playerController.isGunEquipped = true;
    }

    public IEnumerator shoot() {
        if(currentAmmo >= 1) {
            Vector3 gunAccuracy = new Vector3(0, Random.Range(-accuracy/2, accuracy/2), 0) * accuracyMod;
            print(accuracyMod);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.AddForce(firePoint.up * bulletForce + gunAccuracy, ForceMode2D.Impulse);
            currentAmmo--;
            _canFire = false;
            _uimanager.updateAmmo(currentAmmo, totalAmmo, _isReloading, true);
            yield return new WaitForSeconds(fireRate);
            _canFire = true;
        } else {
            StartCoroutine(reload());
        }
    }

    public IEnumerator reload() {
        _isReloading = true;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, _isReloading, true);
        _uimanager.setProgressBarTimer(reloadRate);
        yield return new WaitForSeconds(reloadRate);
        var diffAmmo = maxMagazineAmmo - currentAmmo;
        totalAmmo -= diffAmmo;
        currentAmmo = maxMagazineAmmo;
        _isReloading = false;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, _isReloading, true);
        _canFire = true;
    }
    public IEnumerator throwGun() {
        _canFire = false;
        _isReloading = false;
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
        // playerController.setIsGunEquipped(false);
        // playerController.isGunEquipped = false;
        yield return new WaitForSeconds(1);
        gunCollider.isTrigger = true;
        isEquipabble = true;
        gunRB.velocity = new Vector2(0, 0);
        Destroy(GetComponent<Rigidbody2D>());
        gameObject.GetComponent<BoxCollider2D>().sharedMaterial = null;
    }
    public bool getCanFire() {
        return _canFire;
    }
    public bool getIsReloading() {
        return _isReloading;
    }
    public bool getIsEquippable() {
        return isEquipabble;
    }
}
