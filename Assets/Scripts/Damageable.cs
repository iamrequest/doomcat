using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Damageable : MonoBehaviour {
    public Slider healthbar;
    public int health, totalHealth;
    public UnityEvent onDamaged, onHealthDepleted;

    public float invincibilityFrames;
    private float timeSinceLastDamaged;
    public bool isInvincible;
    public bool isDead {
        get {
            return health <= 0f;
        }
    }

    public void Start() {
        health = totalHealth;

        if (healthbar != null) {
            healthbar.minValue = 0;
            healthbar.maxValue = totalHealth;
            healthbar.value = health;
            healthbar.wholeNumbers = true;
        }
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

        if (healthbar != null) {
            healthbar.value = health;
        }

        // Invincibility frames
        timeSinceLastDamaged = 0f;
        isInvincible = true;


        onDamaged.Invoke();

        if (health <= 0) {
            onHealthDepleted.Invoke();
        }
    }
}
