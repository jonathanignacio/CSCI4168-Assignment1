using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedLifetime : MonoBehaviour {

    [Tooltip("The amount of seconds that the object should exist before destroying itself")]
    public float lifetime;

    private float timer = 0; // The remaining lifetime

	// Use this for initialization
	void Start () {

	}
	
	void FixedUpdate () {
        timer += Time.fixedDeltaTime;
        if (timer >= lifetime) {
            Destroy(this.gameObject); // Destory the object after the timer completes
        }
	}
}
