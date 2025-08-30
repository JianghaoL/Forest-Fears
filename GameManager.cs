using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("Pause Menu Settings")]
    [SerializeField] KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] bool isPaused = false;
    [SerializeField] MusicManager musicManager;
    [SerializeField] PlayerScript playerScript; // Reference to the player script to check if the player is alive
    //[SerializeField] AK.Wwise.Event uiSFX; // Event to play music when the pause menu is active

    [Header("Game Settings")]
    [SerializeField] float gameVolume;
    [SerializeField] AK.Wwise.RTPC gameVolumeRTPC;
    [SerializeField] Slider volumeSlider;

    private void Start()
    {
        // Initialize the game volume from the RTPC
        gameVolume = 100f;
        volumeSlider.value = gameVolume; // Set the slider to the current volume
        gameVolumeRTPC.SetGlobalValue(gameVolume);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey) && !isPaused && !playerScript.GetTutorialActive())
        {
            PauseGame();
            return;
        }

        if (isPaused)
        {
            gameVolume = volumeSlider.value; // Update the game volume from the slider
            gameVolumeRTPC.SetGlobalValue(gameVolume); // Set the RTPC value for game volume
        }

        if (Input.GetKeyDown(pauseKey) && isPaused)
        {
            ResumeGame();
            return;
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Stop the game time
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        musicManager.PauseMusic(); // Pause the music
    }

    public void ResumeGame()
    {
        musicManager.PlayButtonSFX(); // Play the UI sound effect when resuming the game
        isPaused = false;
        Time.timeScale = 1f; // Resume the game time
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        musicManager.ResumeMusic(); // Resume the music
    }

    public void BackToMainMenu()
    {
        musicManager.PlayButtonSFX(); // Play the UI sound effect when going back to the main menu
        SceneManager.LoadScene(0); // Load the main menu scene
    }
}
