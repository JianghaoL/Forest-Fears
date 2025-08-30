using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTime : MonoBehaviour
{
    [SerializeField] GameObject player, mainCamera;
    void OnEnable()
    {
        // Set the time scale to 1 to ensure normal speed
        Time.timeScale = 1f;
        // Optionally, set the fixed time step if needed
        // Time.fixedDeltaTime = 0.02f; // Default is 0.02 seconds (50 FPS)

        player.GetComponent<PlayerScript>().enabled = true; // Enable player controls
        mainCamera.SetActive(true); // Activate the main camera object
    }
}
