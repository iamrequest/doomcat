using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Fix physics when the car flips
/// <summary>
// Cart physics are based off of this twitter post: https://twitter.com/Nitroneers/status/1238779466832363520
// TL;DR: The cart follows a sphere. 
//        We add torque to the car to turn it left and right
//        We add force to the sphere, rolling in the forward direction of the cart
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CartLocomotion : MonoBehaviour {
    public GameObject car;
    [Tooltip("The rigidbody of the actual cart")]
    private Rigidbody car_rb;
    [Tooltip("The rigidbody of the driving sphere")]
    public Rigidbody rb;

    public float verticalSphereOffset;
    public float forwardSpeed;
    public float turnSpeed;
    public float jumpHeightSpeed;
    public float jumpFlipSpeed;
    public float kickflipJumpSpeed;
    public float kickflipTurnSpeed;
    public bool isGrounded;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        car_rb = car.GetComponent<Rigidbody>();
    }

    private void Update() {
        // Don't allow player input if the player's dead
        if (Player.instance.damagable.isDead) return;

        // Jump
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(Vector3.up * jumpHeightSpeed, ForceMode.Impulse);
        }

        // Kickflip
        if (Input.GetKey(KeyCode.E)) {
            rb.AddForce(Vector3.up * kickflipJumpSpeed, ForceMode.Force);
            car_rb.AddForce(Vector3.up * kickflipJumpSpeed, ForceMode.Force);
            car_rb.AddTorque(car.transform.forward * -kickflipTurnSpeed, ForceMode.Force);
        } else if (Input.GetKey(KeyCode.Q)) {
            rb.AddForce(Vector3.up * kickflipJumpSpeed, ForceMode.Force);
            car_rb.AddForce(Vector3.up * kickflipJumpSpeed, ForceMode.Force);
            car_rb.AddTorque(car.transform.forward * kickflipTurnSpeed, ForceMode.Force);
        }
    }

    void FixedUpdate() {
        // Don't allow player input if the player's dead
        if (!Player.instance.damagable.isDead) {
            // Rotate the car
            if (Input.GetKey(KeyCode.A)) {
                car_rb.AddTorque(car.transform.up * -turnSpeed, ForceMode.Force);
                //forwardDir += transform.right * -turnSpeed;
            }  else if (Input.GetKey(KeyCode.D)) {
                car_rb.AddTorque(car.transform.up * turnSpeed, ForceMode.Force);
                //forwardDir += transform.right * turnSpeed;
            }


            // Roll the sphere, in the direction of the car's forward dir
            if (Input.GetKey(KeyCode.W)) {
                rb.AddForce(car.transform.forward * forwardSpeed, ForceMode.Force);
            } else if (Input.GetKey(KeyCode.S)) {
                rb.AddForce(car.transform.forward * -forwardSpeed, ForceMode.Force);
            }

            // Temp: Scale ball
            if (Input.GetKeyDown(KeyCode.P)) {
                transform.localScale *= 1.1f;
            } else if (Input.GetKeyDown(KeyCode.O)) {
                transform.localScale /= 1.1f;
            }
        }

        // Move the car towards the ball
        car_rb.MovePosition(transform.position + car.transform.up * verticalSphereOffset);
    }

    private void OnCollisionEnter(Collision collision) {
        isGrounded = true;
    }
    private void OnCollisionExit(Collision collision) {
        isGrounded = false;
    }

    public void ResetVelocity() {
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        car_rb.velocity = Vector3.zero;
        car_rb.rotation = Quaternion.identity;
    }

    public void AddExplosionForce(float explosionForce, Vector3 position, float radius, float upForce) {
        rb.AddExplosionForce(explosionForce, position, radius, upForce);
        car_rb.AddExplosionForce(explosionForce, position, radius, upForce);
    }
}
