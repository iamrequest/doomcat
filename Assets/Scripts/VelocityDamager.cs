using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityDamager : MonoBehaviour {
    public float minVelocityToDamage;
    public int damage;
    public LayerMask damageableLayers;

    public void OnCollisionEnter(Collision collision) {
        // Don't bother processing layers that we won't damage
        if ( ((1<<collision.collider.gameObject.layer) & damageableLayers) == 0) {
            return;
        }

        if (collision.collider.TryGetComponent(out Damageable damageable)) {
            // Debug.Log(collision.collider + ":" + collision.impulse.magnitude / Time.fixedDeltaTime);
            float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;

            if (collisionForce > minVelocityToDamage) {
                damageable.TakeDamage(damage);
            }
        }
    }
}
