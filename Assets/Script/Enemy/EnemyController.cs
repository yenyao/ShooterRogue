using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float knockbackStrength;
    private Rigidbody2D rb;
    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(health <= 0) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Gun") {
            knockback(collision.gameObject.transform.position);
        }
        if(collision.gameObject.tag == "Bullet") {
            float damage = collision.gameObject.GetComponent<bullet>().getDamage();
            health -= damage;
            // knockback(collision.gameObject.transform.position);
        }
    }

    void knockback(Vector2 extObjectPos) {
        Vector2 dir = ((Vector2)transform.position - extObjectPos).normalized;
        rb.AddForce(dir * knockbackStrength, ForceMode2D.Impulse);
    }
}