using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunInteraction : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private bool canFire = true;
    public float fireRate;
    private float time;

    public float bulletForce = 20f;
    void Update() {
        handleFireRate();
        if(Input.GetButtonDown("Fire1") && canFire) {
            Shoot();
        }
    }
    void handleFireRate() {
        time += Time.deltaTime;
        float timeToFire = 1 / fireRate;
        if(time >= timeToFire) canFire = true;
    }
    void Shoot() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        canFire = false;
        time = 0;
    }
}
