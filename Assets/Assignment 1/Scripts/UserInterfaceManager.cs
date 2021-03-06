﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterfaceManager : MonoBehaviour {

    public static UserInterfaceManager singleton;

    [Header("Required Prefab References")]
    [Tooltip("A reference to the UI canvas prefab")]
    public GameObject UICanvasPrefab;

    [Tooltip("A reference to the section of the UI for health display")]
    public GameObject healthUIPrefab;

    [Tooltip("A reference to the coin display prefab")]
    public GameObject coinUIPrefab;

    [Tooltip("A reference to the heart container prefab")]
    public GameObject heartContainerPrefab;


    // GameObject instances
    private GameObject abstractInstance; // A reference to the Abstract game object for organizing non-worldspace entities
    private GameObject canvas; // the current instance of the canvas
    private GameObject healthUIInstance; // Current instance of the health UI
    private GameObject coinUIInstance; // Current instance of the coin UI

    private List<GameObject> heartInstances;

    void Awake() {
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
        }
        else {
            Destroy(gameObject);
            return false;
        }
    }

    // Use this for initialization
    void Start () {
        SceneManager.sceneLoaded += this.OnLoadCallback; // Add the callback to the list for when the scene changes
        InitializeCanvas();
        InitializeHearthContainers();
    }

    // Method to call when the scene loads
    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode) {
        InitializeCanvas(); // Build all prefab instances for the UI
        InitializeHearthContainers(); // Create insatnces of heart prefabs for player max health

        // Make sure to reflect the values in PlayerManager after building the elements
        UpdateHealthDisplay();
        UpdateCoinDisplay();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitializeCanvas() {
        abstractInstance = GameObject.FindGameObjectWithTag("Abstract"); // Get the Abstract game object for this scene
        canvas = Instantiate(UICanvasPrefab, abstractInstance.transform); // Create a canvas and set its parent to the Abstract object
        healthUIInstance = Instantiate(healthUIPrefab, canvas.transform); // Create a health UI instance and set its parent to the canvas
        coinUIInstance = Instantiate(coinUIPrefab, canvas.transform); // Createa a coin UI instance and set its parent to the canvas
    }

    // Fill the list of heart containers with new prefab instances and then re-load the display
    private void InitializeHearthContainers() {
        heartInstances = new List<GameObject>(); // Reset the list
        int heartCount = PlayerManager.singleton.playerMaxHealth; // Max health is how many hearts to show

        for (int i = 0; i < heartCount; i++) {
            GameObject heart = Instantiate(heartContainerPrefab, healthUIInstance.transform);
            heart.transform.Translate(Vector3.right * i * 50);
            heartInstances.Add(heart);
        }

        UpdateHealthDisplay(); // Refresh the heart containers
    }

    private void InitializeCoinText() {

    }

    // Update the number of red hearts to display
    public void UpdateHealthDisplay() {
        if (heartInstances != null) { // Can only update hearts if they exist in the list
            int i = 0;
            foreach (GameObject heart in heartInstances) {
                int currentHealth = PlayerManager.singleton.playerHealth; // Get the current player health from the manager
                if (currentHealth > i) { // If this heart is healthy
                    heart.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled = true; // Show the full heart image (first child)
                    heart.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().enabled = false; // Hide the empty heart image (second child)
                }
                else { // Otherwise, show empty container
                    heart.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled = false; // Hide the full heart image (first child)
                    heart.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().enabled = true; // show the empty heart image (second child)
                }
                i++;
            }
        }
    }

    // Get the coin text from the prefab instance and update the value
    public void UpdateCoinDisplay() {
        coinUIInstance.GetComponentInChildren<UnityEngine.UI.Text>().text = "" + PlayerManager.singleton.playerCoins;
    }
}
