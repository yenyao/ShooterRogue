using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    
    [SerializeField] private float damage;
    void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }

    public float getDamage() {
        return damage;
    }
}
