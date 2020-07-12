using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class GroundChaserEnemy : MonoBehaviour {
    public float speed;
    private Rigidbody rb;
    private AudioSource audioSource;
    public AudioClip explosionWarningAudio;
    public AudioClip explosionAudio;

    private bool isExploding;
    public VisualEffect explosionEffect;
    public float explosionEffectLifespan;

    [Header("Explosion by radius")]
    public float explosionTriggerRadius;
    public float explosionDelay;

    [Header("Force")]
    public float explosionForce;
    public float explosionRadius;
    public float explosionUpForce;

    [Header("Damage")]
    public int explosionDamageRadius;
    public int explosionDamage;
    public Damageable damageable;

    // Big lazy
    [Header("Disabled on Explosion")]
    public List<Renderer> renderersToDisable;
    public List<Collider> collidersToDisable;
    public List<GameObject> gameobjectsToDisable;



    private void Start() {
        rb = GetComponent<Rigidbody>();
        isExploding = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        // Move along the x/y plane in the direction of the player
        Vector3 dirToPlayer = Player.instance.catTransform.position - transform.position;
        Vector3 movementDir = Vector3.ProjectOnPlane(dirToPlayer.normalized, Vector3.up);

        rb.AddForce(movementDir * speed, ForceMode.Force);

        // If the enemy is close enough to the player, explode after a delay
        if (!isExploding && dirToPlayer.magnitude < explosionTriggerRadius) {
            ExplodeAfterDelay();
        }
    }

    public void Explode() {
        // Avoid multiple explostions
        if (isExploding) return;
        isExploding = true;

        // Push the player away
        //Player.instance.cartSphereRB.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpForce);
        //Player.instance.cart.rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpForce);
        Player.instance.cart.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpForce);

        // Calculate damage to the player
        float distanceToPlayer = Mathf.Abs((transform.position - Player.instance.catTransform.position).magnitude);
        if (distanceToPlayer < explosionDamageRadius) {
            Player.instance.damagable.TakeDamage(explosionDamage);
        }

        // Play effect, disable mesh
        explosionEffect.enabled = true;
        explosionEffect.Play();

        // Play explosion audio
        audioSource.Stop();
        audioSource.PlayOneShot(explosionAudio);

        // Disable the enemy
        // This should be refactored, lots of room for error
        foreach (Renderer r in renderersToDisable) {
            r.enabled = false;
        }
        foreach (Collider c in collidersToDisable) {
            c.enabled = false;
        }
        foreach (GameObject g in gameobjectsToDisable) {
            g.SetActive(false);
        }
        rb.isKinematic = true;

        damageable.onHealthDepleted.Invoke();

        // Destroy the prefab after the explosion
        //StartCoroutine(DestroyAfterExplosionLifespan());
    }

    private IEnumerator _ExplodeAfterDelay() {
        yield return new WaitForSeconds(explosionDelay);
        if (gameObject != null) {
            Explode();
        }
    }
    public void ExplodeAfterDelay() {
        // Play explosion warning audio
        audioSource.PlayOneShot(explosionWarningAudio);

        StartCoroutine(_ExplodeAfterDelay());
    }
    //private IEnumerator DestroyAfterExplosionLifespan() {
    //    yield return new WaitForSeconds(explosionEffectLifespan);
    //    Destroy(transform.parent.gameObject);
    //}
}
