using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunInteraction : MonoBehaviour
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
    void Update() {
        handleFireRate();
        if(currentAmmo <= 0) canFire = false;
        if(Input.GetButtonDown("Fire1") && canFire && !isReloading) {
            shoot();
        }
        if(Input.GetButtonDown("Reload") && !isReloading) {
            isReloading = true;
            canFire = false;
            _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading);
            StartCoroutine(reload());
        }
    }
    void handleFireRate() {
        firedTime += Time.deltaTime;
        float timeToFire = 1 / fireRate;
        if(firedTime >= timeToFire) canFire = true;
    }

    void shoot() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        currentAmmo--;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading);
        canFire = false;
        firedTime = 0;
    }

    IEnumerator reload() {
        yield return new WaitForSeconds(reloadRate);
        var diffAmmo = maxMagazineAmmo - currentAmmo;
        totalAmmo -= diffAmmo;
        currentAmmo = maxMagazineAmmo;
        isReloading = false;
        _uimanager.updateAmmo(currentAmmo, totalAmmo, isReloading);
        canFire = true;
    }
}
