using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]

public class SpawnPoint : MonoBehaviour {

    public int spawnerNumber; // The priority of this spawner. A higher number means higher priority

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Draw a gizmo for the spawn point for debugging
    void OnDrawGizmos() {
        Gizmos.color = Color.magenta; // Set gizmo
        Gizmos.DrawIcon(transform.position, "Meeple.png");
    }


    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            PlayerManager.singleton.HandlePlayerSpawnPointUpdate(spawnerNumber); // Set this as the player's new spawn if it is higher priority
        }
    }

    // Called by an external manager
    public void SetSpawnerNumber(int number) {
        spawnerNumber = number;
    }
}
