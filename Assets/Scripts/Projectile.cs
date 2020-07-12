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

    private void Awake () {
        rb = GetComponent<Rigidbody>();
        projectilecollider = GetComponent<Collider>();
        projectilerenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        audioSource.loop = false;
    }

    public void OnCollisionEnter(Collision collision) {
        projectilecollider.enabled = false;
        rb.isKinematic = true;
        projectilerenderer.enabled = false;

        explosionEffect.enabled = true;
        explosionEffect.Play();
        audioSource.Play();
    }
}
