using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class LargeDemonEnemy : MonoBehaviour {
    public float speed;

    [Range(0f, 1f)]
    public float turnSpeed;
    private Rigidbody rb;
    private AudioSource audioSource;
    public Transform lookatTargetTransform;

    [Header("Attacking")]
    public float attackCooldown;
    private float timeSinceLastAttack;

    public float fireballSpeed;
    public float fireballLifespan;
    public float attackRadius;
    public Transform attackSpawnTransform;
    public GameObject fireballPrefab;

    [Header("Death Management")]
    public float disappearTime;
    private bool isDying;
    public AudioClip deathAudio;
    public List<Collider> colliders;
    public GameObject ui;
    public VisualEffect deathVFX;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        if (isDying) return;

        Vector3 dirToPlayer = Player.instance.catTransform.position - transform.position;
        Vector3 movementDir = Vector3.ProjectOnPlane(dirToPlayer.normalized, Vector3.up);

        // -- Rotate to face player
        float angle = Mathf.Atan2(movementDir.x, movementDir.z) * Mathf.Rad2Deg;
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.Euler(0f,angle, 0f), turnSpeed));


        // -- Movement
        // Move along the x/y plane in the direction of the player
        rb.AddForce(movementDir * speed, ForceMode.Force);

        // -- Look at player (ie: move head)
        // Move the anim rig lookat target gameobject to the player's position
        lookatTargetTransform.position = Player.instance.catTransform.position;


        // -- Attack logic
        if (timeSinceLastAttack < attackCooldown) {
            timeSinceLastAttack += Time.deltaTime;
        }

        // If the enemy is close enough to the player, explode after a delay
        if (dirToPlayer.magnitude < attackRadius && timeSinceLastAttack >= attackCooldown) {
            Attack();
        }
    }

    public void Attack() {
        timeSinceLastAttack = 0f;

        GameObject fireball = Instantiate(fireballPrefab, attackSpawnTransform.position, attackSpawnTransform.rotation);
        Rigidbody fireball_rb = fireball.GetComponent<Rigidbody>();

        // Add forward force, and destroy it after some time
        fireball_rb.AddForce(attackSpawnTransform.forward * fireballSpeed, ForceMode.Impulse);
        StartCoroutine(DestroyAfterLifespan(fireballLifespan, fireball));
    }

    // Destroy the target gameobject after some time, if it's still in the scene
    // Otherwise, it must have collided with something
    private IEnumerator DestroyAfterLifespan(float lifespan, GameObject target) {
        yield return new WaitForSeconds(lifespan);

        if (target != null) {
            Destroy(target);
        }
    }

    public void Die() {
        if (isDying) return;
        isDying = true;

        audioSource.enabled = true;
        audioSource.PlayOneShot(deathAudio);

        deathVFX.enabled = true;
        deathVFX.Play();

        // -- Disable things so that this enemy doesn't damage the player post-death
        ui.SetActive(false);
        foreach (Collider c in colliders) {
            c.enabled = false;
        }
        rb.freezeRotation = false;

        StartCoroutine(DestroyAfterLifespan(disappearTime, transform.parent.gameObject));
    }
}
