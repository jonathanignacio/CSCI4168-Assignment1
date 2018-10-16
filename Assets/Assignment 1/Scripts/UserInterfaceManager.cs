using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterfaceManager : MonoBehaviour {

    public static UserInterfaceManager singleton;

    public GameObject UICanvasPrefab; // A reference to the UI canvas prefab
    public GameObject healthUIPrefab; // A reference to the section of the UI for health display
    public GameObject coinUIPrefab; // A reference to the coin display
    public GameObject heartContainerPrefab; // A reference to the heart container prefab 


    private int currentCoins = 0; // Number of coins the player has
    private readonly int heartCount = 3; // Number of heart containers
    private int currentHealth; // Number of fully red hearts

    private GameObject abstractInstance; // A reference to the Abstract game object for organizing non-worldspace entities

    // GameObject instances
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
        InitializeCanvas();
        InitializeHearthContainers();
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

        for (int i = 0; i < heartCount; i++) {
            GameObject heart = Instantiate(heartContainerPrefab, healthUIInstance.transform);
            heart.transform.Translate(Vector3.right * i * 50);
            heartInstances.Add(heart);
        }

        UpdateHealthDisplay(currentHealth); // Refresh the heart containers
    }

    // Update the number of red hearts to display
    public void UpdateHealthDisplay(int updatedHealth) {
        currentHealth = updatedHealth; // Update the number of full hearts
        if (heartInstances != null) { // Can only update hearts if they exist in the list
            int i = 0;
            foreach (GameObject heart in heartInstances) {
                if (this.currentHealth > i) { // If this heart is healthy
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
    public void UpdateCoinDisplay(int updatedCoins) {
        currentCoins = updatedCoins;
        coinUIInstance.GetComponentInChildren<UnityEngine.UI.Text>().text = "" + currentCoins;
    }
}
