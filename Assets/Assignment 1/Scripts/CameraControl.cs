using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl: MonoBehaviour {

    public Transform target; // The transform that the camera will look at
    public float rotationSpeed = 1.0f; // The speed that the camera will rotate by
    public float distance = 3f; // How far the camera is from the target
    public float distanceMin = 1.5f; // The closest the camera can be scrolled in
    public float distanceMax = 6f; // The farthest the camera can be scrolled out

    // Rotation limits
    public float verticalAngleMin = 0f; // Minimum vertical rotation
    public float verticalAngleMax = 75f; // Maximum vertical rotation


    private float horizontalRotation = 180f;
    private float verticalRotation = 0f;

	// Use this for initialization
	void Start () {
        horizontalRotation = target.eulerAngles.y; // Initialize mouseX as from the initial character horizontal rotation

        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
	}
	
	// Update is called once per frame
	void Update () {
        // Mouse input
        horizontalRotation += Input.GetAxis("Mouse X");
        verticalRotation -= Input.GetAxis("Mouse Y"); // Invert y rotation
        distance -= Input.GetAxis("Mouse ScrollWheel"); // Invert scroll wheel
        distance = Mathf.Clamp(distance, distanceMin, distanceMax); // Distance must remain within these bounds

        verticalRotation = Mathf.Clamp(verticalRotation, verticalAngleMin, verticalAngleMax); // Do not allow camera to clip into the ground
	}

    // Update is called after other updates
    private void LateUpdate() {
        Quaternion cameraRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f); // Up and Down mouse movements rotate about the x axis, Left and Right are about the y 
        target.rotation = Quaternion.Euler(0f, horizontalRotation, 0f); // Rotate the player about the y-axis
        transform.position = target.position + cameraRotation * Vector3.back * distance; // Move the camera with the rotation
        transform.LookAt(target);
    }
}
