using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour {
    public VisualEffect explosionEffect;
    private Rigidbody rb;
    private Collider projectilecollider;
    private Renderer projectilerenderer;
    private AudioSource audioSource;

    public bool doSplashDamage;
    public float splashDamageRadius;
    public int splashDamage;
    public LayerMask splashDamageLayerMask;

    private void Awake () {
        rb = GetComponent<Rigidbody>();
        projectilecollider = GetComponent<Collider>();
        projectilerenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        audioSource.loop = false;
    }

    public void OnCollisionEnter(Collision collision) {
        // Calculate damage
        if (doSplashDamage) {
            Collider[] collisions = Physics.OverlapSphere(transform.position, splashDamageRadius, splashDamageLayerMask);
            foreach (Collider c in collisions) {
                if (c.TryGetComponent(out Damageable damageable)) {
                    damageable.TakeDamage(splashDamage);
                }
            }
        }

        projectilecollider.enabled = false;
        rb.isKinematic = true;
        projectilerenderer.enabled = false;

        explosionEffect.enabled = true;
        explosionEffect.Play();
        audioSource.Play();
    }
}
