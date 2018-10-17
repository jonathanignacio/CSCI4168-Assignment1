using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class LevelWarpPlayerCollider : MonoBehaviour {

    [Tooltip("The name of the scene to load")]
    public string levelName; // NICETOHAVE: make this a property drawer of only scenes that are included in the builder to avoid potential reference errors

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            LevelManager.singleton.changeScene(levelName); // Signal the level manager to advance the scene
        }
    }
}
