using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class CoinPlayerCollider : MonoBehaviour {

    public GameObject coinPickupEffect; // The particle effect for when a coin is picked up

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            PlayerManager.singleton.GivePlayerCoins(1); // Give one coin to the player
            Instantiate(coinPickupEffect, transform.position, transform.rotation); // Make a gold effect before the coin is destroyed
            Destroy(this.gameObject); // Remove this coin from the world
        }
    }
}
