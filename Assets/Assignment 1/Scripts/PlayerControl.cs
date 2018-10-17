using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class PlayerControl : MonoBehaviour {

    private CharacterController characterController;
    private Animator animator;

    //NICETOHAVE: get and modify these values from the player manager

    [Tooltip("How fast the player will move in units per second")]
    public float moveSpeed = 7.0f;

    [Tooltip("The expected height that a player will climb to, dependent on gravityScale")]
    public float jumpForce = 21f; // The height that a player should climb to

    [Tooltip("The proportion that gravity will affect the player. Best results are a number between 0.1 and 1")]
    public float gravityScale = 0.2f; // Floatier character

    // Movement fields
    private Vector3 currentMovement = new Vector3(0f, 0f, 0f);
    private Vector3 verticalVelocity = Vector3.zero; // The velocity for jumping


	// Use this for initialization
	void Start () {
        characterController = GetComponent<CharacterController>(); // Get a reference to the object's CharacterController
        animator = GetComponent<Animator>(); // Get a reference to the object's Animator
    }
	
	// Update is called once per frame
	void Update () {
       Vector3 directionalMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")); // Get a user input vector
        directionalMovement = Vector3.ClampMagnitude(directionalMovement, 1.0f); // Do not allow directional vector to exceed a magnitude of 1
        directionalMovement = transform.rotation * directionalMovement * moveSpeed; // Rotate the vector by the direction that the player faces and the magnitude (speed)

        animator.SetBool("IsMoving", !directionalMovement.Equals(Vector3.zero));

        // Player directional movement
        currentMovement = new Vector3(0f, 0f, 0f);
        currentMovement.x = directionalMovement.x;
        currentMovement.z = directionalMovement.z;

        // Y movement
        // Cause a jump if the player presses the "Jump" button and the character is not in the air
        if (characterController.isGrounded) {
            animator.ResetTrigger("Jump"); // Player is no longer jumping after landing
            verticalVelocity = Physics.gravity * gravityScale; // Maintain a constant downward velocity 

            if (Input.GetButtonDown("Jump")) {
                animator.SetTrigger("Jump");
                verticalVelocity = jumpForce * Vector3.up;
            }
        }

        currentMovement += verticalVelocity; // Add the effects of jumping/gravity

        animator.SetBool("IsAirborne", !characterController.isGrounded); // Update the animator to reflect if the player is grounded or falling

        characterController.Move(currentMovement * Time.deltaTime); // Scale the movement update by the deltaTime since last update
    }

    private void FixedUpdate() {
        verticalVelocity += Physics.gravity * gravityScale;
    }
}
