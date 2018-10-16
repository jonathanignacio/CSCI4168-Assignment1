using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterfaceManager : MonoBehaviour {

    public static UserInterfaceManager singleton;

    public GameObject UserInterfaceCanvas; // A reference to the UI canvas
    public GameObject heathUI; // A reference to the section of the UI for health display
    public GameObject heartContainer; // An instance of a heart container within the health display
    public GameObject coinText; // A reference to the UI element where coin count is displayed

    private int currentCoins = 0; // Number of coins the player has
    private readonly int heartCount = 3; // Number of heart containers
    private int currentHealth; // Number of fully red hearts

    private List<GameObject> heartPrefabs;

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
        InitializeHearthContainers();
    }

    // Method to call when the scene loads
    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode) {
        InitializeHearthContainers();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Fill the list of heart containers with new prefab instances and then re-load the display
    private void InitializeHearthContainers() {
        heartPrefabs = new List<GameObject>(); // Reset the list

        for (int i = 0; i < heartCount; i++) {
            GameObject heart = Instantiate(heartContainer, heathUI.transform);
            heart.transform.Translate(Vector3.right * i * 50);
            heartPrefabs.Add(heart);
        }

        UpdateHealthDisplay(currentHealth); // Refresh the heart containers
    }

    // Update the number of red hearts to display
    public void UpdateHealthDisplay(int updatedHealth) {
        currentHealth = updatedHealth; // Update the number of full hearts
        int i = 0;
        foreach(GameObject heart in heartPrefabs) {
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

    public void UpdateCoinDisplay(int updatedCoins) {
        currentCoins = updatedCoins;
        coinText.GetComponentInChildren<UnityEngine.UI.Text>().text = "" + currentCoins;
    }
}
