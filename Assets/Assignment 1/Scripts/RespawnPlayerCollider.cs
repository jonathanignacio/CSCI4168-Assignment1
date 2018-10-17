using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]

public class RespawnPlayerCollider : MonoBehaviour {

    [Tooltip("The priority of this spawner. A higher number means higher priority. Maximum priority should be the number of spawn points for a scene")]
    public int spawnerNumber; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerManager.singleton.DamagePlayer(1); // Deal one damage to the player on contact
        }
    }
}
