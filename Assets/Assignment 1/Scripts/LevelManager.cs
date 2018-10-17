using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager singleton;

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
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeScene(string sceneName) {
        // Use a coroutine to load the Scene in the background
        StartCoroutine(AsyncSceneLoad(sceneName));
    }

    // From Unity docs, allows asynchronous loading of a scene
    private IEnumerator AsyncSceneLoad(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
