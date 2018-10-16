using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class PlatformControl : MonoBehaviour {

    public float travelTime = 2f; // Platform will reach destination after this many seconds
    public float pauseTime = 3f; // Number of seconds for the platform to wait at the end of the movement


    public Transform origin; // Where the platform starts
    public Transform destination; // Where the platform ends

    private CharacterController player; // The transform of the player
    private float pauseTimer; // The timer to reduce while the platform is waiting
    private float speed; // How fast the platform moves
    private bool paused = false;

    private Rigidbody platformRigidBody; // A reference to the kinematic Rigidbody of the platform
    private Vector3 playerOffset; // The amount to move the player by to catch it up to the platform
    private bool playerMoved = false; // Signal if the player has already been moved by the script to allow independent player movement

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
        }
    }

    private void Update() {
        UpdateTimer(); // Update the timer here
    }
    private void LateUpdate() {
        playerMoved = false; // After other updates, let the player be moved by the platform again
    }

    void UpdatePlatformPosition() {
        Vector3 to;

        if (returning) { // Platform moving 'to' based on if it is returning or not
            to = origin.position;
        } else {
            to = destination.position;
        }

        float step = speed * Time.fixedDeltaTime; // Move according to the amount of time passed

        Vector3 oldPos = platformRigidBody.position;
        platformRigidBody.MovePosition(Vector3.MoveTowards(platformRigidBody.position, to, step));


        if (player && !playerMoved) {
            player.Move(platformRigidBody.position - oldPos - playerOffset); // Move the player to the center of the platform
            playerMoved = true;
            // TODO make the player move relative to the platform (without parenting)
        }

        if (Vector3.Distance(transform.position, to) <= 0) {
            returning = !returning; // Invert returning (thus inverting platform direction)
            StartTimer();
        }
    }

    void UpdateTimer() {
        pauseTimer -= Time.deltaTime;
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

            // Check again after first null check to allow player offset updating on the first trigger
            if (!playerMoved && player != null) { // Reference to player must be defined
                playerOffset = platformRigidBody.position - player.transform.position; // Update the player offset from the center of the platform
                playerOffset.y = 0; // Do not control vertical motion
            }
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            player = null; // Clear the player transform
        }
    }
}
