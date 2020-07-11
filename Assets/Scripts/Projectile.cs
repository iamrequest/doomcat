using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour {
    public VisualEffect explosionEffect;
    private Rigidbody rb;
    private Collider collider;
    private Renderer renderer;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }

    public void OnCollisionEnter(Collision collision) {
        collider.enabled = false;
        rb.isKinematic = true;
        renderer.enabled = false;

        explosionEffect.enabled = true;
        explosionEffect.Play();
    }
}
