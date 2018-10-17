using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class PlatformControl : MonoBehaviour {

    [Header("Required Transform References")]
    [Tooltip("The trasnform marking the initial position of the platform")]
    public Transform origin;

    [Tooltip("The trasnform marking the destination of the platform")]
    public Transform destination;

    [Header("Platform Speed Settings")]
    [Tooltip("How many second it takes for the platform to move from the origin to the desintation (and vice-versa)")]
    public float travelTime = 2f;

    [Tooltip("How many seconds the platform will wait at either end of its path")]
    public float pauseTime = 3f;

    private CharacterController player; // The transform of the player
    private float pauseTimer; // The timer to reduce while the platform is waiting
    private float speed; // How fast the platform moves
    private bool paused = false;

    private Rigidbody platformRigidBody; // A reference to the kinematic Rigidbody of the platform
    private bool returning = false;
    

	// Use this for initialization
	void Start () {
        platformRigidBody = GetComponent<Rigidbody>();
        transform.position = origin.position; // Move the platform to the starting position
        float moveDistance = Vector3.Distance(origin.position, destination.position);
        speed = moveDistance / travelTime;
	}
	
	// Update is called once per frame, after others
	void FixedUpdate () {
        if (!paused) {
            UpdatePlatformPosition();
        } else {
            UpdateTimer();

        }
    }

    // Called in FixedUpdate
    void UpdatePlatformPosition() {
        Vector3 to;

        if (returning) { // Platform moving 'to' based on if it is returning or not
            to = origin.position;
        } else {
            to = destination.position;
        }

        float step = speed * Time.fixedDeltaTime; // Move according to the amount of time passed

        Vector3 oldPos = platformRigidBody.position; // Save the previous position
        Vector3 newPos = Vector3.MoveTowards(platformRigidBody.position, to, step); // Calculate the new position
        platformRigidBody.MovePosition(newPos); // Updates position at the end of the physics step


        if (player != null) {
            player.Move(newPos - oldPos); // Move the player the same amount that the platform did
        }

        if (Vector3.Distance(transform.position, to) <= 0) {
            returning = !returning; // Invert returning (thus inverting platform direction)
            StartTimer();
        }
    }

    // Called in FixedUpdate
    void UpdateTimer() {
        pauseTimer -= Time.fixedDeltaTime;
        if (pauseTimer <= 0) {
            ResetTimer();
        }
    }

    void StartTimer() {
        paused = true;
        pauseTimer = pauseTime; // Set the timer to the maximum pause time
    }

    void ResetTimer() {
        paused = false;
        pauseTimer = 0;
    }

    private void OnTriggerStay(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            if (player == null) {
                player = collider.gameObject.GetComponent<CharacterController>(); // Get the CharacterController for moving with the platform
            }
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            player = null; // Clear the player transform
        }
    }
}
