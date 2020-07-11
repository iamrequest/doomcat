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
    private Rigidbody car_rb;
    private Rigidbody rb;

    public float forwardSpeed;
    public float turnSpeed;
    public float jumpHeightSpeed;
    public float jumpFlipSpeed;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        car_rb = car.GetComponent<Rigidbody>();
    }

    private void Update() {
        // Temp: Re-orient everything upwards
        if (Input.GetKeyDown(KeyCode.Space)) {
            //car.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);

            rb.AddForce(Vector3.up * jumpHeightSpeed);
            car_rb.AddForce(Vector3.up * jumpHeightSpeed, ForceMode.Impulse);
            car_rb.AddTorque(car.transform.forward* jumpHeightSpeed, ForceMode.Impulse);
        }
    }

    void FixedUpdate() {
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

        // Move the car towards the ball
        car_rb.MovePosition(transform.position);

        // Temp: Scale ball
        if (Input.GetKeyDown(KeyCode.P)) {
            transform.localScale *= 1.1f;
        } else if (Input.GetKeyDown(KeyCode.O)) {
            transform.localScale /= 1.1f;
        }
    }
}
