using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class LargeDemonEnemy : MonoBehaviour {
    public float speed;
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

    private void Start() {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        Vector3 dirToPlayer = Player.instance.catTransform.position - transform.position;
        Vector3 movementDir = Vector3.ProjectOnPlane(dirToPlayer.normalized, Vector3.up);

        // -- Rotate to face player
        float angle = Mathf.Atan2(movementDir.x, movementDir.z) * Mathf.Rad2Deg;
        rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));


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
}
