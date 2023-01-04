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
    private GunController gunController;
    void Start() {
        equippedGun = GameObject.FindGameObjectWithTag("Gun");
        gunController = equippedGun.GetComponent<GunController>();
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        playerMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(Input.GetButtonDown("Fire1") && gunController.getCanFire() && !gunController.getIsReloading()) {
            StartCoroutine(gunController.shoot());
        }
        if(Input.GetButtonDown("Reload") && !gunController.getIsReloading()) {
            StartCoroutine(gunController.reload());
        }
        if(Input.GetButtonDown("Throw")) {
            gunController.throwGun();
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
