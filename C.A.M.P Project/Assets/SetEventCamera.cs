using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEventCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Find the player's main camera
        Camera playerMainCamera = Camera.main;
        // Find the main camera by tag

        if (playerMainCamera != null)
        {
            // Get the Canvas component of the current GameObject
            Canvas canvas = GetComponent<Canvas>();
            // Set the Canvas's worldCamera to the player's main camera
            canvas.worldCamera = playerMainCamera;
        }
        else {
            Debug.LogError("Main camera not found. Make sure you have a camera tagged as MainCamera in your scene.");
        }
    }
}