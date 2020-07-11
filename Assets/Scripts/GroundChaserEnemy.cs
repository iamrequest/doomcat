using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GroundChaserEnemy : MonoBehaviour {
    public float speed;
    private Rigidbody rb;

    [Header("Explosion")]
    public float explosionForce;
    public float explosionRadius;
    public float explosionUpForce;
    public int explosionDamageRadius;
    public int explosionDamage;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            Explode();
        }
    }

    private void FixedUpdate() {
        // Move along the x/y plane in the direction of the player
        Vector3 dirToPlayer = Player.instance.catTransform.position - transform.position;
        Vector3 movementDir = Vector3.ProjectOnPlane(dirToPlayer.normalized, Vector3.up);

        rb.AddForce(movementDir * speed, ForceMode.Force);
    }

    public void Explode() {
        Player.instance.cartSphereRB.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpForce);

        // Calculate damage to the player
        float distanceToPlayer = Mathf.Abs((transform.position - Player.instance.catTransform.position).magnitude);

        if (distanceToPlayer < explosionDamageRadius) {
            Player.instance.damagable.TakeDamage(explosionDamage);
        }
    }

    private IEnumerator _ExplodeAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        if (gameObject != null) {
            Explode();
        }
    }
    public void ExplodeAfterDelay(float delay) {
        StartCoroutine(_ExplodeAfterDelay(delay));
    }
}
