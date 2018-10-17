﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager singleton;

    public int playerMaxHealth = 3;
    public int playerHealth = 3; // Player hitpoints
    public float invincibilityDelay = 1.5f; // How long the player is invincible for before they can take more damage

    public int playerCoins = 0; // The number of coins the player has collected

    public GameObject playerPrefab; // Should be assigned the player prefab

    // Audio Clips
    public AudioClip playerHurtSound; // Sound effect for player damage
    public AudioClip playerGetCoinSound; // Sound effect for player collecting coins
    public AudioClip playerSpendCoinSound; // Sound effect for player spending coins

    // Spawn Point Management
    private List<Transform> spawnPoints; // A list of all spawns in the scene
    private Transform currentSpawnPoint; // The currently selected spawn point

    // Player and Components
    private GameObject playerInstance; // The current instance of the player
    private AudioSource playerAudioSource; // The reference to the player's AudioSource component

    // Invicibility variables
    private bool isInvincible = false; // Toggle whether player can take damage or not
    private float invicibilityTimer; // Amount of time to stay invicible for

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
        SceneManager.sceneLoaded += this.OnLoadCallback; // Add the callback to the list for when the scene changes
        InitializeSpawnPoints();
        InitializePlayer(); // Create the player instance
    }

    // Method to call when the scene loads
    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode) {
        InitializeSpawnPoints();
        InitializePlayer(); // Create the player instance
    }

    void Update() {
        if (Input.GetKeyDown("x")) { // Debugging to deal damage
            DamagePlayer(1);
        } // TODO: remove this
    }

    private void FixedUpdate() {
        UpdateInvincibility(); // Update any invicibility timer
    }

    // Player instance assumed null here. Respawns the player and gets required components
    void InitializeSpawnPoints() {
        spawnPoints = new List<Transform>(); // Instantiate a new list of spawnpoints
        GameObject sceneSpawnListObject = GameObject.FindGameObjectWithTag("SpawnPoint"); // The scene's GameObject tagged 'SpawnPoint' which should have all spawn points as children
        int spawnCount = sceneSpawnListObject.transform.childCount; // Get total number of spawn points
        for (int i = 0; i < spawnCount; i++) {
            spawnPoints.Add(sceneSpawnListObject.transform.GetChild(i)); // Get child at the current index and add it to the list
        }

        currentSpawnPoint = spawnPoints[0]; // Use the first spawn point
    }

    // Player instance assumed null here. Respawns the player and gets required components
    void InitializePlayer() {
        RespawnPlayer();
        playerAudioSource = playerInstance.GetComponent<AudioSource>();
    }

    // Creates a new player instance or simply moves the current one to spawn
    void RespawnPlayer() {
        if (playerInstance == null) {
            playerInstance = Instantiate(playerPrefab, currentSpawnPoint.position, currentSpawnPoint.rotation);  
        } else {
            playerInstance.transform.SetPositionAndRotation(currentSpawnPoint.position, currentSpawnPoint.rotation); // Move player to the current spawn point
        }

        SetPlayerHealth(3); // Reset player health
    }

    // Update the timer for invicibility (should be called in FixedUpdate
    void UpdateInvincibility () {
        if (isInvincible) {
            invicibilityTimer += Time.fixedDeltaTime; // Update the timer
            if (invicibilityTimer >= invincibilityDelay) {
                invicibilityTimer = 0; // Reset to 0
                isInvincible = false; // Make player vulnerable again
            }
        }
    }

    // Toggle if the player can take damage
    private void SetPlayerInvincible(bool invincible) {
        isInvincible = invincible;
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
        CheckPlayerHealth();
    }

    // Checks player health and updates user interface
    private void CheckPlayerHealth() {
        if (playerHealth <= 0) { // If player has no health left
            RespawnPlayer();
        }
        UserInterfaceManager.singleton.UpdateHealthDisplay(); // Signal to the UI manager that it should refresh health display
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
        UserInterfaceManager.singleton.UpdateCoinDisplay(); // Signal to the UI manager that it should refresh coin display
    }
}
