using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityDamager : MonoBehaviour {
    public float minVelocityToDamage;
    public int damage;

    public void OnCollisionEnter(Collision collision) {
        if (collision.collider.TryGetComponent(out Damageable damageable)) {
            // Debug.Log(collision.collider + ":" + collision.impulse.magnitude / Time.fixedDeltaTime);
            float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;

            if (collisionForce > minVelocityToDamage) {
                damageable.TakeDamage(damage);
            }
        }
    }
}
