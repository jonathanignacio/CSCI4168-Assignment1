using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager singleton;

    public int playerMaxHealth = 3;
    public int playerHealth = 3; // Player hitpoints
    public float invincibilityDelay = 1.5f; // How long the player is invincible for before they can take more damage

    public int playerCoins = 0; // The number of coins the player has collected

    public GameObject player; // Should be assigned the player prefab
    public Transform spawnPoint; // Get spawnpoint GameObject

    // Audio Clips
    public AudioClip playerHurtSound; // Sound effect for player damage
    public AudioClip playerGetCoinSound; // Sound effect for player collecting coins
    public AudioClip playerSpendCoinSound; // Sound effect for player spending coins

    // Private members
    private bool isInvincible = false; // Toggle whether player can take damage or not
    private float invicibilityTimer; // Amount of time to stay invicible for
    private AudioSource playerAudioSource; // The reference to the player's AudioSource component

	void Awake () {
        bool execute = SetSingleton();

        if (!execute) {
            return;
        }
	}
	
	bool SetSingleton() {
        // Ensure no other instance exists
        if (singleton == null) {
            singleton = this;
            DontDestroyOnLoad(gameObject);
            return true;
        } else {
            Destroy(gameObject);
            return false;
        }
    }

    void Start() {
        playerAudioSource = player.GetComponent<AudioSource>();
        RespawnPlayer(); // Move the player to spawn
    }

    void Update() {
        if (Input.GetKeyDown("x")) { // Debugging to deal damage
            DamagePlayer(1);
        }

        UpdateInvincibility();
    }

    void RespawnPlayer() {
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation); // Respawn the player
    }

    void UpdateInvincibility () {
        if (isInvincible) {
            invicibilityTimer -= Time.deltaTime; // Reduce the timer
            if (invicibilityTimer <= 0) {
                invicibilityTimer = 0; // Reset to 0
                isInvincible = false; // Make player vulnerable again
            }
        }
    }

    // Toggle if the player can take damage
    private void SetPlayerInvincible(bool invincible) {
        if (invincible) {
            isInvincible = invincible;
            invicibilityTimer = invincibilityDelay; // Set a timer
        } else {
            isInvincible = invincible;
        }
    }

    // Damage the player, if they are not invicible
    public void DamagePlayer(int damage) {
        if (!isInvincible) {
            playerAudioSource.PlayOneShot(playerHurtSound);

            SetPlayerHealth(playerHealth - damage);
            SetPlayerInvincible(true); // The player got hit, so temporarily protect from more damage
        }
    }

    // Manually set player health
    private void SetPlayerHealth(int health) {
        playerHealth = Mathf.Clamp(health, 0, playerMaxHealth);

        if (playerHealth <= 0) { // If player has no health left
            playerHealth = 3;
            RespawnPlayer();
        }

        UserInterfaceManager.singleton.UpdateHealthDisplay(playerHealth);
    }

    // Method to spend player coins. Specify price. If player can afford, deduct coins and return true
    public bool SpendPlayerCoins(int price) {
        if (playerCoins >= price) {
            playerAudioSource.PlayOneShot(playerSpendCoinSound);
            SetPlayerCoins(playerCoins - price);
            return true;
        } else {
            return false; // Not enough coins
        }
    }

    // Public method to give player coins
    public void GivePlayerCoins(int coins) {
        if (coins > 0) {
            playerAudioSource.PlayOneShot(playerGetCoinSound); // Play the sound effect for coins
            SetPlayerCoins(playerCoins + coins); // Add the coins to the playerCoins count
        } else {
            Debug.LogWarning("Attempted to give player negative coins");
        }
    }

    // Set player coins to the specified amount
    private void SetPlayerCoins(int coins) {
        playerCoins = Mathf.Clamp(coins, 0, int.MaxValue); // Do not allow negative coins

        // Update number of coins displayed on 
        UserInterfaceManager.singleton.UpdateCoinDisplay(playerCoins);
    }
}
