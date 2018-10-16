using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class DamagePlayerCollider : MonoBehaviour {

    public int damageAmount = 1; // The amount of damage to deal to the play on contact

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            PlayerManager.singleton.DamagePlayer(1); // Deal one damage to the player on contact
        }
    }
}
