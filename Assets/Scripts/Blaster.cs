﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Blaster : MonoBehaviour {
    public Transform spawnPosition;
    [Tooltip("The rigidbody of the sphere that drives the cart")]
    public Rigidbody cartSphereRB;
    private AudioSource audioSource;

    [Header("Primary weapon")]
    public GameObject primaryProjectilePrefab;
    public float primaryProjectileSpeed;
    public float primaryProjectileLifespan;
    public AudioClip primaryProjectileFiredAudio;
    public AudioClip primaryProjectileFireFailAudio;

    // Cooldown
    public float primaryCooldown;
    private float primaryLastShotFiredTime;

    [Tooltip("The amount of force applied to the cart when fired, relative to the blaster's transform")]
    public Vector3 primaryProjectileCartForce;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        // Update cooldowns
        if (primaryLastShotFiredTime < primaryCooldown) {
            primaryLastShotFiredTime += Time.deltaTime;
        }

        // Don't process input if the player's dead
        if (Player.instance.damagable.isDead) return;

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (primaryLastShotFiredTime < primaryCooldown) {
                // Still on cooldown. Play failing audio
                audioSource.PlayOneShot(primaryProjectileFireFailAudio);
            } else {
                // Fire projectile
                // Reset cooldown
                primaryLastShotFiredTime = 0f;

                audioSource.PlayOneShot(primaryProjectileFiredAudio);

                // Spawn a new projectile
                GameObject projectile = Instantiate(primaryProjectilePrefab, spawnPosition.position, spawnPosition.rotation);
                Rigidbody projectile_rb = projectile.GetComponent<Rigidbody>();

                // Add forward force, and destroy it after some time
                projectile_rb.AddForce(spawnPosition.forward * primaryProjectileSpeed, ForceMode.Impulse);
                StartCoroutine(DestroyAfterLifespan(primaryProjectileLifespan, projectile));

                // Add force to the cart, relative to the blaster's position
                cartSphereRB.AddForce(spawnPosition.TransformDirection(primaryProjectileCartForce), ForceMode.Impulse);
            }
        }
    }

    // Destroy the target gameobject after some time, if it's still in the scene
    // Otherwise, it must have collided with somethign
    private IEnumerator DestroyAfterLifespan(float lifespan, GameObject target) {
        yield return new WaitForSeconds(lifespan);

        if (target != null) {
            Destroy(target);
        }
    }
}
