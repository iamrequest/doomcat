using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour {
    public int health, totalHealth;
    public UnityEvent onDamaged, onHealthDepleted;

    public float invincibilityFrames;
    private float timeSinceLastDamaged;
    public bool isInvincible;

    public void Start() {
        health = totalHealth;
    }

    private void Update() {
        if (isInvincible) {
            timeSinceLastDamaged += Time.deltaTime;
        }

        if (timeSinceLastDamaged >= invincibilityFrames) {
            isInvincible = false;
        }
    }

    public virtual void TakeDamage(int damage) {
        if (isInvincible) return;

        health -= damage;
        timeSinceLastDamaged = 0f;
        isInvincible = true;
        onDamaged.Invoke();

        if (health <= 0) {
            onHealthDepleted.Invoke();
        }
    }
}
